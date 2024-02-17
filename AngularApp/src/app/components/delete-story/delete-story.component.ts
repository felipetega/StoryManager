import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-delete-story',
  standalone: true,
  imports: [
    FormsModule,
    HttpClientModule,
    CommonModule
  ],
  templateUrl: './delete-story.component.html',
  styleUrl: './delete-story.component.css'
})
export class DeleteStoryComponent {
  storyId: number = 0;
  successMessage: string = '';

  constructor(private httpClient: HttpClient) {}

  deleteStory() {
    console.log(this.storyId)
    if (this.storyId < 0) {
      console.error("Invalid input. StoryId is required.");
      return;
    }


    this.httpClient.delete(`https://localhost:7147/stories/${this.storyId}`)
    .subscribe(
      (response: any) => {
        if (response==null) {
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
