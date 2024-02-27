import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { PlatformService } from 'src/app/services/platform.service';
import { UpdatePlatformPageComponent } from './update-platform-page.component';
import { PlatformFormComponent } from './components/platform-form/platform-form.component';

@NgModule({
  declarations: [UpdatePlatformPageComponent, PlatformFormComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [PlatformService]
})
export class UpdatePlatformPageModule {}
