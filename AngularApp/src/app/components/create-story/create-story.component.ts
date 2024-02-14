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
  newCity = { cityName: '', stateName: '' };
  successMessage: string = '';

  constructor(private httpClient: HttpClient) {}

  createCity() {
    this.httpClient.post('/api', this.newCity).subscribe(
      (response) => {
        console.log('City created successfully:', response);
        this.successMessage = 'City created successfully!';
      },
      (error) => {
        console.error('Error creating city:', error);
        this.successMessage = '';
      }
    );
  }

  closeSuccessMessage() {
    this.successMessage = '';
  }
}
