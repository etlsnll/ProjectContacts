import { Component, Inject, AfterViewInit } from '@angular/core';
import { ProjectService } from '../shared/project.service';
import { Project } from '../../project'; // Import the model

@Component({
    selector: 'app-projects',
    templateUrl: './projects.component.html',
    styleUrls: ['./projects.component.css'],
    providers: [ProjectService]
})
/** projects component*/
export class ProjectsComponent implements AfterViewInit {

    public projects: Project[] = [];
    public totalProjects: number = 0;
    public p: number = 1;
    public pageSize: number = 10;
    public pageSizes: number[] = [10, 20, 30, 40, 50];
    public totalPages: number = 0;
    private projectService: ProjectService;
    private deleteIndex: number = 0;

    /** projects ctor */
    constructor(projectService: ProjectService) {
        this.projectService = projectService;
    }

    ngAfterViewInit() {
        this.getProjects();
    }

    public newPage(page: number) {
        this.p = page;
        this.getProjects();
    }

    public changePageSize(size: number) {
        this.p = 1; // reset to first page
        this.pageSize = size;
        this.getProjects();
    }

    private getProjects() {
        // Get count:
        this.projectService.countProjects()
            .subscribe(data => {
                this.totalProjects = data;
                this.totalPages = Math.floor(this.totalProjects / this.pageSize) + 1;
            });

        //Get current page of results:
        this.projectService.getProjects(this.p, this.pageSize)
            .subscribe(data => this.projects = data);
    }

    public delete(project: Project): void {
        this.projectService.deleteProject(project.projectId)
            .subscribe(data => {
                if (data) // Refresh current page:
                    this.getProjects();
            });
    }
}