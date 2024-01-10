
declare let google: any;
const GOOGLE_API_KEY = 'AIzaSyAIFqamCTEKNFTvvwGMO0TR1E5rIxxg1ng';
export const getAddressFromPosition = async (position: GeolocationPosition) => {
  const lat = position.coords.latitude;
  const lang = position.coords.longitude;
  const url = 'https://maps.googleapis.com/maps/api/geocode/json?key=' + GOOGLE_API_KEY + '&latlng=' + lat + ',' + lang + '&sensor=true';
  try {
    const data = await fetch(url).then(response => 
      response.json());
    return data?.results[0]?.formatted_address;
  } catch (ex) {
    return null;
  }
};

export const getLatLngFromAddress = async (address: string) => {
  const url = `https://maps.googleapis.com/maps/api/geocode/json?key=${GOOGLE_API_KEY}&address=${address}`;
  try {
    const data = await fetch(url).then(response => 
      response.json());
    return data.results[0].geometry.location;
  } catch (ex) {
    console.log(ex, 'er');
    return null;
  }
};

export const getTwoPointsDetails = (origin: any, destination: any) => {
  const directionsService = new google.maps.DirectionsService();
  return directionsService
    .route({
      origin: origin,
      destination: destination,
      optimizeWaypoints: false,
      durationInTraffic: true,
      provideRouteAlternatives: true,
      avoidHighways: false,
      avoidTolls: false,
      travelMode: google.maps.DirectionsTravelMode.DRIVING,

    });
};

export const getTwoPointsTime = async (origin: any, destination: any) => {
  const response = await getTwoPointsDetails(origin, destination);
  return response.routes[0].legs[0].duration.text;
};


