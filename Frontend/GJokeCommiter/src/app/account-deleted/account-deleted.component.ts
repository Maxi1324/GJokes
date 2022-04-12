import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-account-deleted',
  templateUrl: './account-deleted.component.html',
  styleUrls: ['./account-deleted.component.css']
})
export class AccountDeletedComponent implements OnInit {

  constructor(private router:Router) { }

  ngOnInit(): void {
  }

  BackToHome():void{
    this.router.navigateByUrl('/');
  }

}
