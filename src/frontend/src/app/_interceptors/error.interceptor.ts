import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthenticationService } from '../_services/authentication.service';
import { MessageService } from '../_services/message.service';
import { EnumMessageType } from '../_models/enums/enum.messagetype';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService, private messageService: MessageService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            if (err.status === 401) {
                //Se o servidor retornar 401, automaticamente forçar o logout e recarregar a URL.
                this.authenticationService.logout();
                location.reload(true);
            }
            else if (err.status === 0) {
                this.messageService.sendMessage('Ops... Estamos com uma indisponibilidade em nossos serviços. Por favor, tente novamente dentro de alguns instantes.', EnumMessageType.ERROR);
            }
            else if (err.status === 400) {
                this.messageService.sendMessage(err.error, EnumMessageType.ERROR);
            }
            else if (err.status === 500) {
                this.messageService.sendMessage('Ops... Parece que ocorreu um problema ao processar sua solicitação. Por favor, tente novamente.', EnumMessageType.ERROR);
            }

            const error = err.error.message || err.statusText;
            return throwError(error);
        }))
    }
}