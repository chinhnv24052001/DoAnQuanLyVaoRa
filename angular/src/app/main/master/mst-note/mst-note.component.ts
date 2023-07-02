import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';
import { AssetGroupServiceProxy, GetAssetGroupInputDto, NoteInputDto, NoteTextServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { CreateOrEditMstNoteComponent } from './create-or-edit-mst-note/create-or-edit-mst-note.component';
import { ToastService } from '@shared/common/ui/toast.service';

@Component({
  selector: 'app-mst-note',
  templateUrl: './mst-note.component.html',
  styleUrls: ['./mst-note.component.less'],
  animations: [appModuleAnimation()]
})
export class MstNoteComponent extends AppComponentBase implements AfterViewInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('CreateOrEditMstNote', { static: true }) CreateOrEditMstNote: CreateOrEditMstNoteComponent;
  filterText: string = '';
  isLoading: boolean = false;
  indexShort: number=0;
  note: NoteInputDto = new NoteInputDto();
  constructor(
    injector: Injector,
    private _noteService: NoteTextServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private toastService: ToastService
  ) {
    super(injector);
    this.filterText = this._activatedRoute.snapshot.queryParams['filterText'] || '';
  }

  ngAfterViewInit(): void {
    this.primengTableHelper.adjustScroll(this.dataTable);
  }

  create() {
    this.CreateOrEditMstNote.show(0);
  }

  edit(id) {
    this.CreateOrEditMstNote.show(id);
  }

  delete(id) {
    this.message.confirm('', this.l(AppConsts.message.confimDelete), (isConfirmed) => {
      if (isConfirmed) {
        this.isLoading = true;
        this._noteService.deleteById(id).subscribe(() => {
          this.getNoteText();
          // this.notify.success(this.l(AppConsts.message.deleteSuccess));
          this.toastService.openSuccessToast(this.l(AppConsts.message.deleteSuccess));
        });
      }
    });
  }

  getNoteText(event?: LazyLoadEvent) {
    this.primengTableHelper.showLoadingIndicator();
    this.note.filter = this.filterText;
    this.note.maxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event) ? this.primengTableHelper.getMaxResultCount(this.paginator, event) : 20;
    this.note.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event) ? this.primengTableHelper.getSkipCount(this.paginator, event) : 0;
    this._noteService.loadAll(this.note).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator())).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
      this.indexShort=this.note.skipCount;
    });
    this.isLoading = false;
  }
}
