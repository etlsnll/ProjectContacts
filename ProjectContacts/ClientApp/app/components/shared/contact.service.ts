import { Injectable, Inject } from '@angular/core';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Observable } from "rxjs";
import { Contact } from '../../contact'; // Import the model

const httpOptions = {
    headers: new Headers({
        'Content-Type': 'application/json'
    })
};

@Injectable()
export class ContactService {

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

    countContacts() {
        return this.http.get(this.url + 'api/Contact/Count')
            .catch(this.handleErrorObservable)
            .map(response => response.json() as number);
    }

    getContacts(pageNum: number, pageSize: number) {
        var search = new URLSearchParams();
        search.set('pageNum', pageNum.toString()); // Add URL query param
        search.set('pageSize', pageSize.toString()); // Add URL query param
        return this.http.get(this.url + 'api/Contact/List', { search: search })
            .catch(this.handleErrorObservable)
            .map(response => response.json() as Contact[]);
    }

    deleteContact(contactId: number) {
        return this.http.delete(this.url + 'api/Contact/Delete/' + contactId.toString())
            .catch(this.handleErrorObservable)
            .map(response => response.json() as boolean);
    }

    addContact(contact: Contact) {
        return this.http.post(this.url + 'api/Contact/Add', contact)
            .catch(this.handleErrorObservable)
            .map(response => response.json() as number);
    }
}
