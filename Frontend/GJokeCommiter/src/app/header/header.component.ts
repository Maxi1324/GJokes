import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public href() {
    //TODO: Get Login Status
    this.content = "Register";
    this.link = "register";
  }

  constructor() { }

  ngOnInit(): void {
    this.href();
  }

  @Input()
  public link: string = 'register';
  public content: string = 'Register';
}
