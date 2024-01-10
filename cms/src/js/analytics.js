'use strict'

const Dimension_Active_Alert = 'Active Alert';
const Dimension_Class_Event_Location = 'Class/Event Location';
const Dimension_National_Physician_ID = 'National Physician ID';
const Dimension_Appointment_Service_Chosen = 'Appointment Service Chosen';
const Dimension_Patient_ID = 'Patient ID';
const Dimension_Patient_Type = 'Patient Type';
const Dimension_Scheduling_For = 'Scheduling For';
const Dimension_Epic_Physician_ID = 'Epic Physician ID';
const Dimension_Physician_Rating = 'Physician Rating';
const Dimension_Appointment_Type = 'Appointment Type';
const Dimension_New_Patient_Record = 'New Patient Record';
const Dimension_Schedule_Flow_Type = 'Schedule Flow Type';
const Dimension_Clearstep_Conversation_ID = 'Clearstep Conversation ID';
const Dimension_Service_ID = 'Service ID';
const Dimension_GA_Client_ID = 'GA Client ID';
const Dimension_Video_Visit_By_Provider_Or_By_Time = 'Video Visit By Provider or By Time';
const Dimension_Recommended_Provider_Flag = 'Recommended Provider Flag';

const Metric_Class_Registration_Complete_Count = 'Metric_Class_Registration_Complete_Count';
const Metric_Event_Registration_Complete_Count = 'Metric_Event_Registration_Complete_Count';
const Metric_Schedule_Appointment_Complete_Count = 'Metric_Schedule_Appointment_Complete_Count';
const Metric_New_Patient_Record_Complete_Count = 'Metric_New_Patient_Record_Complete_Count';
const Metric_Recommended_Provider_Appointment_Count = ' Metric_Recommended_Provider_Appointment_Count';

export class Analytics {

    constructor(loadEvents = true) {
        this.gaId = document.querySelector("meta[property='ga:id']").getAttribute('content');
        if (loadEvents == true) {
            // gtag 'config' triggers a pageview
            gtag('set', {
                'custom_map': {
                    'dimension1': Dimension_Active_Alert,
                    'dimension2': Dimension_Class_Event_Location,
                    'dimension3': Dimension_National_Physician_ID,
                    'dimension4': Dimension_Appointment_Service_Chosen,
                    'dimension5': Dimension_Patient_ID,
                    'dimension6': Dimension_Patient_Type,
                    'dimension7': Dimension_Scheduling_For,
                    'dimension8': Dimension_Epic_Physician_ID,
                    'dimension9': Dimension_Physician_Rating,
                    'dimension10': Dimension_Appointment_Type,
                    'dimension11': Dimension_New_Patient_Record,
                    'dimension12': Dimension_Schedule_Flow_Type,
                    'dimension13': Dimension_Clearstep_Conversation_ID,
                    'dimension14': Dimension_Service_ID,
                    'dimension15': Dimension_GA_Client_ID,
                    'dimension16': Dimension_Video_Visit_By_Provider_Or_By_Time,
                    'dimension17': Dimension_Recommended_Provider_Flag,
                    'metric1': Metric_Class_Registration_Complete_Count,
                    'metric2': Metric_Event_Registration_Complete_Count,
                    'metric3': Metric_Schedule_Appointment_Complete_Count,
                    'metric5': Metric_New_Patient_Record_Complete_Count,
                    'metric6': Metric_Recommended_Provider_Appointment_Count
                }
            });

            this.setupCustomData();

            // wait for the client_id to be available, and then send it with the pageview
            gtag('get', this.gaId, 'client_id', (client_id) => {
                this.customData[Dimension_GA_Client_ID] = client_id;
                gtag('config', this.gaId, this.customData);
            });

            this.events();

            this.newsletterSubscribeStart();
            this.newsletterSubscribeComplete();
            this.needCareCTAs();
        }
    }

