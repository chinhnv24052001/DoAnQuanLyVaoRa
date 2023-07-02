import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { AssetServiceProxy, MstAssetImportDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FormGroup } from '@angular/forms';
import { finalize } from 'rxjs/operators';


@Component({
  selector: 'tem-asset-import',
  templateUrl: './tem-asset-import.component.html',
  styleUrls: ['./tem-asset-import.component.less'],
  animations: [appModuleAnimation()]
})
export class TemAssetImportComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
  isLoading: boolean = false;
  employeesLearnedForm: FormGroup;
  totalTem: number = 0;
  mstAssetImportDto: MstAssetImportDto[] = [];
  isSaving;
  constructor(
    injector: Injector,
    private _assetServiceProxy: AssetServiceProxy,

  ) {
    super(injector);
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  getTemEmployeesLearned(listAssetImport) {
    this.isSaving = true;
    this.mstAssetImportDto = listAssetImport;
    this.primengTableHelper.totalRecordsCount = listAssetImport.length;
    this.primengTableHelper.records = listAssetImport;
    this.primengTableHelper.hideLoadingIndicator();
    this.modal.show();
    this.isLoading = false;
  }

  saveAll() {
    this.spinnerService.show();
    this._assetServiceProxy.saveAllImport(this.mstAssetImportDto)
      .pipe(finalize(() => {
        this.spinnerService.hide();
        this.modalClose.emit();
      }))
      .subscribe((result) => {
        if (result.length != 0) {
          this.getTemEmployeesLearned(result);
          this.isSaving = false;
          this.notify.error(this.l('SaveAllFailed'));
        }
        else {
          this.notify.success(this.l('SaveAllSuccess'));
          this.close();
        }
      });
  }

  close(): void {
    this.mstAssetImportDto = [];
    this.modal.hide();
  }
}
