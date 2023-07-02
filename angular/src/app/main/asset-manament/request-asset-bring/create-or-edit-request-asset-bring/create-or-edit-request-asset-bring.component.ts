import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AioAssetDto, AioEmployeesDto, ApproveOrRejectRequestDto, RequestAssetBringSaveDto, RequestAssetBringServiceProxy } from '@shared/service-proxies/service-proxies';
import { AbpSessionService } from 'abp-ng2-module';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';
import { isDeepStrictEqual } from 'util';
import {HttpClient} from '@angular/common/http';
import { ToastService } from '@shared/common/ui/toast.service';
import { MessageAgreeComponent } from './list-workers/message-confirm/message-agree.component';
import { MessageImportErrorComponent } from './message-import-error/message-import-error.component';
import { ListAssetComponent } from './list-asset/list-asset.component';

@Component({
  selector: 'CreateRequestAssetBring',
  templateUrl: './create-or-edit-request-asset-bring.component.html',
  styleUrls: ['./create-or-edit-request-asset-bring.component.less']
})
export class CreateOrEditRequestAssetBringComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild("dataTableAsset", { static: true }) dataTableEmployess: Table;
  @ViewChild("messageImportError", { static: true }) messageImportError: MessageImportErrorComponent;
  @ViewChild('UploadRequestInfor', { static: false }) uploadRequestInfor: FileUpload;
  // @ViewChild('asset', { static: true }) listAsset: ListAssetComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  requestAssetForm: FormGroup;
  active: Boolean = false;
  saving: Boolean = false;
  isEdit: Boolean = undefined;
  isSaveAsset: Boolean = true;
  userName: string = '';
  department: string = '';
  testDate: Date = new Date();
  newAmEmployeesDto: AioEmployeesDto = new AioEmployeesDto();
  newAmAssetDto: AioAssetDto = new AioAssetDto();
  listEmployeesModal: AioEmployeesDto[] = [];
  listAssetModal: AioAssetDto[] = [];
  requestAssetBringSave: RequestAssetBringSaveDto = new RequestAssetBringSaveDto;
  getRequestAsset: RequestAssetBringSaveDto;
  listAssetDropDown: { value: number, label: string }[];
  listVenderDropDown: { value: number, label: string }[];
  message1: any[];
  // controlAsset:  FormControl;
  controlVender: FormControl;
  assetForms = new FormArray([]);
  emplyeesForms = new FormArray([]);
  typeRequest: number;
  isDraft: number = 2;
  isCreate: number;
  typeRequestName: string = '';
  titleRequest: string = '';
  selectVender: string = this.l('SelectVender');
  requestAssetList: any;
  uploadUrl: string;
  statusSaveIdentity: boolean=true;
  isSendMail: number=0;
  isInternal: Boolean =true;
  constructor(
    injector: Injector,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
    private formBuilder: FormBuilder,
    private _httpClient: HttpClient,
    private toastService: ToastService,
  ) {
    super(injector);
    
  }

  ngOnInit() {
    this.selectDropDownAsset();
    this.selectDropDownVender();
    this.getUser();
    this.isSendMail=0;
  }

  selectDropDownAsset() {
    this._requestAssetBringServiceProxy.getAssetForEdit().subscribe((result) => {
      this.listAssetDropDown = [];
      this.listAssetDropDown.push({ value: 0, label: "." });
      // this.controlAsset = new FormControl(this.listAssetDropDown[0].value);
      result.forEach(ele => {
        this.listAssetDropDown.push({ value: ele.id, label: ele.assetName });
      });
    });
  }

  selectDropDownVender() {
    this._requestAssetBringServiceProxy.getVenderForEdit().subscribe((result) => {
      this.listVenderDropDown = [];
      this.listVenderDropDown.push({ value: 0, label: "." });
      this.controlVender = new FormControl(this.listVenderDropDown[0].value);
      result.forEach(ele => {
        this.listVenderDropDown.push({ value: ele.id, label: ele.venderName });
      });
    });
  }

  show(id, typeRequest?: number): void {
    this.isLoading = true;
    if(typeRequest!=2)
    {
      this.statusSaveIdentity=false;
    }
    this.typeRequestName = typeRequest == 1 ? this.l(AppConsts.TypeRequestName.INTERNALREQUEST) : typeRequest == 2 ? this.l(AppConsts.TypeRequestName.EMPLOYEESVENDERREQUEST) :  typeRequest == 4 ? this.l(AppConsts.TypeRequestName.CLIENTREQUEST) :  this.l(AppConsts.TypeRequestName.ADDASSETVENDER);
    this.titleRequest = typeRequest == 1 ? this.l(AppConsts.TypeRequestName.ADDINTERNAL) : typeRequest == 2 ? this.l(AppConsts.TypeRequestName.CREATEEMPLOYEESVENDER) : typeRequest == 4 ? this.l(AppConsts.TypeRequestName.CREATECLIENTREQUEST) : this.l(AppConsts.TypeRequestName.CREATEASSETVENDER);
    if (!id) {
      this.buildForm(typeRequest);
      this.requestAssetBringSave = new RequestAssetBringSaveDto();
      this.getRequestAsset = new RequestAssetBringSaveDto();
      this.getRequestAsset.typeRequest=typeRequest;
      this.isDraft = 2;
      this._requestAssetBringServiceProxy.loadUserForCreate().subscribe((result) => {
        this.requestAssetForm.patchValue(result);
        this.requestAssetForm.patchValue({
          assetList: [],
          workersList: [],
          dateRequest: undefined
        });
      });
      this.getRequestAsset.assetList = [];
      this.getRequestAsset.workersList = [];
      this.typeRequest = typeRequest;
      this.isEdit = false;
      this.isLoading = false;
      this.active = true;
      this.modal.show();
    }
    else {
      this.isEdit = true;
      this._requestAssetBringServiceProxy.loadById(id).subscribe((result) => {
        this.isDraft = result.statusDraft;
        this.typeRequest = result.typeRequest;
        this.typeRequestName = result.typeRequest== 1 ? this.l(AppConsts.TypeRequestName.INTERNALREQUEST) : result.typeRequest== 2 ? this.l(AppConsts.TypeRequestName.EMPLOYEESVENDERREQUEST) :  result.typeRequest== 4 ? this.l(AppConsts.TypeRequestName.CLIENTREQUEST) :  this.l(AppConsts.TypeRequestName.ADDASSETVENDER);
        this.buildForm(this.typeRequest);
        this.getRequestAsset = result;
        this.requestAssetForm.patchValue(result);
        this.requestAssetForm.patchValue({
          assetList: result.assetList,
          workersList: result.workersList,
          dateRequest: result.dateRequest ? new Date(result.dateRequest?.toString()) : new Date()
        });
        this.active = true;
        this.modal.show();
        this.isLoading = false;
      });
    }
  }

  //edit and send mail
