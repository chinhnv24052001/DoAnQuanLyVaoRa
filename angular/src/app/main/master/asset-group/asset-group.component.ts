import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { AssetGroupServiceProxy, GetAssetGroupInputDto } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { CreateOrEditAssetGroupComponent } from './create-or-edit-asset-group/create-or-edit-asset-group.component';
import { AppConsts } from '@shared/AppConsts';

@Component({
  selector: 'app-asset-group',
  templateUrl: './asset-group.component.html',
  styleUrls: ['./asset-group.component.less'],
  animations: [appModuleAnimation()]
})
export class AssetGroupComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditAssetGroup', { static: true }) CreateOrEditAssetGroup: CreateOrEditAssetGroupComponent;
  filterText: string = '';
  isLoading: boolean = false;
  indexShort: number=0;
  assetGroup: GetAssetGroupInputDto = new GetAssetGroupInputDto();
  constructor(
    injector: Injector,
    private _assetGroupService: AssetGroupServiceProxy,
    private _activatedRoute: ActivatedRoute
  ) {
    super(injector);
    this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  create() {
    this.CreateOrEditAssetGroup.show(0);
  }

  edit(id) {
    this.CreateOrEditAssetGroup.show(id);
  }

  delete(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._assetGroupService.deleteById(id).subscribe(() => {
          this.getAssetGroup();
          this.notify.success(this.l(AppConsts.message.deleteSuccess));
        });
      }
    });
  }

  getAssetGroup(event?: LazyLoadEvent) {
    this.primengTableHelper.showLoadingIndicator();
    this.assetGroup.filter = this.filterText;
    this.assetGroup.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.assetGroup.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._assetGroupService.loadAll(this.assetGroup).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort=this.assetGroup.skipCount;
    });
    this.isLoading = false;
  }
}
