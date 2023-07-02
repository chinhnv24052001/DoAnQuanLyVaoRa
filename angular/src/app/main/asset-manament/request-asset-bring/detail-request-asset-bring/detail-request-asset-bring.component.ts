import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ApproveOrRejectRequestDto, AssetInOutManamentServiceProxy, RequestAssetBringDetailDto, RequestAssetBringServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Table } from 'primeng/table';
import { MessageAgreeComponent } from '../create-or-edit-request-asset-bring/list-workers/message-confirm/message-agree.component';
import { RequestAssetBringComponent } from '../request-asset-bring.component';
import { ConfirmCancelRequestComponent } from './confirm-cancel-request/confirm-cancel-request.component';
import { CreateAssetBarCodeComponent } from './create-asset-bar-code/create-asset-bar-code.component';
import { DetailHistoryAssetInOutComponent } from './detail-history-asset-io/detail-history-asset-io.component';
import { DetailHistoryEmployeesInOutComponent } from './detail-history-employees-io/detail-history-employees-io.component';
import { ReasonOfRefusalModalComponent } from './reason-of-refusal-modal/reason-of-refusal-modal.component';
import { SendMailToManufactureComponent } from './send-mail-to-manufacture/send-mail-to-manufacture.component';
import { ViewImageComponent } from './view-image/view-image.component';

