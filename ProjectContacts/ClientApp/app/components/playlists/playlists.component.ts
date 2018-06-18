import { Component, Inject, AfterViewInit } from '@angular/core';
import { PlaylistService, PlaylistSummary } from '../shared/playlist.service';

@Component({
    selector: 'app-playlists',
    templateUrl: './playlists.component.html',
    styleUrls: ['./playlists.component.css'],
    providers: [PlaylistService]
})
/** playlists component*/
export class PlaylistsComponent implements AfterViewInit {

    public playlists: PlaylistSummary[] = [];
    public totalPlaylists: number = 0;
    public p: number = 1;
    public pageSize: number = 10;
    public pageSizes: number[] = [10, 20, 30, 40, 50];
    public totalPages: number = 0;
    private playlistService: PlaylistService;
    private deleteIndex: number = 0;

    /** playlists ctor */
    constructor(playlistService: PlaylistService) {
        this.playlistService = playlistService;
    }

    ngAfterViewInit() {
        this.getPlaylists();
    }

    public newPage(page: number) {
        this.p = page;
        this.getPlaylists();
    }

    public changePageSize(size: number) {
        this.p = 1; // reset to first page
        this.pageSize = size;
        this.getPlaylists();
    }

    private getPlaylists() {
        // Get count:
        this.playlistService.countPlaylists()
            .subscribe(data => {
                this.totalPlaylists = data;
                this.totalPages = Math.floor(this.totalPlaylists / this.pageSize) + 1;
            });

        //Get current page of results:
        this.playlistService.getPlaylists(this.p, this.pageSize)
            .subscribe(data => this.playlists = data);
    }

    public delete(playlist: PlaylistSummary): void {
        this.playlistService.deletePlaylist(playlist.id)
            .subscribe(data => {
                if (data) // Refresh current page:
                    this.getPlaylists();
            });
    }
}