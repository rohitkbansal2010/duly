/**
 * punchkick/dupage-core-module module for Craft CMS
 *
 * punchkick/dupage-core-module JS
 *
 * @author    Punchkick Interactive
 * @copyright Copyright (c) 2019 Punchkick Interactive
 * @link      https://www.punchkick.com
 * @package   DupageCoreModule
 * @since     0.0.1
 */

'use strict';

// styles
import '../scss/EventRegistration.scss';

const generateDownloadButton = _ => {
    filtersActionElement.innerHTML = "";
    let newActionNode = downloadActionHTML.content.cloneNode(true);

    if (currentMinDate !== "") {
        newActionNode.querySelector('input[name="minDate"]').value = currentMinDate;
    }
    if (currentMaxDate !== "") {
        newActionNode.querySelector('input[name="maxDate"]').value = currentMaxDate;
    }

    filtersActionElement.appendChild(newActionNode);
}

const refreshEventRegistrationsList = _ => {

    // update href query params
    var searchParams = new URLSearchParams(href);

    if (currentMinDate == "") {
        searchParams.delete('minDate');
    } else {
        searchParams.set('minDate', currentMinDate);
    }

    if (currentMaxDate == "") {
        searchParams.delete('maxDate');
    } else {
        searchParams.set('maxDate', currentMaxDate);
    }

    if (searchParams.has('page')) {
        searchParams.set('page', 1);
    }

    href = decodeURIComponent(searchParams.toString());

    // update the view to indicate the data is being loaded
    listOfRegistrationsElement.innerHTML = "";
    filtersActionElement.innerHTML = "";
    filtersActionElement.appendChild(spinnerActionHTML.content.cloneNode(true));

    fetch(href, {
        method: 'GET',
        mode: 'same-origin',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            'Accept': 'text/html'
        }
    }).then(async (response) => {
        const text = await response.text();
        return response.ok ? text : Promise.reject(text);
    }).then((response) => {
        listOfRegistrationsElement.innerHTML = response;
        if (currentMinDate !== "" || currentMaxDate !== "") {
            // show new download link
            generateDownloadButton();
        } else {
            // if filters are removed, remove the download link
            filtersActionElement.innerHTML = "";
        }
    }).catch((response) => {
        filtersActionElement.innerHTML = "";
    });
};

const spinnerActionHTML = document.querySelector('#filter-action-loading');
const downloadActionHTML = document.querySelector('#filter-action-download');

let currentMinDate = "";
let currentMaxDate = "";

let href = location.href.indexOf('?') < 0 ? `${location.href}?ajax=true` : `${location.href}&ajax=true`;

const listOfRegistrationsElement = document.querySelector("#event-registrants-by-location");
const filtersActionElement = document.querySelector(".filters .action");

var searchParams = new URLSearchParams(href);

if (searchParams.has('minDate')) {
    currentMinDate = searchParams.get('minDate');
    document.querySelector('input[id^="min-date"]').value = currentMinDate;
    generateDownloadButton();
}
if (searchParams.has('maxDate')) {
    currentMaxDate = searchParams.get('maxDate');
    document.querySelector('input[id^="max-date"]').value = currentMaxDate;
    generateDownloadButton();
}

var filters = document.querySelectorAll(".filters .event-registrations-filter");

// event listeners for date filter value changes
Array.from(filters).forEach(filter => {
    filter.addEventListener('focusout', ({ target }) => {
        // The input field value does not change right on focusout.
        // Wait a moment for the value to update.
        setTimeout(() => {
            if (target.id.match(/min-date.*-date/)) {
                currentMinDate = target.value;
            } else if (target.id.match(/max-date.*-date/)) {
                currentMaxDate = target.value;
            }
            refreshEventRegistrationsList();
        }, 250);
    });
});