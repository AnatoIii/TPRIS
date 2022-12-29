import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DialogConfig } from 'src/app/dialog/dialog-config';
import { DialogRef } from 'src/app/dialog/dialog-ref';
import { SystemFile } from 'src/app/_models/SystemFile';
import { BoardService } from 'src/app/_services/board.service';

@Component({
  selector: 'app-create-edit-dialog',
  templateUrl: './create-edit-dialog.component.html',
  styleUrls: ['./create-edit-dialog.component.css']
})
export class CreateEditDialogComponent implements OnInit {
  @Input() file: SystemFile;
  @Input() isCreate: boolean = true;

  form: any = {
    filename: null,
    author: null,
    description: null,
    fileData: null
  };
  errorMessage = '';
  fileLoadException = '';

  constructor(public config: DialogConfig, public dialog: DialogRef, private boardService: BoardService) { }

  ngOnInit(): void {
    this.file = this.config.data.file;
    this.isCreate = this.config.data.isCreate;

    if (!this.isCreate) {
      this.form.filename = this.file.Name;
      this.form.description = this.file.Description;
      this.form.author = this.file.Author;
    }
  }

  GetHeader(): string {
    return this.isCreate ? 'Add new file' : `Edit file '${this.file.Name}'`
  }

  onSubmit(): void {
    
    var file = (document.getElementById('#fileUploader') as any).files[0];
    var reader = new FileReader();
    reader.readAsText(file, "UTF-8");

    reader.onload = (evt: any) => {
      this.sendFile(btoa(evt.target.result));
    }
    reader.onerror = function (evt) {
      console.log('error reading file');
    }
  }

  sendFile(fileData?: string): void {
    const { filename, author, description } = this.form;

    var file: SystemFile = {
      Id: this.isCreate ? undefined : this.file.Id,
      Name: filename,
      Description: description,
      Author: author,
      Content: this.isCreate ? fileData : undefined
    };

    this.boardService.sendFile(file).subscribe((res: any) => {

    });
    this.dialog.close();
  }
}
