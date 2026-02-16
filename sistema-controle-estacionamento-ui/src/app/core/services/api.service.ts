import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { environment } from '../../../environments/environment';

@Injectable()
export class ApiService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: Http) {}

  get<T>(endpoint: string, params?: any): Observable<T> {
    const url = `${this.baseUrl}${endpoint}`;
    const options = this.createRequestOptions(params);
    
    return this.http.get(url, options)
      .map(res => res.json())
      .catch(this.handleError);
  }

  post<T>(endpoint: string, body: any): Observable<T> {
    const url = `${this.baseUrl}${endpoint}`;
    const options = this.createRequestOptions();
    
    return this.http.post(url, JSON.stringify(body), options)
      .map(res => res.json())
      .catch(this.handleError);
  }

  put<T>(endpoint: string, body: any): Observable<T> {
    const url = `${this.baseUrl}${endpoint}`;
    const options = this.createRequestOptions();
    
    return this.http.put(url, JSON.stringify(body), options)
      .map(res => res.json())
      .catch(this.handleError);
  }

  private createRequestOptions(params?: any): RequestOptions {
    const headers = new Headers({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    const options = new RequestOptions({ headers });

    if (params) {
      const searchParams = new URLSearchParams();
      Object.keys(params).forEach(key => {
        if (params[key] !== undefined && params[key] !== null) {
          searchParams.set(key, params[key].toString());
        }
      });
      options.search = searchParams;
    }

    return options;
  }

  private handleError(error: any): Observable<any> {
    let errorMessage = 'Erro inesperado. Tente novamente mais tarde.';
    
    if (error.status === 400) {
      errorMessage = error.json().message || 'Dados inválidos. Verifique os campos.';
    } else if (error.status === 404) {
      errorMessage = 'Registro não encontrado.';
    } else if (error.status === 409) {
      errorMessage = 'Conflito detectado. Os dados foram modificados por outro usuário.';
    } else if (error.status === 422) {
      errorMessage = error.json().message || 'Não foi possível processar. Verifique as regras de negócio.';
    }

    return Observable.throw({ status: error.status, message: errorMessage });
  }
}
