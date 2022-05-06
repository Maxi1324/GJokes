import { HoleUserInfos, ConfigInfos,Response, OrderBy, Filter, GetAdminUserInfos, SendPost, BlockUser } from './../BackendCom';
import { Component, OnInit, ViewChild } from '@angular/core';
import { SendGet } from '../BackendCom';
import { Call } from '@angular/compiler';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  users: HoleUserInfos[] = [];
  //selectedUser: 
  /*HoleUserInfos = {
    Id:0,
    Email:"",
  }*/


  @ViewChild('email') email!: any;;
  
  constructor() {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers():void {
    let RequestParameter : GetAdminUserInfos = {
      Page:0,
      OB:OrderBy.Id,
      F:Filter.Authenticated,
      password:"Maxistcool"
    };
    const Callback = (res: any) => {
      this.users = res as HoleUserInfos[];
    }
    SendGet("api/Admin/GetUsers",Callback,false,RequestParameter)
  }

  BlockUser():void{
    const Callback = (res: Response) => {
        if(res.code == 1){
          console.log("User blocked");
        } 
        else{
          console.log("Error: " + res.desc);
        }
    }

    const body:BlockUser = {
      password : "Maxistcool",
      userId : 0
    }
    SendPost("api/Admin/BlockPerson",body,Callback,false)
  }

  UnBlockUser():void{
    const Callback = (res: Response) => {
        if(res.code == 1){
          console.log("User Unblocked");
        } 
        else{
          console.log("Error: " + res.desc);
        }
    }

    const body:BlockUser = {
      password : "Maxistcool",
      userId : 0
    }
    SendPost("api/Admin/UnblockPerson",body,Callback,false)
  }
}
