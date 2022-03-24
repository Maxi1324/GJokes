import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-ambience',
  templateUrl: './ambience.component.html',
  styleUrls: ['./ambience.component.css']
})
export class AmbienceComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  @Input()
  public jmin: number = 0;
  public jmax: number = 25;
  public gusername: string = "";
  public gemail: string = "";

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
}