@Component({
  selector: 'DetailRequestAssetBring',
  templateUrl: './detail-request-asset-bring.component.html',
  styleUrls: ['./detail-request-asset-bring.component.less']
})
export class DetailRequestAssetBringComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild('reasonOfRefusal', { static: true }) reasonOfRefusal: ReasonOfRefusalModalComponent;
  @ViewChild("dataTableAsset", { static: true }) dataTableEmployess: Table;
  @ViewChild('DetailHistoryAssetInOut', { static: true }) DetailHistoryAssetInOut: DetailHistoryAssetInOutComponent;
  @ViewChild('DetailHistoryEmployeesInOut', { static: true }) DetailHistoryEmployeesInOut: DetailHistoryEmployeesInOutComponent;
  @ViewChild('CreateAssetBarCode', {static:true}) CreateAssetBarCode: CreateAssetBarCodeComponent;
  @ViewChild('ConfirmCancelRequest', { static: true }) ConfirmCancelRequest: ConfirmCancelRequestComponent;
  @ViewChild('MessageAgree', { static: true }) messageAgree: MessageAgreeComponent;
  @ViewChild('RequestAssetBring', { static: true }) RequestAssetBring: RequestAssetBringComponent;
  @ViewChild('SendMailToManufacture', { static: true }) SendMailToManufacture: SendMailToManufactureComponent; 
  @ViewChild('viewImage', { static: true }) viewImage: ViewImageComponent;
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalEdit: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalDelete: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalLoadAllRequest: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  requestAssetForm: FormGroup;
  active: Boolean = false;
  saving: Boolean = false;
  isEdit: Boolean = undefined;
  success: Number = 0;
  idRequest =0;
  status;
  isAdminapprove: boolean;
  requestId: number;
  isManagerApproved;
  isADMApproved;
  isTemManagerApproved;
  typeRequest;
  isCreateUser: Boolean =false;
  message1: string = this.l('YourRequestHasBeenApprovedByADM');
  requestAssetBringDetailDto: RequestAssetBringDetailDto = new RequestAssetBringDetailDto();
  approveOrRejectRequestDto: ApproveOrRejectRequestDto = new ApproveOrRejectRequestDto();
  events: { content: string, status: Number, statustrack: Number, approverName: string }[]=[
    { content: this.l(AppConsts.requestAsset.waitManager), status: 0, statustrack: 1, approverName: '' },
    { content: this.l(AppConsts.requestAsset.waitManager), status: 0, statustrack: 0, approverName: '' },
    { content: this.l(AppConsts.requestAsset.adminApproval), status: 0, statustrack:0, approverName: ''},
  ];
  temManageIntervent;
  typeRequestName: string='';
  constructor(
    injector: Injector,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
    private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy
  ) {
    super(injector);
  }

  viewHistoryDetail(id)
  {
    if(this.typeRequest==1 || this.typeRequest==3)
    {
      this.DetailHistoryAssetInOut.show(id, AppConsts.message.historyRequest);
      this.close();
    }
    else{
      this.DetailHistoryEmployeesInOut.show(id, AppConsts.message.historyRequest);
      this.close();
    }
  }

  checkCreateRequestUser(id)
  {
    if(this.appSession.userId==id)
    {
      this.isCreateUser=true;
    }
    else this.isCreateUser=false;
    // console.log(id);
    // console.log(this.appSession.userId);
    // console.log(this.isCreateUser);
  }

  viewDetail(id: number) {
    this.isLoading=true;
    this.requestId=id;
    this._requestAssetBringServiceProxy.getRequestAssetBringDetail(id).subscribe((result) => {
      this.requestAssetBringDetailDto = result;
      this.typeRequest=this.requestAssetBringDetailDto.typeRequest;
      this.typeRequestName=this.requestAssetBringDetailDto.typeRequest==1? this.l(AppConsts.TypeRequestName.INTERNALREQUEST) : this.requestAssetBringDetailDto.typeRequest==2? this.l(AppConsts.TypeRequestName.EMPLOYEESVENDERREQUEST) : this.requestAssetBringDetailDto.typeRequest==4? this.l(AppConsts.TypeRequestName.CLIENTREQUEST) : this.l(AppConsts.TypeRequestName.ADDASSETVENDER);
      this.temManageIntervent=this.requestAssetBringDetailDto.temManageIntervent;
      console.log(this.temManageIntervent);
      if(this.requestAssetBringDetailDto.temManageIntervent)
      {
        this.events=[
          { content: this.l(AppConsts.requestAsset.waitManager), status: 0, statustrack: 1, approverName: '' },
          { content: this.l(AppConsts.requestAsset.waitManager), status: 0, statustrack: 0, approverName: '' },
          { content: this.l(AppConsts.requestAsset.adminApproval), status: 0, statustrack:0, approverName: ''},
        ];
        this.events[0].status = this.requestAssetBringDetailDto.status==21 ? 3 : this.requestAssetBringDetailDto.status==23 ? 2 : 1;
        this.events[0].statustrack= this.requestAssetBringDetailDto.status==23 ? 3 : this.requestAssetBringDetailDto.status==21 ? 1 : 2;
        this.events[0].approverName =this.requestAssetBringDetailDto.temManageApprovalName;
        this.events[0].content = this.requestAssetBringDetailDto.status==21 ? this.l(AppConsts.requestAsset.waitingApprove) : this.requestAssetBringDetailDto.status==23 ? this.l(AppConsts.requestAsset.temManagerRejected) : this.l(AppConsts.requestAsset.temManagerApprovaled);

        this.events[1].status = this.requestAssetBringDetailDto.status==21 ? 0 : this.requestAssetBringDetailDto.status==23 ? 0 : this.requestAssetBringDetailDto.status==2 ? 3 : this.requestAssetBringDetailDto.status==6 ? 2  : 1;
        this.events[1].statustrack= this.requestAssetBringDetailDto.status==2 ? 1 : this.requestAssetBringDetailDto.status==5 ? 2 : this.requestAssetBringDetailDto.status==7 ? 2 : this.requestAssetBringDetailDto.status==8 ? 2: this.requestAssetBringDetailDto.status==6 ? 3 : 0;
        this.events[1].approverName =this.requestAssetBringDetailDto.manageApprovalName;
        this.events[1].content = (this.requestAssetBringDetailDto.status==5|| this.requestAssetBringDetailDto.status==7 ||this.requestAssetBringDetailDto.status==8)? this.l(AppConsts.requestAsset.managerApprovaled) : this.requestAssetBringDetailDto.status==6 ? this.l(AppConsts.requestAsset.managerRejected) : this.l(AppConsts.requestAsset.waitingApprove);
        
        this.events[2].status = this.requestAssetBringDetailDto.status==7 ? 1 : this.requestAssetBringDetailDto.status==8 ? 2 : this.requestAssetBringDetailDto.status==5 ? 3 : 0;
        this.events[2].statustrack = this.requestAssetBringDetailDto.status==7 ? 2 : this.requestAssetBringDetailDto.status==5 ? 1  : this.requestAssetBringDetailDto.status==8 ? 3: 0;
        this.events[2].approverName = this.requestAssetBringDetailDto.adminApprovalName;
        this.events[2].content = this.requestAssetBringDetailDto.status==7 ? this.l(AppConsts.requestAsset.adminApprovaled)  : this.requestAssetBringDetailDto.status==8 ? this.l(AppConsts.requestAsset.adminRejected): this.requestAssetBringDetailDto.status==5 ? this.l(AppConsts.requestAsset.waitingApprove) : this.l(AppConsts.requestAsset.waitingApprove);
      }
      else
      {
        this.events=[
          { content: this.l(AppConsts.requestAsset.waitManager), status: 0, statustrack: 1, approverName: '' },
          { content: this.l(AppConsts.requestAsset.waitManager), status: 0, statustrack: 0, approverName: '' }
        ];
        this.events[0].status = this.requestAssetBringDetailDto.status==2 ? 1 : this.requestAssetBringDetailDto.status==4 ? 1 : this.requestAssetBringDetailDto.status==5 ? 1 : this.requestAssetBringDetailDto.status==3 ? 2  : 3;
        this.events[0].statustrack= this.requestAssetBringDetailDto.status==2 ? 2 : this.requestAssetBringDetailDto.status==4 ? 2 : this.requestAssetBringDetailDto.status==5 ? 2 : this.requestAssetBringDetailDto.status==3 ? 3  : 1;
        this.events[0].approverName =this.requestAssetBringDetailDto.manageApprovalName;
        this.events[0].content = this.requestAssetBringDetailDto.status==2 ? this.l(AppConsts.requestAsset.managerApprovaled) : this.requestAssetBringDetailDto.status==4 ? this.l(AppConsts.requestAsset.managerApprovaled) : this.requestAssetBringDetailDto.status==5 ? this.l(AppConsts.requestAsset.managerApprovaled) : this.requestAssetBringDetailDto.status==3 ? this.l(AppConsts.requestAsset.managerRejected)  : this.l(AppConsts.requestAsset.waitingApprove);
        
        this.events[1].status = this.requestAssetBringDetailDto.status==4 ? 1 : this.requestAssetBringDetailDto.status==5 ? 2 : this.requestAssetBringDetailDto.status==2 ? 3 : 0;
        this.events[1].statustrack = this.requestAssetBringDetailDto.status==4 ? 2 : this.requestAssetBringDetailDto.status==2 ? 1  : this.requestAssetBringDetailDto.status==5 ? 3: 0;
        this.events[1].approverName = this.requestAssetBringDetailDto.admApprovalNameWairting;
        this.events[1].content = this.requestAssetBringDetailDto.status==4 ? this.l(AppConsts.requestAsset.adminApprovaled)  : this.requestAssetBringDetailDto.status==5 ? this.l(AppConsts.requestAsset.adminRejected): this.requestAssetBringDetailDto.status==2 ? this.l(AppConsts.requestAsset.waitingApprove) : this.l(AppConsts.requestAsset.waitingApprove);
      }

      this.isManagerApproved=this.requestAssetBringDetailDto.isManagerApproved;
      this.isADMApproved=this.requestAssetBringDetailDto.isADMApproved;
      this.isTemManagerApproved=this.requestAssetBringDetailDto.isTemManagerApproved;
      this.idRequest =this.requestAssetBringDetailDto.id;
      this.status=this.requestAssetBringDetailDto.status;
      this.isAdminapprove=this.requestAssetBringDetailDto.status==7 ? true: false;
      this.checkCreateRequestUser(result.createByUserId);
      this.active = true;
      this.isLoading=false;
      this.modal.show();
    });
  }

  viewSendMail(requestId)
  {
    this.SendMailToManufacture.show(requestId);
  }

  createAssetBarCode(teamItem, id)
  {
      this._assetInOutManamentServiceProxy.createAssetBarCode(teamItem, id).subscribe((result)=>{
        this._assetInOutManamentServiceProxy.getInfoBarCode(teamItem, id).subscribe((_result)=>
        {
          this.CreateAssetBarCode.show(teamItem, _result, result);
        })
      });
  }

  close(): void {
    this.active = false;
    this.modalClose.emit(null);
    this.modal.hide();
  }

  temMamagerApprove() {
    this.message.confirm('', this.l(AppConsts.message.confirmApprove), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this.approveOrRejectRequestDto.id = this.requestAssetBringDetailDto.id;
        this.approveOrRejectRequestDto.type = this.l(AppConsts.TypeApproveRject.TYPE_APPROVED);
        this._requestAssetBringServiceProxy.temManagerApproveOrReject(this.approveOrRejectRequestDto).subscribe((result) => {
          if (result) {
            this.notify.success(this.l("ApproveSuccess"));
            this.viewDetail(this.idRequest);
          } else {
            this.notify.error(this.l("ApproveError"));
          }
        });
        this.isLoading = false;
      }
    });
  }

  mamagerApprove() {

    // this.modal.hide();
    // this.reasonOfRefusal.open(this.requestAssetBringDetailDto.id, this.l(AppConsts.KeyStatus.STATUS_MANAGER_APPROVE), 1);

    this.message.confirm('', this.l(AppConsts.message.confirmApprove), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this.approveOrRejectRequestDto.id = this.requestAssetBringDetailDto.id;
        this.approveOrRejectRequestDto.type = this.l(AppConsts.TypeApproveRject.TYPE_APPROVED);
        this._requestAssetBringServiceProxy.managerApproveOrReject(this.approveOrRejectRequestDto).subscribe((result) => {
          if (result) {
            this.notify.success(this.l("ApproveSuccess"));
            this.viewDetail(this.idRequest);
          } else {
            this.notify.error(this.l("ApproveError"));
          }
        });
        this.isLoading = false;
      }
    });
  }

  admApprove() {
    // this.modal.hide();
    // this.reasonOfRefusal.open(this.requestAssetBringDetailDto.id, this.l(AppConsts.KeyStatus.STATUS_ADM_APPROVE) , 1);

    this.message.confirm('', this.l(AppConsts.message.confirmApprove), (isConfirmed) => {
      if(isConfirmed) {
        this.isLoading = true;
        this.approveOrRejectRequestDto.id = this.requestAssetBringDetailDto.id;
        this.approveOrRejectRequestDto.type = this.l(AppConsts.TypeApproveRject.TYPE_APPROVED);
        this._requestAssetBringServiceProxy.admApproveOrReject(this.approveOrRejectRequestDto).subscribe((result) => {
          if (result) {
            this.notify.success(this.l("ApproveSuccess"));
            this.viewDetail(this.idRequest);
          } else {
            this.notify.error(this.l("ApproveError"));
          }
        });
        this.isLoading = false;
      }
  });
  }

  managerRequestInfo() {
    this.modal.hide();
    this.reasonOfRefusal.open(this.requestAssetBringDetailDto.id, this.l(AppConsts.KeyStatus.STATUS_MANAGER_REQUEST_INFO) , 1);
  }

  admRequestInfo() {
    this.modal.hide();
    this.reasonOfRefusal.open(this.requestAssetBringDetailDto.id, this.l(AppConsts.KeyStatus.STATUS_ADM_REQUEST_INFO) , 1);
  }

  temManagerReject() {
    // this.bsModalRef.hide();
    this.modal.hide();
    this.reasonOfRefusal.open(this.requestAssetBringDetailDto.id, this.l(AppConsts.KeyStatus.STATUS_TEM_MANAGER_REJECT), 2);
  }

  managerReject() {
    // this.bsModalRef.hide();
    this.modal.hide();
    this.reasonOfRefusal.open(this.requestAssetBringDetailDto.id, this.l(AppConsts.KeyStatus.STATUS_MANAGER_REJECT), 2);
  }

  admReject() {
    this.reasonOfRefusal.open(this.requestAssetBringDetailDto.id, this.l(AppConsts.KeyStatus.STATUS_ADM_REJECT), 2);
    this.modal.hide();
  }

  cancelRequest()
  {
    this._requestAssetBringServiceProxy.checkRequestToCancel(this.requestAssetBringDetailDto.id).subscribe((result) => {
      if(!result)
      {
        this.messageAgree.open(0);
      }
      else
      {
        this.ConfirmCancelRequest.open();
      }
    });
  }

  //Sua lại gui mail
  cancelRequestDelete(id)
  {
    this._requestAssetBringServiceProxy.sendMailToCancelRequest(id).subscribe(() => {
      this.modalDelete.emit(id);
    this.active = false;
    this.modal.hide();
    });
  }

  //Sua lai gui mail
  cancelRequestEdit(id)
  {
      this.modalEdit.emit(id);
    this.active = false;
    this.modal.hide();
  }

  cancelRequestSaveDraft(id)
  {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._requestAssetBringServiceProxy.sendMailToCancelRequest(id).subscribe(() => {
          this._requestAssetBringServiceProxy.saveDraft(id).subscribe(() => {
            this.modalLoadAllRequest.emit();
            this.notify.success(this.l(AppConsts.message.deleteSuccess));
            this.active = false;
            this.modal.hide();
          });
        });
      }
    });
  }

  viewImageByte(assetByteArray)
  {
    if (assetByteArray != null)
    {
      this.viewImage.show(assetByteArray);
    }
  }
}
