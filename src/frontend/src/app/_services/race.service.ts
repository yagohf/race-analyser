import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Listing } from '../_models/infrastructure/listing';
import { RaceSummary } from '../_models/racesummary';
import { RaceResult } from '../_models/raceresult';

@Injectable({
    providedIn: 'root'
})
export class RaceService {
    constructor(private http: HttpClient) { }

    listSummaries(description?: string, page?: number): Observable<Listing<RaceSummary>> {
        let url = `${environment.apiAddress}/races?page=${page || 1}`;
        if (description) {
            url += `&description=${description}`;
        }

        return this.http.get<Listing<RaceSummary>>(url)
            .pipe(
                tap(_ => console.log(_))
            );
    }

    getRaceResult(id: number) {
        const url = `${environment.apiAddress}/races/${id}/result`;
        return this.http.get<RaceResult>(url)
            .pipe(
                tap(_ => console.log(_))
            );
    }
}