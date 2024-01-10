"use strict";

import { SelectAppointmentSplitView } from './_select-appointment/select-appointment-split-view';
import { MDCSelect } from '@material/select';
import { initLottie } from '../utils';
import lottie from 'lottie-web';

class VideoVisit {
    constructor() {
        this.container = document.querySelector('section.videoVisitContainer');
        this.breadcrumb = document.getElementById('breadcrumb');
        this.mobileBreadcrumb = document.getElementById('breadcrumb-mobile');
        if (!this.container) {
            return;
        }
        this.cta = document.getElementById('cta');
        this.ctaButton = this.cta ? this.cta.querySelector('button') : null;
        this.loadingSpinner = document.querySelector('.loading-spinner');
        if (this.container.classList.contains('step-1')) {
            this.handleStepOne();
        }

        if (this.container.classList.contains('step-2')) {
            this.setupCharactersLeftCounter();
        }
    }

    showLoadingSpinner() {
        this.container.classList.add('hidden');
        this.breadcrumb.classList.add('hidden');
        this.mobileBreadcrumb.classList.add('hidden');
        this.loadingSpinner.classList.contains('hidden') && this.loadingSpinner.classList.remove('hidden');
    }


    handleStepOne() {
        const form = document.getElementById('ageLocationPrivacy');
        if (form) {
            const adultChildRadios = document.querySelectorAll('input[name="is_adult"]');
            adultChildRadios.forEach((radio) => {
                radio.addEventListener("change", () => {
                    document
                        .querySelector("#confirmLocationContainer")
                        .scrollIntoView({ behavior: 'smooth', block: 'center' });
                });
            });
            const confirmLocationCheckbox = document.querySelector('#is_in_illinois');
            confirmLocationCheckbox.addEventListener('change', () => {
                document
                    .querySelector("#privacyPracticesNoticeContainer")
                    .scrollIntoView({ behavior: 'smooth', block: 'center' });
            })
            form.addEventListener('change', () => {
                if (form.checkValidity()) {
                    setTimeout(() => {
                        this.showLoadingSpinner();
                        form.submit();
                    }, 200)
                }
            })
        } else {
            const serviceTiles = document.querySelectorAll('form.tile-link');
            const serviceConditionCopyHolders = document.querySelectorAll('.serviceTileConditionCopy');
            const visitReasonContainer = document.getElementById('reasonForVisit');
            const visitReason = new MDCSelect(document.getElementById('appointment_reason_for_visit_id'));
            const inClinic = document.getElementById('inClinic');
            const inClinicSubmit = document.querySelector('#inClinic a');
            inClinicSubmit.addEventListener('click', () => {
                this.selectedServiceIds.forEach((id) => {
                    const input = document.createElement('input');
                    input.type = 'hidden';
                    input.name = 'appointmentServiceIds[]';
                    input.value = id;
                    inClinic.appendChild(input);
                })
                inClinic.submit();
            })
            visitReason.listen('MDCSelect:change', (e) => {
                if (e.detail.value == "") {
                    return;
                }

                if (e.detail.value == "other") {
                    inClinic.classList.remove('hidden');
                    this.calendarWidgetRendered = false;
                    document.querySelector("#datePickerContainer").classList.add('_hidden');
                } else {
                    inClinic.classList.add('hidden');
                    if (!this.calendarWidgetRendered) {
                        this.calendarWidgetRendered = true;
                        const selectedServiceId = this.selectedServiceIds.length ? this.selectedServiceIds[0] : null;
                        const physicianId = this.formData.get('appointment_physician_id');
                        lottie.destroy();
                        const oldElement = document.querySelector('section.select-appointment-content');
                        const newElement = oldElement.cloneNode(true);
                        document.querySelector("#datePickerContainer").replaceChild(newElement, oldElement);
                        initLottie();
                        this.selectAppointment = new SelectAppointmentSplitView(physicianId, 7, selectedServiceId, true);
                        document.querySelector("#datePickerContainer").classList.remove('inactive', '_hidden');
                        setTimeout(() => {
                            document.querySelector("#datePickerContainer").scrollIntoView({ behavior: 'smooth', block: 'center' });

                            // update the appointment service ID fields
                            const form = document.querySelector("#select-appointment-form");
                            form.querySelectorAll("input[name='appointmentServiceIds[]']").forEach(input => {
                                input.remove();
                            });
                            document.querySelectorAll(".tile-link.selected input[name='appointmentServiceIds[]']").forEach(input => {
                                form.appendChild(input.cloneNode());
                            });
                        }, 250);
                    }
                }
            })
            const setActiveService = (service) => {
                this.selectedServiceTile = service;
                serviceTiles.forEach((tile) => {
                    tile.id === service.id ? tile.classList.add('selected') : tile.classList.remove('selected');
                });
                serviceConditionCopyHolders.forEach((holder) => {
                    holder.dataset.servicetile === service.id ? holder.classList.remove('hidden') : holder.classList.add('hidden');
                })
            }
            const updateReasonsForVisit = (tileId) => {
                visitReason.selectedIndex = -1;
                const options = visitReason.menu.list.root.children;
                ;[...options].forEach((option) => {
                    if (!option.dataset.tile_id) {
                        return;
                    }
                    tileId === option.dataset.tile_id ? option.classList.remove('hidden') : option.classList.add('hidden');
                });
            }
            const handleServiceSelection = async (e, service) => {
                e.preventDefault();
                inClinic.classList.add('hidden');
                setActiveService(service);
                this.calendarWidgetRendered = false;
                document.querySelector("#datePickerContainer").classList.add('inactive');
                document.querySelector("#datePickerContainer").classList.remove('_hidden');
                this.formData = new FormData(service);
                this.selectedServiceIds = this.formData.getAll('appointmentServiceIds[]').sort();
                const tileId = this.formData.get('chosen_video_visit_group');
                updateReasonsForVisit(tileId);
                visitReasonContainer.classList.remove('inactive');
                visitReasonContainer.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
            serviceTiles.forEach((service) => {
                service.addEventListener('submit', (e) => handleServiceSelection(e, service));
            })
        }
    }

    setupCharactersLeftCounter() {
        const textarea = document.querySelector('#additional-details-provided-text');
        const characterCountContainer = document.querySelector('#character-count-container');
        let charactersLeft = document.querySelector('#characters-left');
        const characterLimit = parseInt(characterCountContainer.dataset.limit);

        textarea.addEventListener('keyup', ({ target }) => {
            if (parseInt(charactersLeft.innerText) <= 0) {
                target.value = target.value.substr(0, characterLimit);
            }

            charactersLeft.innerText = characterLimit - target.value.length;

            if (parseInt(charactersLeft.innerText) <= 50) {
                characterCountContainer.classList.add('warning');
            } else {
                characterCountContainer.classList.remove('warning');
            }
        });
    }
}

export { VideoVisit }