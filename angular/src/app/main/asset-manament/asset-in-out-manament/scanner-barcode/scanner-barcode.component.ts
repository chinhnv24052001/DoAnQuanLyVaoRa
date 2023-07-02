import { AfterViewInit, Component, EventEmitter, Injector, Input, OnInit, Output, VERSION, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssetInOutManamentServiceProxy, QRCodeResultDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';

@Component({
  selector: 'ScannerBarCode',
  templateUrl: './scanner-barcode.component.html',
  styleUrls: ['./scanner-barcode.component.css']
})
export class ScannerBarCodeComponent extends AppComponentBase implements OnInit,AfterViewInit  {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalBringModal: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  active: Boolean = false;
  private event: Subscription;
  qRCodeResult: QRCodeResultDto;
  typeRequestName: string='';
  statusApproval: number =1;
  bringInOutDto: { statusString: string, id : number, isIn: boolean, stringInOut: string}= {statusString:" ", id: 1, isIn: false, stringInOut: ''};
  constructor(
    injector: Injector,
    private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    // this.qRCodeResult= new QRCodeResultDto();
  }

  ngAfterViewInit() {
  }

  checkInOut(string, id, isIn, statusInOut){
    this.bringInOutDto.statusString=string;
    this.bringInOutDto.id=id;
    this.bringInOutDto.isIn=isIn
    this.bringInOutDto.stringInOut= statusInOut
    this.modalBringModal.emit(this.bringInOutDto);
    this.close();
  }

  show(result, status): void {
    this.statusApproval =status;
    this.qRCodeResult= result;
    // alert(this.qRCodeResult.isIn);
    this.typeRequestName=this.qRCodeResult.typeRequest==1? this.l(AppConsts.TypeRequestName.INTERNALREQUEST) : this.qRCodeResult.typeRequest==2? this.l(AppConsts.TypeRequestName.EMPLOYEESVENDERREQUEST) : this.qRCodeResult.typeRequest==4? this.l(AppConsts.TypeRequestName.CLIENTREQUEST) : this.l(AppConsts.TypeRequestName.ADDASSETVENDER);
    this.active = true;
    this.modal.show();
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
