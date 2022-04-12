import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { HomepageComponent } from './homepage/homepage.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AmbienceComponent } from './ambience/ambience.component';
import { AboutComponent } from './about/about.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SendcodeComponent } from './sendcode/sendcode.component';
import { AccountDeletedComponent } from './account-deleted/account-deleted.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomepageComponent,
    RegisterComponent,
    LoginComponent,
    AmbienceComponent,
    AboutComponent,
    SendcodeComponent,
    AccountDeletedComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
