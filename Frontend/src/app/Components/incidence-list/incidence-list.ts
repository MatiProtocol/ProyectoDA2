import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDialogRef, MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { IncidenceService } from '../../Services/incidence.service';
import { IncidenceCreate } from '../incidence-create/incidence-create';
import { AttractionModel } from '../../Models/attraction.model';
import { MatDialog } from '@angular/material/dialog';
import { AttractionsService } from '../../Services/attractions.service';
import { IncidenceModel } from '../../Models/incidence.model';
import { DateTimeResponse } from '../../Models/date-time.response';
import { DateTimeService } from '../../Services/date-time.service';
@Component({
  selector: 'app-incidence-list',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatInputModule, MatDialogModule, ReactiveFormsModule, MatIconModule],
  templateUrl: './incidence-list.html',
  styleUrl: './incidence-list.css'
})
export class IncidenceList {
  private is = inject(IncidenceService);
  public dialogRef = inject(MatDialogRef<IncidenceCreate>);
  public data = inject(MAT_DIALOG_DATA) as { attraction?: AttractionModel };
  private AttractionService = inject(AttractionsService);
  public incidences: IncidenceModel[] = [];
  public dateService = inject(DateTimeService);  
  public todayDate: DateTimeResponse | null = null;
  
  ngOnInit() {
    this.dateService.GetDateTime().subscribe({
      next: (date) => {
        this.todayDate = date;
        this.LoadIncidences();
      },
      error: (error) => {
      }
    });
  }

  LoadIncidences(){
    if (!this.data.attraction) {
      this.incidences = [];
      return;
    }
    this.is.GetByAttractionId(this.data.attraction.id).subscribe({
      next: (item) => {
        this.incidences = item;
      },
      error: (err) => {
        this.incidences = [];
      }
    });
  }
  deactivateIncidence(incidenceId: number) {
    this.is.DeactivateIncidence(incidenceId).subscribe({
      next: (_) => {
        this.LoadIncidences();    
      },
      error: (err) => {
      }
    });
  }
  dialog = inject(MatDialog);

  onIncidenceAddDialog(attractionId: number) {
    this.dialog.open(IncidenceCreate, {
      width: '400px',
      disableClose: false,
      data: {
        attraction: { id: attractionId }
      }
    }).afterClosed().subscribe(() => {
      window.location.reload();
    });
  }

}
