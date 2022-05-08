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
  selectedUser: HoleUserInfos = {
    configInfos: {
      minCon: 0,
      maxCon: 0,
      repoName: "",
      erstellung: new Date(),
      githubEmail: "",
      githubUsername: ""
    },
    realEmail: "",
    userId: 0,
    blocked: false,
    verified: false
  };
  
  @ViewChild('password') password: any;
  @ViewChild('filterBy') filterBy: any;
  @ViewChild('orderBy') orderBy: any;
  @ViewChild('pageValue') pageValue: any;

  constructor() {}

  ngOnInit(): void {}

  loadUsers():void {
    //Get Page Parameter
    let Page:number = this.pageValue.nativeElement.value;
    //Get OrderBy Parameter
    let ob = this.orderBy.nativeElement.value;
    //Convert OrderBy to OrderByEnum
    let OB:OrderBy = OrderBy.Joined;
    switch(ob){
      case "Email":
        OB = OrderBy.Email;
        break;
      case "Id":
        OB = OrderBy.Id;
        break;
      case "Joined":
        OB = OrderBy.Joined;
        break;
      case "JoinedDesc":
        OB = OrderBy.JoinedDesc;
        break;
    }
    //Get Filter Parameter
    let f = this.filterBy.nativeElement.value;
    //Convert Filter to FilterEnum
    let F:Filter = Filter.All;
    switch(f){
      case "All":
        F = Filter.All;
        break;
      case "Blocked":
        F = Filter.Blocked;
        break;
      case "NotBlocked":
        F = Filter.NotBlocked;
        break;
      case "NotAuthenticated":
        F = Filter.NotAuthenticated;
        break;
      case "Authenticated":
        F = Filter.Authenticated;
        break;
    }
    let RequestParameter : GetAdminUserInfos = {
      Page:Page,
      OB:OB,
      F:F,
      password: this.password.nativeElement.value
    };
    const Callback = (res: any) => {
      this.users = res as HoleUserInfos[];
      this.users.forEach(user => {
        user.configInfos.erstellung = new Date(user.configInfos.erstellung);
      });
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
      password : this.password.nativeElement.value,
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
      password : this.password.nativeElement.value,
      userId : 0
    }
    SendPost("api/Admin/UnblockPerson",body,Callback,false)
  }

  SelectUser(user: HoleUserInfos): void {
    this.selectedUser = user;
  }    
}
