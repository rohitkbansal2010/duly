import { Address } from '@types';

import { firstLetterCapital } from './appointment';

export const formatAddress = ({
  city = '',
  line = '',
  postalCode = '',
  state = '',
}: Address) => {
  const cityInfo = city && line ? `${line} ${city},` : '';
  const addressLine = `${cityInfo} ${state} ${postalCode}`;

  return addressLine.trim();
};

export const formatAddressBottomLine = ({
  city = '',
  postalCode = '',
  state = '',
}: Address) => {
  const formattedCity = `${city},`;
  const formattedAddressLine = `${formattedCity} ${state} ${postalCode}`;

  return firstLetterCapital(formattedAddressLine.trim());
};
