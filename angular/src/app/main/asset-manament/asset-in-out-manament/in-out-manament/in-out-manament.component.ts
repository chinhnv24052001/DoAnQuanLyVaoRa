import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssetInOutManamentServiceProxy, RequestAssetBringServiceProxy, RequestAssetInOutInputDto } from '@shared/service-proxies/service-proxies';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { ScannerBarCodeComponent } from '../scanner-barcode/scanner-barcode.component';
import { ListAssetInComponent } from './in-manament/list-asset-in/list-asset-in.component';
import { ListRequestInComponent } from './in-manament/list-request-in/list-request-in.component';
import { ListWorkersInComponent } from './in-manament/list-workers-in/list-workers-in.component';
import { ListAssetOutComponent } from './out-manament/list-asset-out/list-asset-out.component';
import { ListRequestOutComponent } from './out-manament/list-request-out/list-request-out.component';
import { ListWorkersOutComponent } from './out-manament/list-workers-out/list-workers-out.component';

@Component({
  selector: 'app-in-out-manament',
  templateUrl: './in-out-manament.component.html',
  styleUrls: ['./in-out-manament.component.less'],
  animations: [appModuleAnimation()]
})
export class InOutManamentComponent extends AppComponentBase {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  @ViewChild('ListAssetIn', { static: true }) ListAssetIn: ListAssetInComponent;
  @ViewChild('ListWorkersIn', { static: true }) ListWorkersIn: ListWorkersInComponent;
  @ViewChild('ListRequestIn', { static: true }) ListRequestIn: ListRequestInComponent;


  @ViewChild('ListAssetOut', { static: true }) ListAssetOut: ListAssetOutComponent;
  @ViewChild('ListWorkersOut', { static: true }) ListWorkersOut: ListWorkersOutComponent;
  @ViewChild('ListRequestOut', { static: true }) ListRequestOut: ListRequestOutComponent;

  @ViewChild('ScannerBarCode', { static: true }) ScannerBarCode: ScannerBarCodeComponent;
  @Input() params?: any;

  isLoading: boolean = false;
  indexShort: number = 0;
  isIn: boolean = false;
  listVenderDropDown: { value: number, label: string }[];
  titleRequestInOut = "";
  tabKey = "";
  requestAssetInOutInputDto: RequestAssetInOutInputDto = new RequestAssetInOutInputDto();
  requestIdSelect: number = 0;
  statusRequests: { value: number, label: string }[];
  control: FormControl;
  value: string;
  isError = false;
  isScanner = true;
  showAssetTable = true;
  showEmployeesTable = true;
  @ViewChild('scanner')

  scanner: ZXingScannerComponent;
  hasDevices: boolean;
  hasPermission: boolean;
  qrResultString: string;
  availableDevices: MediaDeviceInfo[];
  currentDevice: MediaDeviceInfo;
  selectVender: string = this.l('SearchByVenderName');
  typeRequest: number = 0;
  inManament: string = AppConsts.message.inManament;
  outManament: string = AppConsts.message.outManament;
  ioStatus: string = 'INOUT';
  constructor(
    injector: Injector,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
    private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    this.inManament = AppConsts.message.inManament;
    this.outManament = AppConsts.message.outManament;
    this.selectDropDownVender();

    this.scanner.camerasFound.subscribe((devices: MediaDeviceInfo[]) => {
      this.hasDevices = true;
      this.availableDevices = devices;
    });
  }

  selectDropDownVender() {
    this._requestAssetBringServiceProxy.getVenderForEdit().subscribe((result) => {
      this.listVenderDropDown = [];
      this.listVenderDropDown.push({ value: 0, label: " " });
      this.control = new FormControl(this.listVenderDropDown[0].value);
      result.forEach(ele => {
        this.listVenderDropDown.push({ value: ele.id, label: ele.venderName });
      });
    });
  }

  callBackSelectIn(input) {
    this.requestAssetInOutInputDto.requestId = input.id;
    this.typeRequest = input.typeRequestId;
    if (this.typeRequest == 1 || this.typeRequest == 3) {
      this.ListAssetIn.getAssetIn();
      document.getElementById("dv-card-employees-in").style.display = "none";
      document.getElementById("dv-card-asset-in").style.display = "block";
    }
    else {
      this.ListWorkersIn.getEmployeesIn();
      document.getElementById("dv-card-employees-in").style.display = "block";
      document.getElementById("dv-card-asset-in").style.display = "none";
    }
  }

  callBackSelectOut(input) {
    this.requestAssetInOutInputDto.requestId = input.id;
    this.typeRequest = input.typeRequestId;
    if (this.typeRequest == 1 || this.typeRequest == 3) {
      this.ListAssetOut.getAssetOut();
      document.getElementById("dv-card-employees-out").style.display = "none";
      document.getElementById("dv-card-asset-out").style.display = "block";
    }
    else {
      this.ListWorkersOut.getEmployeesOut();
      document.getElementById("dv-card-employees-out").style.display = "block";
      document.getElementById("dv-card-asset-out").style.display = "none";
    }
  }

