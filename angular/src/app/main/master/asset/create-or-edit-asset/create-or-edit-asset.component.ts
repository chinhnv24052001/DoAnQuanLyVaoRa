import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssetGroupSelectOutputDto, AssetGroupServiceProxy, AssetSaveDto, AssetServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'CreateOrEditAsset',
  templateUrl: './create-or-edit-asset.component.html',
  styleUrls: ['./create-or-edit-asset.component.less']
})
export class CreateOrEditAssetComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  active: Boolean = false;
  saving: Boolean = false;
  assetForm: FormGroup;
  asset: AssetSaveDto = new AssetSaveDto();
  selectAssetGroup: string=this.l('SelectAssetGroup');
  
  listAssetGroupsDropDown: { value: number, label: string }[];
  constructor(
    injector: Injector,
    private _assetService: AssetServiceProxy,
    private formBuilder: FormBuilder
  ) {
    super(injector);
  }

  ngOnInit() {
    this.selectDropDownAssetGroups();
  }

  selectDropDownAssetGroups() {
    this._assetService.getAssetGroupForEdit().subscribe((result) => {
      this.listAssetGroupsDropDown = [];
      this.listAssetGroupsDropDown.push({ value: 0, label: "." });
      result.forEach(ele => {
        this.listAssetGroupsDropDown.push({ value: ele.id, label: ele.assetGroupName });
      });
    });
  }

  isEdit: Boolean = true;

  show(id): void {
    this.validation();
    if (!id) {
      this.isEdit = false;
      this.asset = new AssetSaveDto();
      this.active = true;
      this.modal.show();
    }
    else {
      this._assetService.loadById(id).subscribe((result) => {
        this.asset.id = result.id;
        this.asset.assetName = result.assetName;
        this.asset.assetGroupId = result.assetGroupId;
        // this.asset.tagCode = result.tagCode;
        this.isEdit = true;
        this.active = true;
        this.modal.show();
      }
      );
    }
  }

  validation(): void {
    this.assetForm = this.formBuilder.group({
      AssetGroupId: [undefined, GlobalValidator.required],
      AssetName: [undefined, GlobalValidator.required]
      // TagCode: [undefined, GlobalValidator.required]
    });
  }

  save(): void {
    this.saving = true;
    this._assetService.save(this.asset).pipe(
      finalize(() => {
        this.saving = false;
      })).subscribe(() => {
        this.notify.info(this.l(AppConsts.message.saveSuccess));
        this.close();
        this.modalSave.emit(null);
        this.asset = new AssetSaveDto();
        this.saving = false;
      });
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
