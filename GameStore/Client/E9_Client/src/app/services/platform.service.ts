import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoaderService } from '../componetns/loader-component/loader.service';
import { appConfiguration } from '../configuration/configuration-resolver';
import { Platform } from '../models/platform.model';
import { BaseService } from './base.service';

@Injectable()
export class PlatformService extends BaseService {
  constructor(http: HttpClient, loaderService: LoaderService) {
    super(http, loaderService);
  }

  getPlatform(id: string): Observable<Platform> {
    return this.get<Platform>(
      appConfiguration.platformApiUrl.replace(environment.routeIdIdentifier, id)
    );
  }

  getPlatformsByGameKey(gameKey: string): Observable<Platform[]> {
    return this.get<Platform[]>(
      appConfiguration.platformsByGameApiUrl.replace(
        environment.routeKeyIdentifier,
        gameKey
      )
    );
  }

  getPlatforms(): Observable<Platform[]> {
    return this.get<Platform[]>(appConfiguration.platformsApiUrl);
  }

  addPlatform(platform: Platform): Observable<any> {
    return this.post(appConfiguration.addPlatformApiUrl, { platform });
  }

  updatePlatform(platform: Platform): Observable<any> {
    return this.put(appConfiguration.updatePlatformApiUrl, { platform });
  }

  deletePlatform(id: string): Observable<any> {
    return this.delete(
      appConfiguration.deletePlatformApiUrl.replace(
        environment.routeIdIdentifier,
        id
      ),
      {}
    );
  }
}
