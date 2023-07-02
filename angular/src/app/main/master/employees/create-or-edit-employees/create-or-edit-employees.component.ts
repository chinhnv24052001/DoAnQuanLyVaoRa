import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EmployeesServiceProxy, EmployessSaveDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'CreateOrEditEmployees',
  templateUrl: './create-or-edit-employees.component.html',
  styleUrls: ['./create-or-edit-employees.component.less']
})
export class CreateOrEditEmployeesComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  active: Boolean = false;
  saving: Boolean = false;
  employeesForm: FormGroup;
  employees: EmployessSaveDto = new EmployessSaveDto();
  listVenderDropDown: { value: number , label: string }[];
  phonenumberValidate: boolean=false;
  constructor(
    injector: Injector,
    private _employeesService: EmployeesServiceProxy,
    private formBuilder: FormBuilder,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.selectDropDownVender();
  }

  selectDropDownVender()
{
  this._employeesService.getVenderForEdit().subscribe((result)=>{
    this.listVenderDropDown = [];
    this.listVenderDropDown.push({value: 0, label: ""});
    result.forEach(ele => {
            this.listVenderDropDown.push({value: ele.id, label: ele.venderName});
  });
});
}

  isEdit:Boolean=true;

  show(id): void {
    this.validation();
    if (!id) {
      this.isEdit=false;
      this.employees=new EmployessSaveDto();
      this.active = true;
      this.modal.show();
    }
      else{
        this._employeesService.loadById(id).subscribe((result)=>
        {
          this.employees.id=result.id;
          this.employees.employeesName=result.employeesName;
          this.employees.address=result.address;
          this.employees.phoneNumber= result.phoneNumber;
          this.employees.venderId=result.venderId;
          this.isEdit=true;
          this.active = true;
          this.modal.show();
        }
        );
    }
  }

  validation(): void {
    this.employeesForm = this.formBuilder.group({
      VenderId: [undefined, GlobalValidator.required],
      EmployeesName: [undefined, GlobalValidator.required],
      Address: [undefined, GlobalValidator.required],
      PhoneNumber: [undefined, Validators.compose([GlobalValidator.required, GlobalValidator.phoneFormat])],
    });
  }
  save(): void {
    if(AppConsts.phoneNumber_regex.test(this.employees.phoneNumber)==false)
    {
      this.phonenumberValidate=true;
    }
    else
    {
      this.phonenumberValidate=false;
      this.saving = true;
      this._employeesService.save(this.employees).pipe(
        finalize(() => {
          this.saving = false;
        })).subscribe(() => {
          this.notify.info(this.l(AppConsts.message.saveSuccess));
          this.close();
          this.modalSave.emit(null);
          this.employees = new EmployessSaveDto();
          this.saving = false;
        });
    }
  }
  
  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
