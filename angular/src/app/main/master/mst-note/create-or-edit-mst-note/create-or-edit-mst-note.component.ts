import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalValidator } from '@app/shared/validators';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ToastService } from '@shared/common/ui/toast.service';
import { AssetGroupSelectOutputDto, AssetGroupServiceProxy, NoteSelectOutputDto, NoteTextServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'CreateOrEditMstNote',
  templateUrl: './create-or-edit-mst-note.component.html',
  styleUrls: ['./create-or-edit-mst-note.component.less']
})
export class CreateOrEditMstNoteComponent extends AppComponentBase {
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active: Boolean = false;
  saving: Boolean = false;
  noteForm: FormGroup;
  @Input() isLoading;
  mstNote: NoteSelectOutputDto = new NoteSelectOutputDto();
  constructor(
    injector: Injector,
    private _mstNoteService: NoteTextServiceProxy,
    private formBuilder: FormBuilder,
    private toastService: ToastService
  ) {
    super(injector);
  }

  ngOnInit() {
  }
  isEdit: Boolean = true;

  show(id): void {
    this.validation();
    if (!id) {
      this.isEdit = false;
      this.mstNote = new NoteSelectOutputDto();
      this.active = true;
      this.modal.show();
    }
    else {
      this._mstNoteService.loadById(id).subscribe((result) => {
        this.mstNote.id = result.id;
        this.mstNote.noteText = result.noteText;
        this.isEdit = true;
        this.active = true;
        this.modal.show();
      }
      );
    }
  }

  validation(): void {
    this.noteForm = this.formBuilder.group({
      NoteText: [undefined, GlobalValidator.required]
    });
  }

  save(): void {
    this.saving = true;
    this.isLoading = true;
    this._mstNoteService.save(this.mstNote).pipe(
      finalize(() => {
        this.saving = false;
      })).subscribe(() => {
        // this.notify.info(this.l(AppConsts.message.saveSuccess));
        this.toastService.openSuccessToast(this.l(AppConsts.message.saveSuccess));
        this.close();
        this.modalSave.emit(null);
        this.mstNote = new NoteSelectOutputDto();
        this.saving = false;
      });
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
