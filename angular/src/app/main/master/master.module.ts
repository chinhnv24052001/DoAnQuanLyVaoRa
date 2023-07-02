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
import { MasterRoutingModule } from './master-routing.module';
import { VenderComponent } from './vender/vender.component';
import { CreateOrEditVenderComponent } from './vender/create-or-edit-vender/create-or-edit-vender.component';
import { CreateOrEditEmployeesComponent } from './employees/create-or-edit-employees/create-or-edit-employees.component';
import { EmloyeesComponent } from './employees/employees.component';
import { AssetGroupComponent } from './asset-group/asset-group.component';
import { CreateOrEditAssetGroupComponent } from './asset-group/create-or-edit-asset-group/create-or-edit-asset-group.component';
import { AssetComponent } from './asset/asset.component';
import { CreateOrEditAssetComponent } from './asset/create-or-edit-asset/create-or-edit-asset.component';
import { ButtonModule } from 'primeng/button';
import { CreateOrEditCourceSafetyComponent } from './cource-safety/create-or-edit-cource-safety/create-or-edit-cource-safety.component';
import { CourceSafetyComponent } from './cource-safety/cource-safety.component';
import { EmployeesLearnedSafetyComponent } from './cource-safety/employees-learned-safety/employees-learned-safety.component';
import { CreateOrEditEmployeesLearnedSafetyComponent } from './cource-safety/employees-learned-safety/create-or-edit-employees-learned-safety/create-or-edit-employees-learned-safety.component';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { DynamicEntityPropertyValueModule } from '@app/admin/dynamic-properties/dynamic-entity-properties/value/dynamic-entity-property-value.module';
import { TemEmployeesLearnedSafetyComponent } from './cource-safety/employees-learned-safety/tem-employees-learned-safety/tem-employees-learned-safety.component';
import { CalendarModule } from 'primeng/calendar';
import {InputTextareaModule} from 'primeng/inputtextarea';
import { FileUploadModule } from 'primeng/fileupload';
import { DropdownModule } from 'primeng/dropdown';
import { MstEmployeesLearnedSafetyComponent } from './mst-employees-learned-safety/mst-employees-learned-safety.component';
import { MstNoteComponent } from './mst-note/mst-note.component';
import { CreateOrEditMstNoteComponent } from './mst-note/create-or-edit-mst-note/create-or-edit-mst-note.component';
import { TemAssetImportComponent } from './asset/tem-asset-import/tem-asset-import.component';
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
        MasterRoutingModule,
        CountoModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot(),
        TableModule,
        PaginatorModule,
        ReactiveFormsModule,
        ButtonModule,
        AdminSharedModule,
        CalendarModule,
        InputTextareaModule,
        DropdownModule
    ],
    declarations: [
        VenderComponent,
        CreateOrEditVenderComponent,
        CreateOrEditEmployeesComponent,
        EmloyeesComponent,
        AssetGroupComponent,
        CreateOrEditAssetGroupComponent,
        AssetComponent,
        CreateOrEditAssetComponent,
        CreateOrEditCourceSafetyComponent,
        CourceSafetyComponent,
        EmployeesLearnedSafetyComponent,
        CreateOrEditEmployeesLearnedSafetyComponent,
        TemEmployeesLearnedSafetyComponent,
        MstEmployeesLearnedSafetyComponent,
        MstNoteComponent,
        CreateOrEditMstNoteComponent,
        TemAssetImportComponent
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class MasterModule { }
