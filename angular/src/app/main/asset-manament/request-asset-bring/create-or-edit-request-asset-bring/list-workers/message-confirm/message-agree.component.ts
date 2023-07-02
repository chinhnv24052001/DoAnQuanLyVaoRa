import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ApproveOrRejectRequestDto, RequestAssetBringServiceProxy } from '@shared/service-proxies/service-proxies';
import { result } from 'lodash-es';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'message-agree',
  templateUrl: './message-agree.component.html',
  styleUrls: ['./message-agree.component.css']
})
export class MessageAgreeComponent extends AppComponentBase {
  @ViewChild("reasonOfRefusal", { static: true }) modal: ModalDirective;
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
  indexControl: number=0;
  @Input() message1;
  constructor(injector: Injector,
   ) {
    super(injector);
  }

  open(i) {
    this.modal.show();
    this.indexControl=i;
  }

  close() {
    this.modal.hide();
    if(this.indexControl)
    {
      this.modalClose.emit(this.indexControl)
    }
  }

}
