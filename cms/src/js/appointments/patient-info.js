'use strict';
import { MDCSelect } from '@material/select';
import { AsYouType } from 'libphonenumber-js';
import { InitializeMap, AddMarkers } from './../mapbox';
import moment from 'moment';

export class PatientInfo {
	constructor() {
		this.container = document.querySelector(".patient-information-page");
		if (!this.container) {
			return;
		}

		this.genderSelected = false;
		this.reasonsRequired = false;
		this.setupGenderClickHandlers();
		this.inputFieldHandlers();
		this.editFieldHandler();
		this.phoneHandler();
		this.submitButton();
		this.setupInsuranceSelectionClickHandlers();

		// not all services will have optional reasons for visit
		if (document.querySelector('.reasons')) {
			this.reasonsRequired = true;
			this.visitReasonRadioButtons();
		}

		this.map = InitializeMap(
			document.querySelector(".map"),
			[-88.0084115, 41.8293935],
			(e) => {
			},
			'#map.mapboxgl-map'
		);

		const mapContainer = document.querySelector(".map-container")
		if (mapContainer) {
			mapContainer.classList.remove("hidden");
		}

		if (document.querySelector('.populate-marker')) {
			this.generateLocations();
		}

		this.monthField = document.querySelector(".mdc-text-field__input[name='date_of_birth_month']");
		this.dayField = document.querySelector(".mdc-text-field__input[name='date_of_birth_day']");
		this.yearField = document.querySelector(".mdc-text-field__input[name='date_of_birth_year']");

		if (this.monthField) {
			this.setupDobFieldListeners();
		}

		this.insurancePickers = document.querySelector('.insurance-pickers-container');
		
		if (this.insurancePickers) {
			this.selectedInsuranceProviderIndexField = document.querySelector("input[name='appointment_insurance_provider_id']");
			this.selectedInsurancePlanIndexField = document.querySelector("input[name='appointment_insurance_plan_id']");
			this.setupInsuranceSelectionClickHandlers();
		}
	}

	submitButton() {
		const formSubmitBtn = document.querySelector('#patient-schedule-appointment');
		const form = document.querySelector('#patient-information-booking-form');

		form.addEventListener('submit', (e) => {
			// prevent double clicking of button resulting in booking a time that is already submitted
			formSubmitBtn.disabled = true;
			formSubmitBtn.querySelector('.fa-spin').classList.remove('hidden');
			formSubmitBtn.querySelector('span').innerHTML = '';
		})
	}

	editFieldHandler() {
		if (document.querySelector('.prepopulate-form')) {
			const populatedForm = document.querySelector('.prepopulate-form');
			const editBtn = populatedForm.querySelector('#edit-patient-details');
			const editForm = document.querySelector('.edit-input-container');

			// only fires if form needs to be pre-populated
			this.prepopulateFieldAnimations();

			if (document.querySelector('.mdc-text-field-error')) {
				// don't show the prepopulated form if validation errors are still occurring
				populatedForm.classList.add('hidden');
				editForm.classList.remove('hidden');
			}

			['click', 'keydown'].forEach(event => {
				editBtn.addEventListener(event, (e) => {
					e.preventDefault();
					e.stopPropagation();
					window.scrollTo(0, 0);
					populatedForm.classList.add('hidden');
					editForm.classList.remove('hidden');
				});
			});
		}
	}

	prepopulateFieldAnimations() {
		const inputFields = document.querySelectorAll('.mdc-text-field__input');

		for (let i = 0; i < inputFields.length; i++) {
			let el = inputFields[i];

			if (el.checkValidity()) {
				el.parentNode.classList.remove("mdc-text-field-error");
				el.parentNode.classList.add("mdc-text-field-success");
			} else {
				el.parentNode.classList.add("mdc-text-field-error");
				el.parentNode.classList.remove("mdc-text-field-success");
			}
		};
	}

	generateLocations() {
		let location = document.querySelector('.populate-marker');
		let lat = location.getAttribute('data-latitude');
		let lng = location.getAttribute('data-longitude');
		let address = location.getAttribute('data-address');

		let markers = {
			type: 'FeatureCollection',
			features: [{
				type: 'Feature',
				geometry: {
					type: 'Point',
					coordinates: [lng, lat]
				},
				properties: {
					name: '',
					address: address
				}
			}]
		};

		this.resetMapMarkers(markers);
	}

