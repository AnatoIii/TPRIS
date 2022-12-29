import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenStorageService } from '../_services/token-storage.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(public tokenService: TokenStorageService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const tokenValue = this.tokenService.getToken();

    if (tokenValue !== null) {
      request.headers.append('Authorization', `Bearer ${tokenValue}`);
    }
    return next.handle(request);
  }
}
