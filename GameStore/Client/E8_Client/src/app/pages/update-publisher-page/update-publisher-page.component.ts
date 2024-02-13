import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Publisher } from 'src/app/models/publisher.model';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'gamestore-update-publisher',
  templateUrl: './update-publisher-page.component.html',
  styleUrls: ['./update-publisher-page.component.scss'],
})
export class UpdatePublisherPageComponent
  extends BaseComponent
  implements OnInit
{
  form?: FormGroup;
  publisherPageLink?: string;

  constructor(
    private publisherService: PublisherService,
    private route: ActivatedRoute,
    private builder: FormBuilder,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((companyName) =>
          !!companyName?.length ? this.publisherService.getPublisher(companyName) : of(undefined)
        )
      )
      .subscribe((x) => {
        this.createForm(x);
      });
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  onSave(): void {
    const publisher: Publisher = this.form!.value;

    (!!publisher.id
      ? this.publisherService.updatePublisher(publisher)
      : this.publisherService.addPublisher(publisher)
    ).subscribe((_) =>
      this.router.navigateByUrl(
        !!publisher.id
          ? this.links.get(this.pageRoutes.Publisher) + `/${publisher.companyName}`
          : this.links.get(this.pageRoutes.Publishers) ?? ''
      )
    );
  }

  private createForm(publisher?: Publisher): void {
    this.publisherPageLink = !!publisher
      ? `${this.links.get(this.pageRoutes.Publisher)}/${publisher.companyName}`
      : undefined;

    this.form = this.builder.group({
      id: [publisher?.id ?? ''],
      companyName: [publisher?.companyName ?? '', Validators.required],
      description: [publisher?.description ?? ''],
      homePage: [publisher?.homePage ?? ''],
    });
  }
}
