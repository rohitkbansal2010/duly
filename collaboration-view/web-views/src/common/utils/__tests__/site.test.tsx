import { formatAddress, formatAddressBottomLine } from '../site';

describe('siteUtils', () => {
  it('formatAddress', () => {
    expect(formatAddress({
      city: 'Oak Park',
      line: '1121 South Blvd',
      postalCode: '60302',
      state: 'Illinois',
    })).toEqual('1121 South Blvd Oak Park, Illinois 60302');

    expect(formatAddress({
      city: 'Oak Park',
      line: '1121 South Blvd',
      state: 'Illinois',
    })).toEqual('1121 South Blvd Oak Park, Illinois');
  });

  it('formatAddressBottomLine', () => {
    expect(formatAddressBottomLine({
      city: 'Oak Park',
      postalCode: '60302',
      state: 'Illinois',
    })).toEqual('Oak Park, Illinois 60302');

    expect(formatAddressBottomLine({
      city: 'Oak Park',
      state: 'Illinois',
    })).toEqual('Oak Park, Illinois');
  });
});
