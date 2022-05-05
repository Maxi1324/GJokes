import { HoleUserInfos, ConfigInfos, OrderBy, Filter, GetAdminUserInfos } from './../BackendCom';
import { Component, OnInit, ViewChild } from '@angular/core';
import { SendGet } from '../BackendCom';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  users: HoleUserInfos[] = [];

  @ViewChild('email') email!: any;;
  
  constructor() {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
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
}
