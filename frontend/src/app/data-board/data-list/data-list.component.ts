import { Component, OnInit } from '@angular/core';
import { DialogService } from 'src/app/dialog/dialog.service';
import { SystemFile } from 'src/app/_models/SystemFile';
import { BoardService } from 'src/app/_services/board.service';
import { CreateEditDialogComponent } from '../create-edit-dialog/create-edit-dialog.component';

@Component({
  selector: 'app-data-list',
  templateUrl: './data-list.component.html',
  styleUrls: ['./data-list.component.css']
})
export class DataListComponent implements OnInit {
  files: SystemFile[] = []
  searchedFiles: SystemFile[] = []
  searchText = ''
  constructor(public dialog: DialogService, public boardService: BoardService) { }

  ngOnInit(): void {
    this.loadAllFiles();
  }

  loadAllFiles() {
    this.boardService.getAllFiles().subscribe(data => {
      console.log(data);
      this.files = data;
      this.searchedFiles = this.files;
    });
  }

  openDialog() {
    const ref = this.dialog.open(CreateEditDialogComponent, {data: {isCreate: true} });

    ref.afterClosed.subscribe(result => {
      console.log('Dialog closed', result);
      if (result) {
        this.loadAllFiles();
      }
    });
  }

  onEdit(item: number) {
    const activeFile = this.files.filter(el => el.fileId === item)[0];
    console.log(activeFile);
    const ref = this.dialog.open(CreateEditDialogComponent, {
      data: { isCreate: false, file: activeFile }
    });

    ref.afterClosed.subscribe(result => {
      console.log('Dialog closed', result);
      if (result) {
        this.loadAllFiles();
      }
    });
  }

  searchOnChange(text: string) {
    const searchText = text.toLowerCase();

    this.searchedFiles = this.files.filter(el => 
      el.name?.toLowerCase().includes(searchText)
      || el.author?.toLowerCase().includes(searchText)
      || el.description?.toLowerCase().includes(searchText))
  }
}
