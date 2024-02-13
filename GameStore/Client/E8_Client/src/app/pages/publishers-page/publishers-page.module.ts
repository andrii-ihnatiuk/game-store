import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { PublishersPageComponent } from './publishers-page.component';
import { PublisherService } from 'src/app/services/publisher.service';

@NgModule({
  declarations: [PublishersPageComponent],
  imports: [CommonModule, CommonComponentsModule],
  providers: [PublisherService]
})
export class PublishersPageModule {}
