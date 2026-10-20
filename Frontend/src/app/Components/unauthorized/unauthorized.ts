import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [CommonModule, RouterLink, MatButtonModule],
  templateUrl: './unauthorized.html',
  styleUrl: './unauthorized.css',
})
export class Unauthorized {
}
