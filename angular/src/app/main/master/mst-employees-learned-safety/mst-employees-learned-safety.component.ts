import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { CourceSafetySelectOutputDto, EmployeesLearnedSafetyInputDto, EmployeesLearnedSafetyServiceProxy, TemEmployeesLearnedSafetyServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FormGroup } from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import { FileUpload } from 'primeng/fileupload';
import { ToastService } from '@shared/common/ui/toast.service';
import { CreateOrEditEmployeesLearnedSafetyComponent } from '../cource-safety/employees-learned-safety/create-or-edit-employees-learned-safety/create-or-edit-employees-learned-safety.component';
import { TemEmployeesLearnedSafetyComponent } from '../cource-safety/employees-learned-safety/tem-employees-learned-safety/tem-employees-learned-safety.component';

@Component({
  selector: 'MstEmployeesLearnedSafety',
  templateUrl: './mst-employees-learned-safety.component.html',
  styleUrls: ['./mst-employees-learned-safety.component.less'],
  animations: [appModuleAnimation()]
})
export class MstEmployeesLearnedSafetyComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
  @ViewChild('ExcelFileUploadCheck', { static: false }) excelFileUploadCheck: FileUpload;
  @ViewChild('UploadImageEmp', { static: false }) uploadImageEmp: FileUpload;
  @ViewChild('CreateOrEditEmployeesLearnedSafety', { static: true }) CreateOrEditEmployeesLearnedSafety: CreateOrEditEmployeesLearnedSafetyComponent;
  @ViewChild('TemEmployeesLearnedSafetyComponent', { static: true }) TemEmployeesLearnedSafetyComponent: TemEmployeesLearnedSafetyComponent;

  indexShort: number=0;
  filterText: string = '';
  identityCard: string='';
  personInCharge: string ='';
  employeesInput: EmployeesLearnedSafetyInputDto = new EmployeesLearnedSafetyInputDto();
  isLoading: boolean = false;
  idEmployeesArray: number[] = [];
  // record: CourceSafetySelectOutputDto =  new CourceSafetySelectOutputDto();
  active: Boolean = false;
  employeesLearnedForm: FormGroup;
  uploadUrl: string;
  uploadUrlSetEmployeesLearnedSafety;
  uploadEmpUrl: string;
  titleCource: string;
  isSavingEmployeesLearned: Boolean=true;
  constructor(
    injector: Injector,
    private _employeesLearnedSafetyService: EmployeesLearnedSafetyServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _httpClient: HttpClient,
    private _temEmployeesLearnedSafetyService: TemEmployeesLearnedSafetyServiceProxy,
    private toastService: ToastService,
  ) {
    super(injector);
    this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
    // this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportEmployeesFromExcel';
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportEmployeesFromExcel';
    this.uploadUrlSetEmployeesLearnedSafety = AppConsts.remoteServiceBaseUrl + '/ImportExcel/SetEmployeesFromExcel';
    this.uploadEmpUrl = AppConsts.remoteServiceBaseUrl + '/UploadImage/UploadImageEmployeesLearnedSafety';
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  create() {
    this.CreateOrEditEmployeesLearnedSafety.show(0);
  }

  edit(id) {
    this.CreateOrEditEmployeesLearnedSafety.show(id);
  }

  // showEmployeesLearned(record): void {
  //   // this.record.id =record.id;
  //   this.titleCource = record.courceName;
  //   // this.record.courceName=record.courceName;
  //   this.getEmployeesLearned();
  //    this.active = true;
  //    this.modal.show();
  // }

  delete() {
    if(this.idEmployeesArray.length > 0)
    {
      this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
        if (isConfirmed) {
          
          this.isLoading = true;
          this._employeesLearnedSafetyService.deleteById(this.idEmployeesArray).subscribe(() => {
            this.primengTableHelper.showLoadingIndicator();
            this.getEmployeesLearned();
            this.notify.success(this.l(AppConsts.message.deleteSuccess));
          });
        }
      });
    }
  }

  getEmployeesLearned(event?: LazyLoadEvent) {
    this.employeesInput.employeesName = this.filterText;
    // this.employeesInput.courceId =this.record.id;
    this.employeesInput.personInCharge= this.personInCharge;
    this.employeesInput.identityCard= this.identityCard;
    this.employeesInput.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.employeesInput.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._employeesLearnedSafetyService.loadAllByCourceId(this.employeesInput).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort=this.employeesInput.skipCount;
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
                this.isSavingEmployeesLearned=true;
                this.TemEmployeesLearnedSafetyComponent.getTemEmployeesLearned(response.result.report);
                this.toastService.openSuccessToast(this.l('ImportSuccess'));
            } 
            else if (response.error != null) {
                this.toastService.openFailToast(this.l('ImportFailed'));
            }
            this.spinnerService.hide();
        });
}

