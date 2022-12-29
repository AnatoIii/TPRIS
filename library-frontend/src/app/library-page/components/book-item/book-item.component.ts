import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from '../../models/Book';
import { ChatService } from '../../services/chat.service';

@Component({
  selector: 'app-book-item',
  templateUrl: './book-item.component.html',
  styleUrls: ['./book-item.component.scss']
})
export class BookItemComponent implements OnInit {
  @Input() book: Book;

  constructor(private router: Router, private chatService: ChatService) { }

  ngOnInit(): void {
  }

  openBook(): void {
    this.router.navigateByUrl(`/book/${this.book.bookId}`);
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
