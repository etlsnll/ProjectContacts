import { Component, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { ProjectService } from '../shared/project.service';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Project } from '../../project'; // Import the model

@Component({
    selector: 'app-projects',
    templateUrl: './projects.component.html',
    styleUrls: ['./projects.component.css'],
    providers: [ProjectService]
})
/** projects component*/
export class ProjectsComponent implements AfterViewInit, OnDestroy {

    model = new Project(0, "", new Date());

    public projects: Project[] = [];
    public totalProjects: number = 0;
    public currentPage: number = 1; // Current page
    public pageSize: number = 10;
    public pageSizes: number[] = [10, 20, 30, 40, 50];
    public totalPages: number = 0;
    private projectService: ProjectService;

    private timer: Observable<any> = new Observable();
    private subscription: Subscription = new Subscription();
    public showConfirm: boolean = false;
    private confirmTimeMs: number = 3000;

    /** projects ctor */
    constructor(projectService: ProjectService) {
        this.projectService = projectService;
    }

    ngAfterViewInit() {
        this.getProjects();
    }

    public newPage(page: number) {
        this.currentPage = page;
        this.getProjects();
    }

    public changePageSize(size: number) {
        this.currentPage = 1; // reset to first page
        this.pageSize = size;
        this.getProjects();
    }

    private getProjects() {
        // Get count:
        this.projectService.countProjects()
            .subscribe(data => {
                this.totalProjects = data;
                this.totalPages = Math.floor(this.totalProjects / this.pageSize) + (this.totalProjects % this.pageSize !== 0 ? 1 : 0);
            });

        //Get current page of results:
        this.projectService.getProjects(this.currentPage, this.pageSize)
            .subscribe(data => this.projects = data);
    }

    public delete(project: Project): void {
        this.projectService.deleteProject(project.projectId)
            .subscribe(success => {
                if (success) {
                    // Move to previous page (if possible) if we just deleted only item on this page:
                    if (this.projects.length === 1 && this.currentPage > 1)
                        this.currentPage--;
                    // Refresh current page:
                    this.getProjects();
                }
            });
    }

    // Method to handle the add project form submission
    onSubmit() {
        this.projectService.addProject(this.model)
            .subscribe(newId => {
                if (newId > 0) { // If successful, refresh current page:
                    this.getProjects();
                    // Display confirmation message:
                    this.setConfirmTimer();
                }
            });
    }

    public setConfirmTimer() {

        this.showConfirm = true;

        this.timer = Observable.timer(this.confirmTimeMs); // [milliseconds]
        this.subscription = this.timer.subscribe(() => {
            // Hide element from view after timeout
            this.showConfirm = false;
        });
    }

    public ngOnDestroy() {
        if (this.subscription && this.subscription instanceof Subscription) {
            this.subscription.unsubscribe();
        }
    }
}