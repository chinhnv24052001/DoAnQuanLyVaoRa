import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { AssetInputDto, AssetServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { CreateOrEditAssetComponent } from './create-or-edit-asset/create-or-edit-asset.component';
import { AppConsts } from '@shared/AppConsts';
import {HttpClient} from '@angular/common/http';
import { FileUpload } from 'primeng/fileupload';
import { ToastService } from '@shared/common/ui/toast.service';
import { TemAssetImportComponent } from './tem-asset-import/tem-asset-import.component';

@Component({
  selector: 'app-asset',
  templateUrl: './asset.component.html',
  styleUrls: ['./asset.component.less'],
  animations: [appModuleAnimation()]
})
export class AssetComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditAsset', { static: true }) CreateOrEditAsset: CreateOrEditAssetComponent;
  @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
  @ViewChild('TemAssetImport', { static: true }) TemAssetImport: TemAssetImportComponent;
  filterText: string = '';
  tagCode: string = '';
  assetGroupId: number = 0;
  isLoading: boolean = false;
  indexShort: number = 0;
  asset: AssetInputDto = new AssetInputDto();
  uploadUrl: string;

  listAssetGroupsDropDown: { value: number, label: string }[];
  constructor(
    injector: Injector,
    private _assetService: AssetServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _httpClient: HttpClient,
    private toastService: ToastService,
  ) {
    super(injector);
    this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportAssetFromExcel';
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  create() {
    this.CreateOrEditAsset.show(0);
  }

  edit(id) {
    this.CreateOrEditAsset.show(id);
  }

  delete(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._assetService.deleteById(id).subscribe(() => {
          this.getAsset();
          this.notify.success(this.l(AppConsts.message.deleteSuccess));
        });
      }
    });
  }

  ngOnInit() {
    this.selectDropDownAssetGroups();

  }

  selectDropDownAssetGroups() {
    this._assetService.getAssetGroupForEdit().subscribe((result) => {
      this.listAssetGroupsDropDown = [];
      this.listAssetGroupsDropDown.push({ value: 0, label: " " });
      result.forEach(ele => {
        this.listAssetGroupsDropDown.push({ value: ele.id, label: ele.assetGroupName });
      });
    });
  }

  getAsset(event?: LazyLoadEvent) {
    this.primengTableHelper.showLoadingIndicator();
    this.asset.filter = this.filterText;
    this.asset.assetGroupId = this.assetGroupId;
    this.asset.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.asset.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._assetService.loadAll(this.asset).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort = this.asset.skipCount;
    });
    this.isLoading = false;
  }

  uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    const file = data.files[0];
    formData.append('file', file, file.name);
    // formData.append('check', "123");
    this.spinnerService.show();
    this._httpClient
      .post<any>(this.uploadUrl, formData)
      .pipe(finalize(() => this.excelFileUpload.clear()))
      .subscribe(response => {
        if (response.success) {
          this.TemAssetImport.getTemEmployeesLearned(response.result.report);
          this.toastService.openSuccessToast(this.l('ImportSuccess'));
        }
        else if (response.error != null) {
          this.toastService.openFailToast(this.l('ImportFailed'));
        }
        this.spinnerService.hide();
      });
  }

  onUploadExcelError(): void {
    this.toastService.openFailToast(this.l('ImportFailed'));
  }
}
