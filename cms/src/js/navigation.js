'use strict';
import { RedirectListener } from './appointments/redirect-listener';
import { Analytics } from "./analytics";

export class Navigation {
    constructor() {
        this.analytics = new Analytics(false);

        if (document.querySelector('.main-header')) {
            this.redirectListener = new RedirectListener();
            this.addEventListeners();
            this.mobileEventListeners();
            this.checkForAlerts();

            // header navigation only has this class during registration workflow
            if (document.querySelector('#mobile-top-navigation-scheduling')) {
                this.mobileScheduleNavigationListeners();
            }
        }

        document.addEventListener('click', hideTT);

        window.onload = function() {
            // do not show on Duly Now video visit flows
            if (location.pathname.indexOf("/schedule/video-visit") < 0) {
                setTimeout(showTT, 5000);
            }
        }
        function showTT() {
            if (document.querySelector('#tip-text')) {
                document.getElementById('tip-text').className='show';
            }
            if (document.querySelector('#tip-text-mobile')){
                document.getElementById('tip-text-mobile').className='show';
            }
        }

        function hideTT() {
            if (document.querySelector('#tip-text')) {
                document.getElementById('tip-text').className='hide';
            }
            if (document.querySelector('#tip-text-mobile')){
                document.getElementById('tip-text-mobile').className='hide';
            }
        }
    }

    mobileScheduleNavigationListeners() {
        let events = ['click', 'keydown'];
        const cancelBtn = document.querySelector('.cancel-schedule-process');
        const mobileLogo = document.querySelector('.dmg-logo-mobile');

        events.forEach((event) => {
            cancelBtn.addEventListener(event, (e) => {
                e.preventDefault();
                e.stopPropagation();

                this.redirectListener.allowRedirect((_) => {
                    location.href = '/schedule';
                });
            });

            mobileLogo.addEventListener(event, (e) => {
                e.preventDefault();
                e.stopPropagation();

                this.redirectListener.allowRedirect((_) => {
                    location.href = '/';
                });
            });
        });
    }

