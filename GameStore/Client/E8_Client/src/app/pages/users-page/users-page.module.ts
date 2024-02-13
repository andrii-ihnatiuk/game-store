import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { UsersPageComponent } from './users-page.component';
import { UserService } from 'src/app/services/user.service';

@NgModule({
  declarations: [UsersPageComponent],
  imports: [CommonModule, CommonComponentsModule],
  providers: [UserService]
})
export class UsersPageModule {}
