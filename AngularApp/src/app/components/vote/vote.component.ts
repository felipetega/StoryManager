import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-vote',
  standalone: true,
  imports: [
    HttpClientModule,
    FormsModule,
    CommonModule
  ],
  templateUrl: './vote.component.html',
  styleUrl: './vote.component.css'
})
export class VoteComponent implements OnInit {

  constructor(private httpClient: HttpClient) {}

  data: any = [];
  userId: number = 0; // Change userId to number and initialize to 0

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.httpClient.get("https://localhost:7147/stories").subscribe((data: any) => {
      console.log(data);
      this.data = data;
    });
  }

  vote(storyId: number, voteValue: boolean) {
    if (this.userId <= 0 || storyId <= 0) { // Check if userId and storyId are valid
      console.error("Invalid input. UserId and StoryId are required.");
      return;
    }

    const votePayload = {
      userId: this.userId,
      storyId: storyId,
      voteValue: voteValue
    };

    this.httpClient.post("https://localhost:7147/votes", votePayload).subscribe((response: any) => {
      console.log(response);
    });
  }

  calculateVoteBalance(votes: any[]): number {
    return votes.reduce((acc, vote) => acc + (vote.voteValue ? 1 : -1), 0);
  }

  trackById(index: number, item: any): number {
    return item.id;
  }
}