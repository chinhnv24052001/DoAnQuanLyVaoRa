import { Byte } from '@angular/compiler/src/util';
import { Component, ElementRef, EventEmitter, Injector, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssetInOutManamentServiceProxy, VenderSelectOutputDto, VenderServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'CreateAssetBarCode',
  templateUrl: './create-asset-bar-code.component.html',
  styleUrls: ['./create-asset-bar-code.component.less']
})
export class CreateAssetBarCodeComponent extends AppComponentBase  implements OnInit {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Input() isLoading;
  active: Boolean = false;
  saving: Boolean = false;
  isEdit: Boolean = true;
  valueBarcode: Number = 123;
  date: Date;
  yrs: string;
  lotlot:any;
  imageData: any;  
  sanitizedImageData: any;  
  asetId: any;
  infoBarcode: string;
  @ViewChild('dayOfTheYear') dayOfTheYear: ElementRef;
  isChecked:any;
  px2mmFactor: number;
  teamItem:number;
  constructor(
    injector: Injector,
    private renderer: Renderer2,
    
    private _venderService: VenderServiceProxy,
    private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
 
  }

  show(teamItem, infoBarcode, result): void {
    this.teamItem=teamItem;
    this.infoBarcode=infoBarcode;
    this.imageData=result;
    this.active = true;
    this.modal.show();
  }

  printPage() {
    window.print();
  }
  
  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
