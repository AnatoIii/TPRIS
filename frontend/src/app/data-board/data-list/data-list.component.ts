import { Component, OnInit } from '@angular/core';
import { DialogService } from 'src/app/dialog/dialog.service';
import { SystemFile } from 'src/app/_models/SystemFile';
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
  constructor(public dialog: DialogService) { }

  ngOnInit(): void {
    this.files = [
      {
        Id: 1,
        Name: 'File 1',
        Author: 'Author 1',
        Content: 'asdasdasd',
        Description: 'DEsctipadhasd'
      },
      {
        Id: 1,
        Name: 'File 1',
        Author: 'Author 1',
        Content: 'asdasdasd',
        Description: 'DEsctipadhasd'
      },
      {
        Id: 1,
        Name: 'File 1',
        Author: 'Author 1',
        Content: 'asdasdasd',
        Description: 'DEsctipadhasd'
      },
      {
        Id: 1,
        Name: 'File 1',
        Author: 'Author 1',
        Content: 'asdasdasd',
        Description: 'DEsctipadhasd'
      },
      {
        Id: 1,
        Name: 'File 1',
        Author: 'Author 1',
        Content: 'asdasdasd',
        Description: 'DEsctipadhasd'
      }
    ]
    this.searchedFiles = this.files;
  }

  openDialog() {
    const ref = this.dialog.open(CreateEditDialogComponent, {data: {isCreate: true} });

    ref.afterClosed.subscribe(result => {
      console.log('Dialog closed', result);
    });
  }

  onEdit(item: number) {
    const activeFile = this.files.filter(el => el.Id === item)[0];
    console.log(activeFile);
    const ref = this.dialog.open(CreateEditDialogComponent, {
      data: { isCreate: false, file: activeFile }
    });

    ref.afterClosed.subscribe(result => {
      console.log('Dialog closed', result);
    });
    console.log(item);
  }

  searchOnChange(text: string) {
    const searchText = text.toLowerCase();

    this.searchedFiles = this.files.filter(el => 
      el.Name?.toLowerCase().includes(searchText)
      || el.Author?.toLowerCase().includes(searchText)
      || el.Description?.toLowerCase().includes(searchText))
  }
}
