import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'tempoPermanencia'
})
export class TempoPermanenciaPipe implements PipeTransform {
  transform(dataEntrada: string): string {
    if (!dataEntrada) return '';
    
    const entrada = new Date(dataEntrada);
    const agora = new Date();
    const diffMs = agora.getTime() - entrada.getTime();
    
    const horas = Math.floor(diffMs / (1000 * 60 * 60));
    const minutos = Math.floor((diffMs % (1000 * 60 * 60)) / (1000 * 60));
    
    if (horas > 0) {
      return `${horas}h ${minutos}min`;
    }
    return `${minutos}min`;
  }
}
