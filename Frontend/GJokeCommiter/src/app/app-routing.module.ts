import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { AmbienceComponent } from './ambience/ambience.component';

const routes: Routes = [
  { path: '', component: HomepageComponent },
  { path: 'config', component: AmbienceComponent } 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
