export class SpecialtySelector {
    constructor(onSpecialtySelectedCallback, onSuccessfullAjaxCallback) {
        this.container = document.querySelector(".specialty-custom-picker-container");
        if (!this.container) {
            // abort if specialty selector element not found
            return;
        }

        this.onSpecialtySelectedCallback = onSpecialtySelectedCallback;
        this.onSuccessfullAjaxCallback = onSuccessfullAjaxCallback

        this.setupClickEvents();
        this.setupSearch();
        this.setupAccessibilityNavigation();
    }

    hideRadioContainers() {
        let radioContainers = document.querySelectorAll('.radio-container');

        if (radioContainers) {
            radioContainers.forEach(container => {
                container.classList.add('hidden');
            });
        };
    }

    hideServiceDetails() {
        let serviceDetails = document.querySelectorAll('.service-offered');

        if (serviceDetails) {
            serviceDetails.forEach(detail => {
                detail.classList.add('hidden');
            });
        };
    }

    setupClickEvents() {
        // show the search list
        this.container.addEventListener("click", _ => this.container.classList.add("active"));
        const dynamicContent = document.querySelector('.dynamic-content');

        // close the search list and mark the selection
        let selectableRows = document.querySelectorAll(".specialty:not(.has-subspecialties) .main-name, .specialty.has-subspecialties .subspecialty");
        selectableRows.forEach(selectableRow => selectableRow.addEventListener("click", e => {
            let target = e.target;
            this.container.querySelector(".default-state-text").innerText = target.innerText;
            this.container.classList.remove("active");
            this.hideRadioContainers();
            this.onSpecialtySelectedCallback(target);
            this.container.classList.add("option-selected-border");
            this.container.querySelector(".default-state-text").classList.add("option-selected-text");

            if (target.id != 'show-all-services') {
                // no event should fire if user selects to show all services again
                this.fireAjaxEvent(target);
            } else {
                dynamicContent.classList.add('hidden');
            }
        }));

        let parentRows = document.querySelectorAll(".specialty .main-name");
        parentRows.forEach(row => {
            row.addEventListener('click', e => {
                let target = e.target;
                let id = target.getAttribute('data-attr-services');
                let radioContainer = document.querySelector(`.radio-container[data-attr-service-id="${id}"]`);
                let walkin = document.querySelector(".location-details-container .walk-in-services");

                if (radioContainer) {
                    if (walkin) {
                        walkin.classList.add('hidden');
                    }

                    radioContainer.classList.remove('hidden');
                    this.hideServiceDetails();
                    this.resetRadioButtons(radioContainer);
                    dynamicContent.classList.add('hidden');
                    this.container.querySelector(".default-state-text").innerText = target.innerText;
                    this.container.classList.remove("active");
                }
            })
        })

        let radioButtons = document.querySelectorAll('.radio-container input[type="radio"]');

        if (radioButtons) {
            radioButtons.forEach(button => {
                button.addEventListener('click', e => {
                    let parent = e.target.closest('.radio-btn');

                    if (parent) {
                        let text = parent.querySelector('label').innerText;
                        this.container.querySelector(".default-state-text").innerText = text;
                        this.container.classList.remove("active");
                        this.hideRadioContainers();
                        this.onSpecialtySelectedCallback(parent);
                        this.fireAjaxEvent(parent);
                    }
                });
            });
        }

        // hide search list if clicked outside
        document.querySelector("body").addEventListener("click", e => {
            let target = e.target;
            if (target.classList.contains("specialty-custom-picker-container") || target.classList.contains("default-state-text") || target.classList.contains("search-containier") || target.classList.contains("active-list") || target.classList.contains("search-input")) {
                // clicked inside the search list
                return;
            }

            this.container.classList.remove("active");
        });
    }

    resetRadioButtons(container) {
        let radioBtns = container.querySelectorAll('input');

        if (radioBtns) {
            radioBtns.forEach(btn => {
                btn.checked = false;
            });
        };
    }

    setupSearch() {
        this.container.querySelector("input.search-input").addEventListener("keyup", e => {
            document.querySelectorAll(".specialty:not(.has-subspecialties) .main-name").forEach(elem => {
                elem.classList.remove("hidden");
                let requiredString = e.target.value.toLowerCase();
                if (!elem.innerText.toLowerCase().match(new RegExp(`.*${requiredString}.*`))) {
                    elem.classList.add("hidden");
                }
            });

            document.querySelectorAll(".specialty.has-subspecialties .subspecialty").forEach(elem => {
                elem.classList.remove("hidden");
                let requiredString = e.target.value.toLowerCase();
                if (!elem.innerText.toLowerCase().match(new RegExp(`.*${requiredString}.*`))) {
                    elem.classList.add("hidden");
                }
            });
        });
    }

    getClosest(elem, selector) {
        for (; elem && elem !== document; elem = elem.parentNode) {
            if (elem.matches(selector)) return elem;
        }
        return null;
    }

    setupAccessibilityNavigation() {
        this.container.addEventListener("keyup", e => {
            if (e.keyCode === 13) {
                // apply existing onclick listeners
                this.container.dispatchEvent(new Event('click'));
            }
        });
        document.querySelectorAll(".specialty:not(.has-subspecialties) .main-name, .specialty.has-subspecialties .subspecialty").forEach(row => {
            row.addEventListener("keyup", e => {
                if (e.keyCode === 13) {
                    // apply existing onclick listeners
                    row.dispatchEvent(new Event('click'));
                    setTimeout(() => {
                        // close the prompt
                        document.querySelector("body").dispatchEvent(new Event('click'));
                    }, 100);
                }
            });
            row.addEventListener("keyup", e => {
                if (e.keyCode === 27) {
                    // close the prompt
                    document.querySelector("body").dispatchEvent(new Event('click'));
                }
            });
        });
    }

    /**
     * Update the view with new locations data using current filters.
     */
    async fireAjaxEvent(specialty) {
        const serviceId = specialty.getAttribute('data-attr-services');
        const currentLocationId = document.querySelector('.location-details-container').id;
        const container = document.querySelector('.dynamic-content');
        const scheduleBtn = document.querySelector('.cta.schedule');
        container.classList.add('hidden');

        if (scheduleBtn) {
            scheduleBtn.classList.add('hidden');
        }

        let url = "/locations/detail?service=" + serviceId + "&locationId=" + currentLocationId;

        fetch(url, {
            method: 'GET',
            mode: 'same-origin',
            cache: 'no-cache',
            follow: true,
            async: true,
            credentials: 'same-origin',
            headers: {
                'Accept': 'text/html',
                'x-isAjax': true
            }
        }).then(async (response) => {
            const text = await response.text();
            if (response.ok) {
                return text;
            } else {
                return Promise.reject(text)
            }
        }).then(async (data) => {
            container.innerHTML = data;
            container.classList.remove('hidden');

            if (scheduleBtn) {
                scheduleBtn.classList.remove('hidden');
            }

            if (this.onSuccessfullAjaxCallback) {
                this.onSuccessfullAjaxCallback();
            }
        }).catch((response) => {});
    }
}