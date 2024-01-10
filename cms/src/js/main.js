'use strict';

import smoothscroll from 'smoothscroll-polyfill';
import { Bugfender } from '@bugfender/sdk';
import '../scss/style.scss';
import './recaptcha';
import { Forms } from './forms.js';
import { Content } from './content.js';
import { SelectDropdown } from './select-dropdown.js';
import { Navigation } from './navigation.js';
import { parsePhoneNumberFromString } from 'libphonenumber-js';
import { Analytics } from './analytics.js';
import { SchedulingAnalytics } from './analytics-scheduling';
import { Footer } from './appointments/footer.js';
import { ModalListener } from './appointments/modal-listener';
import { initLottie } from './utils';
import { polyfill } from "seamless-scroll-polyfill";
require('webp-hero/dist-cjs/polyfills.js');
import { WebpMachine } from "webp-hero/dist-cjs";
import 'focus-visible';

// kick off the polyfill!
smoothscroll.polyfill();
polyfill();

class Main {
    constructor() {
        this.domReady();
    }

    domReady(event) {
        // Initialize the BugFender SDK if we have metadata for it
        const bugfenderAppKey = document.querySelector('meta[property="bugfender:appkey"]');
        if (bugfenderAppKey != null) {
            Bugfender.init({
                appKey: bugfenderAppKey.getAttribute('content')
            });

            // If the user is authenticated, add their EPI to the logging request to track issues as it relates to a specific user
            const patientEpi = document.querySelector('meta[property="patient:epi"]');
            if (patientEpi != null) {
                Bugfender.setDeviceKey('epi', patientEpi.getAttribute('content'));
            }
        }

        const webpMachine = new WebpMachine();
        webpMachine.polyfillDocument();

        /**
         * DMG-1933 (race condition, some analytics data missing)
         * 
         * window.google_tag_manager (from gtag.js) is not guaranteed to be immediately available at this stage
         * but this does not imply that the user's browser blocked the gtag.js request. 
         * Previous implementation stopped GA execution at first try!
         * 
         * Simply, let's wait for window.google_tag_manager to be available.
         * If it does not become available within 10s, simply give up.
         * #sad
         */

        let secondsPassed = 0;
        const gaIntervalTimer = setInterval(() => {
            secondsPassed = performance.now() / 1000 - secondsPassed;
            if (window.google_tag_manager) {
                clearInterval(gaIntervalTimer);
                const analytics = new Analytics();
                const schedulingAnalytics = new SchedulingAnalytics(analytics);
            } else if (secondsPassed > 10) {
                // giving up on GA...
                clearInterval(gaIntervalTimer);
            }
        }, 500);

        const forms = new Forms;
        const content = new Content;
        const selectDropdown = new SelectDropdown;
        const navigation = new Navigation
        const modalListener = new ModalListener();
        const footer = new Footer();

        const phoneNumbers = document.querySelectorAll(".phone-number");
        phoneNumbers.forEach((el) => {
            const number = el.getAttribute('data-attr-number');
            if (!number) {
                return;
            }
            const num = parsePhoneNumberFromString(number);
            el.text = num.formatNational();
        });

        initLottie();
    }
}

export default new Main();

if (module.hot) {
    module.hot.accept();
}