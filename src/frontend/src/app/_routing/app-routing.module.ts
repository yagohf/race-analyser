import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from '../home/home.component';
import { AccessComponent } from '../access/access.component';
import { AuthGuard } from '../_guards/auth.guard';
import { ResultsComponent } from '../results/results.component';
import { SubmitComponent } from '../submit/submit.component';

const routes: Routes = [
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },
    { path: 'access', component: AccessComponent },
    { path: 'results', component: ResultsComponent },
    { path: 'submit', component: SubmitComponent, canActivate: [AuthGuard] }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {

}