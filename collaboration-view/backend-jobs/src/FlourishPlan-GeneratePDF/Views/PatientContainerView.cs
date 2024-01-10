﻿using Duly.UI.Flourish.GeneratePDF.Model;
using PDFGeneratorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF.Views
{
    public static class PatientContainerView
    {
        public static string CreatePatientContainer(DateTimeOffset createdDate, PatientViewModel patient, string patientName, string patientImage, AppointmentDetailsBySMSStatusViewModel pendingData)
        {
            string container = "";
            container += @"<div class='justify--space--between display--flex'> <div class='display--flex' style='align-items: flex-end;'>
                <div class='display--flex' style='padding-right: 15px;'>
                    <svg width='112' height='51.8' viewBox='0 0 178 82' fill='none' xmlns='http://www.w3.org/2000/svg'>
                        <path d='M10.655 73.8438V76.9556H8.11816V73.8438H6.25781V81.9176H8.11816V78.7217H10.655V81.9176H12.5153V73.8438H10.655Z' fill='#00A5DF'/>
                        <path d='M15.6445 73.8438V81.9176H20.7182V80.1514H17.5049V78.7217H20.3799V76.9555H17.5049V75.6099H20.6336V73.8438H15.6445Z' fill='#00A5DF'/>
                        <path d='M26.8064 75.9463L27.7366 78.8899H25.8762L26.8064 75.9463ZM25.6225 73.8438L22.832 81.9176H24.8615L25.2843 80.6561H28.3285L28.7513 81.9176H30.7807L27.9902 73.8438H25.6225Z' fill='#00A5DF'/>
                        <path d='M33.1484 73.8438V81.9176H37.9684V80.1514H35.0088V73.8438H33.1484Z' fill='#00A5DF'/>
                        <path d='M39.0664 73.8438V75.6099H41.1804V81.9176H43.0408V75.6099H45.0702V73.8438H39.0664Z' fill='#00A5DF'/>
                        <path d='M52.0046 73.8438V76.9555H49.4678V73.8438H47.6074V81.9176H49.4678V78.7217H52.0046V81.9176H53.8649V73.8438H52.0046Z' fill='#00A5DF'/>
                        <path d='M63.2513 75.1053L64.6889 79.0581H61.8138L63.2513 75.1053ZM62.6594 73.8438L59.6152 81.9176H60.7991L61.4756 80.1514H65.1117L65.7881 81.9176H66.972L63.9278 73.8438H62.6594Z' fill='#00A5DF'/>
                        <path d='M74.2449 73.8438V79.815L70.1015 73.8438H69.2559V81.9176H70.3552V75.8622L74.4986 81.9176H75.3442V73.8438H74.2449Z' fill='#00A5DF'/>
                        <path d='M79.572 80.9084H81.686C82.5316 80.9084 83.2081 80.6561 83.7154 80.0673C84.2228 79.4786 84.4765 78.8058 84.4765 77.8807C84.4765 77.0397 84.2228 76.2827 83.7154 75.694C83.2081 75.1053 82.5316 74.853 81.686 74.853H79.572V80.9084ZM78.4727 73.8438H81.6014C82.7007 73.8438 83.6309 74.2643 84.3074 75.0212C85.0684 75.7781 85.4066 76.7032 85.4066 77.8807C85.4066 78.974 85.0684 79.9832 84.3074 80.7402C83.5463 81.4971 82.7007 81.9176 81.6014 81.9176H78.4727V73.8438Z' fill='#00A5DF'/>
                        <path d='M95.1304 82.0019C93.9465 82.0019 92.9318 81.5814 92.0862 80.8245C91.3251 80.0676 90.9023 79.0583 90.9023 77.7968C90.9023 76.6194 91.3251 75.6101 92.0862 74.8532C92.8472 74.0122 93.862 73.6758 95.1304 73.6758C95.8914 73.6758 96.4833 73.844 97.1598 74.1804C97.7517 74.5168 98.2591 74.9373 98.5974 75.526L96.9907 76.4512C96.8216 76.1147 96.5679 75.8624 96.2297 75.6942C95.8914 75.526 95.5532 75.4419 95.1304 75.4419C94.3693 75.4419 93.862 75.6942 93.4392 76.1147C93.0164 76.5353 92.7627 77.124 92.7627 77.8809C92.7627 78.6378 92.9318 79.2265 93.4392 79.6471C93.862 80.0676 94.4539 80.3199 95.1304 80.3199C95.5532 80.3199 95.8914 80.2358 96.2297 80.0676C96.5679 79.8994 96.8216 79.647 96.9907 79.3106L98.5974 80.2358C98.2591 80.8245 97.7517 81.245 97.1598 81.5814C96.4833 81.9178 95.8069 82.0019 95.1304 82.0019Z' fill='#00A5DF'/>
                        <path d='M104.433 75.9463L105.364 78.8899H103.503L104.433 75.9463ZM103.25 73.8438L100.459 81.9176H102.488L102.911 80.6561H105.955L106.378 81.9176H108.408L105.617 73.8438H103.25Z' fill='#00A5DF'/>
                        <path d='M113.989 75.5258H112.636V77.6284H113.989C114.242 77.6284 114.496 77.5443 114.665 77.292C114.834 77.1237 114.919 76.8714 114.919 76.535C114.919 76.1986 114.834 76.0304 114.665 75.7781C114.496 75.694 114.242 75.5258 113.989 75.5258ZM116.948 81.9176H115.003L113.481 79.3104H112.636V81.9176H110.775V73.8438H113.989C114.75 73.8438 115.426 74.0961 115.934 74.6848C116.441 75.1894 116.779 75.8622 116.779 76.6191C116.779 77.1238 116.61 77.5443 116.356 77.9648C116.103 78.3853 115.68 78.7217 115.257 78.974L116.948 81.9176Z' fill='#00A5DF'/>
                        <path d='M119.568 73.8438V81.9176H124.642V80.1514H121.429V78.7217H124.304V76.9555H121.429V75.6099H124.642V73.8438H119.568Z' fill='#00A5DF'/>
                        <path d='M35.4309 0V21.1938C31.7948 18.4185 27.3131 16.8205 22.4086 16.8205C9.97816 16.8205 0 27.3333 0 40.2851C0 53.2369 10.0627 63.7497 22.4086 63.7497C27.5668 63.7497 32.3867 61.8994 36.192 58.7876C36.953 61.563 40.1663 63.2451 43.1259 63.4974H49.5525V62.0676V49.1158V47.9384V0H35.4309ZM23.0005 50.882C17.4195 50.882 12.9378 46.1723 12.9378 40.2851C12.9378 34.482 17.4195 29.6882 23.0005 29.6882C28.5815 29.6882 33.0632 34.3979 33.0632 40.2851C33.0632 46.1723 28.5815 50.882 23.0005 50.882Z' fill='#002855'/>
                        <path d='M125.15 0.253906H110.943V63.5831H125.15V0.253906Z' fill='#002855'/>
                        <path d='M85.6603 53.9936C80.1639 53.9936 75.5976 49.5362 75.5976 43.9854V26.4921C75.5976 21.1936 71.2004 16.8203 65.8731 16.8203H57.8398V44.5741C57.8398 65.4315 85.6603 70.4777 85.6603 53.9936Z' fill='#002855'/>
                        <path d='M85.6602 26.4921C85.6602 21.1936 90.0573 16.8203 95.3846 16.8203H103.418V34.8182C103.418 47.5177 94.2853 54.33 85.6602 54.0777V26.4921Z' fill='#00A5DF'/>
                        <path d='M170.39 16.3166C167.345 16.653 164.386 18.9238 163.878 21.7833V41.2951C163.878 41.5474 163.878 41.8838 163.878 42.1361C163.878 47.3505 159.566 50.2099 156.522 50.2099C153.478 50.2099 149.672 47.3505 149.672 42.1361C149.672 41.7156 149.588 41.2951 149.588 40.7905V22.2879C149.25 19.1761 145.698 16.653 142.485 16.3166H134.705V30.6982V40.7905C134.705 48.612 136.735 53.6581 140.624 57.6951C144.599 61.732 149.926 63.7504 156.437 63.7504C158.889 63.7504 161.173 63.3299 163.287 62.4889C164.132 62.1525 164.893 61.8161 165.57 61.3115C165.316 63.414 164.555 66.1053 163.202 67.535C161.595 69.2171 159.312 69.974 156.353 69.974C153.478 69.974 151.194 68.9648 149.503 66.7781H135.297V66.8622C136.988 71.4879 139.694 75.1884 143.33 77.8797C147.051 80.655 151.364 82.0007 156.268 82.0007C160.412 82.0007 164.132 80.9914 167.43 79.0571C170.728 77.1227 173.349 74.4314 175.21 71.0673C177.07 67.7032 178 63.9186 178 59.8817V29.6889V16.1484H170.39V16.3166Z' fill='#002855'/>
                        </svg>
                </div>
                <div style='padding-left: 15px;' class='first-page-header'>
                    <h1 class='title font--weight--600'>Flourish Summary</h1>
                    <h3 class='date'>{Created-Date-Time}</h3>
                </div>
            </div><div class='display--flex align--items--center'> <div class='info display--flex'> <span>Patient - </span> <div class='patient-detail display--flex' style='align-items:flex-end;'> <span class='info-color'>&nbsp;{Patient-Name}</span> <span class='subTitle'>{Patient-Gender}, {Patient-Age} years old</span> </div></div><img src='{Profile-Pic-Url}' class='profilePic border-radius-css'/> </div></div> ".Replace("{Created-Date-Time}",pendingData.AppointmentTime).Replace("{Patient-Name}", patientName).Replace("{Patient-Gender}", patient.Gender.ToString()).Replace("{Patient-Age}", ((DateTime.Now - patient.BirthDate).Days / 365).ToString()).Replace("{Profile-Pic-Url}", patientImage);
            Console.WriteLine("Patients Done");
            return container;
        }
    }
}
