import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssetGroupSelectOutputDto, AssetGroupServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'CreateOrEditAssetGroup',
  templateUrl: './create-or-edit-asset-group.component.html',
  styleUrls: ['./create-or-edit-asset-group.component.less']
})
export class CreateOrEditAssetGroupComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active: Boolean = false;
  saving: Boolean = false;
  assetGroupForm: FormGroup;
  @Input() isLoading;
  assetGroup: AssetGroupSelectOutputDto = new AssetGroupSelectOutputDto();
  constructor(
    injector: Injector,
    private _assetGroupService: AssetGroupServiceProxy,
    private formBuilder: FormBuilder
  ) {
    super(injector);
  }

  ngOnInit() {
  }
  isEdit: Boolean = true;

  show(id): void {
    this.validation();
    if (!id) {
      this.isEdit = false;
      this.assetGroup = new AssetGroupSelectOutputDto();
      this.active = true;
      this.modal.show();
    }
    else {
      this._assetGroupService.loadById(id).subscribe((result) => {
        this.assetGroup.id = result.id;
        this.assetGroup.assetGroupName = result.assetGroupName;
        this.isEdit = true;
        this.active = true;
        this.modal.show();
      }
      );
    }
  }

  validation(): void {
    this.assetGroupForm = this.formBuilder.group({
      AssetGroupName: [undefined, GlobalValidator.required]
    });
  }

  save(): void {
    this.saving = true;
    this.isLoading = true;
    this._assetGroupService.save(this.assetGroup).pipe(
      finalize(() => {
        this.saving = false;
      })).subscribe(() => {
        this.notify.info(this.l(AppConsts.message.saveSuccess));
        this.close();
        this.modalSave.emit(null);
        this.assetGroup = new AssetGroupSelectOutputDto();
        this.saving = false;
      });
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
