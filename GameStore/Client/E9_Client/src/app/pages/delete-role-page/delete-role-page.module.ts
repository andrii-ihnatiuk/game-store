import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DeleteRolePageComponent } from './delete-role-page.component';
import { RoleService } from 'src/app/services/role.service';

@NgModule({
  declarations: [DeleteRolePageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [RoleService]
})
export class DeleteRolePageModule {}
