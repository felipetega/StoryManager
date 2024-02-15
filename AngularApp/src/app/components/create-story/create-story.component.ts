import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
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
      (response) => {
        console.log('Story created successfully:', response);
        this.successMessage = 'Story created successfully!';
      },
      (error) => {
        console.error('Error creating story:', error);
        this.successMessage = '';
      }
    );
  }

  closeSuccessMessage() {
    this.successMessage = '';
  }
}
