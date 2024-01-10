﻿using System.Collections.Generic;

namespace Duly.UI.Flourish.GeneratePDF.Helpers
{
    public static class ImagingLocationCptCodesHelper
    {
        public static List<string> cptRangeForCT()
        {
            return new List<string> { "70450", "70460", "70470", "70480", "70481", "70482", "70486", "70487", "70488", "70490", "70491", "70492", "70494", "70496", "70498", "71250", "71260", "71270", "71271", "71275", "71277", "72125", "72126", "72127", "72128", "72129", "72130", "72131", "72132", "72133", "72191", "72192", "72193", "72194", "72292", "73200", "73201", "73202", "73206", "73700", "73701", "73702", "73706", "74150", "74160", "74170", "74174", "74175", "74176", "74177", "74178", "74261", "74262", "74263", "75571", "75572", "75573", "75574", "75600", "75605", "75625", "75630", "75635", "75680", "75894", "76070", "76071", "77073", "76362", "75956", "75957", "75958", "75959" };
        }

        public static List<string> cptRangeForXray()
        {
            return new List<string> { "70010", "70015", "700030", "70100", "70110", "70120", "70130", "70134", "70140", "70150", "70160", "70200", "70210", "70220", "70240", "70250", "70260", "70300", "70310", "70328", "70330", "70332", "70350", "70355", "70360", "70370", "70371", "70373", "70380", "70390", "71010", "71015", "71020", "71021", "71022", "71023", "71030", "71034", "71035", "71038", "71045", "71046", "71047", "71048", "71090", "71100", "71101", "71110", "71111", "71120", "71130", "72010", "72020", "72040", "72050", "76101", "76102", "76140", "76145", "71040", "73030", "73040", "71060", "72072",
                "72052", "72069", "72070", "72074", "72080", "72081", "72082", "72083", "72084", "72090", "72100", "72110", "72114", "72120", "72170", "72190", "72200", "72202", "72220", "72240", "72255", "72265", "72270", "72275", "72285", "72291", "72295", "72470", "73000", "73010", "73020", "73040", "73050", "73060", "73070", "73080", "73085", "73090", "73092", "73100", "73110", "73115", "73120", "73130", "73140", "73500", "73501", "73502", "73503", "73510", "73520", "73521", "73522", "73523", "73525", "73530", "73540", "73542", "72550", "73551", "73552", "73560", "73562", "73564", "73580", "73590", "73592", "73600", "73610", "73615", "73620", "73630", "73660", "74000", "74010", "74190", "74210", "74220", "74221", "74230", "74240", "74241", "74245", "74246", "74247", "74248", "74249", "74250", "74251", "74260", "74270", "74280", "74283", "74290", "74291", "74300", "74301", "74305", "74320", "74327", "74329", "74330", "74340", "74350", "74355", "74360", "74363", "74400", "74405", "74410", "74415", "74420", "74425", "74430", "74440", "74445", "74450", "74455", "74470", "74475", "74480", "74485", "74710", "74740", "74742", "74775", "75650", "75658", "75660", "75685", "75809", "76006", "76010", "76020", "76040", "76061", "76062", "76065", "76066", "76078", "76080", "76086", "73067", "73550", "73565", "76100", "77071" };
        }

        public static List<string> cptRangeMri()
        {
            return new List<string> { "70336", "70540", "70541", "70542", "70543", "70544", "70545", "70546", "70547", "70548", "70549",
  "71550", "71551", "71552", "71555", "72141", "72142", "72146", "72147", "72148", "72149", "72156", "72157", "72158", "72159", "72195", "72196", "72197", "72198", "73218", "73219", "73220", "73221", "73222", "73223", "73225", "73320", "73718", "73719", "73720", "73721", "73722", "73723", "73725", "74018", "74019", "74020", "74021", "74022", "74181", "74182", "74183", "74185", "74712", "74713", "75552", "75553", "75554", "75555", "75556", "75557", "75558", "75559", "75560", "75561", "75562", "75563", "75564", "75565", "76093", "76094", "77077", "77072", "71549", "75791", "76499"};
        }

        public static List<string> cptRangeForFlouro()
        {
            return new List<string> { "71036", "75998", "76000", "76001", "76003", "76005", "77001", "77002", "77003", "76496" };
        }

        public static List<string> cptRangeForInterventionalRadiology()
        {
            return new List<string> {"75662", "75665", "75671", "75676", "75710", "75705", "75716", "75722", "75724", "75726", "75731", "75733", "75736", "75741", "75743", "75746", "75756", "75774", "75790", "75801", "75803", "75805", "75807", "75810", "75820", "75822", "75825", "75827", "75831", "75833", "75840", "75842", "75860", "75870", "75872", "75880", "75885", "75887", "75889", "75891",
  "75893", "75894", "75896", "75898", "75900", "75901", "75902", "75940", "75945", "75946", "75952", "75953", "75954" };
        }

        public static List<string> cptRangeForDexa()
        {
            return new List<string> { "76075", "76076", "76077", "77082", "77081", "77085", "77083", "77084", "77085", "77086", "77089", "77090", "77091", "77092" };
        }

        public static List<string> cptRangeForBreastService()
        {
            return new List<string> { "76096", "76098" };
        }
    }
}