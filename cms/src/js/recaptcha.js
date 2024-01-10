'use strict';

/**
 * This method will execute reCAPTCHA, verify the token, verify the human, and finally call the provided callback function.
 * 
 * @param {callback} callback Fn that will be executed after reCAPTCHA verifies the human
 */
export const executeCAPTCHAToken = (callback = _ => { }) => {
    if (!grecaptcha) {
        callback(null);
    }
    window.grecaptchaCallback = token => {
        callback(token);
    };

    grecaptcha.reset();
    grecaptcha.execute();
};

window.onReCAPTCHALoadCallback = _ => {
    const reCAPTCHAElement = document.querySelector("#recaptcha");
    grecaptcha.render(
        reCAPTCHAElement,
        {
            sitekey: reCAPTCHAElement.dataset.siteKey,
            size: "invisible",
            callback: "grecaptchaCallback"
        }
    );
};