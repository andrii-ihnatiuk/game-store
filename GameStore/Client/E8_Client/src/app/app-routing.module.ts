import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { resolveRoutesAsync } from './configuration/routes-resolver';
import { MainPageComponent } from './pages/main-page/main-page.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: MainPageComponent,
  },
  {
    path: '**',
    redirectTo: '',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { anchorScrolling: 'enabled' })],
  exports: [RouterModule],
})
export class AppRoutingModule {
  constructor(router: Router) {
    resolveRoutesAsync().then((routes) =>
      routes.forEach((x) => router.config.splice(1, 0, x))
    );
  }
}
