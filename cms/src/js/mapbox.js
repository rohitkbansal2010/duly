'use strict';

import mapboxgl from 'mapbox-gl';
import '../scss/mapbox.scss';

// Add custom /osm prefix for OSM style requests
function TransformRequest(url, resourceType) {
    if (["Glyphs"].includes(resourceType)) {
        url = new URL(url);
        url.pathname = url.pathname.replace("/fonts/", "/dist/fonts/pbf/");

        if (window.location.port != "") {
            url.port = window.location.port;
        }

        url = url.href;
    }

    if (["Style", "Tile"].includes(resourceType)) {
        url = new URL(url);
        if ("/osm" !== url.pathname.substring(0, 4)) {
            if (window.location.port != "") {
                url.port = window.location.port;
            }

            url.pathname = "/osm" + url.pathname;
            url = url.href;
        }
    }

    return {
        url: url
    };
};

export function AddMarkers(geojson, map, fitBounds = true) {
    // add markers to map
    let markers = [];
    geojson.features.forEach(marker => {
        // create a HTML element for each feature
        var el = document.createElement('div');
        el.className = 'marker';

        // make a marker for each feature and add to the map
        markers.push(
            new mapboxgl.Marker(el)
                .setLngLat(marker.geometry.coordinates)
                .setPopup(new mapboxgl.Popup({ offset: 25 }) // add popups
                    .setHTML('<p class="name">' + marker.properties.name + '</p>' + '<p class="address">' + marker.properties.address + '</p>'))
                .addTo(map)
        );
    });

    var bounds = new mapboxgl.LngLatBounds();
    markers.forEach(function (feature) {
        bounds.extend(feature.getLngLat());
    });

    if (fitBounds) {
        map.fitBounds(bounds, { padding: { top: 40, bottom: 40, left: 40, right: 40 } });
    }

    return markers;
}

export function InitializeMap(container, center, onGeolocateCallback = null, bbox) {
    if (!container) {
        return;
    }
    const ts = new Date().getTime();
    let dmgMap = new mapboxgl.Map({
        container,
        style: location.origin + '/osm/styles/basic/style.json?ts=' + ts,
        center,
        zoom: 10.5,
        maxZoom: 13,
        minZoom: 6,
        transformRequest: TransformRequest
    });

    dmgMap.addControl(new mapboxgl.NavigationControl());
    dmgMap.addControl(new mapboxgl.FullscreenControl());

    // Add geolocate control to the map
    const geolocate = new mapboxgl.GeolocateControl({
        fitBoundsOptions: {
            maxZoom: 10.5
        },
        positionOptions: {
            enableHighAccuracy: true
        },
        trackUserLocation: true
    });
    if (onGeolocateCallback) {
        geolocate.on('geolocate', onGeolocateCallback);
    }
    dmgMap.addControl(geolocate);

    if (bbox != null) {
        CheckIfMapIsInView(bbox);
    }

    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
        dmgMap.dragPan.disable();
        dmgMap.scrollZoom.disable();
        dmgMap.touchPitch.disable()
        dmgMap.on('touchstart', (e)=> {
            let oe = e.originalEvent;
            if (oe && 'touches' in oe) {
                if (oe.touches.length > 1) {
                    oe.stopImmediatePropagation();
                    dmgMap.dragPan.enable();
                } else {
                    dmgMap.dragPan.disable();
                }
            }
        });
    }
    return dmgMap;
}

export function CheckIfMapIsInView(bbox) {
    const scroll = bbox == ".information-container .information";

    const isInView = (el) => {
        if (!el) {
            return;
        }
        const scroll = window.scrollY || window.pageYOffset
        const boundsTop = el.getBoundingClientRect().top + scroll

        const viewport = {
            top: scroll,
            bottom: scroll + window.innerHeight,
        }

        const bounds = {
            top: boundsTop,
            bottom: boundsTop + el.clientHeight,
        }


        return (bounds.bottom >= viewport.top && bounds.bottom <= viewport.bottom)
            || (bounds.top <= viewport.bottom && bounds.top >= viewport.top);
    }

    const listener = (e) => {
        let bounding = document.querySelector(bbox);

        if (isInView(bounding)) {
            let currentLocationTrigger = document.querySelector(".mapboxgl-ctrl-geolocate");
            currentLocationTrigger && currentLocationTrigger.click();

            window.removeEventListener('scroll', listener, false);
            window.removeEventListener('DOMContentLoaded', listener, false);
        }
    }

    if (scroll == true) {
        window.addEventListener('scroll', listener);
    } else {
        window.addEventListener('DOMContentLoaded', listener);
    }
}