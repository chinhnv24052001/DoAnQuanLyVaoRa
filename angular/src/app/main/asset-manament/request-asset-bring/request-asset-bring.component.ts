import { AfterViewInit, ChangeDetectorRef, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, ActivationEnd, Params, Router, RouterEvent } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestAssetBringInputDto, RequestAssetBringServiceProxy, StatusRequestDto } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditRequestAssetBringComponent } from './create-or-edit-request-asset-bring/create-or-edit-request-asset-bring.component';
import { DetailRequestAssetBringComponent } from './detail-request-asset-bring/detail-request-asset-bring.component';
import {HttpClient} from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import { FileUpload } from 'primeng/fileupload';
import { ToastService } from '@shared/common/ui/toast.service';
@Component({
  selector: 'app-request-asset-bring',
  templateUrl: './request-asset-bring.component.html',
  styleUrls: ['./request-asset-bring.component.less'],
  animations: [appModuleAnimation()]
})
export class RequestAssetBringComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditRequestAssetBring', { static: true }) CreateOrEditRequestAssetBring: CreateOrEditRequestAssetBringComponent;
  @ViewChild('DetailRequestAssetBring', { static: true }) DetailRequestAssetBring: DetailRequestAssetBringComponent;
  @ViewChild('UploadRequestInfor', { static: false }) uploadRequestInfor: FileUpload;

  isLoading: boolean = false;
  // title_request: string='';
  indexShort: number = 0;
  // requestCode: string='';
  // status: string='';
  searchRequestAssetForm: FormGroup;
  admReJect;
  mnReject;
  temMnReject;
  requestDraft;
  managerRQ;
  admRQ;
  requestAsset: RequestAssetBringInputDto = new RequestAssetBringInputDto();
  statusRequests: { value: number, label: string }[];
  typeRequests: { value: number, label: string }[];
  params!: Params;
  tabKey;
  titleRequest="";
  isEdit_Delete;
  isDetail: boolean=true;
  active: boolean=true;
  uploadUrl: string;
  userId: number =0;
  constructor(
    injector: Injector,
    private formBuilder: FormBuilder,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
    private route: ActivatedRoute,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private _httpClient: HttpClient,
    private toastService: ToastService,

  ) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportRequestFromExcel';
    router.events.subscribe((event: RouterEvent) => {
      if (event instanceof ActivationEnd) {
        this.isLoading = true;
        this.requestAsset.tabKey = this.route.snapshot.paramMap.get('st_request');
        this.titleRequest=this.l(this.requestAsset.tabKey);
        if(this.requestAsset.tabKey==AppConsts.StatusRequestMenu.U_WAITTINGME||this.requestAsset.tabKey==AppConsts.StatusRequestMenu.U_APPROVEDBYME||this.requestAsset.tabKey==AppConsts.StatusRequestMenu.U_REJECTEDBYME)
        {
          this.isEdit_Delete=false;
        }
        else this.isEdit_Delete=true;
        if (this.requestAsset.tabKey==AppConsts.StatusRequestMenu.IS_DRAFT)
        {
          this.isDetail=false;
        }
        else this.isDetail=true;
       
        // if(this.requestAsset.tabKey==AppConsts.KeyStatus.STATUS_CREATE_REQUEST) 
        // {
        //   this.titleRequest=this.l(AppConsts.TitleRequest.waitRequest);
        // }
        // else if (this.requestAsset.tabKey==AppConsts.KeyStatus.STATUS_MANAGER_APPROVE)
        // {
        //   this.titleRequest=this.l(AppConsts.requestAsset.managerApprovaled);
        // }
        // else if (this.requestAsset.tabKey==AppConsts.KeyStatus.STATUS_MANAGER_REJECT)
        // {
        //   this.titleRequest=this.l(AppConsts.requestAsset.managerRejected);
        // }
        // else if (this.requestAsset.tabKey==AppConsts.KeyStatus.STATUS_ADM_APPROVE)
        // {
        //   this.titleRequest=this.l(AppConsts.requestAsset.adminApprovaled);
        // }
        // else 
        // {
        //   this.titleRequest=this.l(AppConsts.requestAsset.adminRejected);
        // }
        this.paginator.rows=10;
        this.getRequestTem();
      }
    });
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
    this.changeDetectorRef.detectChanges();
  }

  ngOnInit() {
    this.requestAsset.tabKey = this.route.snapshot.paramMap.get('st_request');
    this.titleRequest=this.l(this.requestAsset.tabKey) ;
    if(this.requestAsset.tabKey==AppConsts.StatusRequestMenu.U_WAITTINGME||this.requestAsset.tabKey==AppConsts.StatusRequestMenu.U_APPROVEDBYME||this.requestAsset.tabKey==AppConsts.StatusRequestMenu.U_REJECTEDBYME)
    {
      this.isEdit_Delete=false;
    }
    else this.isEdit_Delete=true;
    if (this.requestAsset.tabKey==AppConsts.StatusRequestMenu.IS_DRAFT)
    {
      this.isDetail=false;
    }
    else this.isDetail=true;
    this.admReJect = AppConsts.KeyStatus.STATUS_ADM_REJECT;
    this.mnReject = AppConsts.KeyStatus.STATUS_MANAGER_REJECT;
    this.temMnReject =AppConsts.KeyStatus.STATUS_TEM_MANAGER_REJECT;
    this.requestDraft= AppConsts.KeyStatus.STATUS_CREATE_DRAFT;
    this.managerRQ= AppConsts.KeyStatus.STATUS_MANAGER_REQUEST_INFO;
    this.admRQ= AppConsts.KeyStatus.STATUS_ADM_REQUEST_INFO;
    var id = this.route.snapshot.paramMap.get("id");
    if (id != "0") {
      this.viewDetail(id);
    }
    this.getStatusRequest();
    this.getTypeRequest();
    this.buildFormRequestAsset();

    this._requestAssetBringServiceProxy.getUserLoginId().subscribe((result) =>{
      this.userId = result;
      // console.log("Day la " +this.userId);
    })
  }

  viewDetail(id) {
    this.DetailRequestAssetBring.viewDetail(id);
  }

  edit(id) {
    this.CreateOrEditRequestAssetBring.show(id);
  }

  editCancel(id) {
    this.CreateOrEditRequestAssetBring.showEditCancel(id);
  }

  create(typeRequest: number) {
    this.CreateOrEditRequestAssetBring.show(0, typeRequest);
  }

  delete(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._requestAssetBringServiceProxy.deleteById(id).subscribe(() => {
          this.getRequestAssetBring();
          this.notify.success(this.l(AppConsts.message.deleteSuccess));
        });
      }
    });
  }

  //delete and saen mail
  deleteCancel(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._requestAssetBringServiceProxy.sendMailToCancelRequest(id).subscribe(() => {
          this._requestAssetBringServiceProxy.deleteById(id).subscribe(() => {
            this.getRequestAssetBring();
            this.notify.success(this.l(AppConsts.message.deleteSuccess));
          });
        });
      }
    });
  }

  buildFormRequestAsset(): void {
    this.searchRequestAssetForm = this.formBuilder.group({
      TitleRequest: [undefined],
      RequestCode: [undefined],
      DateRequest: [undefined],
      TypeRequest: [undefined],
      Status: [undefined]
    });
  }

  getStatusRequest() {
    this._requestAssetBringServiceProxy.getStatusRequest().subscribe((result) => {
      this.statusRequests = [];
      this.statusRequests.push({ value: 0, label: "" });
      result.forEach(ele => {
        this.statusRequests.push({ value: ele.id, label: ele.statusRequestName });
      });
    });
  }

  getTypeRequest() {
      this.typeRequests = [];
      this.typeRequests.push({ value: 0, label: '' });
      this.typeRequests.push({ value: 1, label: this.l(AppConsts.TypeRequestName.INTERNAL) });
      this.typeRequests.push({ value: 2, label: this.l(AppConsts.TypeRequestName.EMPLOYEESVENDER) });
      this.typeRequests.push({ value: 3, label: this.l(AppConsts.TypeRequestName.ASSETVENDER) });
      this.typeRequests.push({ value: 4, label: this.l(AppConsts.TypeRequestName.CLIENTREQUEST) });
  }

  getRequestTem(){
    this.requestAsset.maxResultCount = 10;
    this.requestAsset.skipCount = 0;
    this._requestAssetBringServiceProxy.loadAll(this.requestAsset).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.requestAsset.skipCount;
      this.isLoading = false;
      this.active=false;
    });
  }

  getRequestAssetBring(event?: LazyLoadEvent) {
    this.primengTableHelper.showLoadingIndicator();
    this.requestAsset.dateRequest = this.searchRequestAssetForm.get('DateRequest').value;
    this.requestAsset.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 10;
    this.requestAsset.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._requestAssetBringServiceProxy.loadAll(this.requestAsset).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.requestAsset.skipCount;
      this.isLoading = false;
      this.active=false;
    });
  }

  onUploadExcelError(): void {
    this.notify.error(this.l('ImportFailed'));
  }

  uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    const file = data.files[0];
    var number1=0;
    formData.append('file', file, file.name);
    // formData.append(number1, this.typeRequest);
    this._httpClient
        .post<any>(this.uploadUrl, formData)
        .pipe(finalize(() => this.uploadRequestInfor.clear()))
        .subscribe(response => {
            if (response.success) {
                // this.TemEmployeesLearnedSafetyComponent.getTemEmployeesLearned(response.result.report);
                this.CreateOrEditRequestAssetBring.showOnUpload(response.result.report);
                // this.notify.success(this.l('ImportSuccess'));
                this.toastService.openSuccessToast(this.l('ImportSuccess'));
            } 
            else if (response.error != null) {
                this.toastService.openFailToast(this.l('ImportFailed'));
            }
        });
  }
}
