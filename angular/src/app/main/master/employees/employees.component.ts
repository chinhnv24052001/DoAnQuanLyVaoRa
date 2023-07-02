import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { CreateOrEditEmployeesComponent } from './create-or-edit-employees/create-or-edit-employees.component';
import { EmployeesInputDto, EmployeesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.less'],
  animations: [appModuleAnimation()]
})
export class EmloyeesComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditEmployees', { static: true }) CreateOrEditEmployees: CreateOrEditEmployeesComponent;
  filterText: string = '';
  venderId: number = 0;
  isLoading: boolean = false;
  employees: EmployeesInputDto = new EmployeesInputDto();
  listvenderDropDown: { value: number, label: string }[];
  constructor(
    injector: Injector,
    private _employeesService: EmployeesServiceProxy,
    private _activatedRoute: ActivatedRoute
  ) {
    super(injector);
    this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  create() {
    this.CreateOrEditEmployees.show(0);
  }

  edit(id) {
    this.CreateOrEditEmployees.show(id);
  }

  delete(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._employeesService.deleteById(id).subscribe(() => {
          this.getEmployees();
          this.notify.success(this.l(AppConsts.message.deleteSuccess));
        });
      }
    });
  }

  ngOnInit() {
    this.selectDropDownVender();
  }

  selectDropDownVender() {
    this._employeesService.getVenderForEdit().subscribe((result) => {
      this.listvenderDropDown = [];
      this.listvenderDropDown.push({ value: 0, label: "" });
      result.forEach(ele => {
        this.listvenderDropDown.push({ value: ele.id, label: ele.venderName });
      });
    });
  }

  getEmployees(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }
    this.primengTableHelper.showLoadingIndicator();
    this.employees.filter = this.filterText;
    this.employees.venderId = this.venderId;
    this.employees.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.employees.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._employeesService.loadAll(this.employees).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
    this.isLoading = false;
  }
}
