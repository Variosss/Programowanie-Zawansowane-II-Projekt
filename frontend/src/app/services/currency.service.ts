import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CurrencyRate } from '../models/currency-rate.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CurrencyService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getCurrencies(day?: number, month?: number, year?: number): Observable<CurrencyRate[]> {

    let params = new HttpParams();

    if (year != null) params = params.set('year', year);
    if (month != null) params = params.set('month', month);
    if (day != null) params = params.set('day', day);

    return this.http.get<CurrencyRate[]>(`${this.apiUrl}/currencies`, { params });
  }

  fetchCurrencies(): Observable<any> {
    return this.http.post(`${this.apiUrl}/currencies/fetch`, {});
  }
}