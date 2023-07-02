import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { CourceSafetyInputDto, CourceSafetyServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { CreateOrEditCourceSafetyComponent } from './create-or-edit-cource-safety/create-or-edit-cource-safety.component';
import { EmployeesLearnedSafetyComponent } from './employees-learned-safety/employees-learned-safety.component';

@Component({
  selector: 'app-cource-safety',
  templateUrl: './cource-safety.component.html',
  styleUrls: ['./cource-safety.component.less'],
  animations: [appModuleAnimation()]
})
export class CourceSafetyComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditCourceSafety', { static: true }) CreateOrEditCourceSafety: CreateOrEditCourceSafetyComponent;
  @ViewChild('EmployeesLearnedSafety', { static: true }) EmployeesLearnedSafety: EmployeesLearnedSafetyComponent;

  courceName: string = '';
  courceSafety: CourceSafetyInputDto = new CourceSafetyInputDto();
  isLoading: boolean = false;
  indexShort: number=0;
  constructor(
    injector: Injector,
    private _courceSafetyService: CourceSafetyServiceProxy,
    private _activatedRoute: ActivatedRoute
  ) {
    super(injector);
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }


  create() {
    this.CreateOrEditCourceSafety.show(0);
  }

  edit(id) {
    this.CreateOrEditCourceSafety.show(id);
  }

  view(record) {
    this.EmployeesLearnedSafety.showEmployeesLearned(record);
  }

  delete(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._courceSafetyService.deleteById(id).subscribe(() => {
          this.primengTableHelper.showLoadingIndicator();
          this.getCourceSafety();
          this.notify.success(this.l(AppConsts.message.deleteSuccess));
        });
      }
    });
  }

  getCourceSafety(event?: LazyLoadEvent) {
    this.courceSafety.courceName = this.courceName;
    this.courceSafety.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.courceSafety.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._courceSafetyService.loadAll(this.courceSafety).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort=this.courceSafety.skipCount;
    });
    this.isLoading = false;
  }
}
