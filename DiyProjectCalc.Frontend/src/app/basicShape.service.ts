import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { BasicShape } from './basicShape';
import { ProjectWithBasicShapes } from './project';

@Injectable({ providedIn: 'root' })
export class BasicShapeService {

  private baseUrl = 'https://localhost:7163/api';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient
  ) { }

  getBasicShapes(projectId: number): Observable<ProjectWithBasicShapes> {
    const url = `${this.baseUrl}/projects/${projectId}/basicShapes`;
    return this.http.get<ProjectWithBasicShapes>(url)
      .pipe(
        catchError(this.handleError<ProjectWithBasicShapes>('getBasicShapes'))
      );
  }

  getBasicShape(projectId: number, id: number): Observable<BasicShape> {
    const url = `${this.baseUrl}/projects/${projectId}/basicshapes/${id}`;
    return this.http.get<BasicShape>(url).pipe(
      catchError(this.handleError<BasicShape>(`getBasicShape id=${id}`))
    );
  }

  updateBasicShape(projectId: number, basicShape: BasicShape): Observable<any> {
    const url = `${this.baseUrl}/projects/${projectId}/basicshapes/${basicShape.basicShapeId}`;

    return this.http.put(url, basicShape, this.httpOptions).pipe(
      catchError(this.handleError<any>('updateBasicShape'))
    );
  }

  addBasicShape(projectId: number, basicShape: BasicShape): Observable<BasicShape> {
    const url = `${this.baseUrl}/projects/${projectId}/basicshapes`;
    return this.http.post<BasicShape>(url, basicShape, this.httpOptions).pipe(
      catchError(this.handleError<BasicShape>('addBasicShape'))
    );
  }

  deleteBasicShape(projectId: number, id: number): Observable<BasicShape> {
    const url = `${this.baseUrl}/projects/${projectId}/basicshapes/${id}`;

    return this.http.delete<BasicShape>(url, this.httpOptions).pipe(
      catchError(this.handleError<BasicShape>('deleteBasicShape'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
