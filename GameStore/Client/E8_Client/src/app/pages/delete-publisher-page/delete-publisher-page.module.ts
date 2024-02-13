import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DeletePublisherPageComponent } from './delete-publisher-page.component';
import { PublisherService } from 'src/app/services/publisher.service';

@NgModule({
  declarations: [DeletePublisherPageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [PublisherService]
})
export class DeletePublisherPageModule {}
