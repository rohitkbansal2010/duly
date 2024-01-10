'use strict';
import { enableFocusTrap } from '../utils';

export class ModalListener { 
    constructor() {
        if (!document.querySelector('.modal')) { 
            return; 
        }

        this.setupListeners();
    }

    setupListeners() { 
        let modals = document.querySelectorAll('.modal');
        let body = document.querySelector('body');

        // if modal is displayed, body needs fixed position to prevent scrolling in the background
        modals.forEach(modal => {
            if (!modal.classList.contains('hidden')) {
                let focusString = '.';

                modal.classList.forEach((className, i) => {
                    i == modal.classList.length - 1 ? focusString += `${className}` : focusString += `${className}.`;
                });

                enableFocusTrap(`${focusString} button, ${focusString} span.material-icons.close`);
                body.classList.add('fixed');
            }

            const observer = new MutationObserver((mutations) => { 
                mutations.forEach((mutation) => {
                    const el = mutation.target;
                    if (!mutation.target.classList.contains('hidden')){
                        body.classList.add('fixed');
                    } else {
                        body.classList.remove('fixed');
                    }
                });
            });
    
            observer.observe(modal, { 
                attributes: true, 
                attributeOldValue: true, 
                attributeFilter: ['class'] 
            });
        });
    };
}
    