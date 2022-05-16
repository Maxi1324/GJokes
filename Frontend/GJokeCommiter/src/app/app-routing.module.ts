import { GenerateContComponent } from './generate-cont/generate-cont.component';
import { AccountDeletedComponent } from './account-deleted/account-deleted.component';
import { SendcodeComponent } from './sendcode/sendcode.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { AmbienceComponent } from './ambience/ambience.component';
import { AboutComponent } from './about/about.component';
import { AdminComponent } from './admin/admin.component';

const routes: Routes = [
  { path: '', component: HomepageComponent },
  { path: 'config', component: AmbienceComponent },
  { path: 'about', component: AboutComponent },
  { path: 'register', component: HomepageComponent },
  { path: 'VerifyCode', component: SendcodeComponent },
  { path: 'login', component: HomepageComponent },
  { path: 'DeletedAccount', component: AccountDeletedComponent },
  { path: 'admin', component: AdminComponent },
  {path:'GenerateCont', component:GenerateContComponent},
  { path: ' ', component: GenerateContComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
