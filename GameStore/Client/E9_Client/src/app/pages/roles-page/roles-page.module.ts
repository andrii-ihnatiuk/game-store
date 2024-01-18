import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { RolesPageComponent } from './roles-page.component';
import { RoleService } from 'src/app/services/role.service';

@NgModule({
  declarations: [RolesPageComponent],
  imports: [CommonModule, CommonComponentsModule],
  providers: [RoleService]
})
export class RolesPageModule {}
