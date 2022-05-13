import { EmailVerDataService } from './../email-ver-data.service';
import { SendEmailVerification, SendGet, UserCred } from './../BackendCom';
import { Router } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ValidationErrors,
  Validator,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Response, SendPost } from '../BackendCom';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {

  ErrorMessage: string = "";
  hidden: string = "hidden";

  active: boolean = true;

  @ViewChild('email') Email!: any;;
  @ViewChild('password') Password1!: any;;

  constructor(private fb: FormBuilder, private http: HttpClient, private route: Router, private EVDS: EmailVerDataService) { }

  ngOnInit(): void {
    //If the user is already logged in, redirect to homepage
    if (localStorage.getItem('GJokes-Mail') != null) {
      window.location.href = '/';
    }
    //Make Login Visible
    if (window.location.pathname == '/login') {
      document.getElementById("login")?.classList.remove("hidden-strong");
      document.getElementById("cover")?.classList.remove("hidden-strong");
    }
  }

  loginForm = this.fb.group({
    email: ['', [Validators.required, this.email_valid()]],
    password: [
      '',
      [Validators.required, Validators.minLength(6), Validators.maxLength(20)],
    ],
  });

  email_valid(): ValidatorFn {
    //Check if the email is valid with a regex /.+\@.+\..+/
    return (control: AbstractControl): ValidationErrors | null => {
      //Check if input is not empty
      if (control.value != '') {
        const email_regex = /.+\@.+\..+/;
        const valid = email_regex.test(control.value);
        return valid ? null : { email: true };
      }
      return null;
    };
  }

  login() {
    this.active = false;
    let LC: LoginComponent = this;

    let Callback = function (res: Response) {
      switch (res.code) {
        case 1:
          LC.ErrorMessage = "";
          LC.hidden = "hidden";
          localStorage.setItem("JWTToken", res.desc);
          LC.route.navigateByUrl('/config');
          break;
        case -1:
          LC.ErrorMessage = res.desc;
          LC.hidden = "";
          break;
        case -2:
          LC.GetUserIdAndSendToken();
          break;
      }
      LC.active = true;
    }

    let body = {
      email: this.Email.nativeElement.value,
      password: this.Password1.nativeElement.value
    }
    SendPost("api/Account/Login", body, Callback, false)
  }


  SendToken(UserID: number, wasReSent: boolean = false) {
    const RC: LoginComponent = this;
    const Callback = function (r: Response) {
      switch (r.code) {
        case 1:
          RC.EVDS.changeCUserID(UserID)
          RC.EVDS.changeWasresent(wasReSent);
          RC.hidden = "hidden";
          RC.route.navigateByUrl('/VerifyCode');
          break;
        case -1:
          console.error("User not found");
          break;
        case -2:
          RC.ErrorMessage = "Email is blocked, due to too many requests";
          RC.hidden = "";
          break;
      }
      RC.active = true;
    }
    SendEmailVerification(UserID, Callback)
  }

  GetUserIdAndSendToken() {
    let RegisterComponent = this;
    this.active = false;
    var Callback2 = function (res: Response) {
      RegisterComponent.SendToken(res.code, true);
    }
    var Parameter = {
      Mail: this.Email.nativeElement.value
    }
    SendGet("api/Account/GetUserIdFromMail", Callback2, false, Parameter)
  }
}
