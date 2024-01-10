using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duly.UI.Flourish.GeneratePDF.Helpers
{
    public static class ImagingLocationHelper
    {
        public static string GetImagingLocationServiceSlug(string aptType, string givenCode)
        {
            if (aptType != null && aptType.Length > 0)
            {
                var cptCode = Convert.ToInt32(givenCode);
                if (checkForAnnualScreeningMammogram(aptType, cptCode))
                {
                    return "annual-screening-mammogram";
                }
                if (checkForCt(aptType, cptCode)){
                    return "ct";
                }
                if (checkForDexa(aptType, cptCode)){
                    return "dexa";
                }
                if (checkForFluro(aptType, cptCode)){
                    return "fluoroscopy";
                }
                if (checkForMemmo(aptType, cptCode)){
                    return "breast-services";
                }
                if (checkForMri(aptType, cptCode)){
                    return "mri";
                }
                if (checkForNeuclearMedicine(aptType, cptCode)){
                    return "nuclear-medicine";
                }
                if (checkForPetCT(aptType, cptCode)){
                    return "pet-ct";
                }
                if (checkForultrasound(aptType, cptCode)){
                    return "ultrasound";
                }
                if (checkForXray(aptType, cptCode)){
                    return "x-ray";
                }
                if (checkForIR(aptType, cptCode)){
                    return "interventional-radiology";
                }
                if (checkForRadiology(aptType, cptCode)){
                    return "radiology";
                }
                if (checkForBreastServices(aptType, cptCode)){
                    return "breast-services";
                }
                else
                {
                    return "radiology";
                }
            }
            return null;
        }

        private static bool checkForCt(string aptType, int cptCode)
        {
            var cptRange = ImagingLocationCptCodesHelper.cptRangeForCT();
            foreach (var cpt in cptRange)
            {
                if (Convert.ToInt32(cpt) == cptCode)
                {
                    return true;
                }
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "ct ", "ct,", "computed tomography", "ct-scan", "cta " };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForXray(string aptType, int cptCode)
        {
            var cptRange = ImagingLocationCptCodesHelper.cptRangeForXray();
            foreach (var cpt in cptRange)
            {
                if (Convert.ToInt32(cpt) == cptCode)
                {
                    return true;
                }
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "xr ", "xr,", "xray", "x-ray", "x-rays", "arteriography" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForMri(string aptType, int cptCode)
        {
            var cptRange = ImagingLocationCptCodesHelper.cptRangeMri();
            foreach (var cpt in cptRange)
            {
                if (Convert.ToInt32(cpt) == cptCode)
                {
                    return true;
                }
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "mri ", "mr,", "mr ", "mra", "magnetic resonance", "mrv" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForFluro(string aptType, int cptCode)
        {
            var cptRange = ImagingLocationCptCodesHelper.cptRangeForFlouro();
            foreach (var cpt in cptRange)
            {
                if (Convert.ToInt32(cpt) == cptCode)
                {
                    return true;
                }
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "fluoroscopy ", "fluoro" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForPetCT(string aptType, int cptCode)
        {
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "pet/ct ", "pet", "pet-ct", "78608", "78814", "78815", "78816" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForultrasound(string aptType, int cptCode)
        {
            if (cptCode >= 76506 && cptCode <= 76999)
            {
                return true;
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "ultrasound", "us", "91200", "echo", "echocardiography" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForMemmo(string aptType, int cptCode)
        {
            if (cptCode >= 77031 && cptCode <= 77067)
            {
                return true;
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "mammo", "mammogram", "mammography", "diag mamm", "19081", "19082", "mam diagnostic", "mamm screening" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForAnnualScreeningMammogram(string aptType, int cptCode)
        {
            if (cptCode == 77067)
            {
                return true;
            }

            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "screen mammogram", "screen mammo", "mamm screening" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForBreastServices(string aptType, int cptCode)
        {
            var cptRange = ImagingLocationCptCodesHelper.cptRangeForBreastService();
            foreach (var cpt in cptRange)
            {
                if (Convert.ToInt32(cpt) == cptCode)
                {
                    return true;
                }
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "breast" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForDexa(string aptType, int cptCode)
        {
            var cptRange = ImagingLocationCptCodesHelper.cptRangeForDexa();
            foreach (var cpt in cptRange)
            {
                if (Convert.ToInt32(cpt) == cptCode)
                {
                    return true;
                }
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "dxa", "dual energy", "dexa" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForIR(string aptType, int cptCode)
        {
            var cptRange = ImagingLocationCptCodesHelper.cptRangeForInterventionalRadiology();
            foreach (var cpt in cptRange)
            {
                if (Convert.ToInt32(cpt) == cptCode)
                {
                    return true;
                }
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "interventional-radiology", "ir ", "ir,", "angio", "angiography", "aortography" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForRadiology(string aptType, int cptCode)
        {
            if (cptCode >= 77261 && cptCode <= 77799)
            {
                return true;
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "radn", "radiation", "nm", "nuclear med", "nuclear" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool checkForNeuclearMedicine(string aptType, int cptCode)
        {
            if (cptCode >= 78000 && cptCode <= 78999)
            {
                return true;
            }
            if (cptCode >= 79000 && cptCode <= 79999)
            {
                return true;
            }
            if (aptType != null)
            {
                aptType = aptType.ToLower();
                var searchFor = new List<string> { "nuclear medicine", "78452", "nm", "nuclear med", "nuclear" };
                foreach (var search in searchFor)
                {
                    if (aptType.Contains(search))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}