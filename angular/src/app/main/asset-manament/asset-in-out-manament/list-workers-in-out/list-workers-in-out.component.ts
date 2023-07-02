import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ToastService } from '@shared/common/ui/toast.service';
import { AssetInOutManamentServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';
import { DetailHistoryEmployeesInOutComponent } from '../../request-asset-bring/detail-request-asset-bring/detail-history-employees-io/detail-history-employees-io.component';
import { ReasonOfApprovalInOutComponent } from '../reason-of-approval-inout/reason-of-approval-inout.component';

@Component({
  selector: 'app-list-workers-in-out',
  templateUrl: './list-workers-in-out.component.html',
  styleUrls: ['./list-workers-in-out.component.less']
})
export class ListWorkersInOutComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('DetailHistoryEmployeesInOut', { static: true }) DetailHistoryEmployeesInOut: DetailHistoryEmployeesInOutComponent;
  @Output() modalCall: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalCallBack: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('ReasonOfApprovalInOut', { static: true }) ReasonOfApprovalInOut: ReasonOfApprovalInOutComponent;
  isLoading: boolean=false;
  @Input() isIn;
  @Input() requestAssetInOutInputDto;
  @Input() typeRequest;
  indexShort: number=0;
  totalCount: number=0;
  selectedWorker;
  constructor(injector: Injector, private toastService: ToastService,  private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy,) {
    super(injector);
  }
  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }
  ngOnInit(): void {

  }
  
  getEmployeesInOut(event?: LazyLoadEvent) {
    this.isLoading=true;
    this.primengTableHelper.showLoadingIndicator();
    this.requestAssetInOutInputDto.isRequestAssetIn=this.isIn;
    this.requestAssetInOutInputDto.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.requestAssetInOutInputDto.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._assetInOutManamentServiceProxy.loadAllEmployeesInOut(this.requestAssetInOutInputDto).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
     
      if(this.primengTableHelper.totalRecordsCount==0)
      {
        document.getElementById("employeesTable").style.display = "none";
        this.modalCall.emit(null);
      }
      else  document.getElementById("employeesTable").style.display = "block";
      this.totalCount=result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort=this.requestAssetInOutInputDto.skipCount;
    });
    this.isLoading=false;
  }

  callBackReasonOfApproval(reasonDto)
  {
    this.confirmBringModal(reasonDto.id, reasonDto.noteId, reasonDto.stringInOut);
  }

  confirmBringModal(id, noteId, stringInOut) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading=true;
        this._assetInOutManamentServiceProxy.checkInOutEmployees(id, noteId, stringInOut).subscribe((result) => {
          this.requestAssetInOutInputDto.requestId=result;
          this.getEmployeesInOut();
        
          if(this.isIn)
          {
            // this.notify.success(this.l(AppConsts.message.checkOutSuccess));
            this.toastService.openSuccessToast(this.l(AppConsts.message.checkOutSuccess));
          }
          else 
          {
            // this.notify.success(this.l(AppConsts.message.checkInSuccess));
            this.toastService.openSuccessToast(this.l(AppConsts.message.checkInSuccess));
          }
        });
      }
    });
  }

  bringModal(id, stringInOut) {
    this._assetInOutManamentServiceProxy.checkEffectiveDateEmployeesInOut(id).subscribe((result) => {
      if(!result)
      {
        this.ReasonOfApprovalInOut.show(id, stringInOut);
      }
      else 
      {
        this.confirmBringModal(id, 0, stringInOut);
      }
    });
  }

  OnRowSingleClick(event?: LazyLoadEvent){
    this.DetailHistoryEmployeesInOut.show(this.selectedWorker.id, AppConsts.message.historyEmployees);
  }

  closeEmpty(event){
    //Dont delete
  }
}
