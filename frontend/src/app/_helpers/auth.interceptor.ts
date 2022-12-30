import { HTTP_INTERCEPTORS, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';

import { TokenStorageService } from '../_services/token-storage.service';
import { catchError, Observable, throwError } from 'rxjs';

const TOKEN_HEADER_KEY = 'xxx-authorization';       // for Spring Boot back-end

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private token: TokenStorageService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // const token = this.token.getToken();
    // if (token != null) {
    //   // for Spring Boot back-end
    //   // authReq = req.clone({ headers: req.headers.set(TOKEN_HEADER_KEY, 'Bearer ' + token) });

    //   // for Node.js Express back-end
    //   req = req.clone({
    //     setHeaders: {Authorization: `${token}`}
    //  });
    // }
    // return next.handle(req);
    const token = this.token.getToken();

    if (token) {
      // If we have a token, we set it to the header
      request = request.clone({
         setHeaders: {Authorization: `Bearer ${token}`}
      });
   }
 
   return next.handle(request).pipe(
     catchError((err) => {
       if (err instanceof HttpErrorResponse) {
           if (err.status === 401) {
           // redirect user to the logout page
        }
     }
     return throwError(err);
   })
    )
  }
}