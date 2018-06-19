import { Component, Inject, AfterViewInit } from '@angular/core';
import { ContactService } from '../shared/contact.service';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import { Contact } from '../../contact'; // Import the model

@Component({
    selector: 'app-contacts',
    templateUrl: './contacts.component.html',
    styleUrls: ['./contacts.component.css'],
    providers: [ContactService]
})
/** contacts component*/
export class ContactsComponent implements AfterViewInit {

    model = new Contact(0, "", "", "");

    public contacts: Contact[] = [];
    public totalContacts: number = 0;
    public currentPage: number = 1;
    public pageSize: number = 10;
    public pageSizes: number[] = [10, 20, 30, 40, 50];
    public totalPages: number = 0;
    private contactService: ContactService;

    private timer: Observable<any> = new Observable();
    private subscription: Subscription = new Subscription();
    public showConfirm: boolean = false;
    private confirmTimeMs: number = 3000;

    /** contacts ctor */
    constructor(contactService: ContactService) {
        this.contactService = contactService;
    }

    ngAfterViewInit() {
        this.getContacts();
    }

    public newPage(page: number) {
        this.currentPage = page;
        this.getContacts();
    }

    public changePageSize(size: number) {
        this.currentPage = 1; // reset to first page
        this.pageSize = size;
        this.getContacts();
    }

    private getContacts() {
        // Get count:
        this.contactService.countContacts()
            .subscribe(data => {
                this.totalContacts = data;
                this.totalPages = Math.floor(this.totalContacts / this.pageSize) + (this.totalContacts % this.pageSize !== 0 ? 1 : 0);
            });

        //Get current page of results:
        this.contactService.getContacts(this.currentPage, this.pageSize)
            .subscribe(data => this.contacts = data);
    }

    public delete(contact: Contact): void {
        this.contactService.deleteContact(contact.contactId)
            .subscribe(data => {
                if (data) {
                    // Move to previous page (if possible) if we just deleted only item on this page:
                    if (this.contacts.length === 1 && this.currentPage > 1)
                        this.currentPage--;
                    // Refresh current page:
                    this.getContacts();
                }
            });
    }

    // Method to handle the add contact form submission
    onSubmit() {
        this.contactService.addContact(this.model)
            .subscribe(newId => {
                if (newId > 0) { // If successful, refresh current page:
                    this.getContacts();
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
}