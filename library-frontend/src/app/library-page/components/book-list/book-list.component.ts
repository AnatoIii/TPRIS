import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { AddBookDialogComponent } from 'src/app/components/dialogs/add-book-dialog/add-book-dialog.component';
import { Book } from '../../models/Book';
import { AddBook } from '../../models/books/AddBook';
import { Filter } from '../../models/Filter';
import { Genre } from '../../models/Genre';
import { LibraryService } from '../../services/library.service';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss']
})
export class BookListComponent implements OnInit, OnDestroy {
  private booksOnPage = 8;

  public bookList: Book[];
  public genres: Genre[];
  public searchBookName: string;
  public authorName: string;
  public selectedGenres: number[] = [];
  public booksCount: number;
  public currentPage = 1;

  private getBookSubscription: Subscription;
  private searchBooksSubject = new Subject();

  constructor(
    private libraryService: LibraryService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog
  ) {
    this.activatedRoute.queryParams.subscribe(params => {
      if (params['author'] != null) {
        this.authorName = params['author'];
      }

      if (params['genreId'] != null) {
        const genreId = params['genreId'];
        this.selectedGenres = [+genreId];
      }
    });
  }

  ngOnInit(): void {
    this.router.navigateByUrl('');
    this.loadBooks();

    this.libraryService.getGenres().subscribe(result => {
      this.genres = result;
    });

    this.getBookSubscription = this.searchBooksSubject.pipe(
      debounceTime(1000),
      map(() => this.loadBooks())
    ).subscribe();
  }

  private loadBooks() {
    const filter = this.formFilter();

    this.libraryService.getBooks(filter).subscribe(result => {
      this.bookList = result.bookTOs;
      this.booksCount = result.count;
    }); 
  }

  private formFilter(): Filter {
    return {
      author: this.authorName ?? "",
      genres: this.selectedGenres,
      title: this.searchBookName ?? "",
      offset: (this.currentPage - 1) * this.booksOnPage,
      count: this.booksOnPage
    }
  }

  bookNameChange(event) {
    this.searchBookName = event;
    this.searchBooksSubject.next();
  }

  authorNameChange(event) {
    this.authorName = event;
    this.searchBooksSubject.next();
  }

  checkGenre(genreId) {
    if (this.selectedGenres.includes(genreId)) {
      this.selectedGenres = this.selectedGenres.filter(genre => genre != genreId);
    } else {
      this.selectedGenres = [...this.selectedGenres, genreId];
    }
    this.searchBooksSubject.next();
  }

  getPagesArray() {
    if (!this.booksCount) {
      return [];
    }
    const pagesAmount = Math.ceil(this.booksCount / this.booksOnPage);
    if (pagesAmount == 1) {
      return [];
    }

    if (this.currentPage < 5) {
      return this.getNumbers(Math.min(this.currentPage + 4, pagesAmount));
    }

    return this.getNumbers(Math.min(this.currentPage + 4, pagesAmount)).slice(Math.max(this.currentPage - 4, 0));
  }

  selectPage(pageNumber: number) {
    this.currentPage = pageNumber;

    this.searchBooksSubject.next();
  }

  activeGenre(genreId: number) {
    return this.selectedGenres.includes(genreId);
  }

  getNumbers(to: number) {
    return Array.from(Array(to).keys()).map(el => el + 1);
  }

  ngOnDestroy() {
    this.getBookSubscription.unsubscribe();
  }

  addBook() {
    const dialogRef = this.dialog.open(AddBookDialogComponent, {
      width: '500px',
      data: this.genres,
      panelClass: 'add-book-dialog',
      backdropClass: 'add-book-dialog-backdrop'
    });

    dialogRef.afterClosed().subscribe((result: AddBook) => {
      console.log('The dialog was closed', result);

      if (result != null) {
        this.libraryService.addBook(result).subscribe(res => {
          console.log(res);
          this.bookList = [res, ...this.bookList];
        })
      }
    });
  }
}