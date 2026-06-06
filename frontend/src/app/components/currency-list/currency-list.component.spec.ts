import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { CurrencyListComponent } from './currency-list.component';
import { CurrencyService } from '../../services/currency.service';

describe('CurrencyListComponent', () => {

  let component: CurrencyListComponent;
  let fixture: ComponentFixture<CurrencyListComponent>;
  let service: jasmine.SpyObj<CurrencyService>;

  beforeEach(async () => {

    const serviceMock = jasmine.createSpyObj('CurrencyService', [
      'getCurrencies',
      'fetchCurrencies'
    ]);

    await TestBed.configureTestingModule({
      imports: [CurrencyListComponent],
      providers: [
        { provide: CurrencyService, useValue: serviceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CurrencyListComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(CurrencyService) as jasmine.SpyObj<CurrencyService>;
  });


  it('should create', () => {
    expect(component).toBeTruthy();
  });


  it('should load data on init', () => {

    service.getCurrencies.and.returnValue(
      of([
        {
          id: '1',
          currencyCode: 'USD',
          currencyName: 'US Dollar',
          rate: 4.0,
          effectiveDate: '2026-01-01'
        }
      ])
    );

    fixture.detectChanges();

    expect(service.getCurrencies).toHaveBeenCalled();
    expect(component.currencies.length).toBe(1);
  });


  it('should call fetchCurrencies on button click', () => {

    service.fetchCurrencies.and.returnValue(of({}));
    service.getCurrencies.and.returnValue(of([]));

    component.fetchData();

    expect(service.fetchCurrencies).toHaveBeenCalled();
  });


  it('should reload data after fetch', () => {

    service.fetchCurrencies.and.returnValue(of({}));

    service.getCurrencies.and.returnValue(of([
      {
        id: '2',
        currencyCode: 'EUR',
        currencyName: 'Euro',
        rate: 4.3,
        effectiveDate: '2026-01-01'
      }
    ]));

    component.fetchData();

    expect(service.getCurrencies).toHaveBeenCalled();
    expect(component.currencies.length).toBe(1);
  });
});