import { Component } from '@angular/core';
import { MatDialogModule } from "@angular/material/dialog";
import { inject } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  selector: 'app-confirmation-dialog',
  imports: [
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './confirmation.dialog.html',
  styleUrl: './confirmation.dialog.css'
})
export class ConfirmationDialog {
  private data = inject(MAT_DIALOG_DATA) as { title: string; message: string };
  private dialogRef = inject(MatDialogRef<ConfirmationDialog>);

  title = this.data.title;
  message = this.data.message;

  onConfirm(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

}
