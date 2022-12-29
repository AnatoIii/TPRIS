import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderModule } from './header/header.module';
import { LibraryPageModule } from './library-page/library-page.module';
import { MaterialDataModule } from './material-data/material-data.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { environment } from 'src/environments/environment';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UrlInterceptor } from './interceptors/url.interceptor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BookPageModule } from './book-page/book-page.module';
import { AddBookDialogComponent } from './components/dialogs/add-book-dialog/add-book-dialog.component';
import { TokenInterceptor } from './interceptors/token.Interceptor';

@NgModule({
  declarations: [
    AppComponent,
    AddBookDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HeaderModule,
    LibraryPageModule,
    BookPageModule,
    MaterialDataModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: UrlInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    { provide: "BASE_API_URL", useValue: environment.apiUrl }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
