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
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'gamestore-login',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent extends BaseComponent implements OnInit {
  form?: FormGroup;
  platformPageLink?: string;

  constructor(
    private builder: FormBuilder,
    private router: Router,
    private userService: UserService
  ) {
    super();
  }

  ngOnInit(): void {
    this.userService.clearAuth();
    this.createForm();
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  onLogin(isInternalAuth: boolean): void {
    const model = {
      login: this.form!.value.name,
      password: this.form!.value.password,
      internalAuth: isInternalAuth,
    };

    this.userService
      .login(model)
      .subscribe((_) => this.router.navigateByUrl(''));
  }

  private createForm(platform?: Platform): void {
    this.form = this.builder.group({
      name: ['', Validators.required],
      password: ['', Validators.required],
    });
  }
}
