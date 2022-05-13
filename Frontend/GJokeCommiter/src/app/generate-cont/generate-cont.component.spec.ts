import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateContComponent } from './generate-cont.component';

describe('GenerateContComponent', () => {
  let component: GenerateContComponent;
  let fixture: ComponentFixture<GenerateContComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenerateContComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateContComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
