import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';
import {MatDividerModule} from '@angular/material/divider';
import {MatButtonModule} from '@angular/material/button';
import { StoryService } from '../../Service/StoryService/story.service';
import { VoteService } from '../../Service/VoteService/vote.service';


@Component({
  selector: 'app-vote',
  standalone: true,
  imports: [
    HttpClientModule,
    FormsModule,
    CommonModule,
    MatIconModule,
    MatDividerModule,
    MatButtonModule
  ],
  templateUrl: './vote.component.html',
  styleUrl: './vote.component.css'
})
export class VoteComponent implements OnInit {

  constructor(private StoryService: StoryService, private VoteService: VoteService) {}

  data: any = [];
  userId: number = 0;

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.StoryService.getAll().subscribe((data: any) => {
      this.data = data;
    });
  }

  vote(storyId: number, voteValue: boolean) {
    if (this.userId <= 0) {
      console.error("UserId is required.");
      alert("UserId is required.");
      return;
    }

    const votePayload = {
      userId: this.userId,
      storyId: storyId,
      voteValue: voteValue
    };

    this.VoteService.post(votePayload).subscribe(
      (response: any) => {
        if (response == null) {
          this.fetchData();
        }
      },
      (error: any) => {
        console.error(error);

        if (error instanceof HttpErrorResponse) {
          if (error.status === 404) {
            alert(`Error ${error.status}: Not Found - User ID don't exist.`);
          } else if (error.status === 400) {
            alert(`Error ${error.status}: Bad Request - Invalid input.`);
          } else {
            alert(`An error occurred: ${error.status}`);
          }
        }
      }
    );
  }

  calculateVoteBalance(votes: any[]): number {
    return votes.reduce((acc, vote) => acc + (vote.voteValue ? 1 : -1), 0);
  }

  trackById(index: number, item: any): number {
    return item.id;
  }
}
