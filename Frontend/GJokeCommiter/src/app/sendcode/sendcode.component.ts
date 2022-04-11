import { Router } from '@angular/router';
import { EmailVerDataService } from './../email-ver-data.service';
import { ActivateAccountInfo } from './../BackendCom';
import { Response, UserCred } from './../BackendCom';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, ValidatorFn } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { SendPost } from '../BackendCom';

@Component({
  selector: 'app-sendcode',
  templateUrl: './sendcode.component.html',
  styleUrls: ['./sendcode.component.css']
})
export class SendcodeComponent implements OnInit {

  @ViewChild('Token') Token!: any;

  hidden: string = "hidden";
  ErrorMessage: string = "";

  hidden2: string = "hidden";
  Message: string = " ";

  active:boolean = true;

  constructor(private router: Router, private EVDS: EmailVerDataService) { }

  ngOnInit(): void {
    document.getElementById("cover")?.classList.remove("hidden-strong");
    let sc: SendcodeComponent = this;
    this.EVDS.currentWasReseent.subscribe(wasresent => {
      if (wasresent) {
        sc.hidden2 = "";
        sc.Message = "Your Email is already registered. We've sent you the verification code again, but we didn't change the login informations";
      }
      else {
        sc.hidden = "hidden";
      }
    });

  }

  SendToken() {
    this.active = false;
    this.EVDS.CUserID.subscribe(CUserID => {
      var body: ActivateAccountInfo = {
        userId: CUserID,
        code: this.Token.nativeElement.value,
      };

      var Callback = (res: Response) => {
        this.active = true;
        if (res.code == 1) {
          this.ErrorMessage = ""
          this.hidden = "hidden";
          sessionStorage.setItem("JWTToken", res.desc);
          this.router.navigateByUrl('/config');
        } else {
          this.hidden = "";
          this.ErrorMessage = res.desc;
        }
      }
      SendPost("api/Account/ActivateAccount", body, Callback, false);
    });
  }
}
