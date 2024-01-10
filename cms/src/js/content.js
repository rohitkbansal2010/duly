'use strict';
import { breakpoints } from './variables';

export class Content {
    constructor() {
        if (document.querySelector('.health-topics, .static-page')) {
            this.isMobile = window.innerWidth < parseInt(breakpoints["bs-large"]) ? true : false;

            if (document.querySelector('.blog-entry')) {
                this.content();
            }

            this.nav();
        }
    }

    nav() {
        const li = document.querySelectorAll('nav#health-topics-navigation-bar ul li:not(:first-of-type), nav#rail ul li:not(:first-of-type)');

        li.forEach((el) => {
            el.addEventListener('click', (event) => {
                const a = el.querySelector("a");
                // Disable the default click behavior
                a.addEventListener('click', (e) => {
                    e.preventDefault();
                });

                // Rebind to the entire element and open in the correct window if applicable
                if (a.getAttribute('target') == "_blank") {
                    window.open(a.getAttribute('href'), '_blank');
                } else {
                    window.location = a.getAttribute('href');
                }
            });
        });
    }

    content() {
        const iframes = document.querySelectorAll(".article-contents .content-body figure iframe");
        iframes.forEach((el) => {
            if (this.isMobile) {
                el.style.height = el.height + "px";
            } else {
                let width = el.offsetWidth;
                // 0.5625 = 9 / 16 | 16:9 aspect ratio for video
                let height = 0.5625 * width;
                el.style.height = height + "px";
            }
        });
    }
}