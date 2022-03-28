import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {

  constructor(private route:Router) {}

  ngOnInit(): void {
    //5Head get from Router Instance
    const route = this.route;
    //Get cover div and add the event listener to it to change the href to /
    let cover = document.getElementById("cover");
    cover?.addEventListener("click", function() {
      route.navigate(['/']);
    });
  }

}
