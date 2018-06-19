import { Component, Inject, AfterViewInit, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { FormControl } from '@angular/forms';
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
export class ProjectComponent implements AfterViewInit, OnDestroy {

    model = new ProjectDetails(0, "", new Date(), new Array<Contact>());
    contacts: Contact[] = [];
    searchName: FormControl = new FormControl();

    private projectService: ProjectService;

    private timer: Observable<any> = new Observable();
    private subscription: Subscription = new Subscription();
    public showConfirm: boolean = false;
    private confirmTimeMs: number = 3000;

    /** project ctor */
    constructor(projectService: ProjectService, private route: ActivatedRoute) {
        this.projectService = projectService;

        this.searchName.valueChanges
            .debounceTime(400)
            .subscribe(data => this.searchContacts());
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

    searchContacts() {
        var name: string = (this.searchName.value !== null ? this.searchName.value.toString() : "");

        if (name === "") {
            this.clearResults();
        }
        else {
            this.setWaitCursor();
            this.projectService.searchContacts(this.model.projectId, name)
                .subscribe(result => {
                    this.contacts = result;
                    this.resetWaitCursor();
                });
        }
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

    public ngOnDestroy() {
        if (this.subscription && this.subscription instanceof Subscription) {
            this.subscription.unsubscribe();
        }
    }

    remove(c: Contact) {
        this.projectService.projectDeleteParticipant(this.model.projectId, c)
            .subscribe(data => {
                if (data !== null)
                    this.model.participants = data;
            });
    }

    clearResults() {
        this.contacts = new Array<Contact>();
        this.searchName.setValue("");
    }

    add(c: Contact) {
        this.projectService.projectAddContact(this.model.projectId, c)
            .subscribe(data => {
                if (data !== null) {
                    // Update project list:
                    this.model.participants = data;
                    // Remove this item from search results now (can only add to project once):
                    var index = this.contacts.indexOf(c);
                    if (index > -1) {
                        this.contacts.splice(index, 1);
                    }
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

    setWaitCursor() {
        this.setCursorStyle('wait');
    }

    resetWaitCursor() {
        this.setCursorStyle('default');
    }

    setCursorStyle(style: string) {
        document.body.style.cursor = style;
        var t = document.getElementById('name');
        if (t !== null)
            t.style.cursor = style;
    }
}