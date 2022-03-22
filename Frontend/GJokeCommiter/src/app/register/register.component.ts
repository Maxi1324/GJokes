import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    //If the user is already logged in, redirect to homepage
    if(localStorage.getItem('GJokes-Mail') != null){
      window.location.href = '/';
    }
  }

}
