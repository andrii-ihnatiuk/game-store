import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DeleteUserPageComponent } from './delete-user-page.component';
import { UserService } from 'src/app/services/user.service';

@NgModule({
  declarations: [DeleteUserPageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [UserService]
})
export class DeleteUserPageModule {}
