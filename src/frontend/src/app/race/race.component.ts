import { Component, OnInit } from '@angular/core';
import { RaceService } from '../_services/race.service';
import { RaceSummary } from '../_models/racesummary';

@Component({
    selector: 'app-Race',
    templateUrl: './Race.component.html',
    styleUrls: ['./Race.component.css']
})
export class RaceComponent implements OnInit {

    constructor(private raceService: RaceService) { }

    ngOnInit() {
    }
}
