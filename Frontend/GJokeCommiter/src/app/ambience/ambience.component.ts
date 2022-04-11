import { ConRange } from './../BackendCom';
import { Component, Input, OnInit, NgModule, ViewChild } from '@angular/core';
import { ConfigInfos, GithubAccountSettings, Response, SendGet, SendPost } from '../BackendCom';

@Component({
  selector: 'app-ambience',
  templateUrl: './ambience.component.html',
  styleUrls: ['./ambience.component.css']
})
export class AmbienceComponent implements OnInit {

  constructor() { }

  @Input()
  public jmin: number = 0;
  public jmax: number = 15;
  public gusername: string = "";
  public gemail: string = "";

  @ViewChild('minCon') minCon!: any;
  @ViewChild('maxCon') maxCon!: any;

  @ViewChild('Username') Username!: any;
  @ViewChild('Email1') Email!: any;

  GSFeedback:string = "";
  color1:string = "green";
  GSActive:boolean = true

  color2:string = "green";
  ConFeedback:string = "";
  ConActive:boolean = true



  public save_github_settings( event: Event, gusername: string, gemail: string): void {
    console.log("Saving github settings");
    console.log(gusername);
    console.log(gemail);
  }

  public save_jokes_settings( event: Event, jmin: number, jmax: number): void {
    console.log("Saving jokes settings");
    console.log(jmin);
    console.log(jmax);
  }
  
  ngOnInit(): void {
    this.FetchData()
  }

  FetchData():void{
    let AC:AmbienceComponent = this;
    const Callback=function(CI:ConfigInfos){
      console.log(CI);
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
}