    setupCustomData(category, action, label, cb, classEventId) {
        this.customData = {};

        this.setupCustomDataActiveAlert();
        this.setupCustomDataClassEventLocation(category, classEventId);
        this.setupCustomDataNationalPhysicianID();
        this.setupCustomDataAppointmentServiceChosen();
        this.setupCustomDataPatientID();
        this.setupCustomDataPatientType();
        this.setupCustomDataSchedulingFor();
        this.setupCustomDataEpicPhysicianID();
        this.setupCustomDataPhysicianRating();
        this.setupCustomDataAppointmentType();
        this.setupCustomDataNewPatientRecord();
        this.setupCustomDataScheduleFlowType();
        this.setupCustomDataClearstepConversationID();
        this.setupCustomDataServiceID();
        this.setupDulyNowByTimeOrByProvider();
        this.setupCustomDataRecommendedProviderFlag();
    }

    setupCustomDataActiveAlert() {
        // Process alerts on the page
        const defaultAlerts = document.querySelectorAll(".body-alert-container div.emergency-alert.default:not(.hide)");
        const locationAlerts = document.querySelectorAll(".body-alert-container div.emergency-alert.location:not(.hide)")
        const hasDefaultAlerts = defaultAlerts.length != 0;
        const hasLocationAlerts = locationAlerts.length != 0;

        if (hasDefaultAlerts && hasLocationAlerts) {
            this.customData[Dimension_Active_Alert] = 'Both';
        } else if (hasDefaultAlerts && !hasLocationAlerts) {
            this.customData[Dimension_Active_Alert] = 'Global';
        } else if (!hasDefaultAlerts && hasLocationAlerts) {
            this.customData[Dimension_Active_Alert] = 'Local';
        }
    }

    setupCustomDataClassEventLocation(category, classEventId) {
        // Classes custom event
        if (category == "Classes and Events") {
            if (typeof classEventId == "undefined") {
                classEventId = "Location Not Defined";
            }

            this.customData[Dimension_Class_Event_Location] = classEventId;
        }
    }

    setupCustomDataNationalPhysicianID() {
        if (document.querySelector('[name="national-physician-id"]')) {
            this.customData[Dimension_National_Physician_ID] = document.querySelector('[name="national-physician-id"]').value;
        }
    }

    setupCustomDataAppointmentServiceChosen() {
        if (document.querySelector('[name="appointment-service-chosen"]')) {
            this.customData[Dimension_Appointment_Service_Chosen] = document.querySelector('[name="appointment-service-chosen"]').value;
        }
    }

    setupCustomDataPatientID() {
        const eid = document.querySelector("#PatientIdentifier");
        if (eid != null) {
            this.customData.user_id = eid.getAttribute("data-eid");
            this.customData[Dimension_Patient_ID] = eid.getAttribute("data-eid");
        }
    }

    setupCustomDataPatientType() {
        if (document.querySelector('[name="scheduling-for"]')) {
            this.customData[Dimension_Scheduling_For] = document.querySelector('[name="scheduling-for"]').value;
        }
    }

    setupCustomDataSchedulingFor() {
        if (document.querySelector('[name="patient-type"]')) {
            this.customData[Dimension_Patient_Type] = document.querySelector('[name="patient-type"]').value;
        }
    }

    setupCustomDataNewPatientRecord() {
        if (document.querySelector('[name="new-patient-record"]')) {
            this.customData[Dimension_New_Patient_Record] = Dimension_New_Patient_Record;
            this.newPatientRecord = true;
        }
    }

    setupCustomDataEpicPhysicianID() {
        if (document.querySelector('[name="epic-physician-id"]')) {
            this.customData[Dimension_Epic_Physician_ID] = document.querySelector('[name="epic-physician-id"]').value;
        }
    }

    setupCustomDataPhysicianRating() {
        if (document.querySelector('[name="physician-rating"]')) {
            this.customData[Dimension_Physician_Rating] = document.querySelector('[name="physician-rating"]').value;
        } else {
            this.customData[Dimension_Physician_Rating] = "N/A";
        }
    }

