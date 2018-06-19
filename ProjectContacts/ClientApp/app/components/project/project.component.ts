import { Component, Inject, AfterViewInit } from '@angular/core';
import { ProjectService } from '../shared/project.service';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute, Params } from '@angular/router';
import { ProjectDetails } from '../../project'; // Import the model
import { Contact } from '../../contact';

@Component({
    selector: 'app-project',
    templateUrl: './project.component.html',
    styleUrls: ['./project.component.css'],
    providers: [ProjectService]
})
/** project component*/
export class ProjectComponent implements AfterViewInit {

    model = new ProjectDetails(0, "", new Date(), new Array<Contact>());

    private projectService: ProjectService;

    private timer: Observable<any> = new Observable();
    private subscription: Subscription = new Subscription();
    public showConfirm: boolean = false;
    private confirmTimeMs: number = 3000;

    /** project ctor */
    constructor(projectService: ProjectService, private route: ActivatedRoute) {
        this.projectService = projectService;
    }

    ngAfterViewInit() {
        // Get ID from param in URL and load page data:
        this.route.params
            .subscribe((params: Params) => {
                var id = params['id'];
                this.projectService.getProject(id)
                    .subscribe(project => {
                        if (project !== null)
                            this.model = project;
                    });
            })
    }

    // Method to update project name:
    onSubmit(): void {
        this.projectService.updateProject(this.model)
            .subscribe(data => {
                if (data !== null) {
                    this.model = data;
                    this.setConfirmTimer();
                }
            });
    }

    remove(c: Contact) {
        this.projectService.projectDeleteParticipant(this.model.projectId, c)
            .subscribe(data => {
                if (data !== null)
                    this.model.participants = data;
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
}