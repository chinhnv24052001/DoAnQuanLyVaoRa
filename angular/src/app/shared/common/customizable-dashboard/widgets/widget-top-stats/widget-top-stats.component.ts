import { Component, OnInit, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CourceSafetyServiceProxy, RequestAssetBringServiceProxy, TenantDashboardServiceProxy, VenderServiceProxy } from '@shared/service-proxies/service-proxies';
import { DashboardChartBase } from '../dashboard-chart-base';
import { WidgetComponentBaseComponent } from '../widget-component-base';


class DashboardTopStats extends DashboardChartBase {

  totalProfit = 0; totalProfitCounter = 0;
  newFeedbacks = 0; newFeedbacksCounter = 0;
  newOrders = 0; newOrdersCounter = 0;
  newUsers = 0; newUsersCounter = 0;

  totalProfitChange = 76; totalProfitChangeCounter = 0;
  newFeedbacksChange = 85; newFeedbacksChangeCounter = 0;
  newOrdersChange = 45; newOrdersChangeCounter = 0;
  newUsersChange = 57; newUsersChangeCounter = 0;

  init(totalProfit, newFeedbacks, newOrders, newUsers) {
    this.totalProfit = totalProfit;
    this.newFeedbacks = newFeedbacks;
    this.newOrders = newOrders;
    this.newUsers = newUsers;
    this.hideLoading();
  }
}

@Component({
  selector: 'app-widget-top-stats',
  templateUrl: './widget-top-stats.component.html',
  styleUrls: ['./widget-top-stats.component.css']
})
export class WidgetTopStatsComponent extends WidgetComponentBaseComponent implements OnInit {

  dashboardTopStats: DashboardTopStats;
  countVender;
  countCource;
  countDraftReq;
 

  constructor(injector: Injector,
    private _tenantDashboardServiceProxy: TenantDashboardServiceProxy,
    private _venderService: VenderServiceProxy,
    private _courceSafetyService: CourceSafetyServiceProxy,
    private _requestAssetBringServiceProxy: RequestAssetBringServiceProxy,
    private router: Router
  ) {
    super(injector);
    this.dashboardTopStats = new DashboardTopStats();
  }

  ngOnInit() {
    this.loadTopStatsData();
    this.fnCountVender();
    this.fnCountCource();
    this.fnCountDraftReq();
  }

  fnCountVender() {
    this._venderService.countVender().subscribe((result)=>{
      this.countVender=result;
    });
  }

  fnCountCource() {
    this._courceSafetyService.countCource().subscribe((result)=>{
      this.countCource=result;
    });
  }

  fnCountDraftReq() {
    this._requestAssetBringServiceProxy.countDraftRequest().subscribe((result)=>{
      this.countDraftReq=result;
    });
  }

  loadTopStatsData() {
    this._tenantDashboardServiceProxy.getTopStats().subscribe((data) => {
      this.dashboardTopStats.init(data.totalProfit, data.newFeedbacks, data.newOrders, data.newUsers);
    });
  }

  test()
  {
    this.router.navigate(['../master/app-vender'])
  }
}
