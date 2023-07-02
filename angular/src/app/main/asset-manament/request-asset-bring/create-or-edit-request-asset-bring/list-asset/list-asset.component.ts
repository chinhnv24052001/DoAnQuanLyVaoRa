import { Component, Injector, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AioAssetDto, AioEmployeesDto, RequestAssetBringSaveDto } from '@shared/service-proxies/service-proxies';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { ToastService } from '@shared/common/ui/toast.service';
import { ViewImageComponent } from '../../detail-request-asset-bring/view-image/view-image.component';
@Component({
  selector: 'app-list-asset',
  templateUrl: './list-asset.component.html',
  styleUrls: ['./list-asset.component.less']
})
export class ListAssetComponent extends AppComponentBase implements OnInit, OnChanges {
  @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
  @ViewChild('UploadImageAsset', { static: false }) uploadImageAsset: FileUpload;
  @ViewChild('viewImage', { static: true }) viewImage: ViewImageComponent;
  @Input() formAssetRequest: FormGroup;
  @Input() assetList: Array<any>;
  @Input() internal;

  @Input() assetRequest: RequestAssetBringSaveDto;
  assetArray: FormArray;
  uploadUrl: string;
  // uploadUrlInternal: string;
  assetUrl: string;
  asset: string = this.l('Asset');
  constructor(injector: Injector, private formBuilder: FormBuilder, private _httpClient: HttpClient,  private toastService: ToastService,) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportAssetRequestFromExcel';
    // this.uploadUrlInternal = AppConsts.remoteServiceBaseUrl + '/ImportExcel/ImportAssetRequestInternalFromExcel';
  }

  ngOnInit(): void {
    this.assetArray = this.formAssetRequest.get('assetList') as FormArray;
    this.checkForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.assetArray) {
      for (let i = this.assetArray.length - 1; i >= 0; i--) {
        this.assetArray.removeAt(i);
      }
      this.assetArray = this.formAssetRequest.get('assetList') as FormArray;
      this.checkForm();
    }
  }

  addAsset(val?: AioAssetDto) {
    // const length = this.assetArray.controls.length;
    
    if(this.internal)
    {
      const asset = this.formBuilder.group({
        id: [0],
        assetId: [undefined, GlobalValidator.required],
        tagCode: [undefined, GlobalValidator.required],
        total: [undefined, GlobalValidator.required],
        dateStart: [undefined, GlobalValidator.required],
        dateEnd: [undefined],
        aviationIsBack: [undefined],
        assetImage: [undefined],
        valAviationIsBack: [false]
      });

      if (val) {
        asset.patchValue(Object.assign({}, val));
        const dateStart = val.dateStart ? new Date(val.dateStart?.toString()) : new Date();
        const dateEnd = val.dateEnd ? new Date(val.dateEnd?.toString()) : new Date();
        asset.get('dateStart').patchValue(dateStart);
        asset.get('dateEnd').patchValue(dateEnd);
      }
      this.assetArray.push(asset);
    }
    else
    {
      const asset = this.formBuilder.group({
        id: [0],
        assetId: [undefined, GlobalValidator.required],
        tagCode: [undefined, GlobalValidator.required],
        total: [undefined, GlobalValidator.required],
        dateStart: [undefined, GlobalValidator.required],
        dateEnd: [undefined, GlobalValidator.required],
        aviationIsBack: [undefined],
        assetImage: [undefined],
      });

      if (val) {
        asset.patchValue(Object.assign({}, val));
        const dateStart = val.dateStart ? new Date(val.dateStart?.toString()) : new Date();
        const dateEnd = val.dateEnd ? new Date(val.dateEnd?.toString()) : new Date();
        asset.get('dateStart').patchValue(dateStart);
        asset.get('dateEnd').patchValue(dateEnd);
      }
      this.assetArray.push(asset);
    }
  }

  deleteAsset(i) {
    this.assetArray.removeAt(i);
  }

  checkForm() {
    if (this.assetRequest && this.assetRequest.assetList.length) {
      this.assetRequest.assetList.forEach(asset => {
        this.addAsset(asset);
      });
    }
  }

  uploadExcel(data: { files: File }): void {
    const formData: FormData = new FormData();
    const file = data.files[0];
    formData.append('file', file, file.name);
    var check = this.internal ? 'IN' : 'VENDER';
    formData.append('check', check);
    this.spinnerService.show();
    this._httpClient
      .post<any>(this.uploadUrl, formData)
      .pipe(finalize(() =>
      {
        this.excelFileUpload.clear(); 
        this.spinnerService.hide();
      }))
      .subscribe(response => {
        if (response.success) {
         
          if (this.assetArray) {
            for (let i = this.assetArray.length - 1; i >= 0; i--) {
              this.assetArray.removeAt(i);
            }
          }

          response.result.report.forEach(asset => {
            this.addAsset(asset);
          });

          // this.notify.success(this.l('ImportSuccess'));
          this.toastService.openSuccessToast(this.l('ImportSuccess'));
        }
        else if (response.error != null) {
          this.toastService.openFailToast(this.l('ImportFailed'));
        }
        
      });
  }

  onUploadExcelError(): void {
    // this.notify.error(this.l('ImportFailed'));
    this.toastService.openFailToast(this.l('ImportFailed'));
   
  }

  uploadAssetImg(event, i): void {
    const file = event.files[0];
    const reader = new FileReader();
    reader.onload = (e) => {
      const base64img = reader.result + '';
      var base64result = '';
      if (base64img) {
        base64result = base64img.substr(base64img.indexOf(',') + 1);
      }
      this.assetArray.controls[i].get('assetImage').setValue(base64result);
      // console.log(base64img);
    };
    reader.readAsDataURL(file);
  }

  onUploadAssetError(): void {
    // this.notify.error(this.l('ImportFailed'));
    this.toastService.openFailToast(this.l('ImportFailed'));
  }

  viewImageByte(assetByteArray)
  {
    if (assetByteArray != null)
    {
      this.viewImage.show(assetByteArray);
    }
  }

  // checkListAsset()
  // {
  //   var statusSaveByAssetList = true;
  //   if(this.internal)
  //   {
  //     for (let i = this.assetArray.length - 1; i >= 0; i--) {
  //       if(this.assetArray.controls[i].get('dateEnd').value() ==  null && this.assetArray.controls[i].get('aviationIsBack').value() ==  true)
  //       {
  //         this.assetArray.controls[i].get('valAviationIsBack').setValue(true);
  //         statusSaveByAssetList = false
  //       }
  //     }
  //   }
  //   return statusSaveByAssetList;
  // }
}



//window.open(this.src)