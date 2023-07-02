import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AssetInManamentComponent } from './asset-in-out-manament/asset-in-manament/asset-in-manament.component';
import { AssetOutManamentComponent } from './asset-in-out-manament/asset-out-manament/asset-out-manament.component';
import { InOutManamentComponent } from './asset-in-out-manament/in-out-manament/in-out-manament.component';
import { RequestAssetBringComponent } from './request-asset-bring/request-asset-bring.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'app-request-asset-bring/:id/:st_request', component: RequestAssetBringComponent },
                    { path: 'app-asset-in-manament', component: AssetInManamentComponent },
                    { path: 'app-asset-out-manament', component: AssetOutManamentComponent },
                    { path: 'app-in-out-manament', component: InOutManamentComponent }
                    
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class AssetManamentRoutingModule { }
