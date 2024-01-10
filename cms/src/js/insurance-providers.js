'use strict';

export class InsuranceProviders {
    constructor() {
        this.container = document.querySelector('.providers-container')
        if (this.container) {
            this.addClickListener();
            this.addSearchListener();
            this.showPlansOnMobile();
        }
    }

    addClickListener() {
        let logoContainer = document.querySelector('.logo-container');
        let logos = logoContainer.querySelectorAll('.logo');
        let self = this;
        let events = ['click', 'keydown'];

        for (let i = 0; i < logos.length; i++) {

            events.forEach((event) => {
                logos[i].addEventListener(event, function(e) {
                    if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                        self.container.classList.add('showPlans');
                        self.removeActive(logos, 'selected');
                        self.expandProviderDetails(logos[i].id);
                        this.classList.add('selected');
        
                        // scroll to top for logos on bottom of list
                        if (window.innerWidth >= 800) {
                            window.scrollTo({top: 0, behavior: 'smooth'});
                        }
                    }
                });
            });
        }
    }

    showPlansOnMobile() {
        let logoContainer = document.querySelector('.logo-container');
        let logos = logoContainer.querySelectorAll('.logo');
        let self = this;

        for (let i = 0; i < logos.length; i++) {
            logos[i].querySelector('.image-container').addEventListener('click', function() {
                if (this.parentNode.classList.contains('expanded')) {
                    self.removeActive(logos, 'expanded');
                } else {
                    self.removeActive(logos, 'expanded');
                    this.parentNode.classList.add('expanded');
                }
            })
            self.clonedSectionForMobile(logos[i]);
        }
    }

    clonedSectionForMobile(element) {  
        let id = element.getAttribute('id');
        let elementFound = document.querySelector("#" + id + "-provider") !== null;
        if (elementFound) {
            let planDetails = document.querySelector("#" + id + '-provider');
            let clonedEle = planDetails.cloneNode(true);
            clonedEle.classList.add('plans-on-mobile');
            const headline = document.createElement('h2')
            headline.innerHTML = 'Plans we accept from'
            clonedEle.insertBefore(headline, clonedEle.firstChild)
            clonedEle.setAttribute("id", "");
            element.appendChild(clonedEle);
        }
    }

    removeActive(logos, className) {
        for (let x = 0; x < logos.length; x++) {
            logos[x].classList.remove(className);
        }
    }

    expandProviderDetails(planName) {
        let providers = document.querySelectorAll('.single-provider-container');
        let defaultState = document.querySelector('.default-state');
        defaultState.classList.add('hide');

        for (let y = 0; y < providers.length; y++) {
            if (providers[y].classList.contains(planName)) {
                providers[y].parentNode.classList.remove('hide');
                providers[y].classList.remove('hide');
            } else {
                providers[y].classList.add('hide');
            }
        }
    }

    addSearchListener() {
        let searchField = document.querySelector('#search-provider');
        let logoContainer = document.querySelector('.logo-container');
        let logos = logoContainer.querySelectorAll('.logo');
        let planTypes = document.querySelectorAll('.plan-type');
        let self = this;

        searchField.addEventListener('keyup', function(e) {
            self.searchThroughLogos(logos, this.value.trim());
            self.searchThroughPlans(planTypes, logoContainer, this.value.trim());
            self.checkForResults(logos);
        })
    }

    searchThroughPlans(plans, logoContainer, searchTerm) {
        let providers = [];

        for (let i = 0; i < plans.length; i++) {
            let planName = plans[i].innerHTML.toLowerCase();
            let planNameWithoutSpaces = plans[i].innerHTML.split(' ').join('').toLowerCase();

            if (planName.includes(searchTerm.toLowerCase()) || planNameWithoutSpaces.includes(searchTerm.toLowerCase())) {
                // provider name is ID of parent div
                let provider = plans[i].parentNode.parentNode.id.split('-provider')[0];

                providers.push(provider);
            }
        }

        if (providers.length) {
            for (let x = 0; x < providers.length; x++) {
                let logo = logoContainer.querySelector('#' + providers[x]);
                if (logo) {
                    logo.classList.remove('hide');
                    logo.classList.add('show');
                }
            }
        }
    }

    checkForResults(logos) {
        let noResults = document.querySelector('.no-search-results');
        let hidden = 0;

        for (let i = 0; i < logos.length; i++) {
            if (logos[i].classList.contains('hide')) {
                hidden += 1;
            }
        }

        // if all logos are hidden, no results statement is displayed
        if (hidden === logos.length) {
            noResults.classList.remove('hide');
        } else {
            noResults.classList.add('hide');
        }
    }

    searchThroughLogos(logos, searchTerm) {
        for (let i = 0; i < logos.length; i++) {
            let planName = logos[i].id.split("-").join(" ").toLowerCase();
            let planNameWithoutSpaces = logos[i].id.split("-").join("").toLowerCase()

            if (planName.includes(searchTerm.toLowerCase()) || planNameWithoutSpaces.includes(searchTerm.toLowerCase())) {
                logos[i].classList.remove('hide');
                logos[i].classList.add('show');
            } else {
                logos[i].classList.remove('show');
                logos[i].classList.add('hide');
            }
        }
    }
}