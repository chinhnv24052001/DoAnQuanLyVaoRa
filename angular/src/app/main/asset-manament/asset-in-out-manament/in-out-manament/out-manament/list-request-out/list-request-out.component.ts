import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute, ActivationEnd, Router, RouterEvent } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssetInOutManamentServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-list-request-out',
  templateUrl: './list-request-out.component.html',
  styleUrls: ['./list-request-out.component.less']
})
export class ListRequestOutComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  isLoading: boolean=false;
  @Output() modalCallBack: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalCallBackApproval: EventEmitter<any> = new EventEmitter<any>();
  @Input() isIn;
  @Input() requestAssetInOutInputDto;
  isTrue: boolean=true;
  indexShort: number = 0;
  selectedCar;
  constructor(injector: Injector, 
     private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy,
     private route: ActivatedRoute,
     private router: Router,
    ) {
    super(injector);
    
  }
  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  ngOnInit(): void {
    this.isTrue=true;
    this.requestAssetInOutInputDto.maxResultCount=10;
  }


  getRequestOut(event?: LazyLoadEvent) {
    this.isLoading=true;
    this.primengTableHelper.showLoadingIndicator();
    this.requestAssetInOutInputDto.isRequestAssetIn=this.isIn;
    this.requestAssetInOutInputDto.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 10;
    this.requestAssetInOutInputDto.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._assetInOutManamentServiceProxy.loadAllRequestAssetInOut(this.requestAssetInOutInputDto).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.requestAssetInOutInputDto.skipCount;
      this.isTrue=true;
    });
    this.isLoading=false;
  }

  bringModal(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading=true;
        this._assetInOutManamentServiceProxy.checkInOutRequest(id, this.isIn, '').subscribe(() => {
          this.getRequestOut();
          this.modalCallBackApproval.emit(null);
          if(this.isIn)
          {
            this.notify.success(this.l(AppConsts.message.checkOutSuccess));
          }
          else 
          {
            this.notify.success(this.l(AppConsts.message.checkInSuccess));
          }
        });
      }
    });
  }

  OnRowSingleClick(event?: LazyLoadEvent){
    this.modalCallBack.emit(this.selectedCar);
  }
}
