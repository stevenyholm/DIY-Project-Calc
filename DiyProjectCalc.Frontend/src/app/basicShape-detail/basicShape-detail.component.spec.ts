import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasicShapeDetailComponent } from './basicShape-detail.component';

describe('BasicShapeDetailComponent', () => {
  let component: BasicShapeDetailComponent;
  let fixture: ComponentFixture<BasicShapeDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BasicShapeDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BasicShapeDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
