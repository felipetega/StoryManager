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

  it('should return Bad Request when posting a vote with invalid input', () => {
    const votePayload = { userId: 0, storyId: 1, voteValue: true };

    service.post(votePayload).subscribe(
      () => {
        fail('Expected an error response');
      },
      (error) => {
        expect(error.status).toBe(400);
        expect(error.statusText).toBe('Bad Request');
      }
    );

    const req = httpTestingController.expectOne('https://localhost:7147/votes');
    expect(req.request.method).toBe('POST');

    req.flush({ error: 'Bad Request' }, { status: 400, statusText: 'Bad Request' });
  });

  it('should handle 404 error when posting a vote for a non-existing user', () => {
    const votePayload = { userId: 999, storyId: 1, voteValue: true };

    service.post(votePayload).subscribe(
      () => {
        fail('Expected an error response');
      },
      (error) => {
        expect(error.status).toBe(404);
        expect(error.statusText).toBe('Not Found');
      }
    );

    const req = httpTestingController.expectOne('https://localhost:7147/votes');
    expect(req.request.method).toBe('POST');

    req.flush('Not Found', { status: 404, statusText: 'Not Found' });
  });

});
