import { ErrorHandler, Inject, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { apiErrorTitle } from './http-interceptor';

@Injectable()
export class GlobalErrorHandlerService extends ErrorHandler {
  constructor(@Inject(Injector) private injector: Injector) {
    super();
  }

  private get toastrService(): ToastrService {
    return this.injector.get(ToastrService);
  }

  handleError(error: Error | HttpErrorResponse): void {
    if (error.message === apiErrorTitle) {
      return;
    }

    this.toastrService.error(error.message, 'Something goes wrong!');

    super.handleError(error);
  }
}
