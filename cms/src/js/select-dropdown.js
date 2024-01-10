'use strict';

export class SelectDropdown {
    constructor() {
        if (document.querySelector('.custom-select')) {
            this.createOptionValues();
        }
    }

    createOptionValues() {
        let select = document.querySelector('.navigation-select');
        let ul = document.querySelector('nav#health-topics-navigation-bar')
        let categories = ul.querySelectorAll('li');

        for (let i = 0; i < categories.length; i++) {
            // default category should not be included
            if (categories[i].querySelector('a')) {
                let option = document.createElement('option');
                let text = categories[i].querySelector('a').text;
                let url = categories[i].querySelector('a').href;
                if (categories[i].classList.contains('active')) {
                    option.selected = true;
                }
                option.innerHTML = text;
                option.value = url;
                select.appendChild(option);
            }
        }

        this.addCustomSelect();
    }

    closeAllSelect(elmnt) {
        /* A function that will close all select boxes in the document,
        except the current select box: */
        var x, y, i, arrNo = [];
        x = document.getElementsByClassName("select-items");
        y = document.getElementsByClassName("select-selected");
        for (i = 0; i < y.length; i++) {
            if (elmnt == y[i]) {
                arrNo.push(i)
            } else {
                y[i].classList.remove("select-arrow-active");
            }
        }
        for (i = 0; i < x.length; i++) {
            if (arrNo.indexOf(i)) {
                x[i].classList.add("select-hide");
            }
        }
    }

    addEventListenerForClose(elmnt) {
        let self = this;
        let events = ["click", "keydown"];

        events.forEach((event) => {
            elmnt.addEventListener(event, function(e) {
                if ((event === 'keydown' && e.keyCode === 13) || event === "click") {
                    /* When the select box is clicked, close any other select boxes,
                    and open/close the current select box: */
                    e.stopPropagation();
                    self.closeAllSelect(this);
                    this.nextSibling.classList.toggle("select-hide");
                    this.classList.toggle("select-arrow-active");
                }
            });
        });

        // close dropdown if click event happens outside of the opened dropdown container
        document.addEventListener("click", function() {
            let dropdownOpen = document.querySelector('.select-arrow-active') !== null;
            if (dropdownOpen) {
                self.closeAllSelect(this);
                this.nextSibling.classList.toggle("select-hide");
                this.classList.toggle("select-arrow-active");
            }
        })
    }

    addCustomSelect() {
        let x, i, j, selElmnt, a, b, c;

        /* Look for any elements with the class "custom-select": */
        x = document.getElementsByClassName("custom-select");

        for (i = 0; i < x.length; i++) {
            selElmnt = x[i].getElementsByTagName("select")[0];

            /* For each element, create a new DIV that will act as the selected item: */
            a = document.createElement("DIV");

            if (selElmnt.options[selElmnt.selectedIndex].innerHTML == 'Health Topics: A - Z') {
                a.setAttribute("class", "select-selected transparent-bg");
                a.setAttribute("tabindex", 0);
            } else {
                a.setAttribute("class", "select-selected");
                a.setAttribute("tabindex", 0);

            }
            a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
            x[i].appendChild(a);

            /* For each element, create a new DIV that will contain the option list: */
            b = document.createElement("DIV");
            b.setAttribute("class", "select-items select-hide");
    
            for (j = 0; j < selElmnt.length; j++) {
                /* For each option in the original select element,
                create a new DIV that will act as an option item: */
                c = document.createElement("DIV");
                c.innerHTML = selElmnt.options[j].innerHTML;

                if (selElmnt.options[j].value === 'default-select-placeholder') {
                    c.setAttribute('class', 'hide');
                } else {
                    c.setAttribute('data-url-label', selElmnt.options[j].value);
                    c.setAttribute('tabindex', 0);
                }
                
                c.addEventListener("keydown", function(e) {
                    // 'enter' event for tab index
                    if (e.keyCode === 13) {
                        window.location.href = c.getAttribute('data-url-label');
                    }
                })

                c.addEventListener("click", function (e) {
                    /* When an item is clicked, update the original select box,
                    and the selected item: */
                    var y, i, k, s, h;
                    s = this.parentNode.parentNode.getElementsByTagName("select")[0];
                    h = this.parentNode.previousSibling;
                    for (i = 0; i < s.length; i++) {
                        if (s.options[i].innerHTML == this.innerHTML) {
                            s.selectedIndex = i;
                            h.innerHTML = this.innerHTML;
                            y = this.parentNode.getElementsByClassName("same-as-selected");
                            for (k = 0; k < y.length; k++) {
                                y[k].removeAttribute("class");
                            }
                            this.setAttribute("class", "same-as-selected");
                            
                            if (!(this.getAttribute('data-url-label') === 'default-select-placeholder')) {
                                window.location.href = this.getAttribute('data-url-label');
                            }
                            break;
                        }
                    }
                    h.click();
                });
                b.appendChild(c);
            }
            x[i].appendChild(b);

            this.addEventListenerForClose(a);
        }
    }
}
