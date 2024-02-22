import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VoteService {

  private apiUrl = 'https://localhost:7147/votes';

  constructor(private httpClient: HttpClient) { }

  post(votePayload: any): Observable<any> {
    return this.httpClient.post(this.apiUrl, votePayload);
  }
}
