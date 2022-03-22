import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  public href() {
    //TODO: Get Login Status
    let status = false;
    this.content = 'Register';
    this.link = 'register';
    //Check if email and password are set
    if (localStorage.getItem('email') && localStorage.getItem('password')) {
      //Check if token is set
      if (!sessionStorage.getItem('token')) {
        //Get Token from API to check email and password.
        this.content = 'Config';
        this.link = 'config';
        status = true;
      } else status = true;
    }
    if (status === true) {
      this.content = 'Config';
      this.link = 'config';
    }
  }

  constructor() {}

  ngOnInit(): void {
    this.href();
  }

  @Input()
  public link: string = 'register';
  public content: string = 'Register';
}
