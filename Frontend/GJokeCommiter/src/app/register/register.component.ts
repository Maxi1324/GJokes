import { Response, UserCred } from './../BackendCom';
import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { AbstractControl, FormBuilder, ValidatorFn } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { SendPost } from '../BackendCom';

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


  constructor(private fb: FormBuilder, private http: HttpClient) { }

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
    else if (!this.check.nativeElement.checked){
      this.ErrorMessage = "You must accept our Terms of Service"
      this.hidden = "";
    }
    else {
      let uc = {
        Email: this.Email.nativeElement.value,
        Password: this.Password1.nativeElement.value
      }
      var Callback = (res: Response) => {
        if (res.code >= 0) {
          this.hidden = "hidden";
          sessionStorage.setItem("UserId",res.code.toString());
          console.log("Successfully registered");
        }
        else {
          this.ErrorMessage = res.desc;
          this.hidden = "";
        }
      }

      SendPost("api/Account/CreateAccount", uc, Callback, false);
    }
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
