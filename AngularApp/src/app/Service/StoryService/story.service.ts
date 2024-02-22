import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StoryView } from '../../ViewModels/StoryView';

@Injectable({
  providedIn: 'root'
})
export class StoryService {

  private apiUrl = 'https://localhost:7147/stories';

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<any> {
    return this.httpClient.get(this.apiUrl);
  }

  create(newStory: any): Observable<any> {
    return this.httpClient.post(this.apiUrl, newStory);
  }

  update(storyId: number, updatedStory: StoryView): Observable<any> {
    const url = `${this.apiUrl}/${storyId}`;
    return this.httpClient.put(url, updatedStory);
  }

  delete(storyId: number): Observable<any> {
    return this.httpClient.delete(`${this.apiUrl}/${storyId}`);
  }

}
