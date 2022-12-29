import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SystemFile } from '../_models/SystemFile';
import { API_URL } from '../env';

@Injectable({
  providedIn: 'root'
})
export class BoardService {
  constructor(private http: HttpClient) { }

  sendFile(file: SystemFile): Observable<any> {
    return this.http.post(API_URL + 'signin', file);
  }
}
