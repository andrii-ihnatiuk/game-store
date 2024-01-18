import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { LoaderService } from '../componetns/loader-component/loader.service';
import { appConfiguration } from '../configuration/configuration-resolver';
import { gameCountSubject } from '../configuration/shared-info';

export abstract class BaseService {
  constructor(private http: HttpClient, private loaderService: LoaderService) {}

  protected get<TModel>(url: string): Observable<TModel> {
    return this.interceptRequest(
      this.http.get<TModel>(appConfiguration.baseApiUrl + url, {
        observe: 'response',
      })
    );
  }

  protected getWithParams<TResponseModel>(
    url: string,
    params: HttpParams
  ): Observable<TResponseModel> {
    return this.interceptRequest(
      this.http.get<TResponseModel>(appConfiguration.baseApiUrl + url, {
        params: params,
        observe: 'response',
      })
    );
  }

  protected getFile(url: string): Observable<Blob> {
    return this.interceptRequest(
      this.http.get(appConfiguration.baseApiUrl + url, {
        responseType: 'blob',
        observe: 'response',
      })
    );
  }

  protected getFilePost<TRequestModel, TResponseModel>(
    url: string,
    body: TRequestModel
  ): Observable<Blob> {
    return this.interceptRequest(
      this.http.post(appConfiguration.baseApiUrl + url, body, {
        responseType: 'blob',
        observe: 'response',
      })
    );
  }

  protected post<TRequestModel, TResponseModel>(
    url: string,
    body: TRequestModel
  ): Observable<TResponseModel> {
    return this.interceptRequest(
      this.http.post<TResponseModel>(appConfiguration.baseApiUrl + url, body, {
        observe: 'response',
      })
    );
  }

  protected put<TRequestModel, TResponseModel>(
    url: string,
    body: TRequestModel
  ): Observable<TResponseModel> {
    return this.interceptRequest(
      this.http.put<TResponseModel>(appConfiguration.baseApiUrl + url, body, {
        observe: 'response',
      })
    );
  }

  protected delete<TRequestModel, TResponseModel>(
    url: string,
    body: TRequestModel
  ): Observable<TResponseModel> {
    return this.interceptRequest(
      this.http.delete<TResponseModel>(appConfiguration.baseApiUrl + url, {
        body: body,
        observe: 'response',
      })
    );
  }

  private interceptRequest<TResult>(
    request: Observable<HttpResponse<TResult>>
  ): Observable<TResult> {
    return this.loaderService.openForLoading(
      request.pipe(
        tap((x) =>
          gameCountSubject.next(x.headers.get('x-total-numbers-of-games'))
        ),
        map((x) => x.body ?? ({} as TResult))
      )
    );
  }
}