    setupCustomDataAppointmentType() {
        if (document.querySelector('[name="appointment-type"]')) {
            this.customData[Dimension_Appointment_Type] = document.querySelector('[name="appointment-type"]').value;
        }
    }

    setupCustomDataScheduleFlowType() {
        if (document.querySelector('[name="schedule-flow-type"]')) {
            this.customData[Dimension_Schedule_Flow_Type] = document.querySelector('[name="schedule-flow-type"]').value;
        }
    }

    setupCustomDataClearstepConversationID() {
        if (document.querySelector('[name="clearstep-conversation-id"]')) {
            this.customData[Dimension_Clearstep_Conversation_ID] = document.querySelector('[name="clearstep-conversation-id"]').value;
        }
    }

    setupCustomDataServiceID() {
        if (document.querySelector('[name="service-id"]') && document.querySelector('[name="service-id"]').value) {
            this.customData[Dimension_Service_ID] = document.querySelector('[name="service-id"]').value;
        }
    }

    setupDulyNowByTimeOrByProvider() {
        if (document.querySelector('[name="duly-now-by-time-or-by-provider"]')) {
            this.customData[Dimension_Video_Visit_By_Provider_Or_By_Time] = document.querySelector('[name="duly-now-by-time-or-by-provider"]').value;
        }
    }

    setupCustomDataRecommendedProviderFlag() {
        const appointmentRecommendedPhysicianId = document.querySelector('[name="appointment_selected_recommended_physician_id"]');
        if (appointmentRecommendedPhysicianId && appointmentRecommendedPhysicianId.value) {
            this.userIsSchedulingWithRecommendedProvider = true;
            this.customData[Dimension_Recommended_Provider_Flag] = "Recommended Provider Chosen";
        }
    }

    newsletterSubscribeStart() {
        const subscribeButton = document.querySelector('.sign-up .newsletter');
        if (subscribeButton) {
            subscribeButton.addEventListener('click', _ => {
                this._sendEvent('Newsletter', 'Newsletter Subscribe Start');
            });
        }
    }

    newsletterSubscribeComplete() {
        const subscribeButton = document.querySelector('#mc_embed_signup #mc-embedded-subscribe');
        if (subscribeButton) {
            subscribeButton.addEventListener('click', _ => {
                this._sendEvent('Newsletter', 'Newsletter Subscribe Complete');
            });
        }
    }

    eventsSearch() {
        const clickEvents = ['click', 'keydown'];
        const button = document.querySelector('.event-listings .event-search-btn');
        const input = document.querySelector('.event-listings #search-events');

        clickEvents.forEach((event) => {
            [button, input].forEach(elem => {
                elem && elem.addEventListener(event, e => this.eventsSearchListener(e, event));
            });
        });
    }

    needCareCTAs() {
        // click events for the dropdown options
        document.querySelectorAll("#care-nav-mobile .care-dropdown-container-mobile .link a, #care-nav .care-dropdown-container .link a").forEach(element => {
            element.addEventListener('click', ({ target }) => {
                const link = target.closest('.link');
                if (!link) {
                    return;
                }
                const title = link.querySelector('.title');
                if (!title) {
                    return;
                }
                this._sendEvent('Clearstep', `Dropdown ${title.innerText} Click`);
            });
        });
    }

    clearStepImmediateCareCheckSymptomsEvent() {
        this._sendEvent('Clearstep', `Check Symptoms Immediate Care Page Click`);
    }

