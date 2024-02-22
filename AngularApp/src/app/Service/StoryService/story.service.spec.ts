import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { StoryService } from './story.service';
import { StoryView } from '../../ViewModels/StoryView';

describe('StoryService', () => {
  let service: StoryService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [StoryService]
    });

    service = TestBed.inject(StoryService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should send a GET request to the correct endpoint', () => {
    service.getAll().subscribe();

    const req = httpTestingController.expectOne('https://localhost:7147/stories');
    expect(req.request.method).toEqual('GET');

    req.flush({});
  });

  it('should send a POST request to the correct endpoint with the provided payload', () => {
    const newStory = { title: 'Test Story', description: 'Test Description', department: 'Test Department' };

    service.create(newStory).subscribe();

    const req = httpTestingController.expectOne('https://localhost:7147/stories');
    expect(req.request.method).toEqual('POST');
    expect(req.request.body).toEqual(newStory);

    req.flush({});
  });

  it('should send a PUT request to the correct endpoint with the provided storyId and updatedStory', () => {
    const storyId = 1;
    const updatedStory: StoryView = { title: 'Updated Story', description: 'Updated Description', department: 'Updated Department' };

    service.update(storyId, updatedStory).subscribe();

    const req = httpTestingController.expectOne(`https://localhost:7147/stories/${storyId}`);
    expect(req.request.method).toEqual('PUT');
    expect(req.request.body).toEqual(updatedStory);

    req.flush({});
  });

  it('should send a DELETE request to the correct endpoint with the provided storyId', () => {
    const storyId = 1;

    service.delete(storyId).subscribe();

    const req = httpTestingController.expectOne(`https://localhost:7147/stories/${storyId}`);
    expect(req.request.method).toEqual('DELETE');

    req.flush({});
  });

});
