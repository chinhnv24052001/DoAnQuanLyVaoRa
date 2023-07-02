import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { VenderInputDto, VenderServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditVenderComponent } from './create-or-edit-vender/create-or-edit-vender.component';
import { AppConsts } from '@shared/AppConsts';

@Component({
  selector: 'app-vender',
  templateUrl: './vender.component.html',
  styleUrls: ['./vender.component.less'],
  animations: [appModuleAnimation()]
})
export class VenderComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditVender', { static: true }) CreateOrEditVender: CreateOrEditVenderComponent;
  filterText: string = '';
  vender: VenderInputDto = new VenderInputDto();
  isLoading: boolean = false;
  indexShort: number=0;
  constructor(
    injector: Injector,
    private _venderService: VenderServiceProxy,
    private _activatedRoute: ActivatedRoute
  ) {
    super(injector);
    this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  create() {
    // this.isLoading=true;
    this.CreateOrEditVender.show(0);
  }

  edit(id) {
    // this.isLoading=true;
    this.CreateOrEditVender.show(id);
  }

  delete(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._venderService.deleteById(id).subscribe(() => {
          this.primengTableHelper.showLoadingIndicator();
          this.getVender();
          this.notify.success(this.l(AppConsts.message.deleteSuccess));
        });
      }
    });
  }

  getVender(event?: LazyLoadEvent) {
    this.vender.filter = this.filterText;
    this.vender.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.vender.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._venderService.loadAll(this.vender).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort=this.vender.skipCount;
    });
    this.isLoading = false;
  }
}
