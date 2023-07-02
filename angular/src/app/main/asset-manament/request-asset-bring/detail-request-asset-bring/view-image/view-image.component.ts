import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'view-image',
  templateUrl: './view-image.component.html',
  styleUrls: ['./view-image.component.less']
})
export class ViewImageComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  imageArr;
  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.imageArr ="";
  }

  show(imageArr): void {
    this.imageArr = imageArr;
    this.modal.show();
  }

  close(): void {
    this.modal.hide();
  }
}
