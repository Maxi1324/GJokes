import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  //Rendered Component should update when array changes
  //https://angular.io/guide/component-interaction#!#parent-and-children-communicate-via-a-service
  users: User[] = [];

  @ViewChild('email') email!: any;;
  
  constructor() {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    let tmp = [];
    //TODO: Load users from backend
    //Test Data
    for (let i = 0; i < 10; i++) {
      //give user random email
      let email = "";
      for (let j = 0; j < Math.floor(Math.random() * 10) + 5; j++) {
        email += String.fromCharCode(Math.floor(Math.random() * 26) + 97);
      }
      email += "@gmail.com";
      tmp.push(new User(i, email, new Date()));
    }
    this.users = tmp;
  }
}

interface User {
  id: number;
  email: string;
  joined: Date;
}

class User implements User {
  constructor(
    public id: number,
    public email: string,
    public joined: Date
  ) {}
}