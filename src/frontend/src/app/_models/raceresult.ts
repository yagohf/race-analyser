import { DriverResult } from "./driverresult";
import { BestLap } from "./bestlap";

export class RaceResult {
    constructor() {
        this.bestLap = new BestLap();
    }

    raceId: number;
    raceDescription: string;
    totalLaps: number;
    raceDate: Date;
    winner: string
    uploader: string;
    uploadDate: Date;
    bestLap: BestLap;
    results: DriverResult[];
}