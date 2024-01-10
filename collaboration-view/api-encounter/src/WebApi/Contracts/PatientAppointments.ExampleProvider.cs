// <copyright file="PatientAppointments.ExampleProvider.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1649:File name should match first type name",
        Justification = "Examples")]
    public class PatientAppointmentsExampleProvider : IExamplesProvider<PatientAppointments>
    {
        private const string VisitReasonHipProblems = "Patient fell - Hip problims";
        private const string VisitReasonHipCheck = "Hip check - PhysicalTherapy";
        private const string VisitReasonUpdatePPP = "Update a personalized prevention plan";
        private const string VisitReasonCheckUp = "Check weight and blood pressure";

        public PatientAppointments GetExamples()
        {
            return new PatientAppointments()
            {
                RecentAppointments = BuildRecentAppointments(),
                UpcomingAppointments = BuildUpcomingAppointments()
            };
        }

        private static PatientAppointmentsGroup[] BuildRecentAppointments()
        {
            var recentAppointments = new List<PatientAppointmentsGroup>
            {
                GenerateRecentPatientAppointmentGroupForServiceTypePhysicalTherapy(),
                GenerateRecentPatientAppointmentGroupForServiceTypeWellnessCheck(),
                GenerateRecentPatientAppointmentGroupForServiceTypeCheckUp()
            };

            return recentAppointments
                .OrderByDescending(x => x.NearestAppointmentDate)
                .ToArray();
        }

        private static PatientAppointmentsGroup GenerateRecentPatientAppointmentGroupForServiceTypePhysicalTherapy()
        {
            var practitioners = new PractitionersGeneralInfoExampleProvider().GetExamples().ToArray();
            var practitioner1 = practitioners[0];
            var practitioner2 = practitioners[1];
            var practitioner3 = practitioners[2];

            var recentAppointments = new List<PatientAppointment>
            {
                new()
                {
                    IsTelehealth = true,
                    Practitioner = practitioner2,
                    Status = PatientAppointmentStatus.Completed,
                    Reason = VisitReasonHipProblems,
                    StartTime = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2021, 11, 2, 14, 45, 0), DateTimeKind.Utc)),
                    MinutesDuration = 30
                },
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner1,
                    Status = PatientAppointmentStatus.Completed,
                    Reason = VisitReasonHipCheck,
                    StartTime = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2021, 10, 5, 12, 30, 0), DateTimeKind.Utc)),
                    MinutesDuration = 60
                },
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner2,
                    Status = PatientAppointmentStatus.Cancelled,
                    Reason = VisitReasonHipCheck,
                    StartTime = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2021, 11, 2, 11, 15, 0), DateTimeKind.Utc)),
                    MinutesDuration = 60
                },
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner3,
                    Status = PatientAppointmentStatus.Completed,
                    Reason = VisitReasonHipCheck,
                    StartTime = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2021, 7, 15, 13, 0, 0), DateTimeKind.Utc)),
                    MinutesDuration = 60
                }
            };

            var group = new PatientAppointmentsGroup
            {
                Title = "Physical Therapy",
                Appointments = recentAppointments
                    .OrderByDescending(x => x.StartTime.DateTime)
                    .ToArray()
            };

            group.NearestAppointmentPractitioner = group.Appointments.First().Practitioner;
            group.NearestAppointmentDate = group.Appointments.First().StartTime;
            group.FarthestAppointmentDate = group.Appointments.Last().StartTime;

            return group;
        }

        private static PatientAppointmentsGroup GenerateRecentPatientAppointmentGroupForServiceTypeWellnessCheck()
        {
            var practitioners = new PractitionersGeneralInfoExampleProvider().GetExamples().ToArray();
            var practitioner2 = practitioners[1];
            var practitioner3 = practitioners[2];

            var recentAppointments = new List<PatientAppointment>
            {
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner2,
                    Status = PatientAppointmentStatus.Completed,
                    Reason = VisitReasonUpdatePPP,
                    StartTime = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2018, 1, 5, 13, 30, 0), DateTimeKind.Utc)),
                    MinutesDuration = 60
                },
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner3,
                    Status = PatientAppointmentStatus.Completed,
                    Reason = VisitReasonUpdatePPP,
                    StartTime = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2021, 1, 5, 14, 30, 0), DateTimeKind.Utc)),
                    MinutesDuration = 60
                },
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner3,
                    Status = PatientAppointmentStatus.Cancelled,
                    Reason = VisitReasonUpdatePPP,
                    StartTime = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2020, 1, 2, 17, 15, 0), DateTimeKind.Utc)),
                }
            };

            var group = new PatientAppointmentsGroup
            {
                Title = "Wellness Check",
                Appointments = recentAppointments
                    .OrderByDescending(x => x.StartTime.DateTime)
                    .ToArray()
            };

            group.NearestAppointmentPractitioner = group.Appointments.First().Practitioner;
            group.NearestAppointmentDate = group.Appointments.First().StartTime;
            group.FarthestAppointmentDate = group.Appointments.Last().StartTime;

            return group;
        }

        private static PatientAppointmentsGroup GenerateRecentPatientAppointmentGroupForServiceTypeCheckUp()
        {
            var practitioners = new PractitionersGeneralInfoExampleProvider().GetExamples().ToArray();
            var practitioner3 = practitioners[2];

            var recentAppointments = new List<PatientAppointment>
            {
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner3,
                    Status = PatientAppointmentStatus.Completed,
                    Reason = VisitReasonCheckUp,
                    StartTime = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2020, 12, 5, 13, 30, 0), DateTimeKind.Utc)),
                    MinutesDuration = 30
                }
            };

            var group = new PatientAppointmentsGroup
            {
                Title = "Check up",
                Appointments = recentAppointments
                    .OrderByDescending(x => x.StartTime.DateTime)
                    .ToArray()
            };

            group.NearestAppointmentPractitioner = group.Appointments.First().Practitioner;
            group.NearestAppointmentDate = group.Appointments.First().StartTime;

            return group;
        }

        private static PatientAppointmentsGroup[] BuildUpcomingAppointments()
        {
            var recentAppointments = new List<PatientAppointmentsGroup>
            {
                GenerateUpcomingPatientAppointmentGroupForServiceTypePhysicalTherapy(),
                GenerateUpcomingPatientAppointmentGroupForServiceTypeWellnessCheck(),
                GenerateUpcomingPatientAppointmentGroupForServiceTypeCheckUp()
            };

            return recentAppointments
                .OrderBy(x => x.NearestAppointmentDate)
                .ToArray();
        }

        private static PatientAppointmentsGroup GenerateUpcomingPatientAppointmentGroupForServiceTypePhysicalTherapy()
        {
            var practitioners = new PractitionersGeneralInfoExampleProvider().GetExamples().ToArray();
            var practitioner2 = practitioners[1];

            var utcToday = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

            var recentAppointments = new List<PatientAppointment>
            {
                new()
                {
                    IsTelehealth = true,
                    Practitioner = practitioner2,
                    Status = PatientAppointmentStatus.Scheduled,
                    Reason = VisitReasonHipProblems,
                    StartTime = new DateTimeOffset(utcToday.AddDays(21).AddHours(13)),
                    MinutesDuration = 30
                },
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner2,
                    Status = PatientAppointmentStatus.Scheduled,
                    Reason = VisitReasonHipCheck,
                    StartTime = new DateTimeOffset(utcToday.AddDays(40).AddHours(13).AddMinutes(30)),
                    MinutesDuration = 60
                }
            };

            var group = new PatientAppointmentsGroup
            {
                Title = "Physical Therapy",
                Appointments = recentAppointments
                    .OrderBy(x => x.StartTime.DateTime)
                    .ToArray()
            };

            group.NearestAppointmentPractitioner = group.Appointments.First().Practitioner;
            group.NearestAppointmentDate = group.Appointments.First().StartTime;
            group.FarthestAppointmentDate = group.Appointments.Last().StartTime;

            return group;
        }

        private static PatientAppointmentsGroup GenerateUpcomingPatientAppointmentGroupForServiceTypeWellnessCheck()
        {
            var practitioners = new PractitionersGeneralInfoExampleProvider().GetExamples().ToArray();
            var practitioner2 = practitioners[1];

            var utcToday = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

            var recentAppointments = new List<PatientAppointment>
            {
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner2,
                    Status = PatientAppointmentStatus.Scheduled,
                    Reason = VisitReasonUpdatePPP,
                    StartTime = new DateTimeOffset(utcToday.AddDays(15).AddHours(14).AddMinutes(45)),
                    MinutesDuration = 60
                }
            };

            var group = new PatientAppointmentsGroup
            {
                Title = "Wellness Check",
                Appointments = recentAppointments
                    .OrderBy(x => x.StartTime.DateTime)
                    .ToArray()
            };

            group.NearestAppointmentPractitioner = group.Appointments.First().Practitioner;
            group.NearestAppointmentDate = group.Appointments.First().StartTime;

            return group;
        }

        private static PatientAppointmentsGroup GenerateUpcomingPatientAppointmentGroupForServiceTypeCheckUp()
        {
            var practitioners = new PractitionersGeneralInfoExampleProvider().GetExamples().ToArray();
            var practitioner3 = practitioners[2];

            var utcToday = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);

            var recentAppointments = new List<PatientAppointment>
            {
                new()
                {
                    IsTelehealth = false,
                    Practitioner = practitioner3,
                    Status = PatientAppointmentStatus.Scheduled,
                    Reason = VisitReasonCheckUp,
                    StartTime = new DateTimeOffset(utcToday.AddDays(30).AddHours(12).AddMinutes(30))
                }
            };

            var group = new PatientAppointmentsGroup
            {
                Title = "Check up",
                Appointments = recentAppointments
                    .OrderBy(x => x.StartTime.DateTime)
                    .ToArray()
            };

            group.NearestAppointmentPractitioner = group.Appointments.First().Practitioner;
            group.NearestAppointmentDate = group.Appointments.First().StartTime;

            return group;
        }
    }
}
