import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ReportComponent } from './report/report.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    {
                        path: 'master',
                        loadChildren: () => import('./master/master.module').then(m => m.MasterModule), //Lazy load main module
                        data: { preload: true }
                    },
                    {
                        path: 'asset-manament',
                        loadChildren: () => import('./asset-manament/asset-manament.module').then(m => m.AssetManamentModule), //Lazy load main module
                        data: { preload: true }
                    },
                    { path: 'report', component: ReportComponent, data: { permission: '' } },
                    { path: '**', redirectTo: 'dashboard' },
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
