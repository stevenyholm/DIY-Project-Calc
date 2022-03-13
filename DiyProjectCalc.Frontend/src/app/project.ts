import { BasicShape } from "./basicShape";

export interface Project {
  projectId: number;
  name: string;
}

export interface ProjectWithBasicShapes {
  projectId: number;
  name: string;
  basicShapes: BasicShape[];
}
