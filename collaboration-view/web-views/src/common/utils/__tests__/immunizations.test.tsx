import { formatDose } from '../immunizations';

describe('immunizationsUtils', () => {
  it('formatDose', () => {
    expect(formatDose(
      {
        amount: 1,
        unit: 'mL',
      }
    )).toEqual('1 mL');

    expect(formatDose(
      { amount: 1 }
    )).toEqual('1');

    expect(formatDose()).toEqual('N/A');
  });
});
