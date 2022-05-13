import { MatDatepickerModule } from '@angular/material/datepicker';
import {MatNativeDateModule, MAT_DATE_LOCALE} from '@angular/material/core';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {MatFormFieldModule} from '@angular/material/form-field';
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
import { AdminComponent } from './admin/admin.component';
import { GenerateContComponent } from './generate-cont/generate-cont.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatHint } from '@angular/material/form-field';

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
    AdminComponent,
    GenerateContComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
