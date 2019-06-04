import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        //Adicionar o token de autenticação a cada request.
        let loggedUser = this.authenticationService.getLoggedUser();
        if (loggedUser) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${loggedUser.token}`
                }
            });
        }

        return next.handle(request);
    }
}