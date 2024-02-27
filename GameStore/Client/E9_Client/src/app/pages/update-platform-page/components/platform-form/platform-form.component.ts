import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, of } from 'rxjs';
import { finalize, switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { PlatformService } from 'src/app/services/platform.service';
import { Platform } from 'src/app/models/platform.model';

@Component({
  selector: 'app-platform-form',
  templateUrl: './platform-form.component.html',
  styleUrls: ['./platform-form.component.scss']
})
export class PlatformFormComponent extends BaseComponent implements OnChanges {

  form?: FormGroup;
  platformPageLink?: string;

  @Input()
  culture?: string;

  @Input()
  id?: string;

  private platformSub?: Subscription;

  constructor(
    private platformService: PlatformService,
    private builder: FormBuilder,
    private router: Router
  ) {
    super();
  }

  ngOnChanges(): void {
    if (this.platformSub) {
      this.platformSub.unsubscribe();
    }

    if (!this.culture) {
      return;
    }

    localStorage.setItem('overrideLocale', this.culture);

    this.platformSub = of(this.id)
      .pipe(
        switchMap((id) =>
          !!id?.length ? this.platformService.getPlatform(id) : of(undefined)
        ),
        finalize(() => {
          localStorage.removeItem('overrideLocale');
        })
      )
      .subscribe((x) => this.createForm(x));
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  onSave(): void {
    const platform: Platform = this.form!.value;

    (!!platform.id
      ? this.platformService.updatePlatform(platform, this.culture!)
      : this.platformService.addPlatform(platform)
    ).subscribe((_) =>
      this.router.navigateByUrl(
        !!platform.id
          ? this.links.get(this.pageRoutes.Platform) + `/${platform.id}`
          : this.links.get(this.pageRoutes.Platforms) ?? ''
      )
    );
  }

  private createForm(platform?: Platform): void {
    this.platformPageLink = !!platform ? `${this.links.get(this.pageRoutes.Platform)}/${platform.id}` : undefined;

    this.form = this.builder.group({
      id: [platform?.id ?? ''],
      type: [platform?.type ?? '', Validators.required],
    });
  }

}
