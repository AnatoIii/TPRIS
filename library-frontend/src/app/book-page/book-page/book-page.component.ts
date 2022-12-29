import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from 'src/app/library-page/models/Book';
import { ChatService } from 'src/app/library-page/services/chat.service';
import { LibraryService } from 'src/app/library-page/services/library.service';

@Component({
  selector: 'app-book-page',
  templateUrl: './book-page.component.html',
  styleUrls: ['./book-page.component.scss']
})
export class BookPageComponent implements OnInit {
  book: Book

  constructor(
    private libraryService: LibraryService,
    private activatedRoute: ActivatedRoute,
    private chatService: ChatService,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(el => {
      var id = el.get('id');

      if (!!id) {
        this.libraryService.getBookById(el.get('id')).subscribe(result => {
          this.book = result;
        })
      }
    });
  }

  firstLetter() {
    return this.book.description[0];
  }

  searchByAuthor() {
    this.router.navigate([''], { queryParams: { author: this.book.author } });
  }

  searchByGenre(genreId: number) {
    this.router.navigate([''], { queryParams: { genreId: genreId } });
  }

  description() {
    return this.book.description.substr(1);
  }

  goToChat(event: Event): void {
    event.stopImmediatePropagation();
    
    // window.open(`http://localhost:4200/chat/${this.book.chatId}`,"_self")
    // this.router.navigate(['/chat', this.book.chatId]);
    this.chatService.assignToChat(this.book.chatId).subscribe(_ => {
      alert(`Successfully added to chat "${this.book.title}"`);
    });
  }
}


