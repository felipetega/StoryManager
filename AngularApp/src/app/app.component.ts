import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { CreateStoryComponent } from './components/create-story/create-story.component';
import { VoteComponent } from './components/vote/vote.component';
import { UpdateStoryComponent } from './components/update-story/update-story.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    RouterLink,
    RouterLinkActive,
    NavbarComponent,
    CreateStoryComponent,
    VoteComponent,
    UpdateStoryComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AngularApp';
}
