import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { LoaderService } from "../componetns/loader-component/loader.service";
import { appConfiguration } from "../configuration/configuration-resolver";
import { BaseService } from "./base.service";
import { Image } from "../models/image.model";

@Injectable()
export class ImageService extends BaseService {
  constructor(http: HttpClient, loaderService: LoaderService) {
    super(http, loaderService);
  }

  getImagesByGameKey(key: string): Observable<Image[]> {
    return this.get<Image[]>(
      appConfiguration.imagesByGameApiUrl.replace(environment.routeKeyIdentifier, key)
    )
  }

  getAvailableImages(): Observable<Image[]> {
    return this.get<Image[]>(
      appConfiguration.availableImagesApiUrl
    )
  }

  uploadImages(data: FormData): Observable<void> {
    return this.post(
      appConfiguration.uploadImagesApiUrl,
      data
    )
  }

}