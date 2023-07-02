import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { CourceSafetySelectOutputDto, EmployeesLearnedSafetyInputDto, EmployeesLearnedSafetyServiceProxy, HistoryAssetDetailInputDto, HistoryInOutServiceProxy, TemEmployeesLearnedSafetyServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'DetailHistoryAssetInOut',
  templateUrl: './detail-history-asset-io.component.html',
  styleUrls: ['./detail-history-asset-io.component.less'],
  animations: [appModuleAnimation()]
})
export class DetailHistoryAssetInOutComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @Output() modalReloadLoadDetail: EventEmitter<any> = new EventEmitter<any>();
  @Input() assetCheckId;
  @Input() requestId; 
  @Input() typeRequest;
  isViewAsset;
  isViewEmployees;
  isLoading: boolean = false;
  active: boolean =false;
  indexShort: number=0;
  historyAssetDetailInputDto: HistoryAssetDetailInputDto = new HistoryAssetDetailInputDto();
 
  constructor(
    injector: Injector,
    private _historyInOutServiceProxy: HistoryInOutServiceProxy,
  ) {
    super(injector);
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  show(id, typeRequest): void {
    if(typeRequest==AppConsts.message.historyRequest)
    {
      this.historyAssetDetailInputDto.requestId=id;
    }
    else 
    {
      this.historyAssetDetailInputDto.assetIOId=id;
    }
    this.getAssetHistoryDetail();
    this.active = true;
    this.modal.show();
  }

  getAssetHistoryDetail(event?: LazyLoadEvent) {
    this.historyAssetDetailInputDto.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.historyAssetDetailInputDto.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._historyInOutServiceProxy.loadAllHistoryAssetDetail(this.historyAssetDetailInputDto).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort=this.historyAssetDetailInputDto.skipCount;
    });
  }


  close(): void {
    this.active = false;
    this.modal.hide();
    this.modalReloadLoadDetail.emit(this.requestId);
  }
}
