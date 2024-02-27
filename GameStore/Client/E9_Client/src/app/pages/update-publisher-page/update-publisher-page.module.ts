import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { UpdatePublisherPageComponent } from './update-publisher-page.component';
import { PublisherService } from 'src/app/services/publisher.service';
import { PublisherFormComponent } from './components/publisher-form/publisher-form.component';

@NgModule({
  declarations: [UpdatePublisherPageComponent, PublisherFormComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [PublisherService]
})
export class UpdatePublisherPageModule {}
