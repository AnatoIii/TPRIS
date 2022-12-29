import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Book } from '../models/Book';
import { BookData } from '../models/BookData';
import { AddBook } from '../models/books/AddBook';
import { Filter } from '../models/Filter';
import { Genre } from '../models/Genre';

@Injectable({
  providedIn: 'root'
})
export class LibraryService {
  constructor(private http: HttpClient) { }

  getBooks(filter: Filter) {
    return this.http.post<BookData>("books/filter", filter);
  }
  
  getBookById(id: string) {
    return this.http.get<Book>(`books/${id}`);
  }

  getGenres() {
    return this.http.get<Genre[]>('books/genres');
  }
  
  addBook(book: AddBook) {
    return this.http.post<Book>('books', book);
  }
}
