import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { Playlist } from '../../playlist'; // Import the form model
import { PlaylistService } from '../shared/playlist.service';

@Component({
    selector: 'app-add-playlist',
    templateUrl: './add-playlist.component.html',
    styleUrls: ['./add-playlist.component.css'],
    providers: [PlaylistService]
})
/** add-playlist component*/
export class AddPlaylistComponent {

    model = new Playlist("");
    submitted = false;
    newPlaylistId: number = 0;
    
    /** add-playlist ctor */
    constructor(private playlistService: PlaylistService) { }

    // Method to handle the form submission
    onSubmit() {
        this.submitted = true;
        this.playlistService.addPlaylist(this.model)
            .subscribe(data => this.newPlaylistId = data);
        console.log("Added new playlist: " + this.model.name);
    }
}