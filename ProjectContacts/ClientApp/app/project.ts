import { Contact } from './contact'; // Import the contact model

export class Project {
    constructor(
        public projectId: number,
        public title: string,
        public created: Date
    ) { }
}

export class ProjectDetails extends Project {
    constructor(
        public projectId: number,
        public title: string,
        public created: Date,
        public participants: Contact[]
    ) { super(projectId, title, created); }
}