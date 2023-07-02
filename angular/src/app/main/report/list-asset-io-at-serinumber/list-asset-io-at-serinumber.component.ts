import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { ReportServiceProxy, VenderInputDto, VenderServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  selector: 'ListAssetInOutAtSeriNumber',
  templateUrl: './list-asset-io-at-serinumber.component.html',
  styleUrls: ['./list-asset-io-at-serinumber.component.less'],
  animations: [appModuleAnimation()]
})
export class ListAssetInOutAtSeriNumber extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  isLoading: boolean = false;
  maxResultCount;
  skipCount;
  seriNumberinout: string='';
  dateInOut;
  indexShort;
  // dateInOut;
  // sagdsa: DateTimeInputDto = new DateTimeInputDto();
  constructor(
    injector: Injector,
    private _reportServiceProxy: ReportServiceProxy,
    private _fileDownloadService: FileDownloadService,
  ) {
    super(injector);

  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  ngOnInit() {
  }

  getAssetInOutAtSeriNumber(event?: LazyLoadEvent) {
    this.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._reportServiceProxy.getAssetInOutAtSeriNumber(this.seriNumberinout, this.dateInOut, null, undefined, undefined, null, this.maxResultCount, this.skipCount).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      console.log(result);
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      console.log(this.primengTableHelper);
      this.indexShort = this.skipCount;
    });
    this.isLoading = false;
  }



  exportToExcelAuditLogs(): void {
    const self = this;
    self._reportServiceProxy.getAssetInOutAtSeriNumberToExcel(
      this.seriNumberinout, this.dateInOut, null, undefined, undefined, null, this.maxResultCount, this.skipCount)
        .subscribe(result => {
            self._fileDownloadService.downloadTempFile(result);
        });
}
}
