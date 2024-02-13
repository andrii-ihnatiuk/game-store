import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { UpdateUserPageComponent } from './update-user-page.component';
import { UserService } from 'src/app/services/user.service';
import { RoleService } from 'src/app/services/role.service';

@NgModule({
  declarations: [UpdateUserPageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [UserService, RoleService]
})
export class UpdateUserPageModule {}
