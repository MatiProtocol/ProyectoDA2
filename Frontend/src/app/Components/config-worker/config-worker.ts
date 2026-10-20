import { Component, inject, signal } from '@angular/core';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { map, Observable, startWith } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from "@angular/material/card";
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatButton } from '@angular/material/button';
import { WorkerService } from '../../Services/worker.service';
import { WorkerModel } from '../../Models/worker.model';
import { WorkerRequest } from '../../Models/worker.request';

@Component({
  selector: 'app-config-worker',
  standalone: true,
  imports: [
    MatCardModule,
    MatDialogModule,
    ReactiveFormsModule,
    FormsModule,
    MatAutocompleteModule,
    MatInputModule, 
    MatFormFieldModule, 
    MatProgressSpinner, 
    MatButton, 
    AsyncPipe],
  templateUrl: './config-worker.html',
  styleUrl: './config-worker.css'
})
export class ConfigWorker {
  public dialogRef = inject(MatDialogRef<ConfigWorker>);
  loading = signal(false);
  error = signal<string | null>(null);
  success = signal(false);
  private WorkerService = inject(WorkerService);
  private data = inject(MAT_DIALOG_DATA) as { workerId: number };

  control = new FormControl<string | null>(null);
  options: string[] = [];
  filteredOptions?: Observable<string[]>;
  
  ngOnInit() {
    this.loading.set(true);
    this.LoadStrategies();
  }

  displayFn(strategy: string): string {
    return strategy ?? '';
  }
  
  SaveChanges() {
    this.loading.set(true);
    this.error.set(null);
    this.success.set(false);

    if(this.options.length === 0) return;
    if(this.control.value === null) return;
    if(this.control.value !== this.options.find(option => option === this.control.value)){
      this.error.set("Rol inválido");
      this.loading.set(false);
      return;
    }
    let workerData: WorkerModel;
    this.WorkerService.GetById(this.data.workerId).subscribe({
      next: (worker: WorkerModel) => {
        workerData = worker;
        this.updateRole(workerData);
        this.success.set(true);
        this.loading.set(false);
        this.error.set(null);
      },
      error: (err) => {
        this.error.set("Error al actualizar el rol del trabajador");
        this.loading.set(false);
        this.success.set(false);
        return;
      }
    });
  }

  private updateRole(workerData: WorkerModel) {
    let selectedRole = "";
    if(this.control.value === "Operador"){
      selectedRole = "Operator,";
    } else if (this.control.value === "Administrador"){
      selectedRole = "ParkAdmin,";
    } else if (this.control.value === "Ambos"){
      selectedRole = "ParkAdmin,Operator,";
    }
    console.log(selectedRole);
    console.log(workerData);
    if(workerData.roles.includes(selectedRole)){
      this.error.set("Rol ya asignado al trabajador");
      this.loading.set(false);
      return;
    }
  
    if (selectedRole) {
      const worker: WorkerRequest = {
        Name: workerData.name,
        Email: workerData.email,
        Roles: selectedRole,
        NewPassword: "WorkerPass"
      };
      console.log('Updating worker with payload:', worker);
      this.WorkerService.Update(this.data.workerId, worker).subscribe({
      next: () => {
          this.error.set(null);
          this.success.set(true);
          this.loading.set(false);
        },
        error: (err) => {
          this.success.set(false);
          this.error.set(err.message || "Error al actualizar la estrategia de puntuación");
          this.loading.set(false);
        }
      });
    }
  }

  private LoadStrategies() {
  this.options = [ "Operador", "Administrador", "Ambos"];
    this.FilterOptions();
    this.loading.set(false);
  }

  private FilterOptions() {
    this.filteredOptions = this.control.valueChanges.pipe(
      startWith(''),
      map(value => this.Filter(value || '') )
    );
  }
    private Filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

}
