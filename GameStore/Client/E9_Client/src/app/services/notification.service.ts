import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { LoaderService } from "../componetns/loader-component/loader.service";
import { BaseService } from "./base.service";
import { Observable } from "rxjs";
import { NotificationMethod } from "../models/notification-method.model";
import { appConfiguration } from "../configuration/configuration-resolver";

@Injectable()
export class NotificationService extends BaseService {
  constructor(http: HttpClient, loaderService: LoaderService) {
    super(http, loaderService);
  }

  getNotificationMethods(): Observable<NotificationMethod[]> {
    return this.get<NotificationMethod[]>(
      appConfiguration.notificationMethodsApiUrl
    )
  }
}