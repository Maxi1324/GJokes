import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, ValidatorFn } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-sendcode',
  templateUrl: './sendcode.component.html',
  styleUrls: ['./sendcode.component.css']
})
export class SendcodeComponent implements OnInit {

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
}
