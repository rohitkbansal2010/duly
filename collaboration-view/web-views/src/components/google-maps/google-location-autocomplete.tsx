import 'react-bootstrap-typeahead/css/Typeahead.css';
import './location-autocomplete.css';

import { useEffect, useState } from 'react';
import { FormControl } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { ErrorData } from '@constants';
import { blueLocationIcon } from '@icons';
import { refreshDataAction, showScheduleAppointmentError } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { getLatLngFromAddress } from '@utils';

import styles from './location-autocomplete.scss';

declare let google: any;

type GoogleLocationAutocompleteProps = {
  placeholder?: string;
  defaultValue?: any;
  onSelected?: any;
}

type SelectedDataType = {
  lat?: number;
  lng?: number;
  address?: string;
}

function GoogleLocationAutocomplete({ 
  placeholder, 
  defaultValue, 
  onSelected, 
}: GoogleLocationAutocompleteProps) {
  const [ selected, setSelected ] = useState('');
  const [ selectedData, setSelectedData ] = useState<SelectedDataType>();
  const { isRefreshRequired, refreshLocation } = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.refreshData);
  const { isScheduleAppointmentErrorShown } = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE.ScheduleAppointmentErrorData);

  const dispatch = useDispatch();

  const getAddress = (place: any) => {
    let address = '';
    const addressComponents = place.address_components;
    const componentMap = {
      street_number: 'street_number',
      route: 'route',
      locality: 'locality',
      administrative_area_level_1: 'administrative_area_level_1',
      country: 'country',
    };
    try {
      for (const addressComponent of addressComponents) {
        const types = addressComponent.types;
        for(const type of types) {
          if (Object.prototype.hasOwnProperty.call(componentMap, type)) {
            address += ', ' + addressComponent['long_name'];
          }
        }
      }
    } finally {
      console.log('Finally Block');
    }
    return place.name + address;
  };

  const initInput = () => {
    const input = document.getElementById('pac-input') as HTMLInputElement;
    const options = {
      fields: [ 'address_components', 'geometry', 'icon', 'name' ],
      strictBounds: false,
    };

    const autocomplete = new google.maps.places.Autocomplete(input, options);
    autocomplete.setFields([ 'place_id', 'geometry', 'name' ]);
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
      const place = autocomplete.getPlace();
      const address = getAddress(place);
      console.log(place);
      console.log(address);
      setSelectedData({ 
        lat: place.geometry.location.lat(),
        lng: place.geometry.location.lng(), 
        address: address, 
      });
    });
  };
  const getLatLng = async (address: string) => {
    if (address) {
      const data = await getLatLngFromAddress(address);
      if(data){
        setSelectedData({ lat: data?.lat, lng: data?.lng, address: address });
      }
      else {
        dispatch(refreshDataAction({
          isRefreshRequired:true,
          refreshLocation:'googleMaps',
        }));
        dispatch(showScheduleAppointmentError(ErrorData.BadNetwork));
      }
    }
  };
  
  useEffect(() => { setSelected(defaultValue); 
    getLatLng(defaultValue);
  }, [ defaultValue ]);
   
  useEffect(() => {
    if (selectedData && onSelected)
      onSelected(selectedData);
  }, [ selectedData ]);
  useEffect(()=>{
    if(isRefreshRequired === true && refreshLocation === 'chooseProvider' && isScheduleAppointmentErrorShown === false){
      onSelected(selectedData);
    }
  }, [ isRefreshRequired, refreshLocation, isScheduleAppointmentErrorShown ]);

  useEffect(() => { initInput(); }, []);
  return (
    <>
      <style type="text/css">
        {`            
            .pac-container:after {
              background-image: none !important;
              height:0 !important;
        }
        `}
      </style>
      <FormControl
        id="pac-input"
        placeholder={placeholder}
        className={styles.searchBar}
        defaultValue={selected}
        onChange={(e) => { setSelected(e?.target?.value); }}
      />
      <img src={blueLocationIcon} alt="location-icon" className={styles.blueLocoIcon} />
    </>
  );
}

export default GoogleLocationAutocomplete;

