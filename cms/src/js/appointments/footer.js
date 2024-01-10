'use strict';

export class Footer { 
    constructor() {
        // cta form is the class that anchors the CTA during the scheduling flow
        if (!document.querySelector('.cta-form')) { 
            return; 
        }

        this.addAnchorClass();
    }

    addAnchorClass() { 
        let body = document.querySelector('body');
        body.classList.add('has-anchored-cta');
    };
}
    