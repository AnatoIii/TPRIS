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

  createFile(file: SystemFile): Observable<any> {
    return this.http.post(API_URL + 'files', file);
  }

  editFile(file: SystemFile): Observable<any> {
    return this.http.put(API_URL + 'files', file);
  }

  getAllFiles(): Observable<any> {
    return this.http.get(API_URL + 'files');
  }
}
