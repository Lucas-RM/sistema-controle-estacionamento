import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'placaFormat'
})
export class PlacaFormatPipe implements PipeTransform {
  transform(placa: string): string {
    if (!placa) return '';
    
    // Remove espaços e hífens
    placa = placa.replace(/[\s-]/g, '').toUpperCase();
    
    // Formato Brasil antigo: ABC1234 -> ABC-1234
    if (/^[A-Z]{3}\d{4}$/.test(placa)) {
      return `${placa.substr(0, 3)}-${placa.substr(3)}`;
    }
    
    // Formato Brasil Mercosul: ABC1D23 -> ABC-1D23
    if (/^[A-Z]{3}\d[A-Z]\d{2}$/.test(placa)) {
      return `${placa.substr(0, 3)}-${placa.substr(3)}`;
    }
    
    // Retorna sem formatação para outros formatos
    return placa;
  }
}
