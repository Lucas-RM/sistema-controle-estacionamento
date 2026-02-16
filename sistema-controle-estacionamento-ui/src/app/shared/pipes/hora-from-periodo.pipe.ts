import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'horaFromPeriodo'
})
export class HoraFromPeriodoPipe implements PipeTransform {
  transform(value: string): number | string {
    if (!value) {
      return '';
    }

    if (value.includes(' - ')) {
      value = value.split(' - ')[0].trim();
    }

    try {
      const date = this.parseUTCDate(value);
      
      if (isNaN(date.getTime())) {
        return '';
      }

      return date.getHours();
    } catch (e) {
      return '';
    }
  }

  private parseUTCDate(dateString: string): Date {

    dateString = dateString.trim();

    if (/[Zz]|[\+\-]\d{2}:?\d{2}$/.test(dateString)) {
      return new Date(dateString);
    }

    const match = dateString.match(/^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}(?::\d{2})?)/);
    if (match) {
      return new Date(match[1] + 'Z');
    }

    return new Date(dateString);
  }
}
