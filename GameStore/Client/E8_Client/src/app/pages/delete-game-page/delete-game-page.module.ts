import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonComponentsModule } from 'src/app/componetns/common-components.module';
import { GameService } from 'src/app/services/game.service';
import { ReactiveFormsModule } from '@angular/forms';
import { DeleteGamePageComponent } from './delete-game-page.component';

@NgModule({
  declarations: [DeleteGamePageComponent],
  imports: [CommonModule, CommonComponentsModule, ReactiveFormsModule],
  providers: [GameService]
})
export class DeleteGamePageModule {}
