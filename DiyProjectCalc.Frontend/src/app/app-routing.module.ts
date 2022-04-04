import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProjectsComponent } from './projects/projects.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';
import { BasicShapesComponent } from './basicShapes/basicShapes.component';
import { BasicShapeDetailComponent } from './basicShape-detail/basicShape-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/projects', pathMatch: 'full' },
  { path: 'projects', component: ProjectsComponent },
  { path: 'projects/:id', component: ProjectDetailComponent },
  { path: 'projects/:projectId/basicShapes', component: BasicShapesComponent },
  { path: 'projects/:projectId/basicShapes/:id', component: BasicShapeDetailComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
