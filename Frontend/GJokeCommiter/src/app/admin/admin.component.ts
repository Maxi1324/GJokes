import { HoleUserInfos, ConfigInfos,Response, OrderBy, Filter, GetAdminUserInfos, SendPost, BlockUser } from './../BackendCom';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { SendGet } from '../BackendCom';
import { AST, Call } from '@angular/compiler';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  users: HoleUserInfos[] = [];
  public selectedUser: HoleUserInfos = {} as HoleUserInfos
  
  @ViewChild('listi') listi: any;

  @ViewChild('password') password: any;
  @ViewChild('filterBy') filterBy: any;
  @ViewChild('orderBy') orderBy: any;
  @ViewChild('pageValue') pageValue: any;

  @ViewChild('SearchMail') SearchMail: any;

  public loggedinasAdming = true;

  public Page:number = 0;

  public log(dings :any):string{
    console.log(dings)
    var re:any = OrderBy[parseInt(dings.keys)].toString();
    console.log(re)
    return re;
  }

  APWrong:string = "";
  ARMessage:string = "";

  public myInnerHeight: number = 0;

  constructor() {this.myInnerHeight= 0}

  ngOnInit(): void {
    window.addEventListener("resize", this.onResize.bind(this));
    this.onResize(this);
    if(sessionStorage.getItem("AdminPasswort") != null){
      this.loggedinasAdming = true;
    }
  }


  loadUsers(page:number = -1):void {
    this.users =[];

    window.scrollTo(0, 0); 
   if(this.listi !== undefined){ this.listi.nativeElement.scrollTop = 0;
  }
    this.selectedUser = {} as HoleUserInfos
    let AC:AdminComponent = this;

    if(page == -1){
      this.Page++;
    }else if (page == -2){
      this.Page--;
      page = -1;
    }else{
      this.Page = 0;
    }

    let RequestParameter : GetAdminUserInfos = {
      Page:(page == -1)?AC.Page:page,
      OB:OrderBy[this.orderBy.nativeElement.value as keyof typeof OrderBy],
      F:Filter[this.filterBy.nativeElement.value as keyof typeof Filter],
      password: (this.password !== undefined)?this.password.nativeElement.value:sessionStorage.getItem('AdminPasswort'),
      Search:(this.SearchMail.nativeElement.value == "")?"-":this.SearchMail.nativeElement.value
    };

    const Callback = (res: any) => {
      if(res != null){
      AC.APWrong = "";
      if(this.password !== undefined){
        sessionStorage.setItem("AdminPasswort", this.password.nativeElement.value )
        this.loggedinasAdming = false;
      }
      this.users = res as HoleUserInfos[];
      this.users.forEach(user => {
        user.configInfos.erstellung = new Date(user.configInfos.erstellung);
      });
      }
    }

    const ErrorCallback = (res: any) => {
      AC.APWrong = "The Admin Password is wrong";
    }
    SendGet("api/Admin/GetUsers",Callback,false,RequestParameter, ErrorCallback)
  }
  BlockUser():void{
    let id:number = this.selectedUser.userId;
    console.log(this.selectedUser)
    const Callback = (res: Response) => {
        if(res.code == 1){
          console.log("User blocked");
          this.ARMessage = `User ${id} blocked`;
        } 
        else{
          console.log("Error: " + res.desc);
          this.ARMessage = "Error: " + res.desc;
        }
    }

    const body:BlockUser = {
      password :sessionStorage.getItem('AdminPasswort')??"",
      userId : id
    }
    SendPost("api/Admin/BlockPerson",body,Callback,false)

    this.loadUsers();
  }

  UnBlockUser():void{

    console.log(this.selectedUser.userId)
    let id:number = this.selectedUser.userId;
    const Callback = (res: Response) => {
        if(res.code == 1){
          console.log("User Unblocked");
          this.ARMessage = `User ${id} Unblocked`;
        } 
        else{
          console.log("Error: " + res.desc);
          this.ARMessage = "Error: " + res.desc;
        }
    }

    const body:BlockUser = {
      password : sessionStorage.getItem('AdminPasswort')??"",
      userId : this.selectedUser.userId
    }
    SendPost("api/Admin/UnblockPerson",body,Callback,false)
    this.loadUsers();
  }

  SelectUser(user: HoleUserInfos): void {
    this.selectedUser = user;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event:any) {
    this.myInnerHeight = window.innerHeight+1000;
}
}
