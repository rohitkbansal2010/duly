'use strict';
import { ClosestCareLocation } from './closest-care-location';
import { Analytics } from './analytics';

export class ImmediateCare {
    constructor() {
        if (!document.querySelector(".body-container.immediate-care")) {
            return;
        }

        this.detectEnterKeysOnExpandableCells();
        this.watchInputChange();
        this.addEventListeners();
        this.configureSymptomCheckerModule();

        this.analytics = new Analytics(false);
    }

    addEventListeners() {
        const btn = document.querySelector('#trigger-location');
        const locationsCta = document.querySelector('.locations-cta');
        const spinnerContainer = document.querySelector('.spinner-container');

        ['click', 'keydown'].forEach((event => {
            btn.addEventListener(event, (e) => {
                if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                    locationsCta.classList.add('hidden');
                    spinnerContainer.classList.remove('hidden');

                    if (navigator.geolocation) {
                        if (navigator.permissions) {
                            navigator.permissions.query({ name: 'geolocation' }).then(PermissionStatus => {
                                if (PermissionStatus.state == 'denied') {
                                    // if denied, show content without distance
                                    this.routeToLocations();
                                }
                            });
                        }

                        navigator.geolocation.getCurrentPosition(this.showPosition.bind(this), this.routeToLocations.bind(this));
                    } else {
                        console.log("Geolocation is not supported by this browser.");
                        // Safari does not support navigator.permissions, so we cannot quietly use it to check if the user already granted location permissions.
                        // Instead, we will store an authorizedGeoLocation flag in localStorage when the user explicitly asks for closest locations.
                        // On a repeat visit, check the flag. If 1, attempt to get current position, which may reuslt in a locations prompt.
                        if (this.checkauthorizedGeoLocation()) {
                            this.showPosition();
                        }
                    }
                }
            })
        }))
    }

    showPosition(position) {
        new ClosestCareLocation(position, null);
    }

    routeToLocations(error) {
        window.location.href = '/locations?care[]=Immediate+Care&care[]=Express+Care';
    }

    checkauthorizedGeoLocation() {
        if (typeof localStorage['authorizedGeoLocation'] == "undefined" || localStorage['authorizedGeoLocation'] == "0") {
            return false;
        } else {
            return true;
        }
    }

    detectEnterKeysOnExpandableCells() {
        document.querySelectorAll(".conditions-treated .wrap-collabsible, .test .wrap-collabsible").forEach(element => {
            element.addEventListener("keypress", e => {
                if (e.code === "Enter") {
                    const input = e.target.querySelector("input");
                    input.checked = !input.checked;

                    // trigger onchange event so that "onchange" event listeners can react
                    var event = new Event('change');
                    input.dispatchEvent(event);
                }
            });
        });
    }

    watchInputChange() {
        document.querySelectorAll(".conditions-and-testing input").forEach(element => {
            element.addEventListener("change", ({ target }) => {
                if (target.checked) {
                    target.parentNode.classList.add("checked");
                } else {
                    target.parentNode.classList.remove("checked");
                }
            });
        });
    }

    configureSymptomCheckerModule() {
        const form = document.querySelector('.find-right-care');
        const input = form.querySelector("input#find-right-care-symptoms");

        if (form === null) {
            return;
        }

        input.addEventListener('keyup', e => {
            if (e.key === 'Enter') {
                this.sendClearStepRequest();
            }
        });

        form.querySelector('button#submit-symptoms').addEventListener('click', _ => this.sendClearStepRequest());
    }

    sendClearStepRequest() {
        const form = document.querySelector('.find-right-care');
        const csrfToken = document.querySelector("input#csrf-token");
        const input = form.querySelector("input#find-right-care-symptoms");
        const checkbox = form.querySelector("input#on-behalf-of-someone-else");

        fetch(`${window.location.origin}/check-symptoms`, {
            body: JSON.stringify({
                symptoms: input.value,
                is_proxy_user: checkbox.checked,
                [csrfToken.name]: csrfToken.value
            }),
            method: 'POST',
            mode: 'same-origin',
            cache: 'no-cache',
            follow: true,
            async: true,
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
        }).then(async (response) => {
            const json = await response.json();
            const url = json.url;
            if (url) {
                this.analytics.clearStepImmediateCareCheckSymptomsEvent();
                window.open(`https://${url}`, '_blank').focus();
            }
        });

    }
}