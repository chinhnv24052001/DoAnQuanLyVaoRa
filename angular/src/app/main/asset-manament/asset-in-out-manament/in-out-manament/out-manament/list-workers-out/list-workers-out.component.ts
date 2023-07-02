import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DetailHistoryEmployeesInOutComponent } from '@app/main/asset-manament/request-asset-bring/detail-request-asset-bring/detail-history-employees-io/detail-history-employees-io.component';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssetInOutManamentServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-list-workers-out',
  templateUrl: './list-workers-out.component.html',
  styleUrls: ['./list-workers-out.component.less']
})
export class ListWorkersOutComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('DetailHistoryEmployeesInOut', { static: true }) DetailHistoryEmployeesInOut: DetailHistoryEmployeesInOutComponent;
  @Output() modalCall: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalCallBack: EventEmitter<any> = new EventEmitter<any>();
  @Output() callbackdomEmployeesInOut: EventEmitter<any> = new EventEmitter<any>();
  @Output() callbackdomApprovedEmployeesInOut: EventEmitter<any> = new EventEmitter<any>();
  isLoading: boolean = false;
  @Input() isIn;
  @Input() requestAssetInOutInputDto;
  @Input() typeRequest;
  indexShort: number = 0;
  totalCount: number = 0;
  selectedWorker;
  isApproval: boolean = false;
  constructor(injector: Injector, private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy,) {
    super(injector);
  }
  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }
  ngOnInit(): void {

  }

  getEmployeesOut(event?: LazyLoadEvent) {
    this.isLoading = true;
    this.primengTableHelper.showLoadingIndicator();
    this.requestAssetInOutInputDto.isRequestAssetIn = this.isIn;
    this.requestAssetInOutInputDto.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.requestAssetInOutInputDto.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._assetInOutManamentServiceProxy.loadAllEmployeesInOut(this.requestAssetInOutInputDto).pipe(finalize(() => {
      this.isLoading = false;
      this.isApproval = false;
      this.primengTableHelper.hideLoadingIndicator();
    }
    )).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;

      if (this.isApproval) {
        if (this.primengTableHelper.totalRecordsCount == 0) {
          this.modalCall.emit(null);
        }
      this.callbackdomApprovedEmployeesInOut.emit(this.primengTableHelper.totalRecordsCount);
      }

      this.callbackdomEmployeesInOut.emit(this.primengTableHelper.totalRecordsCount);

      this.totalCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.requestAssetInOutInputDto.skipCount;
    });
  }

  bringModal(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._assetInOutManamentServiceProxy.checkInOutEmployees(id, 0, '').subscribe((result) => {
          this.requestAssetInOutInputDto.requestId = result;
          this.isApproval = true;
          this.getEmployeesOut();

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
    this.DetailHistoryEmployeesInOut.show(this.selectedWorker.id, AppConsts.message.historyEmployees);
  }

  closeEmpty(event) {
  }
}
