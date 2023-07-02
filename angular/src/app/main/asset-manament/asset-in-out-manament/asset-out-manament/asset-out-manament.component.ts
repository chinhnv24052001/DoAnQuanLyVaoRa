import { AfterViewInit, Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ToastService } from '@shared/common/ui/toast.service';
import { AssetInOutManamentServiceProxy, AssetInOutSelectOutputDto, RequestAssetBringServiceProxy, RequestAssetInOutInputDto } from '@shared/service-proxies/service-proxies';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { ListAssetInOutComponent } from '../list-asset-in-out/list-asset-in-out.component';
import { ListRequestInOutComponent } from '../list-request-in-out/list-request-in-out.component';
import { ListWorkersInOutComponent } from '../list-workers-in-out/list-workers-in-out.component';
import { ScannerBarCodeComponent } from '../scanner-barcode/scanner-barcode.component';

@Component({
  selector: 'app-asset-out-manament',
  templateUrl: './asset-out-manament.component.html',
  styleUrls: ['./asset-out-manament.component.less'],
  animations: [appModuleAnimation()]
})
export class AssetOutManamentComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('ListAssetInOut', { static: true }) ListAssetInOut: ListAssetInOutComponent;
  @ViewChild('ListWorkersInOut', { static: true }) ListWorkersInOut: ListWorkersInOutComponent;
  @ViewChild('ListRequestInOut', { static: true}) ListRequestInOut: ListRequestInOutComponent;
  @ViewChild('ScannerBarCode', { static: true }) ScannerBarCode: ScannerBarCodeComponent;
  @Input() params?: any;
  isLoading: boolean = false;
  indexShort: number = 0;
  isIn: boolean = true;
  listVenderDropDown: { value: number, label: string }[];
  control: FormControl;
  titleRequestInOut="";
  tabKey="";
  assetSelectOutput: AssetInOutSelectOutputDto = new AssetInOutSelectOutputDto();
  requestAssetInOutInputDto: RequestAssetInOutInputDto = new RequestAssetInOutInputDto();
  statusRequests: { value: number, label: string }[];
  value: string;
  isError = false;
  isScanner = true;
  scanner: ZXingScannerComponent;
  hasDevices: boolean;
  availableDevices: MediaDeviceInfo[];
  currentDevice: MediaDeviceInfo;
  selectVender: string=this.l('SearchByVenderName');
  typeRequest: number=0;
  time =0;
  constructor(
    injector: Injector,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
    private _assetInOutManamentServiceProxy: AssetInOutManamentServiceProxy,
    private toastService: ToastService
  ) {
    super(injector);
  }

ngAfterViewInit(): void {
    this.getAssetIOManament();
}

  ngOnInit() {
    this.time =0;
        setInterval(()=>{
            this.time++;
        },1000)
    this.isIn=true;
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

  callBackSelect(input)
  {
    this.requestAssetInOutInputDto.requestId=input.id;
    this.typeRequest = input.typeRequestId;
    this.ListAssetInOut.getAssetInOut();
    this.ListWorkersInOut.getEmployeesInOut();
  }

  callBackRequestApproval(){
    this.requestAssetInOutInputDto.requestId=0;
    this.ListRequestInOut.getRequestInOut();
    this.ListAssetInOut.getAssetInOut();
    this.ListWorkersInOut.getEmployeesInOut();
  }

  callBackApproval()
  {
    this.requestAssetInOutInputDto.requestId=0;
    this.ListRequestInOut.getRequestInOut();
  }

  getAssetIOManament(input?: number) {
    this.requestAssetInOutInputDto.requestId = input;
    this.ListRequestInOut.getRequestInOut();
    this.ListAssetInOut.getAssetInOut();
    this.ListWorkersInOut.getEmployeesInOut();
    this.isLoading = false;
  }

  onError(error) {
    console.error(error);
    this.isError = true;
  }

  handleQrCodeResult(resultString: string) {
    if (this.isScanner&&resultString!=null && this.time > 1 ) {
      this._assetInOutManamentServiceProxy.readQRCode(resultString).subscribe((result) => {
        if(result.statusQRScanner == 1 || result.statusQRScanner == 2)
        {
          this.ScannerBarCode.show(result, result.statusQRScanner);
          this.getAssetIOManament(result.requestId);
        this.time =0;
        this.toastService.openSuccessToast(this.l('ScannerSuccess'));
        }
        // else if (result.statusQRScanner == 2)
        // {
        //   this.time =0;
        //   this.toastService.openFailToast(this.l('RequestHasNotBeenApproved'));
        // }
        else 
        {
          this.time =0;
          this.toastService.openFailToast(this.l('ScannerError'));
        }
       
      })
    }
  }

  bringInOut(item){

    if(item.statusString=='a')
    {
      this.ListAssetInOut.bringModal(item.id, item.stringInOut);
      return;
    }
    this.ListWorkersInOut.bringModal(item.id, item.stringInOut);
  }
}
