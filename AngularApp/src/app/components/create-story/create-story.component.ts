import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-story',
  standalone: true,
  imports: [
    FormsModule,
    HttpClientModule,
    CommonModule
  ],
  templateUrl: './create-story.component.html',
  styleUrl: './create-story.component.css'
})
export class CreateStoryComponent {
  newStory = { title: '', description: '', department: '' };
  successMessage: string = '';

  constructor(private httpClient: HttpClient) {}

  createStory() {
    this.httpClient.post('https://localhost:7147/stories', this.newStory).subscribe(
      (response: any) => {
        if (response==null) {
          console.log('Story created successfully:', response);
          this.successMessage = 'Story created successfully!';
        }
      },
      (error: any) => {
        console.error('Error creating story:', error);
        
        if (error instanceof HttpErrorResponse) {
          if (error.status === 400) {
            alert(`Error ${error.status}: Bad Request - Invalid input`);
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
