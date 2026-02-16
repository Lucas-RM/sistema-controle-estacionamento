import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'periodoFormat'
})
export class PeriodoFormatPipe implements PipeTransform {
  transform(value: string, format: string = 'dd/MM/yyyy HH:mm'): string {
    if (!value) {
      return '';
    }

    if (value.includes(' - ')) {
      const parts = value.split(' - ');
      const dataInicio = parts[0].trim();
      const dataFim = parts[1].trim();
      
      try {
        const dateInicio = this.parseUTCDate(dataInicio);
        const dateFim = this.parseUTCDate(dataFim);
        
        if (isNaN(dateInicio.getTime()) || isNaN(dateFim.getTime())) {
          return value;
        }

        const formattedInicio = this.formatDate(dateInicio, format);
        const formattedFim = this.formatDate(dateFim, format);
        
        return `${formattedInicio} - ${formattedFim}`;
      } catch (e) {
        return value;
      }
    }

    try {
      const date = this.parseUTCDate(value);
      
      if (isNaN(date.getTime())) {
        return value; 
      }

      return this.formatDate(date, format);
    } catch (e) {
      return value;
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

  private formatDate(date: Date, format: string): string {
    const year = date.getFullYear();
    const month = this.padStart(date.getMonth() + 1, 2);
    const day = this.padStart(date.getDate(), 2);
    const hours = this.padStart(date.getHours(), 2);
    const minutes = this.padStart(date.getMinutes(), 2);
    const seconds = this.padStart(date.getSeconds(), 2);

    return format
      .replace('yyyy', String(year))
      .replace('MM', month)
      .replace('dd', day)
      .replace('HH', hours)
      .replace('mm', minutes)
      .replace('ss', seconds);
  }

  private padStart(value: number, length: number): string {
    const str = String(value);
    if (str.length >= length) {
      return str;
    }
    let zeros = '';
    for (let i = 0; i < length - str.length; i++) {
      zeros += '0';
    }
    return zeros + str;
  }
}
