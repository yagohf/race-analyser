import { Component, OnInit } from '@angular/core';
import { RaceService } from '../_services/race.service';
import { RaceSummary } from '../_models/racesummary';
import { PagerService } from '../_services/pager.service';
import { Paging } from '../_models/infrastructure/paging';
import { Listing } from '../_models/infrastructure/listing';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css']
})
export class ResultsComponent implements OnInit {

  constructor(private raceService: RaceService, private pagerService: PagerService) { }

  ngOnInit() {
    this.loadSummaries(1);
  }

  loadSummaries(page: number) {
    this.raceService.listSummaries(this.searchTerm, page).subscribe(data => {
      this.summaries = data;
      this.updatePager(data.paging);
    });
  }

  updatePager(paging: Paging) {
    this.pager = this.pagerService.getPager(paging.totalItems, paging.currentPage);
  }

  setPage(p: number, disabled: boolean) {
    if (disabled) {
      return;
    }

    this.loadSummaries(p);
  }

  forceSearchByName(text: string) {
    this.searchTerm = text;
    this.loadSummaries(1);
  }

  searchTerm: string;
  raceSummaries: RaceSummary[];
  pager: any = {};
  summaries: Listing<RaceSummary> = new Listing<RaceSummary>();
}
