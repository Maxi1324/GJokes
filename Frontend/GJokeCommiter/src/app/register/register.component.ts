import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, ValidatorFn } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit(): void {
    //If the user is already logged in, redirect to homepage
    if(localStorage.getItem('GJokes-Mail') != null){
      window.location.href = '/';
    }
    //Make Register Visible
    if (window.location.pathname == '/register'){
      document.getElementById("register")?.classList.remove("hidden-strong");
      document.getElementById("cover")?.classList.remove("hidden-strong");
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