  callBackRequestApproval(input) {
    this.requestAssetInOutInputDto.requestId = 0;
    this.ListRequestIn.getRequestIn();
    this.ListRequestOut.getRequestOut();

    document.getElementById("dv-card-employees-in").style.display = "none";
    document.getElementById("dv-card-employees-out").style.display = "none";
    document.getElementById("dv-card-asset-in").style.display = "none";
    document.getElementById("dv-card-asset-out").style.display = "none";
  }

  reLoadRequestIn() {
    this.requestAssetInOutInputDto.requestId = 0;
    this.ListRequestIn.getRequestIn();
  }

  reLoadRequestOut() {
    this.requestAssetInOutInputDto.requestId = 0;
    this.ListRequestOut.getRequestOut();
  }

  callBackApproval() {
    this.requestAssetInOutInputDto.requestId = 0;
    this.ListRequestIn.getRequestIn();
    this.ListRequestOut.getRequestOut();
  }

  getAssetIOManament(input?: number) {
    this.requestAssetInOutInputDto.requestId = input;
    if (this.ioStatus == AppConsts.message.inManament) {
      this.ListRequestIn.getRequestIn();

      // document.getElementById("dv-card-employees-in").style.display = "none";
      // document.getElementById("dv-card-asset-in").style.display = "none";
      this.ListAssetIn.getAssetIn();
      this.ListWorkersIn.getEmployeesIn()
    }
    if (this.ioStatus == AppConsts.message.outManament) {
      this.ListRequestOut.getRequestOut();

      // document.getElementById("dv-card-employees-out").style.display = "none";
      // document.getElementById("dv-card-asset-out").style.display = "none";
      this.ListAssetOut.getAssetOut();
      this.ListWorkersOut.getEmployeesOut();
    }

    this.isLoading = false;
  }



  getAssetIOManamentScanner(input?: number, io_tatus?: boolean) {
    this.requestAssetInOutInputDto.requestId = input;
    if (io_tatus == true) {
      this.ListRequestIn.getRequestIn();
      this.ListAssetIn.getAssetIn();
      this.ListWorkersIn.getEmployeesIn();
    }
    else if (io_tatus == false) {
      this.ListRequestOut.getRequestOut();
      this.ListAssetOut.getAssetOut();
      this.ListWorkersOut.getEmployeesOut();
    }
    this.isLoading = false;
  }

  handleQrCodeResult(resultString: string) {
    if (this.isScanner && resultString != null) {
      this._assetInOutManamentServiceProxy.readQRCode(resultString).subscribe((result) => {
        this.ScannerBarCode.show(result, 1);

        this.getAssetIOManamentScanner(result.requestId, result.isIn);
        this.notify.success(this.l('ScannerSuccess'));
      })
    }
  }

  bringInOut(item) {
    if (item.isIn = true) {
      if (item.stringInOut == 'a') {
        this.ListAssetIn.bringModal(item.id, '');
        return;
      }
      this.ListWorkersIn.bringModal(item.id, '');
    }
    else {
      if (item.stringInOut == 'a') {
        this.ListAssetOut.bringModal(item.id);
        return;
      }
      this.ListWorkersOut.bringModal(item.id);
    }
  }

 //Call back Approved
 callbackdomApprovedAssetIn(totalInput) {
  if (totalInput == 0) {
    document.getElementById("dv-card-asset-in").style.display = "none";
  }
  else document.getElementById("dv-card-asset-in").style.display = "block";
  this.reLoadRequestOut();
}

callbackdomApprovedAssetOut(totalInput) {
  if (totalInput == 0) {
    document.getElementById("dv-card-asset-out").style.display = "none";
  }
  else document.getElementById("dv-card-asset-out").style.display = "block";
  this.reLoadRequestIn();
}

callbackdomAprovedEmployeesIn(totalInput) {
  if (totalInput == 0) {
    document.getElementById("dv-card-employees-in").style.display = "none";
  }
  else document.getElementById("dv-card-employees-in").style.display = "block";
  this.reLoadRequestOut();
}

callbackdomApprovedEmployeesOut(totalInput) {
  if (totalInput == 0) {
    document.getElementById("dv-card-employees-out").style.display = "none";
  }
  else document.getElementById("dv-card-employees-out").style.display = "block";
  this.reLoadRequestIn();
} 

//Call back search
  callbackdomAssetIn(totalInput) {
    if (totalInput == 0) {
      document.getElementById("dv-card-asset-in").style.display = "none";
    }
    else document.getElementById("dv-card-asset-in").style.display = "block";
  }

  callbackdomAssetOut(totalInput) {
    if (totalInput == 0) {
      document.getElementById("dv-card-asset-out").style.display = "none";
    }
    else document.getElementById("dv-card-asset-out").style.display = "block";
  }

  callbackdomEmployeesIn(totalInput) {
    if (totalInput == 0) {
      document.getElementById("dv-card-employees-in").style.display = "none";
    }
    else document.getElementById("dv-card-employees-in").style.display = "block";
  }

  callbackdomEmployeesOut(totalInput) {
    if (totalInput == 0) {
      document.getElementById("dv-card-employees-out").style.display = "none";
    }
    else document.getElementById("dv-card-employees-out").style.display = "block";
  }
}

