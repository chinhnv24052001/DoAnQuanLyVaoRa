import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DetailHistoryAssetInOutComponent } from '@app/main/asset-manament/request-asset-bring/detail-request-asset-bring/detail-history-asset-io/detail-history-asset-io.component';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssetInOutManamentServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-list-asset-out',
  templateUrl: './list-asset-out.component.html',
  styleUrls: ['./list-asset-out.component.less']
})
export class ListAssetOutComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('DetailHistoryAssetInOut', { static: true }) DetailHistoryAssetInOut: DetailHistoryAssetInOutComponent;
  @Output() modalCallBack: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalCall: EventEmitter<any> = new EventEmitter<any>();
  @Output() callbackdomAssetInOut: EventEmitter<any> = new EventEmitter<any>();
  @Output() callbackdomApprovedAssetInOut: EventEmitter<any> = new EventEmitter<any>();
  isLoading: boolean = false;
  @Input() isIn;
  @Input() requestAssetInOutInputDto;
  @Input() typeRequest;
  isTrue: boolean = true;
  indexShort: number = 0;
  request;
  totalCount: number = 0;
  selectedAsset;
  isApproval: boolean = false;
  constructor(injector: Injector, private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy) {
    super(injector);

  }
  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  ngOnInit(): void {
    this.isTrue = true;
  }

  getAssetOut(event?: LazyLoadEvent) {
    this.isLoading = true;
    this.primengTableHelper.showLoadingIndicator();
    this.requestAssetInOutInputDto.isRequestAssetIn = this.isIn;
    this.requestAssetInOutInputDto.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.requestAssetInOutInputDto.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._assetInOutManamentServiceProxy.loadAllAssetInOut(this.requestAssetInOutInputDto).pipe(finalize(() => 
    {
    this.isLoading = false;
      this.isApproval=false;
      this.primengTableHelper.hideLoadingIndicator()
    }
   )).subscribe(result => {
      this.totalCount = this.primengTableHelper.totalRecordsCount = result.totalCount;
      if(this.isApproval)
      {
        if (this.primengTableHelper.totalRecordsCount == 0) {
          this.modalCall.emit(null);
        }
      this.callbackdomApprovedAssetInOut.emit(this.primengTableHelper.totalRecordsCount);
      }

      this.callbackdomAssetInOut.emit(this.primengTableHelper.totalRecordsCount);


      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.requestAssetInOutInputDto.skipCount;
      this.isTrue = true;
    });
  }

  bringModal(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._assetInOutManamentServiceProxy.checkInOutAsset(id, 0, '').subscribe((result) => {
          this.requestAssetInOutInputDto.requestId = result;
          this.isApproval=true;
          this.getAssetOut();

          if (this.isIn) {
            this.notify.success(this.l(AppConsts.message.checkOutSuccess));
          }
          else {
            this.notify.success(this.l(AppConsts.message.checkInSuccess));
          }
        });
      }
    });
  }

  OnRowSingleClick(event?: LazyLoadEvent) {
    this.DetailHistoryAssetInOut.show(this.selectedAsset.id, AppConsts.message.historyAsset);
  }

  closeEmpty(event) {

  }
}
