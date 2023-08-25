import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users: any;

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  //When the user opens the app fresh, the login status needs to be persisted
  //flow of code - app component through any other component
  //App component through setCurrentUser finds if there is any existing user key in localstorage
  //And accordingly sets the value obtained to the behaviorsubject to which nav component is subscribed to
  //Behavior subject emits users if app component finds one and emits to nav component
  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
