import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookPageComponent } from './book-page/book-page.component';
import { MaterialDataModule } from '../material-data/material-data.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [BookPageComponent],
  imports: [
    CommonModule,
    MaterialDataModule,
    FormsModule
  ],
  exports: [
    BookPageComponent
  ]
})
export class BookPageModule { }
