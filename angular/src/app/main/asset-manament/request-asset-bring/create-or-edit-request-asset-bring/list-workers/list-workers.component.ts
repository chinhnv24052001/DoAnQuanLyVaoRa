import { Component, EventEmitter, Injector, Input, NgZone, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AioEmployeesDto, CourceSafetyServiceProxy, RequestAssetBringSaveDto, RequestAssetBringServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { MessageAgreeComponent } from './message-confirm/message-agree.component';
import { ToastService } from '@shared/common/ui/toast.service';
import { ViewImageComponent } from '../../detail-request-asset-bring/view-image/view-image.component';

@Component({
  selector: 'app-list-workers',
  templateUrl: './list-workers.component.html',
  styleUrls: ['./list-workers.component.less']
})
export class ListWorkersComponent extends AppComponentBase implements OnInit, OnChanges {
  @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
  @ViewChild("messageAgreeComponent", { static: true }) messageAgreeComponent: MessageAgreeComponent;
  @ViewChild('viewImage', { static: true }) viewImage: ViewImageComponent;
  @Input() formAssetRequest: FormGroup;
  @Input() assetRequest: RequestAssetBringSaveDto;
  @Input() typeRequest;
  @Output() modalInputIdentity: EventEmitter<any> = new EventEmitter<any>();
  workersArray: FormArray;
  uploadUrl: string;
  listCourseDropDown: { value: number, label: string }[];
  courseId:number=0;
  message1: string = this.l('IdentityCardDontMapOnListLearnedSafety');
  constructor(injector: Injector,
    private formBuilder: FormBuilder,
    private _httpClient: HttpClient,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,  private toastService: ToastService,
    private _courseServiceProxy: CourceSafetyServiceProxy) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportWorkerRequestFromExcel';
  }
  ngOnChanges(changes: SimpleChanges,): void {
    if (this.workersArray) {
      for (let i = this.workersArray.length - 1; i >= 0; i--) {
        this.workersArray.removeAt(i);
      }
      this.workersArray = this.formAssetRequest.get('workersList') as FormArray;
      this.checkForm();
    }

  }

  ngOnInit(): void {
    this.workersArray = this.formAssetRequest.get('workersList') as FormArray;
    this.checkForm();
    this.getCourseDropdown();
  }

getCourseDropdown()
{
  this._courseServiceProxy.getCourseForAddEmployees().subscribe((result) => {
    this.listCourseDropDown = [];
    this.listCourseDropDown.push({ value: 0, label: " " });
    result.forEach(ele => {
      this.listCourseDropDown.push({ value: ele.id, label: ele.courceName });
    });
  });
}

  onFileChange(event, i) {
    // var blob = event.target.files[0].slice(0, event.target.files[0].size, 'image/png');
    // const newFile = new File([blob], event.target.files[0].name, { type: 'image/png' })
    // const reader = new FileReader();
    // reader.readAsDataURL(event.target.files[0]);

    // console.log(this.workersArray.controls[i].get('imageEmployees').value);
    
    // reader.onload = (e) => {
    //   // this.workersArray.controls[i].patchValue({ "imageEmployees": reader.result });
    //   console.log(reader.result);
    //   this.workersArray.controls[i].get('imageEmployees').setValue(reader.result);
    // };
    const files = event.target.files;
    const reader = new FileReader();
      reader.onload = (e) => {
        const base64img = reader.result + '';
        var base64result='';
        if(base64img)
        {
           base64result = base64img.substr(base64img.indexOf(',') + 1); 
        }
        
        this.workersArray.controls[i].get('imageEmployees').setValue(base64result);
        // control.push(this.builder.control(base64img));
        console.log(base64img);
      };
      reader.readAsDataURL(files[0]);
  }


  addWorker(val?: AioEmployeesDto) {
    // const length = this.assetArray.controls.length;
    const worker = this.formBuilder.group({
      id: [0],
      employeesName: [undefined, GlobalValidator.required],
      identityCard: [undefined, GlobalValidator.required],
      identityVal: [undefined, GlobalValidator.identitySafatyVal],
      dateStart: [undefined, GlobalValidator.required],
      dateEnd: [undefined, GlobalValidator.required],
      imageEmployees: [undefined]
    });

    if (val) {
      worker.patchValue(Object.assign({}, val));
      const dateStart = val.dateStart ? new Date(val.dateStart?.toString()) : new Date();
      const dateEnd = val.dateEnd ? new Date(val.dateEnd?.toString()) : new Date();
      worker.get('dateStart').patchValue(dateStart);
      worker.get('dateEnd').patchValue(dateEnd);

    }
    this.workersArray.push(worker);
  }

  deleteWorker(i) {
    this.workersArray.removeAt(i);
  }

  checkForm() {
    if (this.assetRequest && this.assetRequest.workersList.length) {
      this.assetRequest.workersList.forEach(worker => {
        this.addWorker(worker);
      });
    }
  }

  callMouseOutEvent(i) {
    var identityCard = this.workersArray.controls[i].get('identityCard').value;
    this.workersArray.controls[i].get('identityVal').setValue(0);

    if (identityCard != null && this.typeRequest==2) {
      this._requestAssetBringServiceProxy.checkWorkerLearnedSafety(identityCard).subscribe((result) => {
        if (result) {
          return;
        }
        else {
          this.messageAgreeComponent.open(i);
          this.workersArray.controls[i].get('identityCard').setValue('');
          // this.message.confirm('', this.l(AppConsts.message.identityCardErrSafety), (isConfirmed) => {
          //   if (isConfirmed) {
          //     this.workersArray.controls[i].get('identityCard').reset();
          //   }
          // });
        }
      });
    }
  }

  closeConfirmCheckEmployees(index)
  {
    this.workersArray.controls[index].get('identityCard').reset();
  }

  uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    const file = data.files[0];
    formData.append('file', file, file.name);

    this._httpClient
      .post<any>(this.uploadUrl, formData)
      .pipe(finalize(() => this.excelFileUpload.clear()))
      .subscribe(response => {
        if (response.success) {

          if (this.workersArray) {
            for (let i = this.workersArray.length - 1; i >= 0; i--) {
              this.workersArray.removeAt(i);
            }
          }

          response.result.report.forEach(worker => {
            this.addWorker(worker);
          });
          this.toastService.openSuccessToast(this.l('ImportSuccess'));
        }
        else if (response.error != null) {
          this.toastService.openFailToast(this.l('ImportFailed'));
        }
      });
  }

  onUploadExcelError(): void {
    this.toastService.openFailToast(this.l('ImportFailed'));
  }

  selectEmployeesByCourseId()
  {
    if (this.workersArray) {
      for (let i = this.workersArray.length - 1; i >= 0; i--) {
        this.workersArray.removeAt(i);
      }
    }

    this._requestAssetBringServiceProxy.loadEmployeesByCourseId(this.courseId).subscribe((result)=>{
      result.forEach(worker => {
        this.addWorker(worker);
    })
    });
  }

  closeSave()
  {
    this.modalInputIdentity.emit('close');
  }

  openSave()
  {
    this.modalInputIdentity.emit('open');
  }
    
  viewImageByte(assetByteArray)
  {
    if (assetByteArray != null)
    {
      this.viewImage.show(assetByteArray);
    }
  }
}
