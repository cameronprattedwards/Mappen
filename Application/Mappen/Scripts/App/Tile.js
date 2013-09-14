var Tile;
(function () {
    Tile = function (bounds, timespan) {
        var _self = this;
        this.Bounds = ko.observable(bounds);
        this.Timespan = timespan;
        this.Events = ko.observableArray();
    }

    Tile.prototype = {
        Copy: function () {
            return new Tile(this.Bounds().Copy(), this.Timespan.Copy());
        },
        Corners: function () {
            var span = this.Timespan,
                start = span.Start(),
                end = span.End(),

                bounds = this.Bounds(),
                ne = bounds.getNorthEast(),
                sw = bounds.getSouthWest();

            return [
                new Corner(start, ne),
                new Corner(start, new google.maps.LatLng(sw.lat(), ne.lng())),
                new Corner(start, sw),
                new Corner(start, new google.maps.LatLng(ne.lat(), sw.lng())),
                new Corner(end, ne),
                new Corner(end, new google.maps.LatLng(sw.lat(), ne.lng())),
                new Corner(end, sw),
                new Corner(end, new google.maps.LatLng(ne.lat(), sw.lng()))
            ]
        },
        Contains: function (corner) {
            var t = this.Timespan.Contains(corner.Date),
                b = this.Bounds().contains(corner.Geocode);
//            console.log(t, b);
            return t && b;
        }
    }

    Tile.FromJson = function (json) {
        var tile = new Tile(
            google.maps.LatLngBounds.FromJson(json.Bounds),
            Timespan.FromJson(json.Timespan)
        );
        tile.Events(json.Events.map(function (e) {
            return MEvent.FromJson(e);
        }));
        return tile;
    }
}());