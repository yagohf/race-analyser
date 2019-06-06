import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../_services/authentication.service';
import { LoggedUser } from '../_models/loggeduser';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  isCollapsed = true;
  loggedUserInfo$: Observable<LoggedUser>;

  constructor(private router: Router, private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.loggedUserInfo$ = this.authenticationService.loggedUserInfo;
  }

  logoff() {
    this.authenticationService.logout();
    this.router.navigate(['/home']);
  }  
}
