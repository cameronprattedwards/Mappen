var Map = function () {
    var _self = this;

    this.Map;
    this.InfoWindow;

    var initialized = false;
    this.Initialize = function () {
        if (!initialized) {
            bounds(_self.Map.getBounds());
            google.maps.event.addListener(this.Map, "bounds_changed", function () {
                bounds(_self.Map.getBounds());
            });
            this.Center.subscribe(function (val) {
                _self.Map.setCenter(val);
            });
            initialized = true;
            app.Mapp.Initialize();
        }
    }

    var markers = [];
    this.Markers = function (val) {
        if (val) {
            markers.forEach(function (marker) {
                if (val.indexOf(marker) == -1) {
                    marker.setMap(null);
                    markers.remove(marker);
                } else {
                    val.splice(val.indexOf(marker), 1);
                }
            });
            for (var i = 0; i < val.length; i++) {
                var marker = val[i];
                marker.setMap(_self.Map);
                markers.push(marker);
                if (!marker.listener)
                    marker.listener = google.maps.event.addListener(marker, "click", function () {
                        _self.InfoWindow.setContent(this.content);
                        _self.InfoWindow.open(_self.Map, this);
                        console.log(_self.InfoWindow.getContent() === this.content);
                    });
            }
        }
        return markers;
    }

    var bounds = ko.observable(new google.maps.LatLngBounds());
    this.Bounds = ko.computed(function () {
        return bounds();
    });
    this.Center = ko.observable(new google.maps.LatLng(40, -111));
}