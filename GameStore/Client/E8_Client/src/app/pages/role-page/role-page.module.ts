import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { MatButtonModule } from '@angular/material/button';
import { RolePageComponent } from './role-page.component';
import { RoleService } from 'src/app/services/role.service';

@NgModule({
  declarations: [RolePageComponent],
  imports: [CommonModule, CommonComponentsModule, AppRoutingModule, MatButtonModule],
  providers: [RoleService]
})
export class RolePageModule {}
