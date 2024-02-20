import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteStoryComponent } from './delete-story.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

describe('DeleteStoryComponent', () => {
  let component: DeleteStoryComponent;
  let fixture: ComponentFixture<DeleteStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        DeleteStoryComponent,
        FormsModule,
        HttpClientModule
      ]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeleteStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize storyId and successMessage', () => {
    expect(component.storyId).toEqual(0);
    expect(component.successMessage).toEqual('');
  });

  it('should close successMessage on closeSuccessMessage call', () => {
    component.successMessage = 'Story updated successfully!';
    component.closeSuccessMessage();
    expect(component.successMessage).toEqual('');
  });
});

