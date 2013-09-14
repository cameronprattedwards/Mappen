var EventUnitOfWork = function () {
    this.Events = new EventRepository();
}

EventUnitOfWork.prototype = {
    GetNearestEvents: function (success, complete) {
        var _self = this;
        if (window.navigator)
            window.navigator.geolocation.getCurrentPosition(function (position) {
                var coords = position.coords,
                    geocode = new google.maps.LatLng(coords.latitude, coords.longitude),
                    date = new Date();
                _self.Events.Where(geocode, date, success, complete);
            });
        else
            complete();
    }
}