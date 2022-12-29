import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private http: HttpClient) { }

  assignToChat(chatId: string) {
    return this.http.post(`chats/assign?chatId=${chatId}`, {});
  }
}
