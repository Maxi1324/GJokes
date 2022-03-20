import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmbienceComponent } from './ambience.component';

describe('AmbienceComponent', () => {
  let component: AmbienceComponent;
  let fixture: ComponentFixture<AmbienceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AmbienceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AmbienceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
