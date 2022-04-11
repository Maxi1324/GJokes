import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmailVerDataService {

  private wasresent = new BehaviorSubject<boolean>(false);
  currentWasReseent = this.wasresent.asObservable();

  private UserID = new BehaviorSubject<number>(-1);
  CUserID = this.UserID.asObservable();

  constructor() { }

  changeWasresent(wasresent: boolean) {
    this.wasresent.next(wasresent);
  }

  changeCUserID(CUserID: number) {
    this.UserID.next(CUserID);
  }
}
