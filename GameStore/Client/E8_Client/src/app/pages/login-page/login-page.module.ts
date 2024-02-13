import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginPageComponent } from './login-page.component';
import { UserService } from 'src/app/services/user.service';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [LoginPageComponent],
  imports: [
    CommonModule,
    CommonComponentsModule,
    ReactiveFormsModule,
    MatButtonModule,
  ],
  providers: [UserService],
})
export class UpdateLoginPageModule {}
