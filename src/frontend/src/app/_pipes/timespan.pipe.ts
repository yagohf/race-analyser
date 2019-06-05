import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'timespan' })
export class TimeSpanPipe implements PipeTransform {

    transform(value: string, discardHour: boolean): string {
        if (!value) return '';

        if (discardHour) {
            return value.substr(0, value.length - 4).substr(3);
        }
        else {
            return value.substr(0, value.length - 4);
        }
    }
}