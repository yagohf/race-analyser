import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BehaviorSubject, throwError } from 'rxjs';
import { LoggedUser } from '../_models/loggeduser';
import { Authentication } from '../_models/authentication';

@Injectable()
export class AuthenticationService {
  constructor(private http: HttpClient) { }

  static OFFLINE_USER: LoggedUser = { authenticated: false, login: null, token: null };

  //Observable para expor o status do usuário como logado ou não.
  private loggedUser = new BehaviorSubject<LoggedUser>(this.getLoggedUser());
  get loggedUserInfo() {
    return this.loggedUser.asObservable();
  }

  login(authentication: Authentication) {
    return this.http.post<any>(`${environment.apiAddress}/users/token`, authentication)
      .pipe(map(result => {
        //Login com sucesso se o retorno contiver um token.
        if (result && result.token) {
          //Guardar o token em localstorage para poder manter o usuário logado entre refreshs.
          localStorage.setItem('loggedUser', JSON.stringify(result));
        }

        //Enviar mensagem de usuário logado (ou não) para quem quer que esteja observando.
        this.loggedUser.next(this.getLoggedUser());
        return result;
      }));
  }

  logout() {
    //Remover o usuário da localstorage.
    localStorage.removeItem('loggedUser');

    //Enviar mensagem de usuário deslogado para quem quer que esteja observando.
    this.loggedUser.next(AuthenticationService.OFFLINE_USER);
  }

  getLoggedUser(): LoggedUser {
    let u = new LoggedUser();
    u.authenticated = false;
    u.login = null;
    u.token = null;

    var userLocalStorage = localStorage.getItem('loggedUser');
    if (userLocalStorage) {
      u = JSON.parse(userLocalStorage);
      u.authenticated = true;
    }

    return u;
  }
}