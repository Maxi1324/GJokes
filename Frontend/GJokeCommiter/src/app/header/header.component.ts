import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { SendGet } from '../BackendCom';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css', './../app.component.css'],
})
export class HeaderComponent implements OnInit {
 
  public LogState:number = 0;

  constructor(private router:Router) {
    router.events.subscribe((val:any) => {
     this.href(); 
    });
  }

  ngOnInit(): void {

    this.href();
  }

  href():void {
    let HC: HeaderComponent = this;
    const Callback = (data: any ) => {
      HC.LogState = 1;
    };
    const ErrorCallback = ()=>{
      HC.LogState = -1;
    }
    SendGet('api/Account/Loggedin',Callback,true,{},ErrorCallback)
  }

 
}
