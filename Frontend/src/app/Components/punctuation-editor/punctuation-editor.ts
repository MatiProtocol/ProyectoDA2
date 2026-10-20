import { Component, signal, inject, OnInit } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { map, Observable, startWith } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from "@angular/material/card";
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatButton } from '@angular/material/button';
import { PunctuationService } from '../../Services/punctuation.service';
import { StrategyListResponse } from '../../Models/strategy-list.response';
import { StrategyActiveResponse } from '../../Models/strategy-active.response';

@Component({
  selector: 'app-punctuation-editor',
  standalone: true,
  imports: [
    FormsModule,
    MatCardModule,
    MatProgressSpinner,
    MatFormFieldModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatButton,
    AsyncPipe,
    MatInputModule
  ],
  templateUrl: './punctuation-editor.html',
  styleUrl: './punctuation-editor.css'
})
export class PunctuationEditor implements OnInit {
  private PunctuationService = inject(PunctuationService);

  control = new FormControl<string | null>(null);
  options: string[] = [];
  filteredOptions?: Observable<string[]>;

  loading = signal(true);
  error = signal<string | null>(null)
  success = signal<string | null>(null);

  ngOnInit() {
    this.loading.set(true);
    this.LoadStrategies();
    this.LoadActualStrategy();
  }


  displayFn(strategy: string): string {
    return strategy ?? '';
  }

  SaveStrategy() {
    this.loading.set(true);
    this.error.set(null);
    this.success.set(null);

    const selectedStrategy = this.control.value;
    if (selectedStrategy) {
      this.PunctuationService.SetStrategy({ strategyname: selectedStrategy }).subscribe({
        next: () => {
          this.success.set("Estrategia de puntuación actualizada con éxito");
          this.LoadStrategies();
          this.LoadActualStrategy();
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set(err.message || "Error al actualizar la estrategia de puntuación");
          this.loading.set(false);
        }
      });
    }
  }

    private LoadStrategies() {
    this.PunctuationService.GetAvailableStrategies().subscribe({
      next: (res: StrategyListResponse) => {
        this.options = res.strategies;
        this.FilterOptions();
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(err.message || "Error al cargar las estrategias");
        this.loading.set(false);
      }
    });
  }

  private FilterOptions() {
    this.filteredOptions = this.control.valueChanges.pipe(
      startWith(''),
      map(value => this.Filter(value || ''))
    );
  }

  private Filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  private LoadActualStrategy() {
  this.PunctuationService.GetStrategyActive().subscribe({
    next: (res: StrategyActiveResponse) => {
      const strategyName = res.strategyName;
      
      if (this.options.includes(strategyName)) {
        this.control.setValue(strategyName);
      } else {
        this.control.setValue(strategyName);
      }
    },
    error: () => {
      this.error.set("Error al cargar la estrategia de puntuación activa");
    }
  });
}

}
