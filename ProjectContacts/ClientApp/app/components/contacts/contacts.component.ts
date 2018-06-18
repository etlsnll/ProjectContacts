import { Component, Inject, AfterViewInit } from '@angular/core';
import { ContactService } from '../shared/contact.service';
import { Contact } from '../../contact'; // Import the model

@Component({
    selector: 'app-contacts',
    templateUrl: './contacts.component.html',
    styleUrls: ['./contacts.component.css'],
    providers: [ContactService]
})
/** contacts component*/
export class ContactsComponent implements AfterViewInit {

    public contacts: Contact[] = [];
    public totalContacts: number = 0;
    public p: number = 1;
    public pageSize: number = 10;
    public pageSizes: number[] = [10, 20, 30, 40, 50];
    public totalPages: number = 0;
    private contactService: ContactService;
    private deleteIndex: number = 0;

    /** contacts ctor */
    constructor(contactService: ContactService) {
        this.contactService = contactService;
    }

    ngAfterViewInit() {
        this.getContacts();
    }

    public newPage(page: number) {
        this.p = page;
        this.getContacts();
    }

    public changePageSize(size: number) {
        this.p = 1; // reset to first page
        this.pageSize = size;
        this.getContacts();
    }

    private getContacts() {
        // Get count:
        this.contactService.countContacts()
            .subscribe(data => {
                this.totalContacts = data;
                this.totalPages = Math.floor(this.totalContacts / this.pageSize) + 1;
            });

        //Get current page of results:
        this.contactService.getContacts(this.p, this.pageSize)
            .subscribe(data => this.contacts = data);
    }

    public delete(contact: Contact): void {
        this.contactService.deleteContact(contact.contactId)
            .subscribe(data => {
                if (data) // Refresh current page:
                    this.getContacts();
            });
    }
}