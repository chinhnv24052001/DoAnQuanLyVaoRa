import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { VenderSelectOutputDto, VenderServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'CreateOrEditVender',
  templateUrl: './create-or-edit-vender.component.html',
  styleUrls: ['./create-or-edit-vender.component.less']
})
export class CreateOrEditVenderComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  active: Boolean = false;
  saving: Boolean = false;
  isEdit: Boolean = true;
  venderForm: FormGroup;
  vender: VenderSelectOutputDto=new VenderSelectOutputDto();
  phonenumberValidate: boolean=false;
  constructor(
    injector: Injector,
    private _venderService: VenderServiceProxy,
    private formBuilder: FormBuilder
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  show(id): void {
    this.validation();
    if (!id) {
      this.isEdit=false;
      this.vender=new VenderSelectOutputDto();
      this.active = true;
      this.modal.show();
    }
      else{
        this._venderService.loadById(id).subscribe((result)=>
        {
          this.vender.id=result.id;
          this.vender.venderName=result.venderName;
          this.vender.address=result.address;
          this.vender.phoneNumber=result.phoneNumber;
          this.isEdit=true;
          this.active = true;
          this.modal.show();
        }
        );
    }
  }

  validation(): void {
    this.venderForm = this.formBuilder.group({
      VenderName: [undefined, GlobalValidator.required],
      Address: [undefined, GlobalValidator.required],
      PhoneNumber: [undefined, Validators.compose([GlobalValidator.required, GlobalValidator.phoneFormat])],
    });
  }

  save(): void {
    this.isLoading=true;
      this.saving = true;
      this._venderService.save(this.vender).pipe(
        finalize(() => {
          this.saving = false;
        })).subscribe(() => {
          this.notify.info(this.l(AppConsts.message.saveSuccess));
          this.close();
          this.modalSave.emit(null);
          this.vender = new VenderSelectOutputDto();
          this.saving = false;
        });
  }
  
  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