    addEventListeners() {
        let searchFieldForm = document.querySelector('#secondary-nav');
        let searchFieldInput = document.querySelector('#search-site');
        let dropDown = document.querySelector('.nav-dropdown');
        let careDropDown = document.querySelector('.care-dropdown');
        let expandEvents = ['click', 'keydown'];

        expandEvents.forEach((event) => {
            dropDown && dropDown.addEventListener(event, e => {
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    let container = document.querySelector('.nav-dropdown-container');

                    if (window.innerWidth >= 1092) {
                        container.classList.remove('animateOut');
                        if (container.classList.contains('collapsed')) {
                            e.target.classList.add('open');
                            container.classList.remove('collapsed');
                            container.classList.add('expanded');
                        } else {
                            e.target.classList.remove('open');
                            container.classList.remove('expanded');
                            container.classList.add('collapsed');
                        }
                    }
                }
            });

            careDropDown && careDropDown.addEventListener(event, e => {
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    let container = document.querySelector('.care-dropdown-container');

                    if (window.innerWidth >= 992) {
                        container.classList.remove('animateOut');
                        if (container.classList.contains('collapsed')) {
                            e.target.classList.add('open');
                            container.classList.remove('collapsed');
                            container.classList.add('expanded');
                            // send an analytics event when opening the dropdown, but not closing it (per requirements)
                            this.analytics._sendEvent('Clearstep', `Dropdown Click`);
                        } else {
                            e.target.classList.remove('open');
                            container.classList.remove('expanded');
                            container.classList.add('collapsed');
                        }
                    }
                }
            });
        });

        searchFieldForm.querySelector('form').addEventListener('submit', (e) => {
            e.preventDefault();
            e.stopPropagation();
            const queryValue = searchFieldInput.value;

            this.redirectListener.allowRedirect((_) => {
                window.location = '/search?query=' + encodeURIComponent(queryValue);
            });
        });

        ['click', 'keydown'].forEach((event) => {
            searchFieldForm.querySelector('.search-icon').addEventListener(event, (e) => {
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    if (searchFieldForm.classList.contains('search-expanded')) {
                        searchFieldForm.querySelector('form').submit();
                    } else {
                        e.preventDefault();
                        searchFieldForm.classList.add('search-expanded');
                        searchFieldForm.querySelector('input').focus();
                    }
                }
            });
        });

        ['click', 'keydown'].forEach((event) => {
            searchFieldForm.querySelector('.material-icons.close').addEventListener(event, (e) => {
                e.preventDefault();
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    searchFieldForm.classList.remove('search-expanded');
                }
            });
        });

        document.addEventListener('click', (e) => {
            const parent = document.querySelector('.nav-dropdown');
            const container = document.querySelector('.nav-dropdown-container');

            // clicking outside filter container should close filter only on desktop
            if (container && window.innerWidth >= 992) {
                if (
                    container !== e.target &&
                    !container.contains(e.target) &&
                    !e.target.classList.contains('dropdown-link') &&
                    !e.target.classList.contains('material-icons')
                ) {
                    parent.classList.remove('open');
                    container.classList.remove('expanded');
                    container.classList.add('collapsed');
                }
            }
        });

        const linksDropdownTriggerElements = document.querySelectorAll(
            '#secondary-nav .material-icons.person_pin, #secondary-nav .material-icons.expand_less, #secondary-nav .material-icons.expand_more, #secondary-nav .more-menu'
        );
        linksDropdownTriggerElements.forEach((elem) => {
            ['click', 'keydown'].forEach((event) => {
                elem.addEventListener(event, (e) => {
                    const more = document.querySelector('#secondary-nav .expand_more');
                    const less = document.querySelector('#secondary-nav .expand_less');
                    if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                        document.querySelector('#secondary-nav .logged-in').classList.toggle('show-dropdown');
                        more.classList.toggle('hidden');
                        less.classList.toggle('hidden');
                    }

                    if (e.target == more || e.target == less) {
                        if (more.classList.contains('hidden')) {
                            less.focus();
                        } else {
                            more.focus();
                        }
                    }
                });
            });
        });
    }

    mobileEventListeners() {
        let hamburger = document.querySelector('#hamburger');
        let navigationContainer = document.querySelector('.nav-container');
        let navDropdown = document.querySelector('.nav-dropdown');
        let dropDownContainer = document.querySelector('.nav-dropdown-container');
        let careDropDown = document.querySelector('.care-dropdown-mobile');
        let header = null;
        if (navDropdown) {
            header = navDropdown.querySelector('.header');
        }
        let bodyOverlay = document.querySelector('.body-container ');
        let backOnMobile = document.querySelector('.back-on-mobile');
        let searchFieldFormMobile = document.querySelector('#main-nav');
        let searchFieldInputMobile = document.querySelector('#search-site-mobile');
        let expandEvents = ['click', 'keydown'];

        expandEvents.forEach((event) => {
            hamburger.addEventListener(event, function (e) {
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    this.classList.toggle('open');

                    if (navigationContainer.classList.contains('expand')) {
                        navigationContainer.classList.remove('hide');
                        navigationContainer.classList.remove('expand');
                        navigationContainer.classList.add('contract');
                        bodyOverlay.classList.remove('overlay');

                        // need to hide element after annimation so tabs aren't picked up with tab index
                        setTimeout(function () {
                            navigationContainer.classList.remove('contract');
                        }, 500);
                    } else {
                        navigationContainer.classList.remove('contract');
                        navigationContainer.classList.add('expand');
                        bodyOverlay.classList.add('overlay');
                    }
                }
            });

            careDropDown && careDropDown.addEventListener(event, e => {
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    let container = document.querySelector('.care-dropdown-container-mobile');

                    container.classList.remove('animateOut');
                    if (container.classList.contains('collapsed')) {
                        e.target.classList.add('open');
                        container.classList.remove('collapsed');
                        container.classList.add('expanded');
                        // send an analytics event when opening the dropdown, but not closing it (per requirements)
                        this.analytics._sendEvent('Clearstep', `Dropdown Click`);
                    } else {
                        e.target.classList.remove('open');
                        container.classList.remove('expanded');
                        container.classList.add('collapsed');
                    }
                }
            });

            navDropdown && header && header.addEventListener(event, function (e) {
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    if (window.innerWidth < 1092) {
                        dropDownContainer.classList.remove('animateOut');
                        dropDownContainer.classList.remove('collapsed');
                        dropDownContainer.classList.add('expanded');
                    }
                }
            });

            backOnMobile.addEventListener(event, function (e) {
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    dropDownContainer.classList.add('animateOut');

                    // remove class once animation is complete
                    setTimeout(function () {
                        dropDownContainer.classList.remove('animateOut');
                        dropDownContainer.classList.add('collapsed');
                    }, 500);

                    dropDownContainer.classList.add('collapsed');
                    dropDownContainer.classList.remove('expanded');
                }
            });

            searchFieldFormMobile.querySelector('.search-icon').addEventListener(event, (e) => {
                if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                    if (searchFieldFormMobile.classList.contains('search-expanded')) {
                        searchFieldForm.querySelector('form').submit();
                    } else {
                        e.preventDefault();
                        searchFieldFormMobile.classList.add('search-expanded');
                        searchFieldFormMobile.querySelector('input').focus();
                    }
                }
            });
        });

        searchFieldFormMobile.querySelector('form').addEventListener('submit', (e) => {
            e.preventDefault();
            e.stopPropagation();
            const queryValue = searchFieldInputMobile.value;

            this.redirectListener.allowRedirect((_) => {
                window.location = '/search?query=' + encodeURIComponent(queryValue);
            });
        });
    }

    checkForAlerts() {
        // check if alert is currently on page
        let alert = document.querySelector('.emergency-alert') !== null;

        if (alert) {
            let alertsContainer = document.querySelectorAll('.emergency-alert');
            let alertBodyContainer = document.querySelector('.body-alert-container');
            let body = document.querySelector('.body-container');

            window.addEventListener('scroll', (e) => {
                let alertContainerHeight = alertBodyContainer.offsetHeight;
                let alertContainerTop = alertBodyContainer.offsetTop;
                let top = window.pageYOffset || document.documentElement.scrollTop;

                for (let i = 0; i < alertsContainer.length; i++) {
                    if (!alertsContainer[i].classList.contains('hide')) {
                        if (top != 0) {
                            alertBodyContainer.classList.add('sticky');
                            // padding needs to be dynamic since multiple alerts can be inside container
                            body.style.paddingTop = alertContainerHeight + alertContainerTop + 'px';
                        } else {
                            alertBodyContainer.classList.remove('sticky');
                            body.style.paddingTop = '0px';
                        }
                    }
                }
            });

            for (let i = 0; i < alertsContainer.length; i++) {
                let closeBtn = alertsContainer[i].querySelector('.close-alert');
                // check to see if user already saw the alert and canceled out
                if (this.getCookie(alertsContainer[i].id)) {
                    // hide message
                    alertsContainer[i].classList.add('hide');
                    continue;
                }

                let expandEvents = ['click', 'keydown'];

                expandEvents.forEach((event) => {
                    if (closeBtn) {
                        closeBtn.addEventListener(event, (e) => {
                            if ((event === 'keydown' && e.keyCode === 13) || event === 'click') {
                                // set cookie so user won't see message again
                                this.setCookie(alertsContainer[i].id, 'DMG-Alert-Seen');
                                alertsContainer[i].classList.add('hide');

                                if (alertBodyContainer.classList.contains('sticky')) {
                                    let alertContainerHeight = alertBodyContainer.offsetHeight;
                                    let alertContainerTop = alertBodyContainer.offsetTop;
                                    body.style.paddingTop = alertContainerHeight + alertContainerTop + 'px';
                                }
                            }
                        });
                    }
                });
            }
        }
    }

    setCookie(name, value, days = 365, path = '/') {
        const expires = new Date(Date.now() + days * 864e5).toUTCString();
        document.cookie =
            name +
            '=' +
            encodeURIComponent(value) +
            '; expires=' +
            expires +
            '; path=' +
            path +
            '; SameSite=Lax' +
            '; secure';
    }

    getCookie(name) {
        return document.cookie.split('; ').reduce((r, v) => {
            const parts = v.split('=');
            return parts[0] === name ? decodeURIComponent(parts[1]) : r;
        }, '');
    }
}
