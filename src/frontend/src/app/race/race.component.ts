import { Component, OnInit } from '@angular/core';
import { RaceService } from '../_services/race.service';
import { RaceResult } from '../_models/RaceResult';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-Race',
    templateUrl: './Race.component.html',
    styleUrls: ['./Race.component.css']
})
export class RaceComponent implements OnInit {
    race: RaceResult = new RaceResult();

    constructor(private raceService: RaceService, private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.loadRaceResult(params['raceId']);
        });
    }

    loadRaceResult(raceId: number) {
        this.raceService.getRaceResult(raceId).subscribe(data => {
            this.race = data;
        });
    }
}
