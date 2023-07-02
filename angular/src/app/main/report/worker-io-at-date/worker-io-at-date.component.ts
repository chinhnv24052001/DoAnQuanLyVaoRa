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
  selector: 'WorkerInOutAtDate',
  templateUrl: './worker-io-at-date.component.html',
  styleUrls: ['./worker-io-at-date.component.less'],
  animations: [appModuleAnimation()]
})
export class WorkerInOutAtDateComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  _searchWorkerExport: FormGroup;
  isLoading: boolean = false;
  maxResultCount;
  skipCount;
  dateInOut;
  indexShort;
  // sagdsa: DateTimeInputDto = new DateTimeInputDto();
  constructor(
    injector: Injector,
    private _reportServiceProxy: ReportServiceProxy,
    private _fileDownloadService: FileDownloadService,
    private formBuilder: FormBuilder
  ) {
    super(injector);
    
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  ngOnInit() {
    this.buildFormWorkerExport();
  }

  buildFormWorkerExport(): void {
    this._searchWorkerExport = this.formBuilder.group({
      DateInOut: [undefined]
    });
  }

  getWorkerInOutAtDate(event?: LazyLoadEvent) {
    this.dateInOut = this._searchWorkerExport.get('DateInOut').value;
    this.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._reportServiceProxy.getWorkerInOutAtDate(this.dateInOut, null, undefined, undefined, null, this.maxResultCount, this.skipCount).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.skipCount;
    });
    this.isLoading = false;
  }

  exportToExcelAuditLogs(): void {
    const self = this;
    self._reportServiceProxy.getWorkerInOutAtDateToExcel(
      this.dateInOut, null, undefined, undefined, null, this.maxResultCount, this.skipCount)
        .subscribe(result => {
            self._fileDownloadService.downloadTempFile(result);
        });
}
}
