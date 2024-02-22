import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { VoteService } from './vote.service';

describe('VoteService', () => {
  let service: VoteService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [VoteService]
    });

    service = TestBed.inject(VoteService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should send a POST request to the correct endpoint with the provided payload', () => {
    const votePayload = { userId: 1, storyId: 2, voteValue: true };

    service.post(votePayload).subscribe();

    const req = httpTestingController.expectOne('https://localhost:7147/votes');
    expect(req.request.method).toEqual('POST');
    expect(req.request.body).toEqual(votePayload);

    req.flush({});
  });

});
