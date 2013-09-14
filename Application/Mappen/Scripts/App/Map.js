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
    var spread = function () {
        var grouped = markers.groupBy(function (marker) {
            return marker.getPosition().toString();
        }),
        output = [];
        for (var pos in grouped) {
            if (grouped[pos].length > 1) {
                var incr = .01;
                for (var i = 0; i < grouped[pos].length; i++) {
                    var position = grouped[pos][i].getPosition();
                    switch ((i + 1) % 4) {
                        case 1:
                            grouped[pos][i].setPosition(new google.maps.LatLng(position.lat() + incr, position.lng()));
                        case 2:
                            grouped[pos][i].setPosition(new google.maps.LatLng(position.lat(), position.lng() + incr));
                        case 3:
                            grouped[pos][i].setPosition(new google.maps.LatLng(position.lat() - incr, position.lng()));
                        case 0:
                            grouped[pos][i].setPosition(new google.maps.LatLng(position.lat(), position.lng() - incr));
                            incr += .01;
                    }
                }
            }
            output = output.concat(grouped[pos]);
        }
        markers = output;
    }

    this.Markers = function (val) {
        if (val) {
            for (var i = markers.length - 1; i >= 0; i--) {
                var marker = markers[i];
                if (val.indexOf(marker) == -1) {
                    marker.setMap(null);
                    markers.splice(markers.indexOf(marker), 1);
                } else {
                    val.splice(val.indexOf(marker), 1);
                }
            }
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
//            spread();
        }
        return markers;
    }

    var bounds = ko.observable(new google.maps.LatLngBounds());
    this.Bounds = ko.computed(function () {
        return bounds();
    });
    this.Center = ko.observable(new google.maps.LatLng(40, -111));
}