import { Component } from '@angular/core';
import { CurrencyListComponent }
from './components/currency-list/currency-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CurrencyListComponent
  ],
  template: `
    <app-currency-list />
  `
})
export class AppComponent {}