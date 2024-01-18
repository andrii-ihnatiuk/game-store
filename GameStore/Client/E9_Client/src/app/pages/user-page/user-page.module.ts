import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { UserPageComponent } from './user-page.component';
import { UserService } from 'src/app/services/user.service';
import { RoleService } from 'src/app/services/role.service';

@NgModule({
  declarations: [UserPageComponent],
  imports: [CommonModule, CommonComponentsModule, AppRoutingModule, MatButtonModule],
  providers: [UserService, RoleService]
})
export class UserPageModule {}
