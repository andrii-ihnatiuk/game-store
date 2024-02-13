import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoaderService } from '../componetns/loader-component/loader.service';
import { appConfiguration } from '../configuration/configuration-resolver';
import { Publisher } from '../models/publisher.model';
import { BaseService } from './base.service';

@Injectable()
export class PublisherService extends BaseService {
  constructor(http: HttpClient, loaderService: LoaderService) {
    super(http, loaderService);
  }

  getPublisher(companyName: string): Observable<Publisher> {
    return this.get<Publisher>(
      appConfiguration.publisherApiUrl.replace(
        environment.routeCompanyNameIdentifier,
        companyName
      )
    );
  }

  getPublishers(): Observable<Publisher[]> {
    return this.get<Publisher[]>(appConfiguration.publishersApiUrl);
  }

  getPublisherByGameKey(gameKey: string): Observable<Publisher> {
    return this.get<Publisher>(
      appConfiguration.publisherByGameApiUrl.replace(
        environment.routeKeyIdentifier,
        gameKey
      )
    );
  }

  addPublisher(publisher: Publisher): Observable<any> {
    return this.post(appConfiguration.addPublisherApiUrl, { publisher });
  }

  updatePublisher(publisher: Publisher): Observable<any> {
    return this.put(appConfiguration.updatePublisherApiUrl, { publisher });
  }

  deletePublisher(id: string): Observable<any> {
    return this.delete(
      appConfiguration.deletePublisherApiUrl.replace(
        environment.routeIdIdentifier,
        id
      ),
      {}
    );
  }
}
