import { Router } from '@angular/router';
import { ConRange, UserCred } from './../BackendCom';
import { Component, Input, OnInit, NgModule, ViewChild } from '@angular/core';
import { ConfigInfos, GithubAccountSettings, Response, SendGet, SendPost } from '../BackendCom';

@Component({
  selector: 'app-ambience',
  templateUrl: './ambience.component.html',
  styleUrls: ['./ambience.component.css']
})
export class AmbienceComponent implements OnInit {

  @Input()
  public jmin: number = 0;
  public jmax: number = 15;
  public gusername: string = "";
  public gemail: string = "";

  @ViewChild('minCon') minCon!: any;
  @ViewChild('maxCon') maxCon!: any;

  @ViewChild('Username') Username!: any;
  @ViewChild('Email1') Email!: any;

  GSFeedback:string = " ";
  color1:string = "green";
  GSActive:boolean = true

  color2:string = "green";
  ConFeedback:string = " ";
  ConActive:boolean = true

  @ViewChild('DelPasswort') DelPassword!: any;
  @ViewChild('DelConfirm') DelConfirm!: any;
  DelFeedback:string = " ";
  DelActive:boolean = true

  @ViewChild('OldPassword') OldPassword!: any;
  @ViewChild('NewPassword') NewPassword!: any;
  ChFeedback:string = " ";
  ChActive:boolean = true
  color3:string = "green";

  constructor(private route: Router) { }

  public save_github_settings( event: Event, gusername: string, gemail: string): void {
    console.log("Saving github settings");
  }

  public save_jokes_settings( event: Event, jmin: number, jmax: number): void {
    console.log("Saving jokes settings");
  }
  
  ngOnInit(): void {
    this.FetchData()
  }

  FetchData():void{
    let AC:AmbienceComponent = this;
    const Callback=function(CI:ConfigInfos){
      AC.jmax = CI.maxCon;
      AC.jmin = CI.minCon;
      AC.gemail = CI.githubEmail;
      AC.gusername = CI.githubUsername;
    }
    SendGet("api/Config/GetUserInfo",Callback,true,{});
  }

  SaveUserGithub():void{
    this.GSFeedback = "";
    this.GSActive = false;
    let body:GithubAccountSettings = {
      githubUserName :this.Username.nativeElement.value,
      githubEmail :this.Email.nativeElement.value
    }

    let AC:AmbienceComponent = this;
    const Callback = function(res:Response){
      AC.FetchData();
      AC.GSFeedback = res.desc
      if(res.code== 1){
        AC.color1="green"
      }
      else{
        AC.color1 = "red"
      }
      AC.GSActive = true;
    }
    SendPost("api/Config/SetGithubAccountSettings",body,Callback,true)
  }


  SaveCons():void{
    console.log(this.color2);
    this.ConFeedback = "";
    this.ConActive = false;
    let body:ConRange = {
      minCon:this.minCon.nativeElement.value,
      maxCon:this.maxCon.nativeElement.value
    }

    let AC:AmbienceComponent = this;
    const Callback = function(res:Response){
      AC.FetchData();
      AC.ConFeedback = res.desc
      if(res.code== 1){
        AC.color2="green"
      }
      else{
        AC.color2 = "red"
      }
      AC.ConActive = true;
    }
    SendPost("api/Config/SetConRange",body,Callback,true)
  }

  DeleteACcount():void{
    let AC:AmbienceComponent = this;
    AC.DelActive = false;

    this.DelFeedback = "";

    let con:string = this.DelConfirm.nativeElement.value;
    if(con != "Gitty"){
      this.DelFeedback = "Pleace type Gitty in the confirmation field";
      AC.DelActive = true;
      return;
    }

    let body:any={
      password:this.DelPassword.nativeElement.value,
    }

    const Callback = function(res:Response){
      if(res.code == 1){
        AC.DelFeedback = res.desc
        AC.DelActive = true;
        AC.route.navigateByUrl('/DeletedAccount');
      }
      else{
        AC.DelFeedback = res.desc
        AC.DelActive = true;
      }
    }
    SendPost("api/Account/DeleteAccount",body,Callback,true);
  }

  ChangePassword():void{
    this.ChActive = false;

    let body:any={
      OldPassword:this.OldPassword.nativeElement.value,
      NewPassword:this.NewPassword.nativeElement.value
    }
    let AC:AmbienceComponent = this
    const Callback = function(res:Response){
      if(res.code == 1){
        AC.ChFeedback = res.desc
        AC.color3 = "green"
        AC.OldPassword.nativeElement.value = "";
        AC.NewPassword.nativeElement.value = "";
      }
      else{
        AC.ChFeedback = res.desc
        AC.color3= "red";
      }
      AC.ChActive = true;
    }
    SendPost("api/Account/ChangePassword",body,Callback,true);
  }
}