showEditCancel(id)
{   
    this.isSendMail=1;
    this.isLoading = true;
      this.isEdit = true;
      this._requestAssetBringServiceProxy.loadById(id).subscribe((result) => {
        this.isDraft = result.statusDraft;
        this.typeRequest = result.typeRequest;
        // alert(this.typeRequest);
        this.typeRequestName = result.typeRequest== 1 ? this.l(AppConsts.TypeRequestName.INTERNALREQUEST) : result.typeRequest== 2 ? this.l(AppConsts.TypeRequestName.EMPLOYEESVENDERREQUEST) :  result.typeRequest== 4 ? this.l(AppConsts.TypeRequestName.CLIENTREQUEST) :  this.l(AppConsts.TypeRequestName.ADDASSETVENDER);
        this.buildForm(this.typeRequest);
        this.getRequestAsset = result;
        this.requestAssetForm.patchValue(result);
        this.requestAssetForm.patchValue({
          assetList: result.assetList,
          workersList: result.workersList,
          dateRequest: result.dateRequest ? new Date(result.dateRequest?.toString()) : new Date()
        });
        this.active = true;
        this.modal.show();
        this.isLoading = false;
      });
}

  showOnUpload(requestImport){
    this.getRequestAsset = requestImport;
    this.requestAssetForm.patchValue(requestImport);
    this.getUser();
    this.requestAssetForm.patchValue({
      assetList: requestImport.assetList,
      workersList: requestImport.workersList,
      dateRequest: requestImport.dateRequest ? new Date(requestImport.dateRequest?.toString()) : new Date()
    });
    
  }

  buildForm(typeRequest?: number): void {
    if (typeRequest == 2) {
      this.requestAssetForm = this.formBuilder.group({
        id: [0],
        title: [undefined, GlobalValidator.required],
        // dateRequest: [undefined, GlobalValidator.required],
        description: [undefined],
        dateAdminApproval: [undefined],
        dateManageApproval: [undefined],
        department: [undefined],
        userName: [undefined],
        venderId: [undefined, GlobalValidator.required],
        liveMonitorName: [undefined, GlobalValidator.required],
        liveMonitorDepartment: [undefined, GlobalValidator.required],
        whereToBring: [undefined, GlobalValidator.required],
        liveMonitorPhoneNumber: [undefined],
        typeRequestName: [undefined],
        personInChargeOfSubName: [undefined, GlobalValidator.required],
        personInChangeOfSubPhone: [undefined],
        assetList: this.formBuilder.array([]),
        workersList: this.formBuilder.array([])
      });
      return;
    }
    if (typeRequest == 3) {
      this.isInternal = false;
      this.requestAssetForm = this.formBuilder.group({
        id: [0],
        title: [undefined, GlobalValidator.required],
        // dateRequest: [undefined, GlobalValidator.required],
        description: [undefined],
        dateAdminApproval: [undefined],
        dateManageApproval: [undefined],
        department: [undefined],
        userName: [undefined],
        venderId: [undefined, GlobalValidator.required],
        liveMonitorName: [undefined, GlobalValidator.required],
        liveMonitorDepartment: [undefined, GlobalValidator.required],
        whereToBring: [undefined, GlobalValidator.required],
        liveMonitorPhoneNumber: [undefined],
        typeRequestName: [undefined],
        personInChargeOfSubName: [undefined, GlobalValidator.required],
        personInChangeOfSubPhone: [undefined],
        assetList: this.formBuilder.array([]),
        workersList: this.formBuilder.array([])
      });
      return;
    }
    if (typeRequest == 4) {
      this.requestAssetForm = this.formBuilder.group({
        id: [0],
        title: [undefined, GlobalValidator.required],
        // dateRequest: [undefined, GlobalValidator.required],
        tradeUnionOrganization: [undefined, GlobalValidator.required],
        departmentClient: [undefined, GlobalValidator.required],
        description: [undefined],
        dateAdminApproval: [undefined],
        dateManageApproval: [undefined],
        department: [undefined],
        userName: [undefined],
        typeRequestName: [undefined],
        personInChargeOfSubName: [undefined, GlobalValidator.required],
        personInChangeOfSubPhone: [undefined],
        assetList: this.formBuilder.array([]),
        workersList: this.formBuilder.array([])
      });
      return;
    }
    this.isInternal = true;
    this.requestAssetForm = this.formBuilder.group({
      id: [0],
      title: [undefined, GlobalValidator.required],
      // dateRequest: [undefined, GlobalValidator.required],
      description: [undefined],
      dateAdminApproval: [undefined],
      dateManageApproval: [undefined],
      department: [undefined],
      userName: [undefined],
      venderId: [undefined],
      typeRequestName: [undefined],
      personInChargeOfSubName: [undefined, GlobalValidator.required],
      personInChangeOfSubPhone: [undefined],
      whereToBring: [undefined],
      assetList: this.formBuilder.array([]),
      workersList: this.formBuilder.array([])
    });
  }

  save(_statusDraft?: number): void {
    // this.requestAssetList = this.requestAssetForm.value;
    this.requestAssetBringSave = this.requestAssetForm.value;
    this.requestAssetBringSave.statusDraft = _statusDraft;
    this.requestAssetBringSave.typeRequest = this.typeRequest;
    this.requestAssetBringSave.status = this.getRequestAsset.status;
    this.requestAssetBringSave.requestCode = this.getRequestAsset.requestCode;
    if ((this.typeRequest == 3 || this.typeRequest == 1) && this.requestAssetBringSave.assetList.length == 0) {
      // alert(this.l(AppConsts.message.youDontAddAsset));
      this.toastService.openWarningToast(this.l(AppConsts.message.youDontAddAsset));
      return;
    }
    if (this.typeRequest == 2 && this.requestAssetBringSave.workersList.length == 0) {
      // alert(this.l(AppConsts.message.youDontAddWorker));
      this.toastService.openWarningToast(this.l(AppConsts.message.youDontAddWorker));
      return;
    }
    if (this.typeRequest == 4 && this.requestAssetBringSave.workersList.length == 0) {
      // alert(this.l(AppConsts.message.youDontAddClient));
      this.toastService.openWarningToast(this.l(AppConsts.message.youDontAddClient));
      return;
    }
    if(this.isSendMail)
    {
      this._requestAssetBringServiceProxy.sendMailToCancelRequest(this.requestAssetBringSave.id);
    }

    if(this.isInternal)
    {
      const found = (element) => element.dateEnd == undefined && !element.aviationIsBack;
      if(this.requestAssetBringSave.assetList.some(found))
      {
        return;
      }
    }

      this.isLoading = true;
      this.saving = true;
      this._requestAssetBringServiceProxy.save(this.requestAssetBringSave)
      .subscribe(() => {
        this.isSendMail=0;
        this.notify.info(this.l(AppConsts.message.saveSuccess));
        this.close();
        this.modalSave.emit(null);
        this.requestAssetBringSave = new RequestAssetBringSaveDto();
        this.saving = false;
        this.isLoading = false;
      });
  }

  getUser() {
    this._requestAssetBringServiceProxy.loadUserForCreate().subscribe((result) => {
      this.userName = result.userName;
      this.department = result.department;
    });
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }

  onUploadExcelError(): void {
    this.notify.error(this.l('ImportFailed'));
  }

  uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    const file = data.files[0];
    var number1=0;
    formData.append('file', file, file.name);
    // formData.append("number1", "123");
    
    if(this.typeRequest==2)
    {
      this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportRequestVenderEmployeesFromExcel';
    }
    else if (this.typeRequest==3)
    {
      this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportRequestVenderAssetFromExcel';
    }
    else if (this.typeRequest==4)
    {
      this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportRequestClientFromExcel';
    }

    // this.message1 = this.l('HeaderVal');
    this._httpClient
        .post<any>(this.uploadUrl, formData)
        .pipe(finalize(() => this.uploadRequestInfor.clear()))
        .subscribe(response => {
            if (response.success) {
                // this.TemEmployeesLearnedSafetyComponent.getTemEmployeesLearned(response.result.report);
                this.showOnUpload(response.result.report);
                // this.notify.success(this.l('ImportSuccess'));
                if(response.result.report.statusWarning)
                {
                  this.toastService.openSuccessToast(this.l('ImportSuccess'));
                }
                else
                {
                  this.message1= response.result.report.errorImportList;
                  this.toastService.openWarningToast(this.l('ImportWarning'));
                  this.messageImportError.open(0);
                }
            } 
            else if (response.error != null) {
                this.toastService.openFailToast(this.l('ImportFailed'));
            }
        });
  }

  saveByIdentity(input)
  {
    if(input=='close')
    {
      this.statusSaveIdentity=true;
    }
    else
    {
      this.statusSaveIdentity=false;
    }
  }

}
