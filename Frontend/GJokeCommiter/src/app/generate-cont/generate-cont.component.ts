import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Response, SendGet } from '../BackendCom';

@Component({
  selector: 'app-generate-cont',
  templateUrl: './generate-cont.component.html',
  styleUrls: ['./generate-cont.component.css'],
})
export class GenerateContComponent implements OnInit {

  GJN:string = "";
  GJNActive:boolean = true;

  @ViewChild('Cons1') GTN1!: any;

  constructor() {}

  ngOnInit(): void {}

  GenContToday(): void {
    let n: number = this.GTN1.nativeElement.value;
    let r: boolean = window.confirm(
      'Do you really want to generate ' + n + ' jokes today?'
    );
    if (r) {
      let GC = this;
      const Callback = (res:Response) => {
        GC.GJN = res.desc;
      };

      let Parameter = {
        num: n,
      };
      SendGet('api/Cont/GenerateContToday', Callback, true, Parameter);
    } else {
    }
  }
}
