import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Book } from 'src/app/library-page/models/Book';
import { AddBook } from 'src/app/library-page/models/books/AddBook';
import { Genre } from 'src/app/library-page/models/Genre';
import { ChatService } from 'src/app/library-page/services/chat.service';

@Component({
  selector: 'app-add-book-dialog',
  templateUrl: './add-book-dialog.component.html',
  styleUrls: ['./add-book-dialog.component.scss']
})
export class AddBookDialogComponent implements OnInit {
  bookForm: FormGroup;
  genresList: Genre[];
  selectedGenres: Genre[];

  constructor(
    public dialogRef: MatDialogRef<AddBookDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public genres: Genre[],
    private formBuilder: FormBuilder) {
      this.genresList = genres;
    }

  onNoClick(): void {
    this.dialogRef.close();
  }
  
  ngOnInit() {
    this.bookForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.min(3)]],
      description: ['', [Validators.required, Validators.min(10)]],
      author: ['', [Validators.required, Validators.min(3)]],
      imageLink: ['', [Validators.required, Validators.min(3)]],
      genres: [[]], //this.formBuilder.array([]),
    });
  }

  onSubmit() {
    (<any>Object).values(this.bookForm.controls).forEach(control => {
      control.markAsTouched();
    });
    
    if (this.bookForm.invalid) {
      return;
    }

    const book: AddBook = {
      title: this.bookForm.value.title,
      description: this.bookForm.value.description,
      genres: this.bookForm.value.genres,
      author: this.bookForm.value.author,
      imageLink: this.bookForm.value.imageLink
    };

    this.dialogRef.close(book);
  }
}
