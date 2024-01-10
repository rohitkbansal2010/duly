import { useEffect, useState } from 'react';

import { getTwoPointsDetails } from '@utils';

import markerIcon from '../../common/icons/google-maps/marker.svg';

declare let google: any;
type GoogleMapsProps = {
  from?: string;
  to?: string;
  id?: string
}

function GoogleMaps({ from, to, id }: GoogleMapsProps) {
  const [ map, setMap ] = useState<any>(null);
  const [ markers ] = useState<any>([]);
  const [ directions ] = useState<any>([]);

  const removeAllMarkers = () => {
    markers.forEach((marker: any) => { marker.setMap(null); });
  };

  const zoom = () =>{
    map?.setZoom(10); 
  };

  const setMarkerOnMap = (latlng: any) => {
    removeAllMarkers();
    const marker: any = new google.maps.Marker({
      position: latlng,
      icon: { url: markerIcon, scaledSize: new google.maps.Size(30, 40) },
      map: map,
      title: to,
    });
    markers.push(marker);
    const latLngObj = new google.maps.LatLng(latlng?.lat, latlng?.lng);
    map?.setCenter(latLngObj); 
    setTimeout(()=>{
      zoom();
    }, 100);
  };

  const initMap = () => {
    const googleMap = new google.maps.Map(
      document.getElementById(id) as HTMLElement,
      { zoom: 7 }
    );
    setMap(googleMap);
  };

  const calculateAndDisplayRoute = (startlatLng: any, endlatLng: any) => {
    directions.forEach(direction =>
      direction.setMap(null));
    const directionsRenderer: any = new google.maps.DirectionsRenderer({
      draggable: true,
      suppressMarkers: true,
    });
    directionsRenderer.setMap(map);
    directions.push(directionsRenderer);
    getTwoPointsDetails(startlatLng, endlatLng).then((response: any) => {
      directionsRenderer.setDirections(response);
      removeAllMarkers();
      setMarkerOnMap(
        {
          lat: response?.routes[0]?.legs[0]?.end_location?.lat(), 
          lng: response?.routes[0]?.legs[0]?.end_location?.lng(), 
        }
      );
    })
      .catch((e: any) => { console.log('Directions request failed', e); });
  };

  const createPath = async () => {
    if (to && from) {
      calculateAndDisplayRoute(from, to);
    }
  };

  useEffect(() => { initMap(); }, []);
  useEffect(() => { createPath(); }, [ map, from, to ]);

  return (
    <div
      style={{
        height: '100%',
        width: '100%',
      }}
      id={id}
    />
  );
}

export default GoogleMaps;

