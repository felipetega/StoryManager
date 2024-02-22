import { StoryService } from './../../Service/StoryService/story.service';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {MatButtonModule} from '@angular/material/button';

@Component({
  selector: 'app-delete-story',
  standalone: true,
  imports: [
    FormsModule,
    HttpClientModule,
    CommonModule,
    MatButtonModule
  ],
  templateUrl: './delete-story.component.html',
  styleUrl: './delete-story.component.css'
})
export class DeleteStoryComponent {
  storyId: number = 0;
  successMessage: string = '';

  constructor(private httpClient: HttpClient, private StoryService: StoryService) {}

  deleteStory() {
    console.log(this.storyId)
    if (this.storyId < 0) {
      console.error("Invalid input. StoryId is required.");
      return;
    }


    this.StoryService.delete(this.storyId).subscribe(
      (response: any) => {
        if (response == null) {
          console.log('Story deleted successfully:', response);
          this.successMessage = 'Story deleted successfully!';
        }
      },
      (error: any) => {
        console.error('Error deleting story:', error);

        if (error instanceof HttpErrorResponse) {
          if (error.status === 404) {
            alert(`Error ${error.status}: Not Found - Story ID doesn't exist`);
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
