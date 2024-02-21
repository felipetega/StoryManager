import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateStoryComponent } from './create-story.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

describe('CreateStoryComponent', () => {
  let component: CreateStoryComponent;
  let fixture: ComponentFixture<CreateStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        CreateStoryComponent,
        FormsModule,
        HttpClientModule
      ],
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateStoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize newStory and successMessage', () => {
    expect(component.newStory).toEqual({ title: '', description: '', department: '' });
    expect(component.successMessage).toEqual('');
  });

  it('should have input fields and a button in the template', () => {
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('label[for="title"]')).toBeTruthy();
    expect(compiled.querySelector('input[id="title"]')).toBeTruthy();
    expect(compiled.querySelector('label[for="description"]')).toBeTruthy();
    expect(compiled.querySelector('input[id="description"]')).toBeTruthy();
    expect(compiled.querySelector('label[for="department"]')).toBeTruthy();
    expect(compiled.querySelector('input[id="department"]')).toBeTruthy();
    expect(compiled.querySelector('button')).toBeTruthy();
  });

  it('should call createStory method on button click', () => {
    spyOn(component, 'createStory');
    const button = fixture.debugElement.nativeElement.querySelector('button');
    button.click();
    expect(component.createStory).toHaveBeenCalled();
  });
  
});
