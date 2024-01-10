// <copyright file="LoincObservationCodesConverter.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.Clinic.Contracts;

namespace Duly.Clinic.Api.Repositories.Mappings.ObservationConverters
{
    /// <summary>
    /// Converts ObservationTypes to Loinc Codes and back.
    /// </summary>
    public static class LoincObservationCodesConverter
    {
        /// <summary>
        /// Tries to match string to ObservationType. Returns false when match was not found.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="detectedType">Conversion result on success.</param>
        /// <returns>Conversion success.</returns>
        public static bool ConvertFromLoincCodeToObservationType(this string str, out ObservationType? detectedType)
        {
            switch (str)
            {
                case "85354-9":
                    detectedType = ObservationType.BloodPressure;
                    return true;
                case "2708-6":
                case "59408-5":
                    detectedType = ObservationType.OxygenSaturation;
                    return true;
                case "8867-4":
                    detectedType = ObservationType.HeartRate;
                    return true;
                case "9279-1":
                    detectedType = ObservationType.RespiratoryRate;
                    return true;
                case "72514-3":
                    detectedType = ObservationType.PainLevel;
                    return true;
                case "8310-5":
                    detectedType = ObservationType.BodyTemperature;
                    return true;
                case "29463-7":
                    detectedType = ObservationType.BodyWeight;
                    return true;
                case "8302-2":
                    detectedType = ObservationType.BodyHeight;
                    return true;
                case "39156-5":
                    detectedType = ObservationType.BodyMassIndex;
                    return true;
                default:
                    detectedType = null;
                    return false;
            }
        }

        /// <summary>
        /// Tries to match ObservationType Loinc Codes. Returns false when match was not found.
        /// </summary>
        /// <param name="observationType">ObservationType to convert.</param>
        /// <param name="loincCodes">Codes that match type.</param>
        /// <returns>Conversion success.</returns>
        public static bool ConvertFromObservationTypeToLoincCode(this ObservationType observationType, out string[] loincCodes)
        {
            switch (observationType)
            {
                case ObservationType.BloodPressure:
                    loincCodes = new[] { "85354-9" };
                    return true;
                case ObservationType.OxygenSaturation:
                    loincCodes = new[] { "2708-6", "59408-5" };
                    return true;
                case ObservationType.HeartRate:
                    loincCodes = new[] { "8867-4" };
                    return true;
                case ObservationType.RespiratoryRate:
                    loincCodes = new[] { "9279-1" };
                    return true;
                case ObservationType.PainLevel:
                    loincCodes = new[] { "72514-3" };
                    return true;
                case ObservationType.BodyTemperature:
                    loincCodes = new[] { "8310-5" };
                    return true;
                case ObservationType.BodyWeight:
                    loincCodes = new[] { "29463-7" };
                    return true;
                case ObservationType.BodyHeight:
                    loincCodes = new[] { "8302-2" };
                    return true;
                case ObservationType.BodyMassIndex:
                    loincCodes = new[] { "39156-5" };
                    return true;
                default:
                    loincCodes = null;
                    return false;
            }
        }
    }
}
