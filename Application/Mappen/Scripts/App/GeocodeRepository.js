var GeocodeRepository = function () { }

GeocodeRepository.prototype = {
    Here: function (deferred) {
        if (window.navigator.geolocation)
            window.navigator.geolocation.getCurrentPosition(function (position) {
                var coords = position.coords;
                deferred.resolve(new google.maps.LatLng(coords.latitude, coords.longitude));
            });
        else
            deferred.reject(["User has not provided permission or geolocation is unavailable."]);
    },
    FromAddress: function (address, deferred) {
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ address: address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK)
                deferred.resolve(results.map(function (result) {
                    return result.geometry.location;
                }));
            else
                deferred.reject([status]);
        });
    }
}