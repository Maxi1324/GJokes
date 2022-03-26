import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validator, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    
  }

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.pattern('/.+\@.+\..+/')]],
    password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(20)]]
  });
  
  login() {
    console.log(this.loginForm.value);
  }
}
