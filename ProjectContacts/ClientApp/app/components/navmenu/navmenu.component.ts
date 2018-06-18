import { Component, ElementRef, Renderer } from '@angular/core';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})

export class NavMenuComponent {

    constructor(private el: ElementRef, private renderer: Renderer) {
    }

    collapseMenu() {
        this.renderer.setElementClass(this.el.nativeElement.querySelector('.navbar-collapse'), 'in', false);
    }
}

