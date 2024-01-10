'use strict';
import lottie from 'lottie-web';
import '../json/Duly_loader_strokestofill.json';
import { AsYouType } from 'libphonenumber-js';

/**
 * Prevents tabbing outside of the provided list of elements.
 * 
 * @param {*} focusableElementsSelectorString 
 */
export const enableFocusTrap = (focusableElementsSelectorString) => {
    let focusable = document.querySelectorAll(focusableElementsSelectorString);
    if (focusable.length) {
        focusable[0].focus();
    }

    window.addEventListener('keydown', e => {
        if (e.keyCode === 9) {
            focusable = document.querySelectorAll(focusableElementsSelectorString);
            if (focusable.length) {
                let first = focusable[0];
                let last = focusable[focusable.length - 1];
                let shift = e.shiftKey;
                if (shift) {
                    if (e.target === first) { // shift-tab pressed on first input in dialog
                        last.focus();
                        e.preventDefault();
                    }
                } else {
                    if (e.target === last) { // tab pressed on last input in dialog
                        first.focus();
                        e.preventDefault();
                    }
                }
            }
        }
    });
};

export const initLottie = () => {
    document.querySelectorAll(".lottie").forEach(element => {
        if (element.lottieInitialized) {
            return;
        }

        let path = null;

        // if CDN is used, we need to remove "/dist" and use the CDN domain
        const mainJSBundleScriptTag = document.querySelector("#main-js-bundle");
        const distCdnUrl = mainJSBundleScriptTag.dataset.distCdnUrl ? mainJSBundleScriptTag.dataset.distCdnUrl : null;
        const distMountPoint = mainJSBundleScriptTag.dataset.distMountPoint ? mainJSBundleScriptTag.dataset.distMountPoint : null;
        if (distCdnUrl !== null || distMountPoint !== null) {
            path = `${distCdnUrl}${distMountPoint}Duly_loader_strokestofill.json`;
        } else {
            path = "/dist/Duly_loader_strokestofill.json";
        }

        lottie.loadAnimation({
            container: element, // Required
            path: path, // Required
            loop: true, // Optional
            autoplay: true // Optional
        });
            
        element.lottieInitialized = true;
    });
}

export const setupPhoneHandler = (input) => {
    ['keyup', 'focus', 'blur', 'paste'].forEach((event) => {
        input.addEventListener(event, (e) => {
            const formattedNumber = new AsYouType('US').input(e.target.value);
            e.target.value = formattedNumber;
        });
    });
}