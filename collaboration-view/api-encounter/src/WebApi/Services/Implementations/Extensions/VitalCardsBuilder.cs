// <copyright file="VitalCardsBuilder.cs" company="Duly Health and Care">
// Copyright (c) Duly Health and Care. All rights reserved.
// </copyright>

using Duly.CollaborationView.Encounter.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duly.CollaborationView.Encounter.Api.Services.Implementations.Extensions
{
    public static class VitalCardsBuilder
    {
        private static readonly VitalsCardType[] _cardTypes = (VitalsCardType[])Enum.GetValues(typeof(VitalsCardType));

        public static IEnumerable<VitalsCard> FilterVitalCards(this IEnumerable<VitalsCard> vitalsCards)
        {
            foreach (var vitalsGroup in vitalsCards.GroupBy(card => card.CardType))
            {
                if (vitalsGroup.Key == VitalsCardType.WeightAndHeight)
                {
                    var vitals = vitalsGroup.SelectMany(card => card.Vitals).ToArray();
                    FilterVitals(vitals);
                    yield return new VitalsCard
                    {
                        CardType = vitalsGroup.Key,
                        Vitals = vitals
                    };
                }
                else
                {
                    var vitalCard = vitalsGroup.Single();
                    FilterVitals(vitalCard.Vitals);
                    yield return vitalCard;
                }
            }
        }

        public static IEnumerable<VitalsCard> AppendMissedVitalsCars(this IEnumerable<VitalsCard> vitalsCards)
        {
            var existingVitalsArray = vitalsCards.ToDictionary(card => card.CardType);
            foreach (var vitalsCardType in _cardTypes)
            {
                if (existingVitalsArray.TryGetValue(vitalsCardType, out var vitalsCard))
                {
                    yield return vitalsCard;
                }
                else
                {
                    yield return BuildMissedVitalsCard(vitalsCardType);
                }
            }
        }

        public static IEnumerable<VitalsCard> SortVitalsCard(this IEnumerable<VitalsCard> vitalsCards)
        {
            //https://kb.epam.com/display/DPGECLOF/US03.11.01+View+Today's+vitals
            return vitalsCards.OrderBy(card => card.CardType);
        }

        private static void FilterVitals(IEnumerable<Vital> vitals)
        {
            foreach (var vital in vitals)
            {
                FilterVitalMeasurement(vital);
            }
        }

        private static void FilterVitalMeasurement(Vital vital)
        {
            if (vital.Measurements.Length < 2)
            {
                return;
            }

            switch (vital.VitalType)
            {
                case VitalType.Temperature:
                    vital.ChooseTemperatureUnitOfMeasure();
                    break;
                case VitalType.Weight:
                    vital.ChooseWeightUnitOfMeasure();
                    break;
                case VitalType.Height:
                    vital.ChooseHeightUnitOfMeasure();
                    break;
            }
        }

        private static VitalsCard BuildMissedVitalsCard(VitalsCardType vitalsCardType)
        {
            return new VitalsCard
            {
                CardType = vitalsCardType,
                Vitals = Array.Empty<Vital>()
            };
        }
    }
}
