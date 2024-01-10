import 'react-bootstrap-typeahead/css/Typeahead.css';
import { useEffect, useState, Fragment } from 'react';
import { AsyncTypeahead } from 'react-bootstrap-typeahead';

import './location-autocomplete.css';

import { blueLocationIcon } from '@icons';
import { locations } from '@mock-data';

import styles from './location-autocomplete.scss';

type LocationAutocompleteProps = {
    placeholder?: string;
    defaultValue?: any;
    onSelected?: any;
    multiple?: boolean;
}

function LocationAutocomplete({
  placeholder, defaultValue, onSelected, multiple, 
}: LocationAutocompleteProps) {
  const [ selected, setSelected ] = useState<any>('');
  const [ options ] = useState<any>(locations);
  const [ isLoading ] = useState(false);

  useEffect(() => {
    setSelected(multiple ? defaultValue : [ defaultValue ]);
  }, [ defaultValue ]);

  useEffect(() => {
    onSelected(multiple ? selected[0] : selected);
  }, [ selected ]);

  const handleSearch = (query: any) => {
    console.log(query);
  };
  return (
    <>
      <AsyncTypeahead
        className={`${styles.searchBar} searchBar`}
        id="location-search-bar"
        onSearch={handleSearch}
        onChange={setSelected}
        options={options}
        placeholder={placeholder}
        selected={selected}
        isLoading={isLoading}
        renderMenuItemChildren={option => 
(
  <Fragment>
    <span>{option}</span>
  </Fragment>
        )}
      />
      <img src={blueLocationIcon} alt="" className={styles.blueLocoIcon} />

    </>
  );
}

export default LocationAutocomplete;
