using PDFGeneratorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.UI.Flourish.GeneratePDF.Views
{
    public static class ImagingContainerView
    {
        public static string _imagingLocation = "";
        public static string CreateImagingContainer(LabAndImagingOrders imagingOrders, List<ImagingLocationsViewModel> imagingLocations, CheckoutDetailsViewModel checkoutData)
        {
            if (imagingOrders.OrderCount == 0)
            {
                return "";
            }
            var allImaging = new List<GetLabOrImaging>();
            var check = new Dictionary<string, GetLabOrImaging>();
            try
            {

                foreach (var imaging in checkoutData.LabDetailsList)
                {
                    if (imaging.Type == "I")
                    {
                        if (check.ContainsKey(imaging.ImagingType))
                        {
                            check[imaging.ImagingType] = imaging;
                        }
                        else
                        {
                            check.Add(imaging.ImagingType, imaging);
                        }
                    }
                }

                
                foreach (var key in check.Keys)
                {
                    allImaging.Add(check[key]);
                }
                foreach (var imaging in imagingOrders.TestOrder)
                {
                    if (!check.ContainsKey(imaging.OrderName))
                    {
                        var unscheduleImaging = new GetLabOrImaging();
                        unscheduleImaging.Skipped = true;
                        unscheduleImaging.ImagingType = imaging.OrderName;
                        check[imaging.OrderName] = unscheduleImaging;
                        allImaging.Add(unscheduleImaging);
                    }
                }

                var imagingTrue = allImaging.Where(x => x.Skipped == true);
                var imagingFalse = allImaging.Where(x => x.Skipped == false);
                allImaging = imagingFalse.ToList();
                allImaging.AddRange(imagingTrue);
                if ((allImaging == null || allImaging.Count == 0) && imagingOrders.OrderCount <= 0)
                {
                    return "";
                }

                var body = "<div class='page-break'>";
                body += @"<div class='header display--flex align--items--center location--flex page-break'>
          <svg style='margin-right:0.5rem;'  width='45' height='45' viewBox='0 0 104 112' fill='none' xmlns='http://www.w3.org/2000/svg'>
            <ellipse cx='52' cy='55.7678' rx='52' ry='55.2678' fill='#00B2F0' fill-opacity='0.3'/>
            <path d='M62.8333 74.6641H41.1667C39.0118 74.6641 36.9452 73.808 35.4214 72.2843C33.8977 70.7606 33.0417 68.6939 33.0417 66.5391V39.4557C33.0417 38.7374 32.7563 38.0486 32.2484 37.5406C31.7405 37.0327 31.0516 36.7474 30.3333 36.7474C29.615 36.7474 28.9262 37.0327 28.4183 37.5406C27.9103 38.0486 27.625 38.7374 27.625 39.4557V66.5391C27.625 70.1305 29.0517 73.5749 31.5913 76.1145C34.1308 78.654 37.5752 80.0807 41.1667 80.0807H62.8333C63.5516 80.0807 64.2405 79.7954 64.7484 79.2875C65.2563 78.7796 65.5417 78.0907 65.5417 77.3724C65.5417 76.6541 65.2563 75.9652 64.7484 75.4573C64.2405 74.9494 63.5516 74.6641 62.8333 74.6641ZM76.375 44.7099C76.3468 44.4611 76.2923 44.216 76.2125 43.9786V43.7349C76.0823 43.4564 75.9086 43.2004 75.6979 42.9766L59.4479 26.7266C59.224 26.5159 58.9681 26.3422 58.6896 26.212H58.4458L57.5792 25.9141H46.5833C44.4284 25.9141 42.3618 26.7701 40.8381 28.2938C39.3144 29.8176 38.4583 31.8842 38.4583 34.0391V61.1224C38.4583 63.2773 39.3144 65.3439 40.8381 66.8676C42.3618 68.3914 44.4284 69.2474 46.5833 69.2474H68.25C70.4049 69.2474 72.4715 68.3914 73.9952 66.8676C75.519 65.3439 76.375 63.2773 76.375 61.1224V44.8724C76.375 44.8724 76.375 44.8724 76.375 44.7099ZM60.125 35.1495L67.1396 42.1641H62.8333C62.115 42.1641 61.4262 41.8787 60.9183 41.3708C60.4103 40.8629 60.125 40.174 60.125 39.4557V35.1495ZM70.9583 61.1224C70.9583 61.8407 70.673 62.5296 70.1651 63.0375C69.6572 63.5454 68.9683 63.8307 68.25 63.8307H46.5833C45.865 63.8307 45.1762 63.5454 44.6683 63.0375C44.1603 62.5296 43.875 61.8407 43.875 61.1224V34.0391C43.875 33.3208 44.1603 32.6319 44.6683 32.124C45.1762 31.6161 45.865 31.3307 46.5833 31.3307H54.7083V39.4557C54.7083 41.6106 55.5644 43.6772 57.0881 45.201C58.6118 46.7247 60.6784 47.5807 62.8333 47.5807H70.9583V61.1224Z' fill='#00A5DF'/>
            </svg>
        
          <span class='patient-detail display--flex font--weight--600 font--size--20'>Imaging ({Imaging-Count})</span>
        </div>".Replace("{Imaging-Count}", Convert.ToString(allImaging.Count()));

                var imagingLocationContainerStart = @"<div class='display--flex padded-white page-break'>";
                var imagingLocationSectionStart = @"<div class='imaging-section'>";



                var allImagingType = new List<string>();

                int count = 0;
                foreach (var imaging in allImaging)
                {
                    if (count == 3)
                    {
                        break;
                    }
                    if (!Convert.ToBoolean(imaging.Skipped))
                    {
                        imagingLocationSectionStart += @"<div class='details-box page-break'>
              
              <div class='display--flex details-info font--weight--600 pa--2 border-bottom page-break'>
<div class='display--flex flex--column location--flex' style='width:50%';>
                  <span class='details-info-title'>APPOINTMENT TYPE</span>
                  <span class='font--size--14 font--weight-700'>{Imaging-AptType}</span>
                  <span class='font--size--14 font--light'>Imaging Test</span>
                </div>
                <div class='display--flex flex--column location--flex' style='width:50%';>
                  <span class='details-info-title'>LOCATION</span>
                  <span class='font--size--14 font--weight-700'>{Imaging-Location-Address}</span>
                  <span class='font--size--14 font--light'>{Imaging-City}</span>
                </div>
                
              </div>
            </div>".Replace("{Pending-Status}", "").Replace("{Imaging-Location-Address}", imaging.Lab_Name).Replace("{Imaging-City}", imaging.ImagingLocation).Replace("{Imaging-AptType}", imaging.ImagingType);
                        if (_imagingLocation == "")
                        {
                            _imagingLocation = imaging.ImagingLocation.Replace(".", "").Replace(",", "").Replace(" ", "+").Replace(" ", "");
                        }
                    }
                    else
                    {
                        imagingLocationSectionStart += @"<div class='details-box page-break'>
              <div class='display--flex align--items--center font--weight--600 font--size--14 pending-status-css color-pink'>{Pending-Status}</div>
              <div class='display--flex details-info font--weight--600 pa--2 border-bottom page-break'>
                <div class='display--flex flex--column location--flex'>
                  <span class='details-info-title'>APPOINTMENT TYPE</span>
                  <span class='font--size--14 font--weight-700'>{Imaging-AptType}</span>
                   <span class='font--size--14 font--light'>Imaging Test</span>
                </div>
              </div>
            </div>".Replace("{Pending-Status}", "Pending").Replace("{Imaging-Location-Address}", imaging.Lab_Name).Replace("{Imaging-AptType}", imaging.ImagingType);
                    }
                    count++;

                }


                imagingLocationSectionStart += " </div> ";
                imagingLocationContainerStart += imagingLocationSectionStart + @"<div id='map2' class='page-break lab-img'> </div></div>";

                var imagingSection2 = "";
                if (allImaging.Count() > 3)
                {
                    imagingSection2 += @"<div class='display--flex padded-white'><div class='imaging-section-2'>";
                    for (; count < allImaging.Count(); count++)
                    {
                        var imaging = allImaging[count];
                        if (!Convert.ToBoolean(imaging.Skipped))
                        {
                            imagingSection2 += @"<div class='details-box page-break'>
              
              <div class='display--flex details-info font--weight--600 pa--2 border-bottom page-break'>
<div class='display--flex flex--column location--flex' style='width:50%';>
                  <span class='details-info-title'>APPOINTMENT TYPE</span>
                  <span class='font--size--14 font--weight-700'>{Imaging-AptType}</span>
                  <span class='font--size--14 font--light'>Imaging Test</span>
                </div>
                <div class='display--flex flex--column location--flex' style='width:50%';>
                  <span class='details-info-title'>LOCATION</span>
                  <span class='font--size--14 font--weight-700'>{Imaging-Location-Address}</span>
                  <span class='font--size--14 font--light'>{Imaging-City}</span>
                </div>
                
              </div>
            </div>".Replace("{Pending-Status}", "").Replace("{Imaging-Location-Address}", imaging.Lab_Name).Replace("{Imaging-City}", imaging.ImagingLocation).Replace("{Imaging-AptType}", imaging.ImagingType);
                            if (_imagingLocation == "")
                            {
                                _imagingLocation = imaging.ImagingLocation.Replace(".", "").Replace(",", "").Replace(" ", "+").Replace(" ", "");
                            }
                        }
                        else
                        {
                            imagingSection2 += @"<div class='details-box page-break'>
              <div class='display--flex align--items--center font--weight--600 font--size--14 pending-status-css color-pink'>{Pending-Status}</div>
              <div class='display--flex details-info font--weight--600 pa--2 border-bottom page-break'>
                <div class='display--flex flex--column location--flex'>
                  <span class='details-info-title'>APPOINTMENT TYPE</span>
                  <span class='font--size--14 font--weight-700'>{Imaging-AptType}</span>
                   <span class='font--size--14 font--light'>Imaging Test</span>
                </div>
              </div>
            </div>".Replace("{Pending-Status}", "Pending").Replace("{Imaging-Location-Address}", imaging.Lab_Name).Replace("{Imaging-City}", imaging.ImagingLocation).Replace("{Imaging-AptType}", imaging.ImagingType);
                        }


                    }
                    imagingSection2 += "</div></div>";
                }




                body += imagingLocationContainerStart + " </div> ";
                body += imagingSection2;
                body += @"<div class='page-break'> <h4 class='other-location-title padded-white' style='color:  rgb(0, 40, 85);'>{Other-Locations}</h4> ".Replace("{Other-Locations}", "Other Locations :");
                var locationContainer = @"<div class='padded-white pb-16 location--grid'>";

                count = 0;
                foreach (var imagingLocation in imagingLocations)
                {
                    if (count >= 4)
                    {
                        break;
                    }
                    locationContainer += @"<div class='display--flex location--flex flex--margin page-break'>
                  <svg class='icon-css' width='19' height='28' viewBox='0 0 19 28' fill='none' xmlns='http://www.w3.org/2000/svg'>
                    <path d='M9.5 0C4.26143 0 0 4.396 0 9.8C0 17.15 9.5 28 9.5 28C9.5 28 19 17.15 19 9.8C19 4.396 14.7386 0 9.5 0ZM14.9286 11.2H10.8571V15.4H8.14286V11.2H4.07143V8.4H8.14286V4.2H10.8571V8.4H14.9286V11.2Z' fill='#00A5DF'/>
                    </svg>
                  <div><span class='font--weight--600' style='color: rgb(0, 40, 85);'>{Location-Imaging-Name}</span><br />
                    <span class='font--size--14''>{Imaging-Location}</span><br/>
                    <span class='font--size--14 color-pink'>{Imaging_distance} miles from you</span><br/>
                    <span class='font--size--14 font--weight--600' style='color: rgb(0, 40, 85);'>{Imaging-Mobile-Number}</span>
                  </div>
                </div>".Replace("{Imaging-Location}", imagingLocation.Address.Address).Replace("{Imaging_distance}", Convert.ToString(Convert.ToInt32(Convert.ToDouble(imagingLocation.Distance) * 0.000621371))).Replace("{Imaging-Mobile-Number}", "(630) 545­-7526").Replace("{Location-Imaging-Name}", imagingLocation.Address.Parts.City);
                    count++;
                }

                locationContainer += " </div> ";
                body += locationContainer;
                body += @"<div class='display--flex justify--space--between padded-white pb-16 bottom-radius'>
                <div class='location-links'>
                    <div class='link-text'>For detailed list of Imaging locations, please visit:</div>
                    <div class='link-details'>
                        <div class='link-icon'>
                            <img style='height: 25px; width: 20px;'
                                src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAB0AAAAhCAYAAAAlK6DZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAS0SURBVFhHvZdNbFRVGIbfudPpTH9mOhZQIVFJQCgRqvwkBGMkRW1EbLBB7YK40RhLQsLKFQtNZOGChSwEjZrowoVI1ZAmEuuizIKYBoyAP0gwQQPF1jaUdmbamc7M9X3vz/TO/4wJPsntzD1zznm/833f+c6pzySowY35HD4fX0RsKoPLszlMpnNIZU0E/T6sChrYEvVjR6cfL68M4IEWwxlVmaqi719P4eDlBYCiaPIBmo8f9sM/GqrRetgFiyaiHX680xXEwdVBNpSnrOjYTBbbz85ZkyAgManUSY5j0iba2gz8+GQY69pLV17ScvjKArZ/e8d+oesaEhTqHzKQoMHrh+/g6B8p54clClZ64NI8PqAo6ohLXWjqpIm3t7TirXVL7s6LHr2WwpsXkkCrI+jjp7tI9TAVtBq4MfailcezGH4qgj33NllNlug0Y7D86xmukB2UIBJMyy3ODHpfsYxW0wupUndZKP4ZEwEazaS2UO7F1a73eRPmwD1WuyW681wCsYlFu5eIdgD9O/lOyySYywLhCLD/DWDXbmDWiblLModDG0N475EWp8EmzaQKDnExjLGM2vdgM05ta4Uxy5fYX7TeFXTpiPKhZVE+ncsBvx84/i5w5hsmWMjpRDj+8KMtJYJigh7MQ/uHGEJhfHojDTTTkloYFJX4qc9ouZMUiiGLxJEujxGVUNgo/NWtRRix6QwndH6ohYRnbhf21z6uFwZ75J8MjF/nmJUNjINRZKHHgzWhzp+sboYq3P9JlnrGfc2NLLMxinNTtDOuxlaeEFaxvgtc51YqCB1fN0X8MHqWUZQZeDf44ib3vrd2U2fv/QEYL65qZiVhw1IJtslVWL7mqDMix37X/ndeND/HbebRZ6XiS2u571h08kivnRWo2BDRFACuXuVWobFV2Hw2bm8t7U/BhR1Yb+9nS/QjVhSdgXmRDC3Y0M1PuqeY1jYu4QjrNMe08LtKpRdOsXE0jp+mPGVV87JyHd9kVy1LtIMbfN8arlZuFmkW9t6+0hor/BSJzwKv7gVGTgOT45jkuCFeZ14YS8J38jZ+mWGDt8pRcHDDUtUqOE99X7LaBGmdXBJh0f/kGPD9sF2Di1HMk3Sh3P0hxedY2JU0zMu8S4VuEuxq9rOWO3jMobVPtFtHkIVW+foh4OnngakJWssa7Y2xJk6zrX8/v9OVWpnc6RUUrD5jPZzXQ8FKRc+5OEb/pnvcmqoVX/nZLvSXztvuFVn26RsAXhksHwaRyuG1tSF8/FjhCVQiKnyn6Sq1untMLlTiZJlgt27aCbZ6jZ1w8wm7TzE80Ffycjbey11QRFlR1YomJkT+JuFF56qQAZUoE0cvBTF10XXj4rO0UPEttkli1QTVn3Gc3sOwVKCsqOhm5fhuVxhIVKhM5ZAgb38XKdhZ5SCpKCqeWdGEE48z8xJVVuYiwYSJ87sj6A47IahAVVEx+FAzTuygMK+RJa52sVxqYrQ3jK30UC3KJlI5TrLiDMRYDFqLkstyac5yaa0VutQtKi6wvG07w/9xlNXaTsrSBRO/UbCrzP8slWhIVFxjYj08wtqrfcU0n+/rQMi9XdeLRP8Lz/0Qd741imn+C/aROR+PEQXYAAAAAElFTkSuQmCC' />
                        </div>
                        <a
                            href='https://www.dulyhealthandcare.com/services' target='_blank'>https://www.dulyhealthandcare.com/services</a>
                    </div>
                </div>
            </div>
        </div>";
                Console.WriteLine("Imaging is done");
                return body;
            }
            catch
            {
                new NullReferenceException();
            }
            return "";
        }
    }
}