    keydownOrClick(event, e) {
        if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
            return true;
        } else {
            return false;
        }
    }

    events() {
        // Alerts
        const alerts = document.querySelectorAll('.body-alert-container div.emergency-alert:not(.hide):not(.immediate-care)');
        alerts.forEach((el) => {
            const closeBtn = el.querySelector("i.material-icons.close");
            if (closeBtn) {
                closeBtn.addEventListener('click', _ => {
                    this._sendEvent('Alert', 'Close Click', closeBtn.closest('.container').querySelector('.message').innerText.replace('View More', ''));
                });
            }

            const link = el.querySelector("a");
            if (link) {
                link.addEventListener('click', (e) => {
                    e.preventDefault();

                    this._sendEvent('Alert', 'Alert Click', el.querySelector('span.message').textContent, () => {
                        // do not want to reroute during scheduling flow
                        // if user confirms to reroute, ExitAlert.js will handle the redirect
                        if (!document.querySelector('.exit-alert-modal')) {
                            document.location = link.getAttribute("href");
                        }
                    });
                });
            }
        });

        // Analytics related events
        const eventEntry = document.querySelector(".event-entry");
        if (eventEntry) {
            const eventType = eventEntry.getAttribute("data-event-type");
            const eventTitle = eventEntry.querySelector(".event-entry .main-content .headline h1").textContent;
            const eventId = eventEntry.getAttribute("data-event-id");

            const eventRegisterBtns = document.querySelectorAll(".event-details button.register");
            for (let eventRegisterBtn of eventRegisterBtns) {
                eventRegisterBtn.addEventListener("click", (e) => {
                    this._sendEvent("Classes and Events", eventType + " Registration Start", eventTitle, () => { }, eventId);
                });
            }

            const phoneRegistrationLinks = document.querySelectorAll(".phone-registration a.phone-number");
            for (let phoneRegistrationLink of phoneRegistrationLinks) {
                phoneRegistrationLink.addEventListener("click", (e) => {
                    this._sendEvent("Classes and Events", eventType + " Register by Phone Click", eventTitle, () => { }, eventId);
                });
            }
        }

        const homepageHeroCtas = document.querySelectorAll(".hero-container .cta a");
        if (homepageHeroCtas) {
            homepageHeroCtas.forEach((cta) => {
                let ctaText = cta.innerText;
                let headerText = document.querySelector(".hero-container .text-container h1").innerText;
                let subheaderText = document.querySelector(".hero-container .text-container .subheadline").innerText;
  
                cta.addEventListener("click", e => {
                    e.preventDefault();
                    this._sendEvent("Homepage Hero CTA", ctaText + " Click", headerText + " : " + subheaderText);
                    window.location = cta.getAttribute('href');
                });
            })
        }

        const headerSearchForm = document.querySelector("#secondary-nav form.search");
        if (headerSearchForm) {
            headerSearchForm.addEventListener("submit", _ => {
                this._sendEvent("Search", "Header Search", "");
            });
        }

        const headerSearchFormMobile = document.querySelector(".search-form-mobile form");
        if (headerSearchFormMobile) {
            headerSearchFormMobile.addEventListener("submit", _ => {
                this._sendEvent("Search", "Header Search", "");
            });
        }

        const mainSearchForm = document.querySelector(".search-page .search-content form.site-wide-search");
        if (mainSearchForm) {
            mainSearchForm.addEventListener("submit", _ => {
                const href = new URL(window.location);
                let label = href.searchParams.get('section');
                label = label && label.length > 0 ? label : 'All';
                this._sendEvent("Search", "Site Search", label);
            });
        }

        const mainSearchFormButton = document.querySelector(".search-page .search-content .search-results-btn");
        if (mainSearchFormButton) {
            mainSearchFormButton.addEventListener("click", _ => {
                const href = new URL(window.location);
                let label = href.searchParams.get('section');
                label = label && label.length > 0 ? label : 'All';
                this._sendEvent("Search", "Site Search", label);
            });
        }

        // content share
        const facebookShare = document.querySelector('.article-contents .share-sheet .facebook');
        if (facebookShare) {
            facebookShare.addEventListener('click', _ => {
                this._sendEvent("Content Share", "Content Share Click", "Facebook");
            });
        }

        const twitterShare = document.querySelector('.article-contents .share-sheet .twitter');
        if (twitterShare) {
            twitterShare.addEventListener('click', _ => {
                this._sendEvent("Content Share", "Content Share Click", "Twitter");
            });
        }

        const emailShare = document.querySelector('.article-contents .share-sheet .mail');
        if (emailShare) {
            emailShare.addEventListener('click', _ => {
                this._sendEvent("Content Share", "Content Share Click", "Email");
            });
        }

    }

    EventRegisterCallback() {
        const eventEntry = document.querySelector(".event-entry");
        if (eventEntry) {
            const eventType = eventEntry.getAttribute("data-event-type");
            const eventTitle = eventEntry.querySelector(".event-entry .main-content .headline h1").textContent;
            const eventId = eventEntry.getAttribute("data-event-id");
            this._sendEvent("Classes and Events", eventType + " Registration Complete", eventTitle, () => { }, eventId);
        }
    }

    EventRegisterFailureCallback(response) {
        const eventEntry = document.querySelector(".event-entry");
        if (eventEntry) {
            const eventType = eventEntry.getAttribute("data-event-type");
            const eventTitle = eventEntry.querySelector(".event-entry .main-content .headline h1").textContent;
            const eventId = eventEntry.getAttribute("data-event-id");
            this._sendEvent("Classes and Events", eventType + " Registration Error", JSON.stringify(response), () => { }, eventId);
        }
    }

    PhysicianSearch(data, actionName) {
        let labels = [];
        for (let [k, v] of Object.entries(data)) {
            if (k != "order_by" && k != "latitude" && k != "longitude") {
                labels.push(k);
            }
        };
        let label = labels.join(",");

        if (typeof data.order_by == "object") {
            label = label + ";" + data.order_by;
        }

        this._sendEvent("Search", actionName, label);
    }

    LocationSearch(data) {
        let labels = [];
        for (let [k, v] of Object.entries(data)) {
            if (k != "order_by" && k != "latitude" && k != "longitude") {
                labels.push(k);
            }
        };
        let label = labels.join(",");

        if (typeof data.order_by == "object") {
            label = label + ";" + data.order_by;
        }

        // this._sendEvent("Search", "Locations Search", label);
    }

    EventsSearch(data) {
        let labels = [];
        for (let [k, v] of Object.entries(data)) {
            if (k != "order_by" && k != "latitude" && k != "longitude") {
                labels.push(k);
            }
        };
        let label = labels.join(",");

        if (typeof data.order_by == "object") {
            label = label + ";" + data.order_by;
        }

        this._sendEvent("Search", "Events Search", label);
    }

    /**
     * Sends a Google Analytics Event
     * @param string category 
     * @param string action 
     * @param string label
     * @param function cb
     * @param string cel
     */
    _sendEvent(category, action, label, cb, classEventId) {
        // Construct the event payload
        let payload = {
            'event_category': category,
            'event_label': label
        };

        this.setupCustomData(category, action, label, cb, classEventId);

        if (action == "Event Registration Complete") {
            this.customData[Metric_Event_Registration_Complete_Count] = "1";
        }

        if (action == "Class Registration Complete") {
            this.customData[Metric_Class_Registration_Complete_Count] = "1";
        }

        if (action == "Schedule Appointment Complete" || action == "Clearstep Appointment Complete" || action == "Duly Now Video Visit Complete") {
            this.customData[Metric_Schedule_Appointment_Complete_Count] = "1";
            // if a new patient was also created during this event, update new patient metric
            if (this.newPatientRecord) {
                this.customData[Metric_New_Patient_Record_Complete_Count] = "1";
            }
            if (this.userIsSchedulingWithRecommendedProvider) {
                this.customData[Metric_Recommended_Provider_Appointment_Count] = "1";
            }
        }

        // Add the gtag:event_callback fn if it is defined
        if (typeof cb != "undefined") {
            this.customData.event_callback = _ => cb();
        }

        if (gtag == undefined) {
            return;
        }

        // wait for the client_id to be available, and then send it with the event
        gtag('get', this.gaId, 'client_id', (client_id) => {
            this.customData[Dimension_GA_Client_ID] = client_id;
            Object.assign(payload, this.customData);
            gtag('event', action, payload);
        });
    }
}