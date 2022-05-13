import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Response, SendGet } from '../BackendCom';

@Component({
  selector: 'app-generate-cont',
  templateUrl: './generate-cont.component.html',
  styleUrls: ['./generate-cont.component.css'],
})
export class GenerateContComponent implements OnInit {
  GJN: string = '';
  GJNActive: boolean = true;

  GJP: string = '';
  GJPActive: boolean = true;

  @ViewChild('Cons1') GTN1!: any;

  @ViewChild('SD') SD!: any;
  @ViewChild('ED') ED!: any;

  @ViewChild('minCon') minCon!: any;
  @ViewChild('maxCon') maxCon!: any;

  constructor() {}

  ngOnInit(): void {}

  GenContToday(): void {
    this.GJN = '';
    let n: string = this.GTN1.nativeElement.value;
    let r: boolean = window.confirm(
      'Do you really want to generate ' + n + ' jokes today?'
    );
    if (r) {
      this.GJNActive = false;
      let GC = this;
      const Callback = (res: Response) => {
        GC.GJN = res.desc;
        this.GJNActive = true;
      };

      let Parameter = {
        num: n == '' ? 0 : n,
      };
      SendGet('api/Cont/GenerateContToday', Callback, true, Parameter);
    } else {
    }
  }

  GenContRange(): void {
    let r: boolean = window.confirm(
      'Do you really want to generate jokes in the past?'
    );
    if (r) {
      this.GJPActive = false;
      this.GJP = "";
      let SD = (this.SD.nativeElement.value as string).replace(/\//g, '-');
      let ED = (this.ED.nativeElement.value as string).replace(/\//g, '-');
      let minCont = this.minCon.nativeElement.value;
      let maxCont = this.maxCon.nativeElement.value;

      let GC: GenerateContComponent = this;
      SendGet(
        'api/Cont/GenerateContsPast',
        (res: Response) => {
          GC.GJPActive = true;
          GC.GJP = res.desc;
        },
        true,
        {
          StartDate: SD == '' ? '-' : SD,
          EndDate: ED == '' ? '-' : ED,
          MinCont: minCont,
          MaxCont: maxCont,
        }
      );
    }
  }
}
