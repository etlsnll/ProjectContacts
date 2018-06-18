/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { AllMusicComponent } from './all-music.component';

let component: AllMusicComponent;
let fixture: ComponentFixture<AllMusicComponent>;

describe('AllMusic component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ AllMusicComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(AllMusicComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});