uploadExcelSetLearnedSafety(data: { files: File }): void {
  const formData: FormData = new FormData();
  const file = data.files[0];
  formData.append('file', file, file.name);
  this.idEmployeesArray =[];
  this.spinnerService.show();
  this._httpClient
      .post<any>(this.uploadUrlSetEmployeesLearnedSafety, formData)
      .pipe(finalize(() =>
      {
          this.setEmployeesLearnedSafetyWhenImportByExcel();
          this.excelFileUpload.clear();
          this.spinnerService.hide();
      } ))
      .subscribe(response => {
          if (response.success) {
              // this.toastService.openSuccessToast(this.l('ImportSuccess'));
                response.result.data.forEach(element => {
                  this.idEmployeesArray.push(element);
                });
               this.notify.success( this.l('SetSuccess') );
          } 
          else if (response.error != null) {
              // this.toastService.openFailToast(this.l('ImportFailed'));
              this.notify.error( this.l('SetError') );
          }
      });
}

uploadEmp(data: { files: File }, id: number): void {
  const formData: FormData = new FormData();
  const file = data.files[0];
  formData.append('file', file, file.name);
  // console.log(formData)
  this._httpClient
      .post<any>(this.uploadEmpUrl + '/' + id, formData)
      .pipe(finalize(() => this.uploadImageEmp.clear()))
      .subscribe(response => {
          if (response.success) {
            this.toastService.openSuccessToast(this.l('ImportSuccess'));
              this.getEmployeesLearned();
          } 
          else if (response.error != null) {
            this.toastService.openFailToast(this.l('ImportFailed'));
          }
      });
}

onUploadExcelError(): void {
  this.toastService.openFailToast(this.l('ImportFailed'));
}

onUploadEmpError(): void {
  this.toastService.openFailToast(this.l('ImportFailed'));
}

setEmployeesLearnedSafety()
{
  if(this.idEmployeesArray.length > 0)
  {
    this.message.confirm('', this.l("ConfirmAction"), (isConfirmed) => {
      if (isConfirmed) {
        this._employeesLearnedSafetyService.setEmployeesLearnedSafety(this.idEmployeesArray).subscribe(() => {
            this.notify.success( this.l(AppConsts.message.confirmSetLearnedSafetySuccess) );
            this.getEmployeesLearned();
            this.idEmployeesArray = [];
        });
      }
    });
  }
}

setEmployeesLearnedSafetyWhenImportByExcel()
{
  this._employeesLearnedSafetyService.setEmployeesLearnedSafety(this.idEmployeesArray).subscribe(() => {
    this.getEmployeesLearned();
    this.idEmployeesArray = [];
});
}


setEmployeesNotLearnedSafety()
{
  if(this.idEmployeesArray.length > 0)
  {
    // this.message.confirm('', this.l("ConfirmAction"), (isConfirmed) => {
    //   if (isConfirmed) {
    //     this._employeesLearnedSafetyService.setEmployeesNotLearnedSafety(this.idEmployeesArray)
    //     .pipe(finalize(() =>
    //     {
    //       this.idEmployeesArray.forEach(element => {
    //         this.primengTableHelper.records.forEach(element1 => {
    //           if(element == element1.id)
    //           {
    //             element1.learnedSafetyST = this.l('DontLearnedSafety');
    //             element1.isLearnedSafety = false;
    //           }
    //         });
    //     });
    //     }))
    //     .subscribe(() => {
    //     this.getEmployeesLearned();
    //       this.notify.success( this.l(AppConsts.message.confirmSetNotLearnedSafetySuccess) );
    //   });
    //   }
    // });
    this.message.confirm('', this.l("ConfirmAction"), (isConfirmed) => {
      if (isConfirmed) {
        this._employeesLearnedSafetyService.setEmployeesNotLearnedSafety(this.idEmployeesArray).subscribe(() => {
            this.notify.success( this.l(AppConsts.message.confirmSetNotLearnedSafetySuccess) );
            this.getEmployeesLearned();
            this.idEmployeesArray = [];
        });
      }
    });
  }
}

toggleEdit(event, id)
{
  if ( event.target.checked ) {
    this.idEmployeesArray.push(id);
   }
   else
   {
    const index = this.idEmployeesArray.indexOf(id);
    if (index > -1) { // only splice array when item is found
      this.idEmployeesArray.splice(index, 1); // 2nd parameter means remove one item only
    }
   }
}
}
