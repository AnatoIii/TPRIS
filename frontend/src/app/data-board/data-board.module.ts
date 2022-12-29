import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataListComponent } from './data-list/data-list.component';
import { DataItemComponent } from './data-item/data-item.component';



@NgModule({
  declarations: [
    DataListComponent,
    DataItemComponent
  ],
  imports: [
    CommonModule
  ]
})
export class DataBoardModule { }
