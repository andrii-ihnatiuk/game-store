import { NgModule } from "@angular/core";
import { ProfilePageComponent } from "./profile-page.component";
import { CommonModule } from "@angular/common";
import { CommonComponentsModule } from "src/app/componetns/common-components.module";
import { AppRoutingModule } from "src/app/app-routing.module";
import { UserService } from "src/app/services/user.service";
import { NotificationService } from "src/app/services/notification.service";

@NgModule({
    declarations: [ProfilePageComponent],
    imports: [CommonModule, CommonComponentsModule, AppRoutingModule],
    providers: [UserService, NotificationService]
  })
  export class ProfilePageModule {}
  