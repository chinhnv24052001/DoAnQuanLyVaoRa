import { Component, OnInit, Injector } from '@angular/core';
import { DashboardChartBase } from '../dashboard-chart-base';
import { TenantDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { WidgetComponentBaseComponent } from '../widget-component-base';
import { Router } from '@angular/router';

// class GeneralStatsPieChart extends DashboardChartBase {

//   public data = [];

//   constructor(private _dashboardService: TenantDashboardServiceProxy,  private router: Router) {
//     super();
//   }

//   init(transactionPercent, newVisitPercent, bouncePercent) {
//     this.data = [
//       {
//         'name': 'Operations',
//         'value': transactionPercent
//       }, {
//         'name': 'New Visits',
//         'value': newVisitPercent
//       }, {
//         'name': 'Bounce',
//         'value': bouncePercent
//       }];

//     this.hideLoading();
//   }

//   test()
//   {
//     this.router.navigate(['../master/app-vender'])
//   }

//   reload() {
//     this.showLoading();
//     this._dashboardService
//       .getGeneralStats()
//       .subscribe(result => {
//         this.init(result.transactionPercent, result.newVisitPercent, result.bouncePercent);
//       });
//   }
// }

@Component({
  selector: 'app-widget-general-stats',
  templateUrl: './widget-general-stats.component.html',
  styleUrls: ['./widget-general-stats.component.css']
})
export class WidgetGeneralStatsComponent extends WidgetComponentBaseComponent implements OnInit {

  constructor(injector: Injector,
    private _dashboardService: TenantDashboardServiceProxy, private router: Router) 
  {
    super(injector);
  }

  ngOnInit() {

  }

  fnToAssetGroupView()
    {
      this.router.navigate(['/app/main/master/app-asset-group'])
    }
 
    fnToAssetView()
    {
      this.router.navigate(['/app/main/master/app-asset'])
    }

    fnToAssetMnInView()
    {
      this.router.navigate(['/app/main/asset-manament/app-asset-in-manament'])
    }

    fnToAssetMnOutView()
    {
      this.router.navigate(['/app/main/asset-manament/app-asset-out-manament'])
    }

    fnToReportView()
    {
      this.router.navigate(['/app/main/report'])
    }

}
