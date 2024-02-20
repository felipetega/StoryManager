import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateStoryComponent } from './update-story.component';

describe('UpdateStoryComponent', () => {
  let component: UpdateStoryComponent;
  let fixture: ComponentFixture<UpdateStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateStoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UpdateStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize storyId, updatedStory, and successMessage', () => {
    expect(component.storyId).toEqual(0);
    expect(component.updatedStory).toEqual({ title: '', description: '', department: '' });
    expect(component.successMessage).toEqual('');
  });

  it('should close successMessage on closeSuccessMessage call', () => {
    component.successMessage = 'Story updated successfully!';
    component.closeSuccessMessage();
    expect(component.successMessage).toEqual('');
  });
});