	setupGenderClickHandlers() {
		const genderForm = document.querySelector("#patient-gender");
		const stateForm = document.querySelector("#patient-state");
		const genderSelect = new MDCSelect(genderForm);
		this.genderSelected = genderSelect.value ? true : false;
		const stateSelect = new MDCSelect(stateForm);
		this.stateSelected = stateSelect.value ? true : false;
		const genderSelectText = genderForm.querySelector('.mdc-select__selected-text');
		const stateSelectText = stateForm.querySelector('.mdc-select__selected-text');

		genderSelect.listen("MDCSelect:change", e => {
			genderForm.classList.add('choice-selected');
			this.genderSelected = true;
			genderForm.querySelector('.mdc-select__selected-text').classList.remove("mdc-text-field-error");
			this.validateForm();
		})
    
		// if no select option was chosen on blur, show inline error
		genderSelectText.addEventListener('blur', ({target}) => {
			target.closest('.mdc-select__anchor').classList.add("mdc-text-field-error");
		});
    
		// if no select option was chosen on blur, show inline error
		stateSelectText.addEventListener('blur', ({target}) => {
			target.closest('.mdc-select__anchor').classList.add("mdc-text-field-error");
		});

		stateSelect.listen("MDCSelect:change", e => {
			stateForm.classList.add('choice-selected');
			this.stateSelected = true;
			stateForm.querySelector('.mdc-select__selected-text').classList.remove("mdc-text-field-error");
			this.validateForm();
		})
	}

	setupDobFieldListeners() {
		this.monthField.addEventListener("keyup", ({ target }) => {
			target.value = target.value.replace(/\D/g, '');
			this.validateForm();

			// skip to the next field
			if (this.monthField.value.length == 2) {
				this.dayField.select();
			}
		});

		this.dayField.addEventListener("keyup", ({ target }) => {
			target.value = target.value.replace(/\D/g, '');
			this.validateForm();

			// skip to the next field
			if (this.dayField.value.length == 2) {
				this.yearField.select();
			}
		});

		this.yearField.addEventListener("keyup", ({ target }) => {
			target.value = target.value.replace(/\D/g, '');
			this.validateForm();
		});

		[this.monthField, this.dayField, this.yearField].forEach(field => field.addEventListener("blur", ({ target }) => {
			target.dataset.modified = "1";
			this.validateForm();
		}));
	}

	setupInsuranceSelectionClickHandlers() {
		const _insuranceProviders = document.querySelector("#insurance-providers");
        const _insurancePlans = document.querySelector("#insurance-plans");

        if (!_insuranceProviders || !_insurancePlans) {
            return;
        }

        const insuranceProviders = new MDCSelect(_insuranceProviders);
		const insurancePlans = new MDCSelect(_insurancePlans);
		
		insurancePlans.disabled = true;

		insuranceProviders.listen("MDCSelect:change", e => {
			this.selectedInsuranceProviderIndex = parseInt(e.detail.value);
			const pickedProvider = document.querySelectorAll("#insurance-providers .mdc-list li")[e.detail.index];

            // reset plan selection
            insurancePlans.selectedIndex = -1;
            insurancePlans.value = '';

            if (pickedProvider.classList.contains("invalid-option")) {
                // disable plan picker until avalid provider is selected
                insurancePlans.disabled = true;
                insurancePlans.valid = true;
            } else {
                // enable plan picker if a avalid provider is selected
                insurancePlans.disabled = false;

                // hide plans not belonging to this provider
                // but keep invalid options (default option, "cash" options, etc.)
                document.querySelectorAll("#insurance-plans .mdc-list .mdc-list-item:not(.invalid-option)").forEach(plan => {
                    if (plan.dataset.providerId !== this.selectedInsuranceProviderIndexField.value) {
                        plan.classList.add("hidden");
                        plan.parentNode.appendChild(plan);
                    } else {
                        plan.classList.remove("hidden");
                    }
                });

                let validPlans = document.querySelectorAll("#insurance-plans .mdc-list .mdc-list-item:not(.invalid-option):not(.hidden)");
                Array.from(validPlans).sort((a, b) => a.innerText > b.innerText ? 1 : -1).forEach(node => {
                    let invalidOptions = node.parentNode.querySelectorAll(".invalid-option");
                    let lastOption = node.parentNode.querySelector('.last-option');
                    node.parentNode.insertBefore(node, invalidOptions[invalidOptions.length - 1].nextSibling);
                    node.parentNode.insertBefore(lastOption, node.nextSibling);
                });
            }
			this.validateForm();
		});
		
		insurancePlans.listen("MDCSelect:change", e => {
			this.selectedInsurancePlanIndex = parseInt(e.detail.value);
            this.validateForm();
        });
	}

