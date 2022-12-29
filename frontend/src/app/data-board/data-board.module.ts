import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataListComponent } from './data-list/data-list.component';
import { DataItemComponent } from './data-item/data-item.component';
import { CreateEditDialogComponent } from './create-edit-dialog/create-edit-dialog.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    DataListComponent,
    DataItemComponent,
    CreateEditDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ]
})
export class DataBoardModule { }
