import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { ButtonModule } from 'primeng/button';
import { AssetManamentRoutingModule } from './asset-manament-routing.module';
import { RequestAssetBringComponent } from './request-asset-bring/request-asset-bring.component';
import { CreateOrEditRequestAssetBringComponent } from './request-asset-bring/create-or-edit-request-asset-bring/create-or-edit-request-asset-bring.component';
import { AddRowDirective } from 'add-row.directive';
import { CalendarModule } from 'primeng/calendar';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DetailRequestAssetBringComponent } from './request-asset-bring/detail-request-asset-bring/detail-request-asset-bring.component';
import { ListAssetComponent } from './request-asset-bring/create-or-edit-request-asset-bring/list-asset/list-asset.component';
import { ListWorkersComponent } from './request-asset-bring/create-or-edit-request-asset-bring/list-workers/list-workers.component';
import { ReasonOfRefusalModalComponent } from './request-asset-bring/detail-request-asset-bring/reason-of-refusal-modal/reason-of-refusal-modal.component';
import {TabViewModule} from 'primeng/tabview';
import { ListWorkersInOutComponent } from './asset-in-out-manament/list-workers-in-out/list-workers-in-out.component';
import { ListAssetInOutComponent } from './asset-in-out-manament/list-asset-in-out/list-asset-in-out.component';
import { AssetOutManamentComponent } from './asset-in-out-manament/asset-out-manament/asset-out-manament.component';
import { CheckboxModule } from 'primeng/checkbox';
import { ListRequestInOutComponent } from './asset-in-out-manament/list-request-in-out/list-request-in-out.component';
import { AssetInManamentComponent } from './asset-in-out-manament/asset-in-manament/asset-in-manament.component';
import { MasterRoutingModule } from '../master/master-routing.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CreateAssetBarCodeComponent } from './request-asset-bring/detail-request-asset-bring/create-asset-bar-code/create-asset-bar-code.component';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { DropdownModule } from 'primeng/dropdown';
import { ScannerBarCodeComponent } from './asset-in-out-manament/scanner-barcode/scanner-barcode.component';
import { MessageAgreeComponent } from './request-asset-bring/create-or-edit-request-asset-bring/list-workers/message-confirm/message-agree.component';
import { InOutManamentComponent } from './asset-in-out-manament/in-out-manament/in-out-manament.component';
import {RadioButtonModule} from 'primeng/radiobutton';
import { DetailHistoryEmployeesInOutComponent } from './request-asset-bring/detail-request-asset-bring/detail-history-employees-io/detail-history-employees-io.component';
import { DetailHistoryAssetInOutComponent } from './request-asset-bring/detail-request-asset-bring/detail-history-asset-io/detail-history-asset-io.component';
import { ListRequestOutComponent } from './asset-in-out-manament/in-out-manament/out-manament/list-request-out/list-request-out.component';
import { ListRequestInComponent } from './asset-in-out-manament/in-out-manament/in-manament/list-request-in/list-request-in.component';
import { ListAssetInComponent } from './asset-in-out-manament/in-out-manament/in-manament/list-asset-in/list-asset-in.component';
import { ListAssetOutComponent } from './asset-in-out-manament/in-out-manament/out-manament/list-asset-out/list-asset-out.component';
import { ListWorkersInComponent } from './asset-in-out-manament/in-out-manament/in-manament/list-workers-in/list-workers-in.component';
import { ListWorkersOutComponent } from './asset-in-out-manament/in-out-manament/out-manament/list-workers-out/list-workers-out.component';
import { MessageImportErrorComponent } from './request-asset-bring/create-or-edit-request-asset-bring/message-import-error/message-import-error.component';
import { SendMailToManufactureComponent } from './request-asset-bring/detail-request-asset-bring/send-mail-to-manufacture/send-mail-to-manufacture.component';
import { ReasonOfApprovalInOutComponent } from './asset-in-out-manament/reason-of-approval-inout/reason-of-approval-inout.component';
import { ConfirmCancelRequestComponent } from './request-asset-bring/detail-request-asset-bring/confirm-cancel-request/confirm-cancel-request.component';
import { ViewImageComponent } from './request-asset-bring/detail-request-asset-bring/view-image/view-image.component';

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        AssetManamentRoutingModule,
        CountoModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot(),
        TableModule,
        PaginatorModule,
        ReactiveFormsModule,
        ButtonModule,
        CalendarModule,
        InputTextareaModule,
        CheckboxModule,
        TabViewModule,
        MasterRoutingModule,
        AdminSharedModule,
        ZXingScannerModule,
        DropdownModule,
        RadioButtonModule
    ],
    declarations: [
        RequestAssetBringComponent,
        CreateOrEditRequestAssetBringComponent,
        DetailRequestAssetBringComponent,
        AddRowDirective,
        ListAssetComponent,
        ListWorkersComponent,
        ListWorkersInOutComponent,
        ListAssetInOutComponent,
        AssetOutManamentComponent,
        ReasonOfRefusalModalComponent,
        CreateAssetBarCodeComponent,
        DetailHistoryEmployeesInOutComponent,
        DetailHistoryAssetInOutComponent,
        ListRequestInOutComponent,
        AssetInManamentComponent,
        ScannerBarCodeComponent,
        MessageAgreeComponent,
        InOutManamentComponent,
        ListRequestOutComponent,
        ListRequestInComponent,
        ListAssetInComponent,
        ListAssetOutComponent,
        ListWorkersInComponent,
        ListWorkersOutComponent,
        MessageImportErrorComponent,
        SendMailToManufactureComponent,
        ReasonOfApprovalInOutComponent,
        ConfirmCancelRequestComponent,
        ViewImageComponent
        
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class AssetManamentModule { }
