import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ToastService } from '@shared/common/ui/toast.service';
import { AssetInOutManamentServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';
import { DetailHistoryAssetInOutComponent } from '../../request-asset-bring/detail-request-asset-bring/detail-history-asset-io/detail-history-asset-io.component';
import { ReasonOfApprovalInOutComponent } from '../reason-of-approval-inout/reason-of-approval-inout.component';

@Component({
  selector: 'app-list-asset-in-out',
  templateUrl: './list-asset-in-out.component.html',
  styleUrls: ['./list-asset-in-out.component.less']
})
export class ListAssetInOutComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('DetailHistoryAssetInOut', { static: true }) DetailHistoryAssetInOut: DetailHistoryAssetInOutComponent;
  @Output() modalCallBack: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalCall: EventEmitter<any> = new EventEmitter<any>();
  isLoading: boolean=false;
  @Input() isIn;
  @Input() requestAssetInOutInputDto;
  @Input() typeRequest;
  @ViewChild('ReasonOfApprovalInOut', { static: true }) ReasonOfApprovalInOut: ReasonOfApprovalInOutComponent;
  isTrue: boolean=true;
  indexShort: number = 0;
  request;
  totalCount: number=0;
  selectedAsset;
  assetId: number = 0;
  constructor(injector: Injector, private toastService: ToastService,  private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy) {
    super(injector);
    
  }
  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  ngOnInit(): void {
    this.isTrue=true;
  }

  getAssetInOut(event?: LazyLoadEvent) {
    this.isLoading=true;
    this.primengTableHelper.showLoadingIndicator();
    this.requestAssetInOutInputDto.isRequestAssetIn=this.isIn;
    this.requestAssetInOutInputDto.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.requestAssetInOutInputDto.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._assetInOutManamentServiceProxy.loadAllAssetInOut(this.requestAssetInOutInputDto).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.totalCount=this.primengTableHelper.totalRecordsCount = result.totalCount;
      
      if(this.primengTableHelper.totalRecordsCount==0)
    {
      document.getElementById("assetTable").style.display = "none";
      this.modalCall.emit(null);
    }
    else  document.getElementById("assetTable").style.display = "block";
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.requestAssetInOutInputDto.skipCount;
      this.isTrue=true;
    });
    this.isLoading=false;
  }

  bringModal(id, stringInOut) {
    this.assetId=id;
    this._assetInOutManamentServiceProxy.checkEffectiveDateAssetInOut(id).subscribe((result) => {
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

  callBackReasonOfApproval(reasonDto)
  {
    this.confirmBringModal(reasonDto.id, reasonDto.noteId, reasonDto.stringInOut);
  }

  confirmBringModal(id, noteId, stringInOut)
  {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading=true;
        this._assetInOutManamentServiceProxy.checkInOutAsset(id, noteId, stringInOut).subscribe((result) => {
          this.requestAssetInOutInputDto.requestId=result;
          this.getAssetInOut();
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

  OnRowSingleClick(event?: LazyLoadEvent){
    this.DetailHistoryAssetInOut.show(this.selectedAsset.id, AppConsts.message.historyAsset);
  }

  closeEmpty(event){
    //dont delete
  }
}
