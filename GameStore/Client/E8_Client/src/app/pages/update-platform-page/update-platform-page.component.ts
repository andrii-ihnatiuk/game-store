import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Platform } from 'src/app/models/platform.model';
import { PlatformService } from 'src/app/services/platform.service';

@Component({
  selector: 'gamestore-update-platform',
  templateUrl: './update-platform-page.component.html',
  styleUrls: ['./update-platform-page.component.scss'],
})
export class UpdatePlatformPageComponent extends BaseComponent implements OnInit {
  form?: FormGroup;
  platformPageLink?: string; 

  constructor(
    private platformService: PlatformService,
    private route: ActivatedRoute,
    private builder: FormBuilder,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'id')
      .pipe(
        switchMap((id) =>
          !!id?.length ? this.platformService.getPlatform(id) : of(undefined)
        )
      )
      .subscribe((x) => this.createForm(x));
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  onSave(): void {
    const platform: Platform = this.form!.value;

    (!!platform.id
      ? this.platformService.updatePlatform(platform)
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
