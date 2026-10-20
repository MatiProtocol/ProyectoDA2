import { Component, inject, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AttractionCreate } from '../attraction-create/attraction.create';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SideNavBar } from "../side-nav-bar/side-nav-bar";
import { AttractionListMenu } from '../attraction-list-menu/attraction-list.menu';

@Component({
  selector: 'app-attraction-menu',
  standalone: true,
  imports: [
    MatButtonModule,
    MatIconModule,
    SideNavBar,
    AttractionListMenu
  ],
  templateUrl: './attraction.menu.html',
  styleUrl: './attraction.menu.css'
})
export class AttractionMenu {
  dialog = inject(MatDialog);
  @ViewChild(AttractionListMenu) attractionListMenu!: AttractionListMenu;

  openUserDialog() {
    this.dialog.open(AttractionCreate, {
      width: '400px',
      disableClose: true,
    }).afterClosed().subscribe(() => {
      this.attractionListMenu.LoadData();
    });
  }
}
