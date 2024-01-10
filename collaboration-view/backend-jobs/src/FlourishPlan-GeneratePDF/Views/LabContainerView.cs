using PDFGeneratorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF.Views
{
    public static class LabContainerView
    {
        public static string usedLabId = "";
        public static string destLocation = "";
        public static string CreateLabContainer(LabAndImagingOrders labsOrders)
        {
            string container = "";
            container += @" <div class='header display--flex align--items--center location--flex'>

        <svg style='margin-right:0.5rem;'  width='45' height='45' viewBox='0 0 104 104' fill='none' xmlns='http://www.w3.org/2000/svg'>
          <circle cx='52' cy='52' r='52' fill='#00A5DF' fill-opacity='0.3'/>
          <g clip-path='url(#clip0_26325_185541)'>
          <path fill-rule='evenodd' clip-rule='evenodd' d='M27.9018 71.0974L27.7491 71.3689C27.2566 72.2449 26.9982 73.2378 27 74.2479C27.0018 75.2581 27.2636 76.25 27.7592 77.1242C28.2548 77.9984 28.9668 78.7242 29.8237 79.2287C30.5156 79.6361 31.2824 79.8877 32.0727 79.9695L32.0816 79.9704C32.2672 79.9893 32.4541 79.9989 32.6416 79.9989H33.8651V79.9731L50.1608 79.9975V80L51 79.9987L51.8392 80V79.9975L68.1349 79.9731V79.9989H69.3583C69.5459 79.9989 69.7328 79.9893 69.9184 79.9704L69.9273 79.9695C70.7176 79.8877 71.4844 79.6361 72.1763 79.2287C73.0332 78.7242 73.7452 77.9984 74.2408 77.1242C74.7364 76.25 74.9982 75.2581 75 74.2479C75.0018 73.2378 74.7434 72.2449 74.2508 71.3689L74.0982 71.0974L60.3188 42.9251V30.575H61.2253C62.7329 30.575 63.955 29.327 63.955 27.7875C63.955 26.248 62.7329 25 61.2253 25H60.255H41.745H40.7747C39.2671 25 38.045 26.248 38.045 27.7875C38.045 29.327 39.2671 30.575 40.7747 30.575H41.6812V42.9251L27.9018 71.0974ZM46.5749 30.575V44.0234L41.9492 53.9182H60.0508L55.4251 44.0234V30.575H46.5749ZM56.6485 30.575V43.7405L61.4065 53.9182H61.4065L56.6486 43.7405V30.575H56.6485ZM62.6091 59.3906H39.3909L32.4631 74.21C32.4617 74.2192 32.461 74.2286 32.461 74.238C32.4611 74.2703 32.4694 74.3021 32.4853 74.3301C32.5012 74.3581 32.524 74.3813 32.5514 74.3975C32.5789 74.4136 32.61 74.4221 32.6416 74.4221L33.8651 74.4232H50.1608H51.8392H68.1349L69.3583 74.4221C69.39 74.4221 69.4211 74.4136 69.4486 74.3975C69.476 74.3813 69.4988 74.3581 69.5147 74.3301C69.5305 74.3021 69.5389 74.2703 69.539 74.238C69.539 74.2286 69.5383 74.2192 69.5369 74.21L62.6091 59.3906ZM69.0645 77.5576L69.3583 75.6725L69.3583 78.7495C69.3584 78.7495 69.3584 78.7495 69.3584 78.7495V75.6725C69.3584 75.6725 69.3584 75.6725 69.3583 75.6725H69.3582L69.0645 77.5576ZM69.999 75.5136C70.0195 75.5029 70.0397 75.4916 70.0597 75.4798C70.273 75.3543 70.4502 75.1736 70.5735 74.9561C70.6969 74.7385 70.762 74.4916 70.7625 74.2402C70.7628 74.0758 70.7354 73.9133 70.6821 73.7595L70.682 73.7595C70.7353 73.9133 70.7627 74.0758 70.7624 74.2402C70.7623 74.3016 70.7583 74.3627 70.7506 74.4232C70.7267 74.6104 70.6667 74.7916 70.5735 74.956C70.4501 75.1736 70.2729 75.3542 70.0597 75.4798C70.0397 75.4916 70.0195 75.5029 69.999 75.5136ZM73.1899 71.9911C73.1899 71.9911 73.1899 71.991 73.1899 71.991L73.1726 72.0011L73.1727 72.0012L73.1899 71.9911Z' fill='#00A5DF'/>
          </g>
          <defs>
          <clipPath id='clip0_26325_185541'>
          <rect width='55.4667' height='55.4667' fill='white' transform='translate(24.2666 24.2656)'/>
          </clipPath>
          </defs>
          </svg>
        
      <span class='patient-detail display--flex font--weight--600 font--size--20'>Labs ({Labs-Count})</span>

    </div> ".Replace("{Labs-Count}", Convert.ToString(labsOrders.TestOrder.Count));


            return container;
        }

        public static string CreateOtherLocationsLabs(List<ImagingLocationsViewModel> LabLocation, List<GetLabOrImaging> allScheduleLabLocations, List<GetLabOrImaging> allUnScheduleLabLocations)
        {
            var allOtherLocations = "";
            var labLinks = @"<div class='display--flex justify--space--between padded-white pb-16 bottom-radius page-break'>
                <div class='location-links'>
                    <div class='link-text'>For detailed list of Labs locations, please visit:</div>
                    <div class='link-details'>
                        <div class='link-icon'>
                            <img style='height: 25px; width: 20px;'
                                src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAB0AAAAhCAYAAAAlK6DZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAS0SURBVFhHvZdNbFRVGIbfudPpTH9mOhZQIVFJQCgRqvwkBGMkRW1EbLBB7YK40RhLQsLKFQtNZOGChSwEjZrowoVI1ZAmEuuizIKYBoyAP0gwQQPF1jaUdmbamc7M9X3vz/TO/4wJPsntzD1zznm/833f+c6pzySowY35HD4fX0RsKoPLszlMpnNIZU0E/T6sChrYEvVjR6cfL68M4IEWwxlVmaqi719P4eDlBYCiaPIBmo8f9sM/GqrRetgFiyaiHX680xXEwdVBNpSnrOjYTBbbz85ZkyAgManUSY5j0iba2gz8+GQY69pLV17ScvjKArZ/e8d+oesaEhTqHzKQoMHrh+/g6B8p54clClZ64NI8PqAo6ohLXWjqpIm3t7TirXVL7s6LHr2WwpsXkkCrI+jjp7tI9TAVtBq4MfailcezGH4qgj33NllNlug0Y7D86xmukB2UIBJMyy3ODHpfsYxW0wupUndZKP4ZEwEazaS2UO7F1a73eRPmwD1WuyW681wCsYlFu5eIdgD9O/lOyySYywLhCLD/DWDXbmDWiblLModDG0N475EWp8EmzaQKDnExjLGM2vdgM05ta4Uxy5fYX7TeFXTpiPKhZVE+ncsBvx84/i5w5hsmWMjpRDj+8KMtJYJigh7MQ/uHGEJhfHojDTTTkloYFJX4qc9ouZMUiiGLxJEujxGVUNgo/NWtRRix6QwndH6ohYRnbhf21z6uFwZ75J8MjF/nmJUNjINRZKHHgzWhzp+sboYq3P9JlnrGfc2NLLMxinNTtDOuxlaeEFaxvgtc51YqCB1fN0X8MHqWUZQZeDf44ib3vrd2U2fv/QEYL65qZiVhw1IJtslVWL7mqDMix37X/ndeND/HbebRZ6XiS2u571h08kivnRWo2BDRFACuXuVWobFV2Hw2bm8t7U/BhR1Yb+9nS/QjVhSdgXmRDC3Y0M1PuqeY1jYu4QjrNMe08LtKpRdOsXE0jp+mPGVV87JyHd9kVy1LtIMbfN8arlZuFmkW9t6+0hor/BSJzwKv7gVGTgOT45jkuCFeZ14YS8J38jZ+mWGDt8pRcHDDUtUqOE99X7LaBGmdXBJh0f/kGPD9sF2Di1HMk3Sh3P0hxedY2JU0zMu8S4VuEuxq9rOWO3jMobVPtFtHkIVW+foh4OnngakJWssa7Y2xJk6zrX8/v9OVWpnc6RUUrD5jPZzXQ8FKRc+5OEb/pnvcmqoVX/nZLvSXztvuFVn26RsAXhksHwaRyuG1tSF8/FjhCVQiKnyn6Sq1untMLlTiZJlgt27aCbZ6jZ1w8wm7TzE80Ffycjbey11QRFlR1YomJkT+JuFF56qQAZUoE0cvBTF10XXj4rO0UPEttkli1QTVn3Gc3sOwVKCsqOhm5fhuVxhIVKhM5ZAgb38XKdhZ5SCpKCqeWdGEE48z8xJVVuYiwYSJ87sj6A47IahAVVEx+FAzTuygMK+RJa52sVxqYrQ3jK30UC3KJlI5TrLiDMRYDFqLkstyac5yaa0VutQtKi6wvG07w/9xlNXaTsrSBRO/UbCrzP8slWhIVFxjYj08wtqrfcU0n+/rQMi9XdeLRP8Lz/0Qd741imn+C/aROR+PEQXYAAAAAElFTkSuQmCC' />
                        </div>
                        <a
                            href='https://www.dulyhealthandcare.com/services/laboratory-services' target='_blank'>https://www.dulyhealthandcare.com/services/laboratory-services</a>
                    </div>
                </div>
            </div>
        <div>";
            var count = 0;
            var number = 0;
            if (LabLocation.Count > 0)
            {
                allOtherLocations = @" <div class='page-break'><h4 class='other-location-title padded-white' style='color:  rgb(0, 40, 85);'>{Other-Locations}</h4> ".Replace("{Other-Locations}", "Other Locations :");

                allOtherLocations += @" <div class='page-break padded-white pb-16 location--grid'> ";
                if (usedLabId == "")
                {
                    usedLabId = "-1";
                }
                for (; count < LabLocation.Count; count++)
                {
                    if (number >= 4)
                    {
                        break;
                    }
                    if (Convert.ToInt32(LabLocation[count].Id) != Convert.ToInt32(usedLabId))
                    {
                        allOtherLocations += @" <div class='display--flex location--flex flex--margin'>
        <svg class='icon-css' width='19' height='28' viewBox='0 0 19 28' fill='none' xmlns='http://www.w3.org/2000/svg'>
          <path d='M9.5 0C4.26143 0 0 4.396 0 9.8C0 17.15 9.5 28 9.5 28C9.5 28 19 17.15 19 9.8C19 4.396 14.7386 0 9.5 0ZM14.9286 11.2H10.8571V15.4H8.14286V11.2H4.07143V8.4H8.14286V4.2H10.8571V8.4H14.9286V11.2Z' fill='#00A5DF'/>
          </svg>
        <div><span class='font--weight--600' style='color: rgb(0, 40, 85);'>{Location-Lab-Name}</span><br />
          <span class='font--size--14'>{Location-Address}</span><br />
          <span class='font--size--14 color-pink'>{Location-Distance} miles from you</span><br />
          <span class='font--size--14 font--weight--600' style='color: rgb(0, 40, 85);'>{Location-Mobile-Number}</span>
        </div>
      </div> ".Replace("{Location-Lab-Name}", LabLocation[count].Address.Parts.City).Replace("{Location-Address}", LabLocation[count].Address.Address).Replace("{Location-Distance}", Convert.ToString(Convert.ToInt32(Convert.ToDouble(LabLocation[count].Distance) * 0.000621371))).Replace("{Location-Mobile-Number}", "(630) 545­-7526");
                        number += 1;

                    }

                }
            }

            allOtherLocations += "</div>";
            allOtherLocations += labLinks;
            return allOtherLocations;
        }

        public static string CreateLabOrdersContainer(LabAndImagingOrders labsOrders, List<GetLabOrImaging> allScheduleLabLocations,
            int start, int end, bool isScheduled)
        {
            string labsDetails = "";

            if (isScheduled == true)
            {

                for (; start <= end; start++)
                {
                    var test = labsOrders.TestOrder[start];
                    labsDetails += @"<div class='font--weight--600 details-box page-break'>
                    <div class='display--flex align--items--center location--flex  location-item'
                        style='padding: 13px 22px 13px 17px;'>
                        <div class='icon'>
                            <svg class='icon-css' width='19' height='28' viewBox='0 0 19 28' fill='none'
                                xmlns='http://www.w3.org/2000/svg'>
                                <path
                                    d='M9.5 0C4.26143 0 0 4.396 0 9.8C0 17.15 9.5 28 9.5 28C9.5 28 19 17.15 19 9.8C19 4.396 14.7386 0 9.5 0ZM14.9286 11.2H10.8571V15.4H8.14286V11.2H4.07143V8.4H8.14286V4.2H10.8571V8.4H14.9286V11.2Z'
                                    fill='#00A5DF' />
                            </svg>
                        </div>
                        <div class='lab-location-details'>
                            <div class='lab-location-heading color-pink'>
                                Lab Location
                            </div>
                            <div class='lab-location-name family-inter' >
                                {Lab-Address}
                            </div>
                        </div>
                    </div>
                    <div class='display--flex details-info flex--column location--flex'
                        style='padding: 13px 22px 13px 17px;'>
                        <span class='details-info-title' style='text-align:left;'>APPOINTMENT TYPE</span>
                        <span class='font--size--14 font--weight-700' style='word-wrap:wrap-word;'>{Appointment-Type}, <span class='font--size--14 font--light'>Lab Test</span></span>
                    </div>
                </div>".Replace("{Lab-Address}", allScheduleLabLocations[0].Lab_Name).Replace("{Appointment-Type}", test.OrderName);
                }
                usedLabId = allScheduleLabLocations[0].Lab_ID;
                if(destLocation == "")
                {
                    destLocation = allScheduleLabLocations[0].Lab_Name.Replace(".", "").Replace(",", "").Replace(" ", "+").Replace(" ", "");
                }
            }

            else
            {
                for (; start <= end; start++)
                {
                    var test = labsOrders.TestOrder[start];
                    labsDetails += @"<div class='details-box page-break'>
                  <div class='display--flex align--items--center font--weight--600 font--size--14 pending-status-css color-pink'>{Pending-Status}</div>

                  <div class='display--flex details-info font--weight--600 pa--2 border-bottom page-break'>
                   
                  <div class='display--flex flex--column location--flex' style='width:50%;'>
                    <span class='details-info-title' style='text-align:left;'>APPOINTMENT TYPE</span>
                    <span class='font--size--14' style='word-wrap:wrap-word;'>{Appointment-Type}</span>
                    <span class='font--size--14 font--light'>Lab Test</span>
                  </div>
                </div>
              </div>".Replace("{Appointment-Type}", test.OrderName).Replace("{Pending-Status}", "Pending");

                }

            }
            Console.WriteLine("Labs Done");
            return labsDetails;
        }
    }
}

