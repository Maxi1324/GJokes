import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'GJokes';
  constructor() { 
    sessionStorage.setItem('BackendRoute', 'https://localhost:7099/');
  }
}
