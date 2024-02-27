import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpStatusCode,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LoaderService } from '../componetns/loader-component/loader.service';
import { Router } from '@angular/router';
import { links } from './routes-resolver';
import { PageRoutes } from './page-routes';

export const apiErrorTitle = 'Error during API call!';

@Injectable()
export class GlobalHttpInterceptorService implements HttpInterceptor {
  constructor(
    private toastr: ToastrService,
    private loaderService: LoaderService,
    private router: Router,
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const authKey = localStorage.getItem('authKey');
    if (!!authKey?.length) {
      request = request.clone({
        setHeaders: {
          Authorization: authKey,
          'Content-Type': 'application/json',
          'Accept-Language': localStorage.getItem('overrideLocale') ?? localStorage.getItem('locale') ?? 'en',
        },
        url: request.url,
      });
    } else {
      request = request.clone({
        setHeaders: {
          'Content-Type': 'application/json',
          'Accept-Language': localStorage.getItem('overrideLocale') ?? localStorage.getItem('locale') ?? 'en',
        },
        url: request.url,
      });
    }

    return next
      .handle(request)
      .pipe(catchError((error) => this.handleError(error)));
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    this.loaderService.closeLoader();
    if (error.status === 0) {
      this.toastr.error('API is unavailable', apiErrorTitle);
    }
    else if (error.status === HttpStatusCode.Unauthorized) {
      const authKey = localStorage.getItem('authKey');
      var message = "Login required";
      if (!!authKey?.length) {
        message = "Your session has timed out, please log in again.";
      }
      this.toastr.info(message);
      this.router.navigate([links.get(PageRoutes.Login)]);
    }
    else {
      this.toastr.error(
        `API '${error.url}' returned code ${error.status}${
          !!error.error ? `, error body was: '${error.error}'` : ''
        }`,
        apiErrorTitle
      );
    }

    throw new Error(apiErrorTitle);
  }
}
