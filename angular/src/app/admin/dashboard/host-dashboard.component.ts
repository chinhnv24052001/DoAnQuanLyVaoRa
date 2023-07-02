import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardCustomizationConst } from '@app/shared/common/customizable-dashboard/DashboardCustomizationConsts';
import { CourceSafetyServiceProxy, RequestAssetBringServiceProxy, VenderServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './host-dashboard.component.html',
    styleUrls: ['./host-dashboard.component.less'],
    encapsulation: ViewEncapsulation.None
})
export class HostDashboardComponent extends AppComponentBase {
    dashboardName = DashboardCustomizationConst.dashboardNames.defaultHostDashboard;

    constructor(injector: Injector
       
        ) {
        super(injector);
    }
}
