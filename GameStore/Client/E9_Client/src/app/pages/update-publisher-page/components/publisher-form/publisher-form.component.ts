import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, of } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Publisher } from 'src/app/models/publisher.model';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'app-publisher-form',
  templateUrl: './publisher-form.component.html',
  styleUrls: ['./publisher-form.component.scss']
})
export class PublisherFormComponent extends BaseComponent implements OnChanges {
  form?: FormGroup;
  publisherPageLink?: string;

  @Input()
  culture?: string;

  @Input()
  id?: string;

  private publisherSub?: Subscription;

  constructor(
    private publisherService: PublisherService,
    private builder: FormBuilder,
    private router: Router
  ) {
    super();
  }

  ngOnChanges(): void {
    if (this.publisherSub) {
      this.publisherSub.unsubscribe();
    }

    if (!this.culture) {
      return;
    }

    localStorage.setItem('overrideLocale', this.culture);

    this.publisherSub = of(this.id)
      .pipe(
        switchMap((id) =>
          !!id?.length ? this.publisherService.getPublisher(id) : of(undefined)
        ),
        finalize(() => {
          localStorage.removeItem('overrideLocale');
        })
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
      ? this.publisherService.updatePublisher(publisher, this.culture!)
      : this.publisherService.addPublisher(publisher)
    ).subscribe((_) =>
      this.router.navigateByUrl(
        !!publisher.id
          ? this.links.get(this.pageRoutes.Publisher) + `/${publisher.id}`
          : this.links.get(this.pageRoutes.Publishers) ?? ''
      )
    );
  }

  private createForm(publisher?: Publisher): void {
    this.publisherPageLink = !!publisher
      ? `${this.links.get(this.pageRoutes.Publisher)}/${publisher.id}`
      : undefined;

    this.form = this.builder.group({
      id: [publisher?.id ?? ''],
      companyName: [publisher?.companyName ?? '', Validators.required],
      description: [publisher?.description ?? ''],
      homePage: [publisher?.homePage ?? ''],
    });
  }
}
