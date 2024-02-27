import { Injectable } from "@angular/core";
import { BaseService } from "./base.service";
import { LoaderService } from "../componetns/loader-component/loader.service";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { Culture } from "../models/culture.model";
import { appConfiguration } from "../configuration/configuration-resolver";

@Injectable()
export class LocalizationService extends BaseService {
    private culturesSubject = new BehaviorSubject<Culture[]>([]);
    cultures$: Observable<Culture[]> = this.culturesSubject.asObservable();

    constructor(http: HttpClient, loaderService: LoaderService) {
        super(http, loaderService);
    }

    loadSupportedCultures(): Observable<Culture[]> {
        this.get<Culture[]>(appConfiguration.culturesApiUrl).subscribe(cultures => {
            this.culturesSubject.next(cultures);
        });

        return this.cultures$;
    }
}
