import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ToastService } from '@shared/common/ui/toast.service';
import { AssetGroupSelectOutputDto, AssetGroupServiceProxy, RequestAssetBringServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'send-mail-to-manufacture',
  templateUrl: './send-mail-to-manufacture.component.html',
  styleUrls: ['./send-mail-to-manufacture.component.less']
})
export class SendMailToManufactureComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active: Boolean = false;
  saving: Boolean = false;
  sendMailGroupForm: FormGroup;
  email: string = "";
  fileUpload: File;
  requestId: number = 0;
  uploadUrl: string = '';
  @Input() isLoading;
  constructor(
    injector: Injector,
    private _assetGroupService: AssetGroupServiceProxy,
    private formBuilder: FormBuilder,
    private toastService: ToastService,
    private _httpClient: HttpClient,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
  ) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportEmployeesFromExcel';
  }

  ngOnInit() {
  }

  show(id): void {
    this.validation();
    this.requestId = id;
    this.active = true;
    this.modal.show();
  }

  validation(): void {
    this.sendMailGroupForm = this.formBuilder.group({
      Email: [undefined, GlobalValidator.required],
    });
  }

  sendMail(): void {
    this.spinnerService.show();
     this._requestAssetBringServiceProxy.sendEmailToManufacture(
      this.requestId,
      this.sendMailGroupForm.get('Email').value)
      .pipe(finalize(() => 
      {
        this.spinnerService.hide();
      }))
       .subscribe(() => {
         this.notify.info(this.l(AppConsts.message.sendSuccessfully));
         this.close();
         this.saving = false;
         this.isLoading = false;
       });
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
