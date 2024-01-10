import {
  writeHumanName,
  showFirstPrefixItem,
  mergeHumanName
} from '../person';

describe('personUtils', () => {
  it('writeHumanName', () => {
    expect(writeHumanName([ 'Maxim' ], 'Novikov')).toEqual('Maxim Novikov');
    expect(writeHumanName([ 'Ana Maria' ], 'Reyes')).toEqual('Ana Maria Reyes');
    expect(writeHumanName([ 'Ana Maria', 'Michael' ], 'Reyes')).toEqual('Ana Maria Michael Reyes');
  });

  it('showFirstPrefixItem', () => {
    expect(showFirstPrefixItem([ 'Maxim' ])).toEqual('Maxim ');
    expect(showFirstPrefixItem([ 'Maxim Novikov' ])).toEqual('Maxim ');
    expect(showFirstPrefixItem([])).toEqual('');
  });

  it('mergeHumanName', () => {
    expect(mergeHumanName({ familyName: 'Maxim', givenNames: [] })).toEqual('Maxim');
    expect(mergeHumanName({ familyName: 'Reyes', givenNames: [ 'Ana Maria' ] })).toEqual('ReyesAna Maria');
    expect(mergeHumanName({ familyName: 'Reyes', givenNames: [ 'Ana Maria', 'Michael' ] })).toEqual('ReyesAna MariaMichael');
  });
});
