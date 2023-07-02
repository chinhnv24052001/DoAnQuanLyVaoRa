import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { CourceSafetySelectOutputDto, EmployeesLearnedSafetyImportDto, EmployeesLearnedSafetyInputDto, EmployeesLearnedSafetyServiceProxy, TemEmployeesLearnedSafetyInputDto, TemEmployeesLearnedSafetyServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FormGroup } from '@angular/forms';


@Component({
  selector: 'TemEmployeesLearnedSafety',
  templateUrl: './tem-employees-learned-safety.component.html',
  styleUrls: ['./tem-employees-learned-safety.component.less'],
  animations: [appModuleAnimation()]
})
export class TemEmployeesLearnedSafetyComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @Output() modalOpen: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
  @Input() _record;  
  @Input() isSavingEmployeesLearned; 
  filterText: string = '';
  temEmployeesInput: TemEmployeesLearnedSafetyInputDto = new TemEmployeesLearnedSafetyInputDto();
  isLoading: boolean = false;
  record: CourceSafetySelectOutputDto = new CourceSafetySelectOutputDto();
  employeesLearnedForm: FormGroup;
  totalTem: number = 0;
  listEmployeesLearnedImport: EmployeesLearnedSafetyImportDto[] = [];
  constructor(
    injector: Injector,
    private _temEmployeesLearnedSafetyService: TemEmployeesLearnedSafetyServiceProxy,
  ) {
    super(injector);
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  getTemEmployeesLearned(listEmployeesLearnedImport) {
    this.listEmployeesLearnedImport = listEmployeesLearnedImport;
    this.primengTableHelper.totalRecordsCount = listEmployeesLearnedImport.length;
    this.primengTableHelper.records = listEmployeesLearnedImport;
    this.primengTableHelper.hideLoadingIndicator();
    this.modalOpen.emit(null);
    this.modal.show();
    this.isLoading = false;
  }

  saveAll() {
    this._temEmployeesLearnedSafetyService.saveAllImport(0, this.listEmployeesLearnedImport).subscribe((result) => {
      if (result.length!=0) {
        this.isSavingEmployeesLearned=false;
        this.getTemEmployeesLearned(result);
        this.notify.error(this.l('SaveAllFailed'));
      }
      else
      {
        this.notify.success(this.l('SaveAllSuccess'));
        this.close();
      } 
    });
  }

  close(): void {
    // this.modalClose.emit(this._record);
    this.listEmployeesLearnedImport = [];
    this.modal.hide();
    this.isSavingEmployeesLearned=true;
  }
}
