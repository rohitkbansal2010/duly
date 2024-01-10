'use strict';

export class LandingPage {
    constructor() {
        if (!document.querySelector('.schedule-an-appointment-page')) {
            return;
        }

        this.setupOnSelectChangelistener();

        this.setupPhysiciansScroll();

        this.setupServiceTileFormListeners();
    }

    setupOnSelectChangelistener() {
        if (document.querySelector('[name="patient-select-mobile"]')) {
            document.querySelector("select[name='patient-select-mobile']").addEventListener('change', e => {
                const selectedOption = document.querySelector(`option[value='${e.target.value}']`);
                document.querySelector(".schedule-navigation.login.mobile .overlay .name").innerText = selectedOption.innerText;
                document.querySelector("form.hidden.change-person-form input[name='appointment_current_epi']").value = e.target.value;
                document.querySelector("form.hidden.change-person-form").submit();
            });
        }
    }

    scrollPhysiciansBy(direction) {
        const physiciansContainer = document.querySelector('.previously-seen-physicians-container .previous-physicians');
        const physicianTile = document.querySelector('.previously-seen-physicians-container .previous-physicians .tile');
        const tileStyle = physicianTile.currentStyle || window.getComputedStyle(physicianTile);

        // scroll by the width of the physician tile + margin
        let scrollBy = physicianTile.offsetWidth + parseFloat(tileStyle.marginRight);

        const scrolledToTheLeft = physiciansContainer.scrollLeft == 0;
        const scrolledToTheRight = physiciansContainer.scrollWidth == (physiciansContainer.scrollLeft + physiciansContainer.offsetWidth);

        // if the view is scrolled all the way to the left and we are scrolling right
        // OR if the view is scrolled all the way to the right and we are scrolling left
        // then we should scroll 120 pixels LESS as to account for the overlay shadow around the left/right arrows
        // so that they don't cover the physician tile content  
        if (scrolledToTheLeft && direction == "right" || scrolledToTheRight && direction == "left") {
            scrollBy -= 120;
        }

        // change scrollBy sign to negative to scroll left
        if (direction == "left") {
            scrollBy *= -1;
        }

        // scroll horizontally by calculated pixels
        physiciansContainer.scrollBy({
            top: 0,
            left: scrollBy,
            behavior: 'smooth'
        });
    }

    setupPhysiciansScroll() {
        const physiciansContainer = document.querySelector('.previously-seen-physicians-container .previous-physicians');

        if (!physiciansContainer) {
            return;
        }

        const physiciansContainerScrollWidth = physiciansContainer.scrollWidth;
        const leftOverflow = document.querySelector('.previously-seen-physicians-container .overflow-overlay.left');
        const rightOverflow = document.querySelector('.previously-seen-physicians-container .overflow-overlay.right');

        // only show the left/right scrol arrows if overflow
        if (physiciansContainerScrollWidth > physiciansContainer.offsetWidth) {
            rightOverflow.classList.remove('hidden');
        }

        rightOverflow.addEventListener('click', _ => {
            this.scrollPhysiciansBy("right");
        });

        leftOverflow.addEventListener('click', _ => {
            this.scrollPhysiciansBy("left");
        });

        // update left and right arrows' visibility after smooth scroll ends
        // there is not native even for "scroll end" so we are inventing our own!
        var lastScrollLeft = physiciansContainer.scrollLeft;
        setInterval(_ => {
            if (physiciansContainer.scrollLeft - lastScrollLeft == 0) {
                if (physiciansContainer.scrollLeft == 0) {
                    leftOverflow.classList.add('hidden');
                } else {
                    leftOverflow.classList.remove('hidden');
                }

                if (physiciansContainer.scrollLeft + physiciansContainer.offsetWidth >= physiciansContainerScrollWidth) {
                    rightOverflow.classList.add('hidden');
                } else {
                    rightOverflow.classList.remove('hidden');
                }
            } else {
                lastScrollLeft = physiciansContainer.scrollLeft;
            }
        }, 100);
    }

    setupServiceTileFormListeners() {
        const serviceTiles = document.querySelectorAll('form.tile-link');
        serviceTiles.forEach((serviceTile) => {
            serviceTile.addEventListener('submit', (e) => {
                e.preventDefault();
                const form = e.target;
                const formData = new FormData(form);
                const schedulingUrl = formData.get('scheduling_url');
                if (schedulingUrl) {
                    window.location.href = schedulingUrl;
                } else {
                    form.submit();
                }
            })
        })
    }
}