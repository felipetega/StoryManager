import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoteComponent } from './vote.component';

describe('VoteComponent', () => {
  let component: VoteComponent;
  let fixture: ComponentFixture<VoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VoteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize userId', () => {
    expect(component.userId).toEqual(0);
  });

  it('should calculate vote balance correctly', () => {
    const votes = [
      { id: 1, voteValue: true },
      { id: 2, voteValue: false },
      { id: 3, voteValue: true }
    ];

    const result = component.calculateVoteBalance(votes);
    expect(result).toEqual(1);
  });

  it('should track items by their id', () => {
    const result = component.trackById(2, { id: 2, voteValue: true });
    expect(result).toEqual(2);
  });

});
