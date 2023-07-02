import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
  selector: 'report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css'],
  animations: [appModuleAnimation()]
})
export class ReportComponent extends AppComponentBase implements AfterViewInit {
  
  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngAfterViewInit(): void {
  }

  ngOnInit(): void {
  }


  
}
