import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PageRoutes } from '../configuration/page-routes';
import { links } from '../configuration/routes-resolver';
import { Labels } from '../locals/base-labels';
import { EnLabels } from '../locals/en-labels';
import { UaLabels } from '../locals/ua-labels';
import { RuLabels } from '../locals/ru-labels';

export abstract class BaseComponent {
  links = links;
  pageRoutes = PageRoutes;

  get labels(): Labels {
    const locale = localStorage?.getItem('locale');
    if (locale === 'uk-UA') {
      return new UaLabels();
    }

    if (locale === 'ru-RU') {
      return new RuLabels();
    }

    return new EnLabels();
  }

  getRouteParam(route: ActivatedRoute, name: string): Observable<string> {
    return route.params.pipe(map((params) => params[name]?.toString()));
  }
}
