import { SendcodeComponent } from './sendcode/sendcode.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { AmbienceComponent } from './ambience/ambience.component';
import { AboutComponent } from './about/about.component';

const routes: Routes = [
  { path: '', component: HomepageComponent },
  { path: 'config', component: AmbienceComponent },
  { path: 'about', component: AboutComponent },
  { path: 'register', component: HomepageComponent },
  { path: 'VerifyCode', component: SendcodeComponent },
  { path: 'login', component: HomepageComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
