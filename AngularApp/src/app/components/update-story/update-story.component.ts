import { StoryService } from './../../Service/StoryService/story.service';
import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { StoryView } from '../../ViewModels/StoryView';

@Component({
  selector: 'app-update-story',
  standalone: true,
  imports: [
    FormsModule,
    HttpClientModule,
    CommonModule,
    MatButtonModule
  ],
  templateUrl: './update-story.component.html',
  styleUrls: ['./update-story.component.css']
})
export class UpdateStoryComponent {
  storyId: number = 0;
  updatedStory: StoryView = { title: '', description: '', department: '' };
  successMessage: string = '';

  constructor(private StoryService: StoryService) {}

  updateStory() {
    console.log(this.storyId)
    if (this.storyId <= 0) {
      alert("Invalid input. StoryId is required.");
      return;
    }

    this.StoryService.update(this.storyId, this.updatedStory)
      .subscribe(
        (response: any) => {
          if (response == null) {
            console.log('Story updated successfully:', response);
            this.successMessage = 'Story updated successfully!';
          }
        },
        (error: any) => {
          console.error('Error updating story:', error);

          if (error instanceof HttpErrorResponse) {
            if (error.status === 400) {
              alert(`Error ${error.status}: Bad Request - Invalid input`);
            } else if (error.status === 404) {
              alert(`Error ${error.status}: Not Found - Story ID don't exist`);
            } else {
              alert(`An unexpected error occurred: ${error.status}`);
            }
          }
        }
      );
  }

  closeSuccessMessage() {
    this.successMessage = '';
  }
}
