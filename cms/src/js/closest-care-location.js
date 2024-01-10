'use strict'

export class ClosestCareLocation {
    /**
     * constructor innitiates the class
     * 
     * @param {*} geoCoords - geoCoords provided by navigator.getCurrentPosition();
     * @param {*} map  - a mapboxgl.Map object
     */

    constructor(
        geoCoords,
        map
    ) 
    {
        if (geoCoords) {
            this.latitude = geoCoords.coords.latitude;
            this.longitude = geoCoords.coords.longitude;
        }

        if (map) {
            this.map = map;
        }
        
        this.findLocations()
    }
        
    findLocations() {
        // don't ask for permissions twice if coordinates already supplied
        if (this.latitude && this.longitude) {
            return this.fireAjaxEvent(this.latitude, this.longitude);    
        }

        if (!navigator.geolocation) {
            document.querySelector(".error").textContent = 'Geolocation is not supported by your browser';
            localStorage['authorizedGeoLocation'] = 0;
        } else {
            navigator.geolocation.getCurrentPosition(position => {
                localStorage['authorizedGeoLocation'] = 1;

                this.latitude = position.coords.latitude;
                this.longitude = position.coords.longitude;
                
                this.fireAjaxEvent(this.latitude, this.longitude);
            }, e => {
                // on any error, set authorizedGeoLocation flag to 0
                localStorage['authorizedGeoLocation'] = 0;
            });
        }
    }

    /**
     * Update the view with closest care location text
     */
    async fireAjaxEvent(latitude, longitude) {
        const url = '/locations/closest?latitude=' + latitude + "&longitude=" + longitude;        

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
                return Promise.reject(response)
            }
        }).then(async (data) => {
            document.querySelector('.dynamic-content').innerHTML = data;
            let immediateCareMiles = 0;
            let expressCareMiles = 0;

            if (document.querySelector('.all-locations')) {
                document.querySelector('.all-locations').classList.add('content-updated');
            }

            if (document.querySelector('.spinner-container')) {
                document.querySelector('.spinner-container').classList.add('hidden');
            }

            if (document.querySelector('#closest-immediate-care-center .miles .bolded')) {
                let distance = document.querySelector('#closest-immediate-care-center .miles .bolded').innerHTML;
                immediateCareMiles = parseFloat(distance);
            }

            if (document.querySelector('#closest-express-care .miles .bolded')) {
                let distance = document.querySelector('#closest-express-care .miles .bolded').innerHTML;
                expressCareMiles = parseFloat(distance);
            }

            if (immediateCareMiles <= expressCareMiles) {
                if (document.querySelector('#closest-express-care')) {
                    document.querySelector('#closest-express-care').classList.add('hidden');
                }
            } else {
                if (document.querySelector('#closest-immediate-care-center')) {
                    document.querySelector('#closest-immediate-care-center').classList.add('hidden');
                }
            }
        }).catch((response) => {
            if (document.querySelector('.fetch-error')) {
                let error = document.querySelector('.fetch-error');
                error.classList.remove('hidden');
                error.textContent = "Sorry, we weren’t able to proceed. Please try again."
            }
        });
    }
}