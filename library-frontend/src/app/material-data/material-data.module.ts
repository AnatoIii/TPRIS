import { NgModule } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
  declarations: [],
  imports: [],
  exports: [
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatDialogModule,
    MatMenuModule,
    MatSelectModule
  ]
})
export class MaterialDataModule { }
