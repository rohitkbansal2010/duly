'use strict';

import { MDCTextField } from '@material/textfield';
import { MDCFormField } from '@material/form-field';
import { MDCCheckbox } from '@material/checkbox';
import { MDCRadio } from '@material/radio';
import { MDCTextFieldHelperText } from '@material/textfield/helper-text';
import { MDCSelect } from '@material/select';

export class Forms {
    constructor() {
        this.handleSelects();

        const formFields = [].map.call(document.querySelectorAll('.mdc-form-field'), (el) => {
            return new MDCFormField(el);
        });

        const checkbox = [].map.call(document.querySelectorAll('.mdc-checkbox'), (el) => {
            return new MDCCheckbox(el);
        });

        const radio = [].map.call(document.querySelectorAll('.mdc-radio'), (el) => {
            return new MDCRadio(el);
        });

        const textField = [].map.call(document.querySelectorAll('.mdc-text-field'), (el) => {
            return new MDCTextField(el);
        });

        const helperText = [].map.call(document.querySelectorAll('.mdc-text-field-helper-text'), (el) => {
            return new MDCTextFieldHelperText(el);
        });

        document.querySelectorAll("input.mdc-text-field__input").forEach((el) => {
            ['change', 'blur'].forEach((event) => {
                el.addEventListener(event, (e) => {
                    if (el.checkValidity()) {
                        el.parentNode.classList.remove("mdc-text-field-error");
                        el.parentNode.classList.add("mdc-text-field-success");
                        const helper = el.parentNode.parentNode.querySelector(".mdc-text-field-helper-line");
                        if (helper != null) {
                            helper.remove();
                        }
                    } else {
                        el.parentNode.classList.add("mdc-text-field-error");
                        el.parentNode.classList.remove("mdc-text-field-success");
                    }
                });
            });
        });
    }

    handleSelects() {
        const _selects = document.querySelectorAll('.mdc-select');
        if (!_selects.length > 0) {
            return;
        }
        const selects = [..._selects].map((s) => new MDCSelect(s));
        let wasActive = false;
        selects.forEach((s) => {
            s.listen('MDCSelect:change', (e) => {
                if (e.detail.value) {
                    s.root.classList.add('mdc-select-success');
                } else {
                    s.root.classList.remove('mdc-select-success');
                }
            });
            const observer = new MutationObserver((mutations) => {
                mutations.forEach((mutation) => {
                    if (mutation.attributeName !== 'class') {
                        return;
                    }
                    const { target } = mutation;
                    const label = target.querySelector('.mdc-floating-label');
                    if (!label) {
                        return;
                    }
                    const isActive = target.classList.contains('mdc-select--activated');
                    if (isActive !== wasActive) {
                        if (!isActive && label.classList.contains('mdc-floating-label--float-above') && !s.root.classList.contains('mdc-select-success')) {
                            // console.log('moving label down...');
                            label.classList.remove('mdc-floating-label--float-above');
                        } else if (isActive && !label.classList.contains('mdc-floating-label--float-above')) {
                            // console.log('moving label up...');
                            label.classList.add('mdc-floating-label--float-above');
                        }            
                    }
                    wasActive = isActive;
                })
            });
            const options = {
                attributes: true,
                attributeFilter: ['class']
            }
            observer.observe(s.root, options);
        })
    }
}