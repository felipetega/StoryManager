import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VoteService {

  private apiUrl = 'https://localhost:7147/stories';

  constructor(private httpClient: HttpClient) { }

  post(storyId: number, votePayload: any): Observable<any> {
    const voteUrl = `${this.apiUrl}/${storyId}/votes`;
    return this.httpClient.post(voteUrl, votePayload);
  }
}
