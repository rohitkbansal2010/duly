using Duly.UI.Flourish.GeneratePDF.Model;
using PDFGeneratorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF.Views
{
    public static class GetHeader
    {
        public static string CustomHeader(DateTimeOffset createdDate, PatientViewModel patient, string patientName, string patientImage, AppointmentDetailsBySMSStatusViewModel pendingData)
        {
            var header = "";
            header += @"<!DOCTYPE html><html lang='en'><head> <meta charset='UTF-8'> <meta http-equiv='X-UA-Compatible' content='IE=edge'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <title>Document</title> <style>*{padding:0; margin:0; box-sizing: border-box;}html{padding:0; margin:0; box-sizing: border-box; font-family: 'Inter';}body{background-color: rgb(240, 243, 248);margin: 0 2rem 2rem 2rem; font-family: sans-serif;}.display--flex{display: flex;}.justify--space--between{justify-content: space-between;}.title{margin: 0px; color: rgb(0, 40, 85); font-size: 1.2em;}.date{font-size: 12px;}.profilePic{width: 40px; height: 40px;}.align--items--center{align-items: center;}.info{font-size: 15px; font-weight: 800; color: rgb(0, 40, 85);margin-right: 20px;text-align: end;}.info-color{color: rgb(18, 135, 238);}.subTitle{font-size: 12px; font-weight: 400;}.patient-detail{flex-direction: column; align-items: flex-end;}.font--weight--600{font-weight: 800;}.display--flex{padding: 5px 12px 5px 9px;}.border-radius-css{border-radius: 50%;}</style></head><body><div class='justify--space--between display--flex align--items--center' style='padding: 15px 12px 15px 9px;'>
        <div>
            <h3 class='title font--weight--600'>Flourish Summary</h3>
            <h3 class='date'>{Created-Date-Time}</h3>
        </div>
        <div class='display--flex align--items--center'>
            <div class='info'> 
                <div><span style='margin-top:5px;'>Patient -<span
                        class='info-color'>&nbsp;{Patient-Name}</span></span></div>
                <div class='subTitle'>{Patient-Gender}, {Patient-Age} years old</div>
                
            </div><img src='{Profile-Pic-Url}' class='profilePic border-radius-css' />
        </div>
    </div></body></html>".Replace("{Created-Date-Time}", pendingData.AppointmentTime).Replace("{Patient-Name}", patientName).Replace("{Patient-Gender}", patient.Gender.ToString()).Replace("{Patient-Age}", ((DateTime.Now - patient.BirthDate).Days / 365).ToString()).Replace("{Profile-Pic-Url}", patientImage); ;
            return header;
        }
    }
}
