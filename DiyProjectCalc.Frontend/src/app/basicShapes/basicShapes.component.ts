import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { BasicShape } from '../basicShape';
import { BasicShapeService } from '../basicShape.service';
import { ProjectWithBasicShapes } from '../project';

@Component({
  selector: 'app-basicShapes',
  templateUrl: './basicShapes.component.html',
  styleUrls: ['./basicShapes.component.css']
})
export class BasicShapesComponent implements OnInit {

  basicShapes: BasicShape[] = [];
  projectWithBasicShapes!: ProjectWithBasicShapes;

  constructor(
    private basicShapeService: BasicShapeService,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.getBasicShapes();
  }

  getBasicShapes(): void {
    const projectId = Number(this.route.snapshot.paramMap.get('projectId'));
    this.basicShapeService.getBasicShapes(projectId)
      .subscribe(projectWithBasicShapes => this.projectWithBasicShapes = projectWithBasicShapes);
  }

  add(name: string, _number1: string, _number2: string, _shapeType: string, projectId: number): void {
    var number1: number = parseFloat(_number1);
    var number2: number = parseFloat(_number2);
    var shapeType: number = parseFloat(_shapeType);
    name = name.trim();
    if (!name) { return; }
    this.basicShapeService.addBasicShape({ name, number1, number2, shapeType, projectId } as BasicShape)
      .subscribe(basicShape => {
        this.projectWithBasicShapes?.basicShapes.push(basicShape);
      });
  }

  delete(basicShape: BasicShape): void {
    if (this.projectWithBasicShapes) {
      this.projectWithBasicShapes.basicShapes = this.projectWithBasicShapes.basicShapes.filter(h => h !== basicShape);
      this.basicShapeService.deleteBasicShape(basicShape.basicShapeId).subscribe();
    }
  }

}
