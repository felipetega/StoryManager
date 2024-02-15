// import necessary modules and components
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

// Define an interface for the updated story
interface UpdatedStory {
  storyId: number;
  title: string;
  description: string;
  department: string;
}

@Component({
  selector: 'app-update-story',
  standalone: true,
  imports: [
    FormsModule,
    HttpClientModule,
    CommonModule
  ],
  templateUrl: './update-story.component.html',
  styleUrls: ['./update-story.component.css']
})
export class UpdateStoryComponent {
  storyId: number = 0;
  updatedStory: UpdatedStory = { storyId: 0, title: '', description: '', department: '' };
  successMessage: string = '';

  constructor(private httpClient: HttpClient) {}

  updateStory() {
    if (this.storyId <= 0) {
      console.error("Invalid input. StoryId is required.");
      return;
    }

    this.httpClient.put<UpdatedStory>(`https://localhost:7147/stories/${this.storyId}`, this.updatedStory)
      .subscribe(
        (response) => {
          console.log('Story updated successfully:', response);
          this.successMessage = 'Story updated successfully!';
        },
        (error) => {
          console.error('Error updating story:', error);
          this.successMessage = '';
        }
      );
  }

  closeSuccessMessage() {
    this.successMessage = '';
  }
}
