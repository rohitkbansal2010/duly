// <copyright file="EventTiming.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Duly.Clinic.Contracts
{
    /// <summary>
    /// Real world event relating to the schedule.
    /// (url: http://hl7.org/fhir/ValueSet/event-timing)
    /// (systems: 2).
    /// </summary>
    [Description("Real world event relating to the schedule")]
    public enum EventTiming
    {
        /// <summary>
        /// Event occurs during the morning. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Morning")]
        MORN,

        /// <summary>
        /// Event occurs during the early morning. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Early Morning")]
        MORN_early,

        /// <summary>
        /// Event occurs during the late morning. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Late Morning")]
        MORN_late,

        /// <summary>
        /// Event occurs around 12:00pm. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Noon")]
        NOON,

        /// <summary>
        /// Event occurs during the afternoon. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Afternoon")]
        AFT,

        /// <summary>
        /// Event occurs during the early afternoon. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Early Afternoon")]
        AFT_early,

        /// <summary>
        /// Event occurs during the late afternoon. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Late Afternoon")]
        AFT_late,

        /// <summary>
        /// Event occurs during the evening. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Evening")]
        EVE,

        /// <summary>
        /// Event occurs during the early evening. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Early Evening")]
        EVE_early,

        /// <summary>
        /// Event occurs during the late evening. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Late Evening")]
        EVE_late,

        /// <summary>
        /// Event occurs during the night. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("Night")]
        NIGHT,

        /// <summary>
        /// Event occurs [offset] after subject goes to sleep. The exact time is unspecified and established by institution convention or patient interpretation.
        /// (system: http://hl7.org/fhir/event-timing)
        /// </summary>
        [Description("After Sleep")]
        PHS,

        /// <summary>
        /// Description: Prior to beginning a regular period of extended sleep (this would exclude naps).  Note that this might occur at different times of day depending on a person's regular sleep schedule.
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("HS")]
        HS,

        /// <summary>
        /// Description: Upon waking up from a regular period of sleep, in order to start regular activities (this would exclude waking up from a nap or temporarily waking up during a period of sleep)
        ///
        ///
        ///                            Usage Notes: e.g.
        ///
        ///                         Take pulse rate on waking in management of thyrotoxicosis.
        ///
        ///                         Take BP on waking in management of hypertension
        ///
        ///                         Take basal body temperature on waking in establishing date of ovulation
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("WAKE")]
        WAKE,

        /// <summary>
        /// Description: meal (from lat. ante cibus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("C")]
        C,

        /// <summary>
        /// Description: breakfast (from lat. cibus matutinus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("CM")]
        CM,

        /// <summary>
        /// Description: lunch (from lat. cibus diurnus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("CD")]
        CD,

        /// <summary>
        /// Description: dinner (from lat. cibus vespertinus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("CV")]
        CV,

        /// <summary>
        /// before meal (from lat. ante cibus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("AC")]
        AC,

        /// <summary>
        /// before breakfast (from lat. ante cibus matutinus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("ACM")]
        ACM,

        /// <summary>
        /// before lunch (from lat. ante cibus diurnus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("ACD")]
        ACD,

        /// <summary>
        /// before dinner (from lat. ante cibus vespertinus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("ACV")]
        ACV,

        /// <summary>
        /// after meal (from lat. post cibus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("PC")]
        PC,

        /// <summary>
        /// after breakfast (from lat. post cibus matutinus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("PCM")]
        PCM,

        /// <summary>
        /// after lunch (from lat. post cibus diurnus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("PCD")]
        PCD,

        /// <summary>
        /// after dinner (from lat. post cibus vespertinus)
        /// (system: http://terminology.hl7.org/CodeSystem/v3-TimingEvent)
        /// </summary>
        [Description("PCV")]
        PCV,
    }
}