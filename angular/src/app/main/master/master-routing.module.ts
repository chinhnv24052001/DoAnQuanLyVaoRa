import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { EmloyeesComponent } from './employees/employees.component';
import { VenderComponent } from './vender/vender.component';
import { AssetGroupComponent } from './asset-group/asset-group.component';
import { AssetComponent } from './asset/asset.component';
import { CourceSafetyComponent } from './cource-safety/cource-safety.component';
import { MstEmployeesLearnedSafetyComponent } from './mst-employees-learned-safety/mst-employees-learned-safety.component';
import { MstNoteComponent } from './mst-note/mst-note.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'app-vender', component: VenderComponent },
                    { path: 'app-employees', component: EmloyeesComponent },
                    { path: 'app-asset-group', component: AssetGroupComponent },
                    { path: 'app-asset', component: AssetComponent },
                    { path: 'app-cource-safety', component: CourceSafetyComponent },
                    { path: 'MstEmployeesLearnedSafety', component: MstEmployeesLearnedSafetyComponent },
                    { path: 'app-mst-note', component: MstNoteComponent },
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MasterRoutingModule { }
