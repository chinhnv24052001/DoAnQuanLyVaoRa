import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EmployeesLearnedSafetySaveDto, EmployeesLearnedSafetySelectOutputDto, EmployeesLearnedSafetyServiceProxy, RequestAssetBringServiceProxy, VenderSelectOutputDto, VenderServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'CreateOrEditEmployeesLearnedSafety',
  templateUrl: './create-or-edit-employees-learned-safety.component.html',
  styleUrls: ['./create-or-edit-employees-learned-safety.component.less']
})
export class CreateOrEditEmployeesLearnedSafetyComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  @Input() record;
  listVenderDropDown: { value: number, label: string }[];
  active: Boolean = false;
  saving: Boolean = false;
  isEdit: Boolean = true;
  _isMan: boolean = true;
  _isWoman: boolean = false;
  selectVender: string = this.l(AppConsts.message.selectVender)
  genderSelect: {value : number, text: string }[] = [{value: 1, text: "Nam" }, {value: 2, text :"Nữ"}];
  editEmployeesLearnedForm: FormGroup;
  employeesLearned: EmployeesLearnedSafetySaveDto=new EmployeesLearnedSafetySaveDto();
  constructor(
    injector: Injector,
    private _employeesLearnedSafetyService: EmployeesLearnedSafetyServiceProxy,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
    private formBuilder: FormBuilder
  ) {
    super(injector);
  }

  ngOnInit() {
    this.selectDropDownVender();
  }

  show(id): void {
    this.validation();
    if (!id) {
      this.isEdit=false;
      this.employeesLearned=new EmployeesLearnedSafetySaveDto();
      // this.employeesLearned.courceId=this.record.id;
      this.active = true;
      this.modal.show();
    }
      else{
        this._employeesLearnedSafetyService.loadById(id).subscribe((result)=>
        {
          this.employeesLearned=result;
          // this.employeesLearned.employeesName=result.employeesName;
          // this.employeesLearned.identityCard=result.identityCard;
          // this.employeesLearned.gender = result.gender;
          // this.employeesLearned.phoneNumber = result.phoneNumber;
          // this.employeesLearned.address = result.address;
          // this.employeesLearned.courceId=result.courceId;
          // this.employeesLearned.venderId=result.venderId;
          this.isEdit=true;
          this.active = true;
          this.modal.show();
        }
        );
    }
  }


  selectDropDownVender() {
    this._requestAssetBringServiceProxy.getVenderForEdit().subscribe((result) => {
      this.listVenderDropDown = [];
      this.listVenderDropDown.push({ value: 0, label: "." });
      result.forEach(ele => {
        this.listVenderDropDown.push({ value: ele.id, label: ele.venderName });
      });
    });
  }

  validation(): void {
    this.editEmployeesLearnedForm = this.formBuilder.group({
      EmployeesName: [undefined, GlobalValidator.required],
      IdentityCard: [undefined, GlobalValidator.required],
      Gender: [undefined, GlobalValidator.required],
      PhoneNumber: [undefined, Validators.compose([GlobalValidator.required])],
      Address: [undefined, GlobalValidator.required],
      venderId: [undefined, GlobalValidator.required],
      PersonInCharge: [undefined, GlobalValidator.required],
      CourceName: []
       });
  }

  save(): void {
    this.isLoading=true;
      this.saving = true;
      this._employeesLearnedSafetyService.save(this.employeesLearned).pipe(
        finalize(() => {
          this.saving = false;
        })).subscribe(() => {
          this.notify.info(this.l(AppConsts.message.saveSuccess));
          this.close();
          // this.modalSave.emit(this.record);
          this.modalSave.emit(0);
          this.employeesLearned = new EmployeesLearnedSafetySaveDto();
          this.saving = false;
        });
  }
  
  close(): void {
    // this.modalSave.emit(this.record);
    this.active = false;
    this.modal.hide();
  }
}