	phoneHandler() {
		const phoneNumberField = document.querySelector('[name=phoneNumber]');
		['keyup', 'focus', 'blur', 'paste'].forEach((event) => {
			phoneNumberField.addEventListener(event, (e) => {
				// remove leading digit "1" (EPIC POST schedule request requires a 10-digit number without the leading "1")
				e.target.value = e.target.value.replace(/^1/,'');
				const formattedNumber = new AsYouType('US').input(e.target.value);
				e.target.value = formattedNumber;
			});
		});
	}

	resetMapMarkers(markers) {
		this.markers = this.markers || [];
		this.markers.forEach(marker => marker.remove());
		if (markers && markers.features.length > 0) {
			this.markers = AddMarkers(markers, this.map);
		}
		this.map.resize();
	}

	validateForm() {
		if (this.monthField) {
			let validDate = parseInt(this.yearField.value) > new Date().getFullYear() - 200 && moment(`${this.monthField.value.padStart(2, '0')}/${this.dayField.value.padStart(2, '0')}/${this.yearField.value.padStart(4, '0')}`, 'MM/DD/YYYY', true).isValid();
			if (this.monthField.dataset.modified === "1" && this.dayField.dataset.modified === "1" && this.yearField.dataset.modified === "1" && !validDate) {
				this.monthField.closest(".mdc-text-field").classList.add("mdc-text-field-error");
				this.dayField.closest(".mdc-text-field").classList.add("mdc-text-field-error");
				this.yearField.closest(".mdc-text-field").classList.add("mdc-text-field-error");
				this.monthField.closest(".mdc-text-field").classList.remove("mdc-text-field-success");
				this.dayField.closest(".mdc-text-field").classList.remove("mdc-text-field-success");
				this.yearField.closest(".mdc-text-field").classList.remove("mdc-text-field-success");
			} else {
				this.monthField.closest(".mdc-text-field").classList.remove("mdc-text-field-error");
				this.dayField.closest(".mdc-text-field").classList.remove("mdc-text-field-error");
				this.yearField.closest(".mdc-text-field").classList.remove("mdc-text-field-error");
				this.monthField.closest(".mdc-text-field").classList.add("mdc-text-field-success");
				this.dayField.closest(".mdc-text-field").classList.add("mdc-text-field-success");
				this.yearField.closest(".mdc-text-field").classList.add("mdc-text-field-success");
			}
		}

		const nodeList = document.querySelectorAll('.mdc-text-field__input');

		let required = 0;
		let valid = 0;

		for (let i = 0; i < nodeList.length; i++) {
			let parent = nodeList[i].parentNode;
			if (nodeList[i].required) {
				required += 1;
				if (parent.classList.contains('mdc-text-field-success')) {
					valid += 1;
				}
			}
		}

		if (this.insurancePickers)  {
			if (document.querySelector("#insurance-providers").classList.contains("mdc-select--required")) {
				required += 1;
			}
			if (document.querySelector("#insurance-plans").classList.contains("mdc-select--required")) {
				required += 1;
			}

			if (this.selectedInsuranceProviderIndex) {
				if (this.selectedInsuranceProviderIndex === 'self-pay' || this.selectedInsuranceProviderIndex === 'no-provider') {
					valid += 2;
				} else {
					valid += this.selectedInsurancePlanIndex ? 2 : 1;
				}
			}
		}

		if (required <= valid && !document.querySelector('.mdc-text-field--invalid') && this.genderSelected && this.stateSelected && !this.reasonsRequired) {
			document.querySelector('#patient-schedule-appointment').disabled = false;
			return true;
		}

		document.querySelector('#patient-schedule-appointment').disabled = true;
		return false;
	}

	inputFieldHandlers() {
		const inputFields = document.querySelectorAll('.mdc-text-field__input');

		inputFields.forEach(element => {
			element.addEventListener('blur', (e) => {
				this.validateForm();
			})
		});
	}

	visitReasonRadioButtons() {
		const container = document.querySelector('.reasons');
		const textarea = document.querySelector('#additional-details-provided-text');
		const radioBtns = container.querySelectorAll('input[type=radio]');
		const characterCountContainer = document.querySelector('#character-count-container');
		let charactersLeft = document.querySelector('#characters-left');
		const characterLimit = parseInt(characterCountContainer.dataset.limit);

		radioBtns.forEach(element => {
			element.addEventListener('change', (e) => {
				textarea.parentNode.parentNode.classList.remove('hidden');
				this.reasonsRequired = false;
				this.validateForm();
				characterCountContainer.classList.remove('hidden');
			});
		});

		textarea.addEventListener('keyup', ({target}) => {
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

	keydownOrClick(event, e) {
		if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
			return true;
		} else {
			return false;
		}
	}
}