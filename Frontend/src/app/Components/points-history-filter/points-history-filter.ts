import { 
  Component,
  inject
} from '@angular/core';
import {
  MatDialogTitle,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-points-history-filter',
  standalone: true,
  imports: [
    MatDialogTitle,
    MatDialogActions,
    MatDialogClose,
    CommonModule,
    MatButtonModule,
    MatDialogContent,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule
  ],
  templateUrl: './points-history-filter.html',
  styleUrl: './points-history-filter.css'
})
export class PointsHistoryFilter {
  private dialogRef = inject(MatDialogRef<PointsHistoryFilter>);
  private fb = inject(FormBuilder);
  data = inject<{ visitorId: number }>(MAT_DIALOG_DATA);

  filterForm: FormGroup;

  constructor() {
    this.filterForm = this.fb.group({
      startDate: [null],
      endDate: [null]
    }, { validators: this.dateRangeValidator });
  }

  dateRangeValidator(form: FormGroup) {
    const startDate = form.get('startDate')?.value;
    const endDate = form.get('endDate')?.value;

    if (startDate && endDate && startDate > endDate) {
      return { dateRange: true };
    }

    return null;
  }

  setLastWeek(): void {
    const today = new Date();
    const lastWeek = new Date();
    lastWeek.setDate(today.getDate() - 7);

    this.filterForm.patchValue({
      startDate: lastWeek,
      endDate: today
    });
  }

  setLastMonth(): void {
    const today = new Date();
    const lastMonth = new Date();
    lastMonth.setMonth(today.getMonth() - 1);

    this.filterForm.patchValue({
      startDate: lastMonth,
      endDate: today
    });
  }

  setLastThreeMonths(): void {
    const today = new Date();
    const threeMonthsAgo = new Date();
    threeMonthsAgo.setMonth(today.getMonth() - 3);

    this.filterForm.patchValue({
      startDate: threeMonthsAgo,
      endDate: today
    });
  }

  clearDates(): void {
    this.filterForm.patchValue({
      startDate: null,
      endDate: null
    });
  }

  applyFilter(): void {
    if (this.filterForm.valid) {
      this.dialogRef.close({
        startDate: this.filterForm.value.startDate,
        endDate: this.filterForm.value.endDate
      });
    }
  }
}
