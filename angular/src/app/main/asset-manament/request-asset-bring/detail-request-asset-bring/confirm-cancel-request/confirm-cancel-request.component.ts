import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ApproveOrRejectRequestDto, RequestAssetBringServiceProxy } from '@shared/service-proxies/service-proxies';
import { result } from 'lodash-es';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'confirm-cancel-request',
  templateUrl: './confirm-cancel-request.component.html',
  styleUrls: ['./confirm-cancel-request.component.css']
})
export class ConfirmCancelRequestComponent extends AppComponentBase {
  @ViewChild("reasonOfRefusal", { static: true }) modal: ModalDirective;
  @Output() modalEdit: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalDelete: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalSaveDraft: EventEmitter<any> = new EventEmitter<any>();
  // indexControl: number=0;
  @Input() requestId;
  constructor(injector: Injector,
   ) {
    super(injector);
  }

  open() {
    this.modal.show();
    // this.indexControl=i;
  }

  close() {
    this.modal.hide();

    // if(this.indexControl)
    // {
    //   this.modalClose.emit(this.indexControl)
    // }
  }

  edit()
  {
    this.modalEdit.emit(this.requestId);
    this.close();
  }

  delete()
  {
    // alert("das");
    
    this.modalDelete.emit(this.requestId);
    this.close();

  }

  saveDraft()
  {
    this.modalSaveDraft.emit(this.requestId);
    this.close();
  }

}
