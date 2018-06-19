import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { AllMusicComponent } from './components/all-music/all-music.component';
import { PagerComponent } from './components/pager/pager.component';
import { AddPlaylistComponent } from './components/add-playlist/add-playlist.component';
import { PlaylistsComponent } from './components/playlists/playlists.component';
import { PlaylistComponent } from './components/playlist/playlist.component';
import { ProjectsComponent } from './components/projects/projects.component';
import { ProjectComponent } from './components/project/project.component';
import { ContactsComponent } from './components/contacts/contacts.component';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        PagerComponent,
        HomeComponent,
        AllMusicComponent,
        AddPlaylistComponent,
        PlaylistsComponent,
        PlaylistComponent,
        ProjectsComponent,
        ProjectComponent,
        ContactsComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'all-music', component: AllMusicComponent },
            { path: 'add-playlist', component: AddPlaylistComponent },
            { path: 'playlists', component: PlaylistsComponent },
            { path: 'playlist/:id', component: PlaylistComponent },
            { path: 'projects', component: ProjectsComponent },
            { path: 'project/:id', component: ProjectComponent },
            { path: 'contacts', component: ContactsComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})

export class AppModuleShared {
}
