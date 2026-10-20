import { Component, inject } from '@angular/core';
import { MatCardModule } from "@angular/material/card";
import { DatetimeEditor } from "../datetime-editor/datetime.editor";
import { PunctuationEditor } from '../punctuation-editor/punctuation-editor';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    MatCardModule,
    DatetimeEditor,
    PunctuationEditor
],
  templateUrl: './settings.html',
  styleUrls: ['./settings.css']
})
export class Settings {
  service = inject(AuthService);
  
  isParkAdmin(): boolean {
    return this.service.GetUserRoles()?.includes('ParkAdmin') || false;
  }
}
