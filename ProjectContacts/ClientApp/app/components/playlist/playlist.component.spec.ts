/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { PlaylistComponent } from './playlist.component';

let component: PlaylistComponent;
let fixture: ComponentFixture<PlaylistComponent>;

describe('playlist component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ PlaylistComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(PlaylistComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});