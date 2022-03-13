import { TestBed } from '@angular/core/testing';

import { BasicShapeService } from './basicShape.service';

describe('BasicShapeService', () => {
  let service: BasicShapeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BasicShapeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
