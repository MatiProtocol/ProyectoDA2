import { Component, inject, OnInit, signal } from '@angular/core';
import { SideNavBar } from "../side-nav-bar/side-nav-bar";
import { MatTableDataSource } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { PunctuationService } from '../../Services/punctuation.service';
import { DailyRankingResponse, DailyRankingEntry } from '../../Models/daily-ranking.response';

@Component({
  selector: 'app-daily-ranking',
  standalone: true,
  imports: [
    SideNavBar,
    CommonModule,
    MatTableModule,
    MatProgressSpinner,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './daily-ranking.html',
  styleUrl: './daily-ranking.css'
})
export class DailyRanking implements OnInit {
  private service = inject(PunctuationService);
  public visitors: DailyRankingResponse | null = null;

  ngOnInit(): void {
    this.LoadData();
  }

  dataSource = new MatTableDataSource<DailyRankingEntry>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  displayedColumns: string[] = [
    'name',
    'todayPoints'
  ];
  
  LoadData() {
    this.loading.set(true);
    this.error.set(null);
    
    this.service.GetDailyRanking().subscribe({
      next: (res: DailyRankingResponse) => {
        this.dataSource.data = res.ranking;
        this.visitors = res;
        this.loading.set(false);
      },
      error: (_) => {
        this.loading.set(false);
        this.error.set('Error fetching daily ranking');
      }
    });
  }

}
