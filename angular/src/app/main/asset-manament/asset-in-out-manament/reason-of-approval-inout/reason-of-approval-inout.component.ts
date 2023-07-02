import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { NoteTextServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-reason-of-approval-inout',
  templateUrl: './reason-of-approval-inout.component.html',
  styleUrls: ['./reason-of-approval-inout.component.less']
})
export class ReasonOfApprovalInOutComponent extends AppComponentBase implements OnInit {
  @ViewChild("reasonOfApproval", { static: true }) modal: ModalDirective;
  @Output() modalCallBackSetInOut: EventEmitter<any> = new EventEmitter<any>();
  @Input() isLoading;
  formNote: FormGroup;
  listNoteDropDown: { value: number, label: string }[];
  reasonDto: {id: number, noteId: number, stringInOut: string }={id: 0, noteId: 0, stringInOut: ''};
  saving: Boolean = false;
  resultQR;
  noteId: number=0;
  selectNoteText: string=this.l('SelectNoteText');
  constructor(injector: Injector,
    private _noteService: NoteTextServiceProxy,
    private formBuilder: FormBuilder,) {
    super(injector);
  }

  ngOnInit(): void {
    this.selectDropDownNoteTexts();
    this.buildForm();
  }

  selectDropDownNoteTexts() {
    this._noteService.getNoteTextForSelect().subscribe((result) => {
      this.listNoteDropDown = [];
      this.listNoteDropDown.push({ value: 0, label: " " });
      result.forEach(ele => {
        this.listNoteDropDown.push({ value: ele.id, label: ele.noteText });
      });
    });
  }

  show(id, stringInOut) {
    this.buildForm();
    // this.resultQR=item;
    this.reasonDto.id=id;
    this.reasonDto.stringInOut= stringInOut;
    this.modal.show();
  }

  buildForm() {
    this.formNote = this.formBuilder.group({
      NoteId: [undefined, GlobalValidator.required]
    });
  }

  confirmInOut()
  {
    this.modal.hide();
    this.modalCallBackSetInOut.emit(this.reasonDto);
  }

  close() {
    this.modal.hide();
  }

}
