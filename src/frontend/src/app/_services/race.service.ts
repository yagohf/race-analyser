import { Injectable } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { Listing } from '../_models/infrastructure/listing';
import { RaceSummary } from '../_models/racesummary';
import { RaceResult } from '../_models/raceresult';
import { RaceType } from '../_models/racetype';

@Injectable({
    providedIn: 'root'
})
export class RaceService {
    constructor(private http: HttpClient) { }

    listRaceTypes() {
        const url = `${environment.apiAddress}/races/types`;
        return this.http.get<RaceType[]>(url)
            .pipe(
                tap(_ => console.log(_))
            );
    }

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

    submit(formData: FormData) {
        const url = `${environment.apiAddress}/races`;

        return this.http.post(url, formData, {
            reportProgress: true,
            observe: 'events'
        }).pipe(map((event) => {
            switch (event.type) {
                case HttpEventType.UploadProgress:
                    const progress = Math.round(100 * event.loaded / event.total);
                    return { status: 'progress', response: progress };
                case HttpEventType.Response:
                    return { status: 'finished', response: event.body };
                default:
                    return { status: 'not-handled', response: `Evento não tratado: ${event.type}` };;
            }
        }));
    }

    downloadExample(): Observable<any> {
        const url = `${environment.apiAddress}/races/example`;
        return this.http.get(url, { responseType: 'arraybuffer' })
            .pipe(
                tap(_ => console.log(_))
            );
    }
}