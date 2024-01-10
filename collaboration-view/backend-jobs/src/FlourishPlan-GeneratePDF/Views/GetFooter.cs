using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF.Views
{
    public static class GetFooter
    {
        public static string GetCustomFooter()
        {
            var footer = "";
            footer += @"<head> <meta charset='UTF-8'> <meta http-equiv='X-UA-Compatible' content='IE=edge'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <title>Document</title> <style>*{margin: 0; padding: 0; box-sizing: border-box;}.text{font-size: 16px; color: rgb(0, 40, 85); margin-left: 15px;}.inner-text{font-weight: 700;}.family-inter{font-family: 'Inter',sans-serif;}</style></head><body style='margin-top: 15px;background-color: rgb(240, 243, 248);'> <span class='text family-inter'>Have questions regarding your Flourish Plan? Contact your Primary Care Physician's office. <span class='inner-text'>Contact us at <span style='color: rgb(71, 10, 104);'>(630) 469-9200</span></span></span></body></html>";
            return footer;
        }
    }
}
