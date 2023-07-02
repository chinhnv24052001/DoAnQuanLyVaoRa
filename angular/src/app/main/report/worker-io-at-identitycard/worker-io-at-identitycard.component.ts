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
  selector: 'WorkerInOutAtIdentityCard',
  templateUrl: './worker-io-at-identitycard.component.html',
  styleUrls: ['./worker-io-at-identitycard.component.less'],
  animations: [appModuleAnimation()]
})
export class WorkerInOutAtIdentityCard extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  isLoading: boolean = false;
  maxResultCount;
  skipCount;
  identityCardinout: string='';
  indexShort;
  dateInOut;
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

  getWorkerInOutAtIdentityCard(event?: LazyLoadEvent) {
    this.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._reportServiceProxy.getWorkerInOutAtIdentityCard(this.identityCardinout, null, undefined, undefined, null, this.maxResultCount, this.skipCount).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.skipCount;
    });
    this.isLoading = false;
  }



  exportToExcelAuditLogs(): void {
    const self = this;
    self._reportServiceProxy.getWorkerInOutAtIdentityCardToExcel(
      this.identityCardinout, null, undefined, undefined, null, this.maxResultCount, this.skipCount)
        .subscribe(result => {
            self._fileDownloadService.downloadTempFile(result);
        });
}
}
