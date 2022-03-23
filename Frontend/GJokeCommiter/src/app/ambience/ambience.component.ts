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
}
