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

    //addPlaylist(playlist: Playlist) {
    //    return this.http.post(this.url + 'api/MusicLibrary/AddPlayList', playlist)
    //        .catch(this.handleErrorObservable)
    //        .map(response => response.json() as number);
    //    //.subscribe(res => console.log(res)); // Note - must subscribe to the response even if not interested for POST to work                       
    //}

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

    //getPlaylist(id: number) {
    //    //console.log("getPlaylist() - Playlist id: " + id);
    //    return this.http.get(this.url + 'api/MusicLibrary/Playlist/' + id.toString())
    //        .catch(this.handleErrorObservable)
    //        .map(response => response.json() as PlaylistDetails);
    //}

    //searchTracks(title: string, artist: string, album: string) {
    //    var search = new URLSearchParams();
    //    search.set('title', title); // Add URL query param
    //    search.set('artist', artist); // Add URL query param
    //    search.set('album', album); // Add URL query param
    //    return this.http.get(this.url + 'api/MusicLibrary/SearchTracks', { search: search })
    //        .catch(this.handleErrorObservable)
    //        .map(response => response.json() as Track[]);
    //}

    //updatePlayListTitle(pl: PlaylistDetails) {
    //    return this.http.put(this.url + 'api/MusicLibrary/UpdatePlayListTitle/' + pl.id.toString(), pl)
    //        .catch(this.handleErrorObservable)
    //        .map(response => response.json() as PlaylistDetails);
    //}

    //playListAddTrack<Track>(playlistId: number, t: Track) {
    //    return this.http.put(this.url + 'api/MusicLibrary/PlayListAddTrack/' + playlistId.toString(), t)
    //        .catch(this.handleErrorObservable)
    //        .map(response => response.json() as Track);
    //}

    //playListDeleteTrack(playlistId: number, t: Track) {
    //    return this.http.delete(this.url + 'api/MusicLibrary/PlayList/' + playlistId.toString() + '/DeleteTrack/' + t.trackId.toString())
    //        .catch(this.handleErrorObservable)
    //        .map(response => response.json() as Track[]);
    //}

    deleteContact(contactId: number) {
        return this.http.delete(this.url + 'api/Contact/Delete/' + contactId.toString())
            .catch(this.handleErrorObservable)
            .map(response => response.json() as boolean);
    }

    //playListMoveTrackUp(playlistId: number, t: Track) {
    //    return this.http.put(this.url + 'api/MusicLibrary/PlayListMoveTrackUp/' + playlistId.toString(), t)
    //        .catch(this.handleErrorObservable)
    //        .map(response => response.json() as Track[]);
    //}

    //playListMoveTrackDown(playlistId: number, t: Track) {
    //    return this.http.put(this.url + 'api/MusicLibrary/PlayListMoveTrackDown/' + playlistId.toString(), t)
    //        .catch(this.handleErrorObservable)
    //        .map(response => response.json() as Track[]);
    //}
}
