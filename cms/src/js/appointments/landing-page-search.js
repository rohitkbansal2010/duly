'use strict';
import { Search } from '../search';

export class LandingPageSearch {
    constructor() {
        if (!document.querySelector('.schedule-an-appointment-page')) {
            return;
        }

        this.query = new Search(
            "#search-services-landing",
            ".search-content form.search-appointment-services",
            location.origin + "/schedule/services/auto-suggestions",
            "query",
            "#html-content-template-recent-searches",
            "#html-content-template-auto-complete"
        );

        this.addEventListeners();
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    addEventListeners() {
        const btn = document.querySelector('.search-results-btn');
        const events = ['click', 'keydown'];

        events.forEach((event) => {
            btn.addEventListener(event, (e)=> {
                if (this.keydownOrClick(event, e)) {
                    const query = document.querySelector('#search-services-landing').value;
                    window.location.href = '/schedule/services?query=' + query;
                }
            })
        });
    }
}