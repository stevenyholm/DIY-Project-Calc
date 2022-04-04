import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BasicShape } from '../basicShape';
import { BasicShapeService } from '../basicShape.service';

@Component({
  selector: 'app-basicShape-detail',
  templateUrl: './basicShape-detail.component.html',
  styleUrls: ['./basicShape-detail.component.css']
})
export class BasicShapeDetailComponent implements OnInit {

  basicShape: BasicShape | undefined;

  constructor(
    private route: ActivatedRoute,
    private basicShapeService: BasicShapeService,
    private location: Location
) { }

  ngOnInit(): void {
    this.getBasicShape();
  }

  getBasicShape(): void {
    const projectId = Number(this.route.snapshot.paramMap.get('projectId'));
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.basicShapeService.getBasicShape(projectId, id)
      .subscribe(basicShape => this.basicShape = basicShape);
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    const projectId = Number(this.route.snapshot.paramMap.get('projectId'));
    if (this.basicShape) {
      this.basicShapeService.updateBasicShape(projectId, this.basicShape)
        .subscribe(() => this.goBack());
    }
  }

}
