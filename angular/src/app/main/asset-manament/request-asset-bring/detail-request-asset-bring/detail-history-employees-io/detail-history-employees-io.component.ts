import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { CourceSafetySelectOutputDto, EmployeesLearnedSafetyInputDto, EmployeesLearnedSafetyServiceProxy, HistoryAssetDetailInputDto, HistoryInOutServiceProxy, HistoryWorkerDetailInputDto, TemEmployeesLearnedSafetyServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppConsts } from '@shared/AppConsts';

@Component({
  selector: 'DetailHistoryEmployeesInOut',
  templateUrl: './detail-history-employees-io.component.html',
  styleUrls: ['./detail-history-employees-io.component.less'],
  animations: [appModuleAnimation()]
})
export class DetailHistoryEmployeesInOutComponent extends AppComponentBase implements AfterViewInit {
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
  historyWorkerDetailInputDto: HistoryWorkerDetailInputDto = new HistoryWorkerDetailInputDto();
  
  constructor(
    injector: Injector,
    private _historyInOutServiceProxy: HistoryInOutServiceProxy,
  ) {
    super(injector);
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  // show(id, typeHistory): void {

  //   var x = document.querySelectorAll<HTMLElement>('.dv-asset')
  //   var y = document.querySelectorAll<HTMLElement>('.dv-employees')
  //     if(this.typeRequest==2 || this.typeRequest==4)
  //     {
  //       this.WorkerDetailHistoryInOut.show(id, typeHistory);
        
  //         // (y[0] as HTMLElement).style.display = "block ";
  //         //  (x[0] as HTMLElement).style.display = "none";
          
  //     document.getElementById("table_workerHistory").style.display = "block";
  //      document.getElementById("table_assetHistory").style.display = "none";
  //     }
  //     else{
  //       this.AssetDetailHistoryInOut.show(id, typeHistory);
        
  //       //  (x[0] as HTMLElement).style.display = "block";
  //       //  (y[0] as HTMLElement).style.display = "none";

  //      document.getElementById("table_assetHistory").style.display = "block";
  //      document.getElementById("table_workerHistory").style.display = "none";
  //     }
  //    this.active = true;
  //    this.modal.show();
  // }


  show(id, typeRequest): void {
    if (typeRequest==AppConsts.message.historyRequest)
    {
      this.historyWorkerDetailInputDto.requestId=id;
    }
    else 
    {
      this.historyWorkerDetailInputDto.workerIOId=id;
    }
    this.getWorkerHistoryDetail();
    this.active = true;
    this.modal.show();
  }

  getWorkerHistoryDetail(event?: LazyLoadEvent) {
    this.historyWorkerDetailInputDto.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.historyWorkerDetailInputDto.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._historyInOutServiceProxy.loadAllHistoryWorkerDetail(this.historyWorkerDetailInputDto).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort=this.historyWorkerDetailInputDto.skipCount;
    });
  }

  close(): void {
    this.active = false;
    this.modal.hide();
    this.modalReloadLoadDetail.emit(this.requestId);
  }
}
