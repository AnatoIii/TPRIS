import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BookPageComponent } from './book-page/book-page/book-page.component';
import { BookListComponent } from './library-page/components/book-list/book-list.component';

const routes: Routes = [
  {
    path: '', component: BookListComponent
  }, 
  {
    path: 'book/:id', component: BookPageComponent
  },
  {
    path: '**', redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
