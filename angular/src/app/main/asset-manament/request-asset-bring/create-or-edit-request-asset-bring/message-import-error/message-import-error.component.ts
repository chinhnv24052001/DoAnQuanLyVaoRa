import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'message-import-error',
  templateUrl: './message-import-error.component.html',
  styleUrls: ['./message-import-error.component.css']
})
export class MessageImportErrorComponent extends AppComponentBase {
  @ViewChild("reasonOfRefusal", { static: true }) modal: ModalDirective;
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
  indexControl: number=0;
  @Input() listErr;
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
