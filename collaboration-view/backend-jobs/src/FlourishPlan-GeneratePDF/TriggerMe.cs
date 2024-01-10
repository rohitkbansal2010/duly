using Duly.UI.Flourish.GeneratePDF;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlourishPlan_GeneratePDF
{
    public class TriggerMe
    {
        public IConfiguration _configuration;
        public TriggerMe(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [JobDisplayName("flourishplan-job")]
        [AutomaticRetry(Attempts = 0)]      
        public async Task RunAsync()
        {
            //var config = new ConfigurationBuilder()
            //      .SetBasePath(Directory.GetCurrentDirectory())
            //      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            GetDataForPDFGeneration obj = new GetDataForPDFGeneration(_configuration);
            obj.Generator();     
        }
    }
}
