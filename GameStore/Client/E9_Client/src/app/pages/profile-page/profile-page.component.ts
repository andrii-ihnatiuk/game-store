import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { mergeMap, switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { NotificationMethod } from 'src/app/models/notification-method.model';
import { UserContactInfo } from 'src/app/models/user-contact-info.model';
import { User } from 'src/app/models/user.model';
import { NotificationService } from 'src/app/services/notification.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent extends BaseComponent implements OnInit {

  contactInfo?: UserContactInfo;
  notificationMethods: NotificationMethod[] = [];
  notificationMethodsItems: string[] = [];

  form?: FormGroup;

  constructor(
    private userService: UserService,
    private notificationService: NotificationService,
    private builder: FormBuilder
  ) {
    super();
  }

  ngOnInit(): void {
    forkJoin({
      info: this.userService.getContactInfo(),
      methods: this.notificationService.getNotificationMethods()
    })
      .subscribe(x => {
        this.contactInfo = x.info;
        this.notificationMethods = x.methods;
        this.createForm();
      });
  }

  createForm(): void {
    this.form = this.builder.group({
      name: [this.contactInfo?.name, Validators.required],
      email: [this.contactInfo?.email, Validators.email],
      phoneNumber: [this.contactInfo?.phoneNumber],
      notificationMethods: this.builder.array(
        this.notificationMethods.map(x => this.contactInfo?.notificationMethods.some(y => y.name === x.name))
      )
    })

    this.notificationMethodsItems = this.notificationMethods.map(x => x.name);
  }

  getFormControl(name: string): FormControl {
    return this.form?.get(name) as FormControl;
  }

  getFormControlArray(name: string): FormControl[] {
    return (this.form?.get(name) as FormArray).controls.map(
      (x) => x as FormControl
    );
  }

  onSave(): void {
    const selectedMethods = this.notificationMethods
      .filter((x, i) => !!this.form?.value.notificationMethods[i]);

    const contactInfo: UserContactInfo = {
      id: this.contactInfo?.id,
      name: this.form?.value.name,
      email: this.form?.value.email,
      phoneNumber: this.form?.value.phoneNumber,
      notificationMethods: selectedMethods
    }

    this.userService.updateContactInfo(contactInfo)
      .subscribe(_ => window.location.reload())
  }

}
