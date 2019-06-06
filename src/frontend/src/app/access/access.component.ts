import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_services/authentication.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Authentication } from '../_models/authentication';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from '../_services/message.service';
import { EnumMessageType } from '../_models/enums/enum.messagetype';

@Component({
    selector: 'app-access',
    templateUrl: './access.component.html',
    styleUrls: ['./access.component.css']
})
export class AccessComponent implements OnInit {

    logging: boolean = false;
    formLogin: FormGroup;
    returnUrl: string;
    submitted: boolean = false;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private authenticationService: AuthenticationService,
        private formBuilder: FormBuilder,
        private messageService: MessageService) { }

    ngOnInit() {
        this.formLogin = this.formBuilder.group({
            user: ['', Validators.required],
            password: ['', Validators.required]
        });

        //Setar URL de retorno após o login.
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    //Getter para facilitar acesso aos controles.
    get f() { return this.formLogin.controls; }

    submeterLogin() {
        this.submitted = true;

        if (this.formLogin.invalid) {
            return;
        }

        this.logging = true;

        let authentication: Authentication = {
            login: this.f.user.value,
            password: this.f.password.value
        };

        this.authenticationService.login(authentication)
            .subscribe(
                result => {
                    console.log(result);
                    if (result && result.token) {
                        this.router.navigate([this.returnUrl]);
                    }
                    else {
                        this.messageService.sendMessage('Usuário ou senha inválidos.', EnumMessageType.ERROR);
                        this.logging = false;
                    }
                },
                error => {
                    console.log(error);
                    this.logging = false;
                });
    }
}