'use strict';
import { enableFocusTrap } from '../utils';

export class ExitAlert { 
    constructor() {
        if (!document.querySelector('.exit-alert-modal')) { 
            return; 
        }

        this.modal = document.querySelector('.exit-alert-modal');
        this.href = '';
        this.setupListeners();
    }

    setupListeners() { 
        let events = ['keydown', 'click']; 
        let links = document.querySelectorAll('a:not(.back-link):not(.pagination-link)');
        let stayOnPage = document.querySelector('#stay-on-page');
        let leavePage = document.querySelector('#leave-scheduling-process');
        let closeBtn = document.querySelector('.exit-alert-modal .close');

        events.forEach(event => { 
            links.forEach(tag => { 
                tag.addEventListener(event, (e) => { 
                    if (this.keydownOrClick(event, e)) {
                        e.preventDefault();
                        this.href = tag.href;
                        this.modal.classList.remove('hidden');
                        // focus on modal while open
                        enableFocusTrap('.exit-alert-modal button, .exit-alert-modal span.material-icons.close');
                    }   
                }); 
            });
            
            stayOnPage.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.modal.classList.add('hidden');
                }
            });

            closeBtn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    this.modal.classList.add('hidden');
                }
            });

            leavePage.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    window.location = this.href;
                }
            });
        }); 
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }
}
    