import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { PlatformService } from 'src/app/services/platform.service';
import { DeletePlatformPageComponent } from './delete-platform-page.component';

@NgModule({
  declarations: [DeletePlatformPageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [PlatformService]
})
export class DeletePlatformPageModule {}
