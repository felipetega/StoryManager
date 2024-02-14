import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';

@Component({
  selector: 'app-vote',
  standalone: true,
  imports: [
    HttpClientModule
  ],
  templateUrl: './vote.component.html',
  styleUrl: './vote.component.css'
})
export class VoteComponent implements OnInit {

  constructor(private httpClient: HttpClient) {}  // Correção: Adicione 'private' antes de 'httpClient'

  data: any = [];

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData() {
    this.httpClient.get("https://localhost:7147/stories").subscribe((data: any) => {
      console.log(data);
      this.data = data;
    });
  }
}
