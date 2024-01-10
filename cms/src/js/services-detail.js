'use strict';
import { YouTubeApi } from './youtube-api';

export class ServicesDetail {
    constructor() {
        if (document.querySelector('.services-detail-container')) {
            this.setup();
        }
    }

    setup() {
        // per designs, not all categories may be loaded based on detail content
        // need to check to make sure containers are loaded in DOM first
        this.initActiveState('.interactive-container');
        this.hideEmptyNavigationBar();

        if (document.querySelector('.expand-procedures')) {
            this.viewMoreProcedures();
        }

        if (document.querySelector('.expand-conditions')) {
            this.viewMoreConditions();
        }

        if (document.querySelector('.hospital-affiliation')) {
            this.hospitalAffliations();
        }

        if (document.querySelector('.service-videos-section')) {
            this.player = new YouTubeApi();
        }

        if (document.querySelector('.choose-patient-prompt')) {
            this.patientLoginModal();
        }

        if (document.querySelector('.nearestLocations')) {
            this.viewNearestLocations();
        }

        if (document.querySelector('.viewAllHours')) {
            this.viewAllHours();
        }

        if (document.getElementById('specials')) {
            this.handleSpecials();
        }
    }

    hideEmptyNavigationBar() {
        const sidebar = document.querySelector('.sidebar-navigation');
        const scheduleCta = sidebar.querySelector('.schedule');
        const height = sidebar.offsetHeight;
        
        if (!height && !scheduleCta) {
            const container = document.querySelector('.services-detail-container');
            sidebar.classList.add('hidden');
            container.classList.add('justify-center');
        }
    }

    removeActiveSelect(categories) {
        for (let i = 0; i < categories.length; i++) {
            categories[i].classList.remove('active');
        }
    }

    showColumns(cols) {
        for (let i = 0; i < cols.length; i++) {
            cols[i].classList.remove('hidden');
        }
    }

    hideColumns(cols) {
        for (let i = 0; i < cols.length; i++) {
            // first two columns should always be shown
            if (i > 1) {
                cols[i].classList.add('hidden');
            }
        }
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click" || event === "touchmove") {
            return true;
        } else {
            return false;
        }
    }

    patientLoginModal() {
        const scheduleBtn = document.querySelector('.servicesForm');
        const modal = document.querySelector('.choose-patient-prompt');

        if (modal !== null && scheduleBtn !== null) {
            scheduleBtn.addEventListener('submit', (e)=> {
                e.preventDefault();
                modal.classList.remove('hidden');
            })
        }
    }

    viewMoreConditions() {
        const container = document.querySelector('.interactive-container');
        const conditions = container.querySelector('.conditions');
        const conditionBtn = container.querySelector('.expand-conditions');
        const cols = conditions.querySelectorAll('.column');
        this.addExpandEvent(conditionBtn, cols)
    }

    viewMoreProcedures() {
        const container = document.querySelector('.interactive-container');
        const procedures = container.querySelector('.procedures');
        const procedureBtn = container.querySelector('.expand-procedures');
        const cols = procedures.querySelectorAll('.column');
        this.addExpandEvent(procedureBtn, cols)
    }

    addExpandEvent(btn, cols) {
        const clickEvents = ['click', 'keydown'];

        clickEvents.forEach((event) => {
            btn.addEventListener(event, (e) => {
                if (this.keydownOrClick(event, e)) {
                    if (btn.classList.contains('view-less')) {
                        this.hideColumns(cols);
                        btn.classList.remove('view-less');
                    } else {
                        this.showColumns(cols);
                        btn.classList.add('view-less');
                    }
                }
            });
        });
    }

    displayCategoryContent(container, element) {
        const categories = container.querySelectorAll('.container');
        const mobileDots = container.querySelectorAll('.dot');
        let className = element.querySelector('h3').innerText;
        let selector = className.toLowerCase().split(" ").join("-").replace(/[^A-Za-z0-9-]/g, "");
        for (let i = 0; i < categories.length; i++) {
            if (categories[i].classList.contains(selector)) {
                if (selector != 'interactive') {
                    if (this.player) {
                        this.player.pauseVideo();
                    }
                }
                categories[i].classList.add('active');
            } else {
                categories[i].classList.remove('active');
            }
        }
    }

    initActiveState(containerElement) {
        const containers = document.querySelectorAll(containerElement);
        containers.forEach((container) => {
            const categories = container.querySelectorAll('.category');
            const clickEvents = ['click', 'keydown'];
    
            // first container displayed can be dynamic
            // grab first one and make it active
    
            let headers = container.querySelectorAll('.category');
            let containers = container.querySelectorAll('.container');
    
            if (headers[0]) {
                headers[0].classList.add('active');
            }
    
            if (containers[0]) {
                containers[0].classList.add('active');
            }
            
            categories.forEach((element) => {
                clickEvents.forEach((event) => {
                    element.addEventListener(event, (e) => {
                        if (this.keydownOrClick(event, e)) {
                            this.removeActiveSelect(categories);
                            element.classList.add('active');
                            this.displayCategoryContent(container, element);
                        }
                    })
                })
            })
        })
    }

    hospitalAffliations() {
        const container = document.querySelector('.hospital-affiliation');
        const hospitalNames = container.querySelectorAll('.name');
        const hospitalContainers = container.querySelectorAll('.hospital');
        const clickEvents = ['click', 'keydown', 'touchmove'];

        hospitalNames.forEach((element) => {
            clickEvents.forEach((event) => {
                element.addEventListener(event, (e)=> {
                    if (this.keydownOrClick(event, e)) {
                        let parent = element.parentNode;

                        if (parent.classList.contains('active')) {
                            parent.classList.remove('active');
                        } else {
                            this.removeActiveSelect(hospitalContainers);
                            parent.classList.add('active');
                        }
                    }
                });
            });
        });
    }

    handleSpecials() {
        const _specials = document.getElementById('specials');
        if (!_specials) {
            return;
        }
        const specials = _specials.querySelectorAll('.specialContainer');
        if (!specials.length) {
            return;
        }
        const viewMoreToggle = document.getElementById('viewMoreToggle');
        if (!viewMoreToggle) { 
            return;
        }
        const toggleText = viewMoreToggle.querySelector('.toggleText');
        let isActive = false;
        viewMoreToggle.addEventListener('click', () => {
            isActive = !isActive;
            toggleText.innerText = isActive ? 'Show Less' : 'Show More';
            isActive ? viewMoreToggle.classList.add('active') : viewMoreToggle.classList.remove('active');
            specials.forEach((special) => {
                if (isActive) {
                    if (special.classList.contains('hidden')) {
                        special.classList.remove('hidden');
                    }
                } else {
                    if (special.dataset.hidden) {
                        special.classList.add('hidden');
                    }
                }
            })
        })
    }

    viewNearestLocations() {
        const nearestLocations = document.querySelectorAll('.nearestLocations');
        nearestLocations.forEach((nearestLocation) => {
            const toggle = nearestLocation.querySelector('.toggle');
            toggle.addEventListener('click', () => {
                nearestLocation.classList.toggle('active');
            })
        });
    }

    viewAllHours() {
        const viewAllHours = document.querySelectorAll('.viewAllHours');
        viewAllHours.forEach((viewAll) => {
            const toggle = viewAll.querySelector('.toggle');
            toggle.addEventListener('click', () => {
                viewAll.classList.toggle('active');
            })
        })
    }
}
