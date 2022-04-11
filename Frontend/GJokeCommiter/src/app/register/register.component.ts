import { EmailVerDataService } from './../email-ver-data.service';
import { Response, SendEmailVerification, SendGet, UserCred, UserInfo } from './../BackendCom';
import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { AbstractControl, FormBuilder, ValidatorFn } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { SendPost } from '../BackendCom';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @ViewChild('email') Email!: any;;
  @ViewChild('password1') Password1!: any;;
  @ViewChild('password2') Password2!: any;;
  @ViewChild('check') check!: any;;

  ErrorMessage: string = "";
  hidden: string = "hidden";

  active:boolean = true;

  constructor(private router: Router, private EVDS:EmailVerDataService ) { }

  ngOnInit(): void {
    //If the user is already logged in, redirect to homepage
    if (localStorage.getItem('GJokes-Mail') != null) {
      window.location.href = '/';
    }
    //Make Register Visible
    if (window.location.pathname == '/register') {
      document.getElementById("register")?.classList.remove("hidden-strong");
      document.getElementById("cover")?.classList.remove("hidden-strong");
    }
  }

  Register() {
    if (this.Password1.nativeElement.value != this.Password2.nativeElement.value) {
      this.ErrorMessage = "Passwords do not match"
      this.hidden = "";
    }
    else if (!this.check.nativeElement.checked) {
      this.ErrorMessage = "You must accept our Terms of Service"
      this.hidden = "";
    }
    else {
      this.active = false;
      let uc = {
        Email: this.Email.nativeElement.value,
        Password: this.Password1.nativeElement.value
      }
      var Callback = (res: Response) => {
        if (res.code >= 0) {
          this.SendToken(res.code,false);
        }else if (res.code == -1){
          this.GetUserIdAndSendToken();
        }
        else {
          this.active = true;
          this.ErrorMessage = res.desc;
          this.hidden = "";
          this.router.navigateByUrl('/register');
        }
      }
      SendPost("api/Account/CreateAccount", uc, Callback, false);
    }
  }

  SendToken(UserID:number, wasReSent:boolean= false){
   const RC :RegisterComponent = this;
    const Callback = function(r:Response){
      switch(r.code){ 
        case 1:
          RC.EVDS.changeCUserID(UserID)
          RC.EVDS.changeWasresent(wasReSent);
          RC.hidden = "hidden";
          RC.router.navigateByUrl('/VerifyCode');
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
    SendEmailVerification(UserID,Callback)
  }

  GetUserIdAndSendToken(){
    let RegisterComponent = this;
    var Callback2 = function(res: Response){
      RegisterComponent.SendToken(res.code,true);
    }
    var Parameter ={
        Mail: this.Email.nativeElement.value
    }
    SendGet("api/Account/GetUserIdFromMail",Callback2,false,Parameter)
  }

  /*
  email_available(): ValidatorFn {
    //Check if the email is available by making a request to the server
    return (control: AbstractControl): {[key: string]: any} | null => {
      return this.http.get('http://localhost:3000/users/' + control.value).subscribe(
        (res: any) => {
          if(res.length > 0) {
            return {'email_available': true};
          }
          return null;
        }
      );
    }
  }
  */
}
