'use strict';
import { enableFocusTrap } from '../utils';

export class RedirectListener {
    allowRedirect(callback) {
        if (document.querySelector('.exit-alert-modal')) {
            const modal = document.querySelector('.exit-alert-modal');
            const buttons = modal.querySelectorAll('button');
            const close = modal.querySelector('.close');
            modal.classList.remove('hidden');
            
            // focus on modal while open
            enableFocusTrap('.exit-alert-modal button, .exit-alert-modal span.material-icons.close');

            ['click', 'keydown'].forEach(event => {
                buttons.forEach(el => {
                    el.addEventListener(event, (e) => {
                        if (this.keydownOrClick(event, e)) {
                            if (el.id == 'stay-on-page') {
                                modal.classList.add('hidden');
                                return false;
                            } else {
                                return callback();
                            }
                        };
                    });
                });

                close.addEventListener(event, (e) => {
                    if (this.keydownOrClick(event, e)) {
                        modal.classList.add('hidden');
                    };
                });
            })
        } else {
            return callback();
        }
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }
}