import { 
  Component,
  inject,
  signal } from '@angular/core';
import {
  MatDialogTitle,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef
} from "@angular/material/dialog";
import { MatRadioModule } from "@angular/material/radio";
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from "@angular/forms";
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { AttractionsService } from '../../Services/attractions.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { PunctuationService } from '../../Services/punctuation.service';
import { TicketService } from '../../Services/ticket.service';
import { AttractionAccessRequest } from '../../Models/attraction-access.response';
import { ProcessAttractionAccessRequest } from '../../Models/process-attraction-access.request';

@Component({
  selector: 'app-attraction-access',
  standalone: true,
  imports: [
    MatDialogTitle,
    MatRadioModule,
    MatDialogActions,
    MatDialogClose,
    ReactiveFormsModule,
    CommonModule,
    MatButtonModule,
    MatDialogContent,
    MatInputModule,
    MatProgressSpinner
],
  templateUrl: './attraction.access.html',
  styleUrl: './attraction.access.css'
})
export class AttractionAccess {
  private attractionService = inject(AttractionsService);
  private ticketService = inject(TicketService);
  
  data = inject(MAT_DIALOG_DATA);
  fb = inject(FormBuilder);
  dialogRef = inject(MatDialogRef<AttractionAccess>);
  punctuationService = inject(PunctuationService);

  loading = signal(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);
  form = this.fb.group({
    typeCode: ['NFC', Validators.required],
    code: ['', Validators.required],
  });
  
  Submit() {
    const code = this.form.value.code;
    if (!code) {
      this.error.set('Código requerido');
      return;
    }

    if(this.form.value.typeCode === 'NFC') {
      const payload: AttractionAccessRequest = { VisitorNFC: code };
      console.log('ValidateAttractionAccess NFC payload', payload);
      this.attractionService.ValidateAttractionAccess(this.data.id, payload).subscribe({
        next: (_) => {
          this.SetPointsToVisitor();
        },
        error: (_) => {
          this.error.set('Error al validar acceso');
          this.loading.set(false);
        }
      });
    }
    else {
      const payload: AttractionAccessRequest = { TicketQR: code };
      console.log('ValidateAttractionAccess QR payload', payload);
      this.attractionService.ValidateAttractionAccess(this.data.id, payload).subscribe({
        next: (_) => {
          this.ticketService.GetTicketByQR(code).subscribe({
            next: (ticket) => {
              this.SetPointsToVisitor(ticket.visitorParkInformationId);
            },
            error: (_) => {
              this.error.set('Error al obtener ticket');
              this.loading.set(false);
            }
          });
        },
        error: (_) => {
          this.error.set('Error al validar acceso');
        }
      });
    }
  }

  SetPointsToVisitor(visitorId?: number) {
    const code = this.form.value.code!;
    const isQR = this.form.value.typeCode === 'QR';
    const processRequest: ProcessAttractionAccessRequest = {
      AttractionId: this.data.id,
      VisitorId: isQR ? visitorId : undefined,
      VisitorNFC: !isQR ? code : undefined
    };
    console.log('ProcessAttractionEntrance payload', processRequest);

    this.punctuationService.ProcessAttractionEntrance(processRequest).subscribe({
      next: (_) => {
        this.success.set('Acceso validado con éxito');
        this.loading.set(false);
        setTimeout(() => {
          this.dialogRef.close();
        }, 1200);
      },
      error: (_) => {
        this.error.set('No se pudo procesar el acceso a la atraccion');
        this.loading.set(false);
      }
    });
  }
}
