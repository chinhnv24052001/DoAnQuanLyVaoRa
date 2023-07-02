import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { AssetServiceProxy, ReportServiceProxy, VenderInputDto, VenderServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  selector: 'AssetOutOfDate',
  templateUrl: './asset-out-of-date.component.html',
  styleUrls: ['./asset-out-of-date.component.less'],
  animations: [appModuleAnimation()]
})
export class AssetOutOfDateComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  _searchWorkerExport: FormGroup;
  isLoading: boolean = false;
  assetId: number=0;
  tagCode;
  seriNumber;
  requestCode;
  maxResultCount;
  skipCount;
  indexShort;
  searchByAssetName: string=this.l('SearchByAssetName');
  listAssetDropDown: { value: number, label: string }[];
  control: FormControl;
  // sagdsa: DateTimeInputDto = new DateTimeInputDto();
  constructor(
    injector: Injector,
    private _reportServiceProxy: ReportServiceProxy,
    private _assetServiceProxy: AssetServiceProxy,
    private _fileDownloadService: FileDownloadService
  ) {
    super(injector);
    
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  ngOnInit() {
    this.getAssetDropdown();
  }

  getAssetDropdown(){
    this._assetServiceProxy.getAssetDropDown().subscribe((result) => {
      this.listAssetDropDown = [];
      this.listAssetDropDown.push({ value: 0, label: " " });
      this.control = new FormControl(this.listAssetDropDown[0].value);
      result.forEach(ele => {
        this.listAssetDropDown.push({ value: ele.id, label: ele.assetName });
      });
    });
  }

  getAssetOutOfDate(event?: LazyLoadEvent) {
    this.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._reportServiceProxy.getAssetOutOfDate(this.assetId, this.tagCode, this.seriNumber, this.requestCode, this.maxResultCount, this.skipCount).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.skipCount;
    });
    this.isLoading = false;
  }

  exportToExcelAuditLogs(): void {
    const self = this;
    self._reportServiceProxy.getAssetOutOfDateToExcel(
      this.assetId, this.tagCode, this.seriNumber, this.requestCode, this.maxResultCount, this.skipCount)
        .subscribe(result => {
            self._fileDownloadService.downloadTempFile(result);
        });
}
}
