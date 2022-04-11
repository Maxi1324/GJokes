import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
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

  active:boolean = true;

  constructor(private fb: FormBuilder, private http: HttpClient, private route:Router) {}

  ngOnInit(): void {
    //If the user is already logged in, redirect to homepage
    if(localStorage.getItem('GJokes-Mail') != null){
      window.location.href = '/';
    }
    //Make Login Visible
    if (window.location.pathname == '/login'){
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
    console.log(this.loginForm.value);

    let LC:LoginComponent = this;

    let Callback = function(res:Response){
      switch(res.code){
        case 1:
          LC.ErrorMessage = "";
          LC.hidden = "hidden";
          LC.route.navigateByUrl('/config');
          break;
        case -1:
          LC.ErrorMessage = res.desc;
          LC.hidden = "";
          break;
      }
      LC.active = true;
    }

    SendPost("api/Account/Login",this.loginForm.value,Callback,false)
  }
}
