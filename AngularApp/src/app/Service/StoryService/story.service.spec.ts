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

  it('should return Bad Request at creating a story', () => {
    const title = 'Test Title';
    const description = 'Test Description';
    const area = 'Test Area';

    service.create({title, description, area}).subscribe(
      () => {
        fail('Expected an error response');
      },
      (error) => {
        expect(error.status).toBe(400);
        expect(error.statusText).toBe('Bad Request');
      }
    );

    const req = httpTestingController.expectOne(`https://localhost:7147/stories`);
    expect(req.request.method).toBe('POST');

    req.flush({ error: 'Bad Request' }, { status: 400, statusText: 'Bad Request' });
  });

  it('should handle 404 error when deleting a story', () => {
    const storyId = 1;

    service.delete(storyId).subscribe(
      () => {
        fail('Expected an error response');
      },
      (error) => {
        expect(error.status).toBe(404);
        expect(error.statusText).toBe('Not Found');
      }
    );

    const req = httpTestingController.expectOne(`https://localhost:7147/stories/${storyId}`);
    expect(req.request.method).toBe('DELETE');

    req.flush('Not Found', { status: 404, statusText: 'Not Found' });
  });

  // ...

it('should return Bad Request when updating a story with invalid input', () => {
  const storyId = 1;
  const updatedStory: StoryView = { title: '', description: '', department: '' };

  service.update(storyId, updatedStory).subscribe(
    () => {
      fail('Expected an error response');
    },
    (error) => {
      expect(error.status).toBe(400);
      expect(error.statusText).toBe('Bad Request');
    }
  );

  const req = httpTestingController.expectOne(`https://localhost:7147/stories/${storyId}`);
  expect(req.request.method).toBe('PUT');

  req.flush({ error: 'Bad Request' }, { status: 400, statusText: 'Bad Request' });
});

it('should handle 404 error when updating a non-existing story', () => {
  const storyId = 1;
  const updatedStory: StoryView = { title: 'Updated Title', description: 'Updated Description', department: 'Updated Department' };

  service.update(storyId, updatedStory).subscribe(
    () => {
      fail('Expected an error response');
    },
    (error) => {
      expect(error.status).toBe(404);
      expect(error.statusText).toBe('Not Found');
    }
  );

  const req = httpTestingController.expectOne(`https://localhost:7147/stories/${storyId}`);
  expect(req.request.method).toBe('PUT');

  req.flush('Not Found', { status: 404, statusText: 'Not Found' });
});


});
