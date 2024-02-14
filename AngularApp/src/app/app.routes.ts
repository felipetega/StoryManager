import { Routes } from '@angular/router';
import { CreateStoryComponent } from './components/create-story/create-story.component';
import { VoteComponent } from './components/vote/vote.component';

export const routes: Routes = [
        { path: '', component: VoteComponent },
        { path: 'create-story', component: CreateStoryComponent }
];
