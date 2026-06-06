import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CurrencyService } from '../../services/currency.service';
import { CurrencyRate } from '../../models/currency-rate.model';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-currency-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './currency-list.component.html',
  styleUrls: ['./currency-list.component.css']
})
export class CurrencyListComponent implements OnInit {

  currencies: CurrencyRate[] = [];


  selectedYear = '';
  selectedQuarter = '';
  selectedMonth = '';
  selectedDay = '';

  constructor(private currencyService: CurrencyService) {}

  ngOnInit(): void {  
    this.loadData();
  }

  fetchData(): void {
    this.currencyService.fetchCurrencies()
      .subscribe(() => {
        this.loadData();
      });
  }

  loadData(): void {
    this.currencyService
      .getCurrencies(
        this.toNumber(this.selectedDay),
        this.toNumber(this.selectedMonth),
        this.toNumber(this.selectedYear)
      )
      .subscribe(data => {
        this.currencies = data;
      });
  }
  private toNumber(value: string): number | undefined {
    const num = Number(value);
    return value ? num : undefined;
  }
}