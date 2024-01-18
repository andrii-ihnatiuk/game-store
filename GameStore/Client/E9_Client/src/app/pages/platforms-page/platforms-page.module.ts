import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { PlatformsPageComponent } from './platforms-page.component';
import { PlatformService } from 'src/app/services/platform.service';

@NgModule({
  declarations: [PlatformsPageComponent],
  imports: [CommonModule, CommonComponentsModule],
  providers: [PlatformService]
})
export class PlatformsPageModule {}
