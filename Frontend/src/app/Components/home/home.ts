import { Component } from '@angular/core';
import { SideNavBar } from '../side-nav-bar/side-nav-bar';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SideNavBar],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {

}
