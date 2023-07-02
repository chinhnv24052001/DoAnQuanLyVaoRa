import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ApproveOrRejectRequestDto, RequestAssetBringServiceProxy } from '@shared/service-proxies/service-proxies';
import { result } from 'lodash-es';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-reason-of-refusal-modal',
  templateUrl: './reason-of-refusal-modal.component.html',
  styleUrls: ['./reason-of-refusal-modal.component.css']
})
export class ReasonOfRefusalModalComponent extends AppComponentBase implements OnInit {
  @ViewChild("reasonOfRefusal", { static: true }) modal: ModalDirective;
  @Output() modalLoadDetail: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  @Input() idRequest;
  id: number = 0;
  typeReject: String;
  formReject: FormGroup;
  approveOrRejectRequestDto: ApproveOrRejectRequestDto = new ApproveOrRejectRequestDto();
  saving: Boolean = false;
  rejectOrAppoval: number = 1;
  stringConfirm: string;
  constructor(injector: Injector,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
    private formBuilder: FormBuilder,) {
    super(injector);
  }

  ngOnInit(): void {

  }

  open(id: number, typeReject: String, _rejectOrAppoval: number) {
    this.rejectOrAppoval = _rejectOrAppoval;
    if(_rejectOrAppoval == 1)
    {
      this.stringConfirm = this.l(AppConsts.message.confirmResend);
    }
    else
    {
      this.stringConfirm = this.l(AppConsts.message.confirmReject);
    }
    this.id = id;
    this.typeReject = typeReject;
    this.buildForm();
    this.modal.show();
  }

  buildForm() {
    this.formReject = this.formBuilder.group({
      reason: [undefined]
    });
  }

  close() {
    this.modal.hide();
    this.modalLoadDetail.emit(this.id)
  }

  ApprovalOrReject() {

    this.message.confirm('', this.stringConfirm, (isConfirmed) => {
      if (isConfirmed) {
        this.saving = true;
        if (this.id > 0) {
          this.approveOrRejectRequestDto.id = this.id;
          this.approveOrRejectRequestDto.reasonRefusal = this.formReject.get('reason').value;

          if (this.typeReject === this.l(AppConsts.KeyStatus.STATUS_TEM_MANAGER_REJECT)) {
             this.approveOrRejectRequestDto.type = this.l(AppConsts.TypeApproveRject.TYPE_REJECT);
            this._requestAssetBringServiceProxy.temManagerApproveOrReject(this.approveOrRejectRequestDto).subscribe((result) => {
              if (result) {
                this.notify.success(this.l(AppConsts.message.rejectSuccess));
                this.modalLoadDetail.emit(this.id)
                this.close();
              } else {
                this.notify.error(this.l(AppConsts.message.rejectError));
              }
            });
          }

          else if (this.typeReject === this.l(AppConsts.KeyStatus.STATUS_MANAGER_REJECT)) {
            this.approveOrRejectRequestDto.type = this.l(AppConsts.TypeApproveRject.TYPE_REJECT);
            this._requestAssetBringServiceProxy.managerApproveOrReject(this.approveOrRejectRequestDto).subscribe((result) => {
              if (result) {
                this.notify.success(this.l(AppConsts.message.rejectSuccess));
                this.modalLoadDetail.emit(this.id)
                this.close();
              } else {
                this.notify.error(this.l(AppConsts.message.rejectError));
              }
            });
          }

          else if (this.typeReject === this.l(AppConsts.KeyStatus.STATUS_ADM_REJECT)) {
            this.approveOrRejectRequestDto.type = this.l(AppConsts.TypeApproveRject.TYPE_REJECT);
            this._requestAssetBringServiceProxy.admApproveOrReject(this.approveOrRejectRequestDto).subscribe((result) => {
              if (result) {
                this.notify.success(this.l(AppConsts.message.rejectSuccess));
                this.modalLoadDetail.emit(this.id)
                this.close();
              } else {
                this.notify.error(this.l(AppConsts.message.rejectError));
              }
            });
          }

          else if (this.typeReject === this.l(AppConsts.KeyStatus.STATUS_MANAGER_REQUEST_INFO))
          {
            this.approveOrRejectRequestDto.type = this.l(AppConsts.TypeApproveRject.TYPE_REQUEST_INFO);
            this._requestAssetBringServiceProxy.managerApproveOrReject(this.approveOrRejectRequestDto).subscribe((result) => {
              if (result) {
                this.notify.success(this.l(AppConsts.message.requestInforSuccess));
                this.modalLoadDetail.emit(this.id)
                this.close();
              } else {
                this.notify.error(this.l(AppConsts.message.requestInfoError));
              }
            });
          }

          else if (this.typeReject === this.l(AppConsts.KeyStatus.STATUS_ADM_REQUEST_INFO)) {
            this.approveOrRejectRequestDto.type = this.l(AppConsts.TypeApproveRject.TYPE_REQUEST_INFO);
            this._requestAssetBringServiceProxy.admApproveOrReject(this.approveOrRejectRequestDto).subscribe((result) => {
              if (result) {
                this.notify.success(this.l(AppConsts.message.requestInforSuccess));
                this.modalLoadDetail.emit(this.id)
                this.close();
              } else {
                this.notify.error(this.l(AppConsts.message.requestInfoError));
              }
            });
          }

        }
         else {
          this.notify.error(this.l(AppConsts.message.rejectError));
        }
        this.saving = false;
      }
    });
  }

}
