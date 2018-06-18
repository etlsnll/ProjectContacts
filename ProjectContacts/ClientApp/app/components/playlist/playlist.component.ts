import { Component, OnInit, OnDestroy } from '@angular/core';
import { Http } from '@angular/http';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { PlaylistService, PlaylistDetails, Track } from '../shared/playlist.service';
import { ActivatedRoute, Params } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
    selector: 'app-playlist',
    templateUrl: './playlist.component.html',
    styleUrls: ['./playlist.component.css'],
    providers: [PlaylistService]
})
    
/** playlist component*/
export class PlaylistComponent implements OnInit {

    model = new PlaylistDetails(0, "Unknown", 0, new Array<Track>());
    id: number = 0;
    tracks: Track[] = [];
    public showConfirm: boolean = false;
    private confirmTimeMs: number = 3000;
    private subscription: Subscription = new Subscription();
    private timer: Observable<any> = new Observable();
    waitElementIds: string[] = ['title', 'album', 'artist'];
    searchTitle: FormControl = new FormControl();
    searchArtist: FormControl = new FormControl();
    searchAlbum: FormControl = new FormControl();
    public waitingMoveTrack: boolean = false;

    /** playlist ctor */
    constructor(private playlistService: PlaylistService,
                private route: ActivatedRoute) {
        // Assign actions to search fields, using debounce delay to limit search requests to web API at server until user finishes typing:
        this.searchTitle.valueChanges
            .debounceTime(400)
            .subscribe(data => this.searchTracks());
        this.searchArtist.valueChanges
            .debounceTime(400)
            .subscribe(data => this.searchTracks());
        this.searchAlbum.valueChanges
            .debounceTime(400)
            .subscribe(data => this.searchTracks());
    }

    ngOnInit() {
        // Get ID from param in URL:
        this.route.params
            .subscribe((params: Params) => {
                this.id = params['id'];
                this.playlistService.getPlaylist(this.id)
                                    .subscribe(pl => {
                                        if (pl !== null)
                                            this.model = pl;
                                    });  
            })
    }

    searchTracks() {
        var title: string = (this.searchTitle.value !== null ? this.searchTitle.value.toString() : "");
        var artist: string = (this.searchArtist.value !== null ? this.searchArtist.value.toString() : "");
        var album: string = (this.searchAlbum.value !== null ? this.searchAlbum.value.toString() : "");

        if (title === "" && artist === "" && album === "") {
            this.clearResults();
        }
        else {
            this.setWaitCursor();
            this.playlistService.searchTracks(title, artist, album)
                                .subscribe(result => {
                                    this.tracks = result;
                                    this.resetWaitCursor();
                                });
        }
    }

    clearResults() {
        this.tracks = new Array<Track>();
        this.searchTitle.setValue("");
        this.searchArtist.setValue("");
        this.searchAlbum.setValue("");
    }

    // Method to update playlist name:
    onSubmit(): void {
        //console.log("TODO: update playlist name: " + this.model.name + ", id: " + this.id);
        this.playlistService.updatePlayListTitle(this.model)
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

    public setConfirmTimer() {

        this.showConfirm = true;

        this.timer = Observable.timer(this.confirmTimeMs); // [milliseconds]
        this.subscription = this.timer.subscribe(() => {
            // Hide element from view after timeout
            this.showConfirm = false;
        });
    }

    addTrack(t: Track) {
        var lastTrackNum = this.model.tracks.length > 0 ? this.model.tracks[this.model.tracks.length - 1].trackNum : 0;
        t.trackNum = lastTrackNum === null ? (this.model.tracks.length + 1) : lastTrackNum + 1;

        this.playlistService.playListAddTrack(this.model.id, t)
            .subscribe(data => {
                if (data !== null) 
                    this.model.tracks.push(data); // Add track to end of array
            });
    }

    removeTrack(t: Track) {
        this.playlistService.playListDeleteTrack(this.model.id, t)
            .subscribe(data => {
                if (data !== null)
                    this.model.tracks = data;
            });
    }

    moveTrackUp(t: Track) {
        this.waitingMoveTrack = true; // Block another move button click from user while waiting for track list to refresh
        this.playlistService.playListMoveTrackUp(this.model.id, t)
            .subscribe(data => {
                this.waitingMoveTrack = false;
                if (data !== null)
                    this.model.tracks = data;
            });
    }

    moveTrackDown(t: Track) {
        this.waitingMoveTrack = true; // Block another move button click from user while waiting for track list to refresh
        this.playlistService.playListMoveTrackDown(this.model.id, t)
            .subscribe(data => {
                this.waitingMoveTrack = false;
                if (data !== null)
                    this.model.tracks = data;
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

        this.waitElementIds.forEach(c => {
            var t = document.getElementById(c);
            if (t !== null)
                t.style.cursor = style;
        });
    }
}

