import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SystemFile } from '../_models/SystemFile';

const API_URL = 'http://localhost:8080/api/test/';

@Injectable({
  providedIn: 'root'
})
export class BoardService {
  constructor(private http: HttpClient) { }

  sendFile(file: SystemFile): Observable<any> {
    return this.http.post(API_URL + 'signin', file);
  }
}
