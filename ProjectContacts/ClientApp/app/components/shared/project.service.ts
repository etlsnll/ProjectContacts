﻿import { Injectable, Inject } from '@angular/core';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Observable } from "rxjs";
import { Project, ProjectDetails } from '../../project'; // Import the model classes
import { Contact } from '../../contact';

const httpOptions = {
    headers: new Headers({
        'Content-Type': 'application/json'
    })
};

@Injectable()
export class ProjectService {

    private url: string;
    private http: Http;
    private totalProjects: number = 0;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;
        this.http = http;
    }

    handleErrorObservable(error: Response | any) {
        console.error(error.message || error);
        return Observable.throw(error.message || error);
    }

    countProjects() {
        return this.http.get(this.url + 'api/Project/Count')
            .catch(this.handleErrorObservable)
            .map(response => response.json() as number);
    }

    getProjects(pageNum: number, pageSize: number) {
        var search = new URLSearchParams();
        search.set('pageNum', pageNum.toString()); // Add URL query param
        search.set('pageSize', pageSize.toString()); // Add URL query param
        return this.http.get(this.url + 'api/Project/List', { search: search })
            .catch(this.handleErrorObservable)
            .map(response => response.json() as Project[]);
    }

    getProject(id: number) {
        return this.http.get(this.url + 'api/Project/' + id.toString())
            .catch(this.handleErrorObservable)
            .map(response => response.json() as ProjectDetails);
    }

    searchContacts(projectId: number, name: string) {
        var search = new URLSearchParams();
        search.set('srchTerm', name); // Add URL query param
        return this.http.get(this.url + 'api/Project/' + projectId.toString() + '/SearchContacts', { search: search })
            .catch(this.handleErrorObservable)
            .map(response => response.json() as Contact[]);
    }

    updateProject(project: ProjectDetails) {
        return this.http.put(this.url + 'api/Project/' + project.projectId.toString(), project)
            .catch(this.handleErrorObservable)
            .map(response => response.json() as ProjectDetails);
    }

    projectAddContact<Contact>(id: number, c: Contact) {
        return this.http.put(this.url + 'api/Project/' + id.toString() + '/AddParticipant', c)
            .catch(this.handleErrorObservable)
            .map(response => response.json() as Contact[]);
    }

    projectDeleteParticipant(projectId: number, c: Contact) {
        return this.http.delete(this.url + 'api/Project/' + projectId.toString() + '/DeleteParticipant/' + c.contactId)
            .catch(this.handleErrorObservable)
            .map(response => response.json() as Contact[]);
    }

    deleteProject(projectId: number) {
        return this.http.delete(this.url + 'api/Project/Delete/' + projectId.toString())
            .catch(this.handleErrorObservable)
            .map(response => response.json() as boolean);
    }

    addProject(project: Project) {
        return this.http.post(this.url + 'api/Project/Add', project)
            .catch(this.handleErrorObservable)
            .map(response => response.json() as number);                  
    }
}
