import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CourceSafetySaveDto, CourceSafetyServiceProxy, VenderSelectOutputDto, VenderServiceProxy } from '@shared/service-proxies/service-proxies';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {InputTextareaModule} from 'primeng/inputtextarea';
import { DateTime } from 'luxon';

@Component({
  selector: 'CreateOrEditCourceSafety',
  templateUrl: './create-or-edit-cource-safety.component.html',
  styleUrls: ['./create-or-edit-cource-safety.component.less']
})
export class CreateOrEditCourceSafetyComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild('SampleDatePicker1', {static: true}) sampleDatePicker1: ElementRef;
  @ViewChild('SampleDatePicker2', {static: true}) sampleDatePicker2: ElementRef;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  active: Boolean = false;
  saving: Boolean = false;
  isEdit: Boolean = true;
  courceSafetyForm: FormGroup;
  date1: Date= new Date;
  courceSafety: CourceSafetySaveDto=new CourceSafetySaveDto();
  effectiveDateStart: Date= new Date();
  effectiveDateEnd: Date= new Date();
  phonenumberValidate: boolean=false;
  constructor(
    injector: Injector,
    private _courceSafetyService: CourceSafetyServiceProxy,
    private formBuilder: FormBuilder,
  ) {
    super(injector);
  }

  show(id): void {
    this.validation();
    if (!id) {
      this.isEdit=false;
      this.courceSafety=new CourceSafetySaveDto();
      this.active = true;
      this.modal.show();
      this.isLoading=false;
    }
      else{
        this._courceSafetyService.loadById(id).subscribe((result)=>
        {
          this.courceSafety=result;
          const effectiveDateStart =result.effectiveDateStart ? new Date(result.effectiveDateStart?.toString()) : new Date();
          const effectiveDateEnd =result.effectiveDateEnd ? new Date(result.effectiveDateEnd?.toString()) : new Date();
          this.courceSafetyForm.get('EffectiveDateStart').patchValue(effectiveDateStart);
          this.courceSafetyForm.get('EffectiveDateEnd').patchValue(effectiveDateEnd);
          this.isEdit=true;
          this.active = true;
          this.modal.show();
        }
        );
    }
  }

  validation(): void {
    this.courceSafetyForm = this.formBuilder.group({
      CourceName: [undefined, GlobalValidator.required],
      EffectiveDateStart: [undefined, GlobalValidator.required],
      EffectiveDateEnd: [undefined, GlobalValidator.required],
      Description: []
    });
  }

  save(): void {
      this.saving = true;
      this.courceSafety.effectiveDateEnd = this.courceSafetyForm.get('EffectiveDateEnd').value;
      this.courceSafety.effectiveDateStart = this.courceSafetyForm.get('EffectiveDateStart').value;
      this._courceSafetyService.save(this.courceSafety).pipe(
        finalize(() => {
          this.saving = false;
        })).subscribe(() => {
          this.notify.info(this.l(AppConsts.message.saveSuccess));
          this.close();
          this.modalSave.emit(null);
          this.courceSafety = new CourceSafetySaveDto();
          this.saving = false;
          this.isLoading=false;
        });
  }
  
  close(): void {
    this.active = false;
    this.modal.hide();
  }

}
