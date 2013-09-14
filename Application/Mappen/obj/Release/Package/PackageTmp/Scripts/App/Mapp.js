var dataBound = false;
var Mapp = function () {
    var markerRepository = new MarkerRepository(),
        _self = this,
        events = [],
        queue = false,
        refresh = function () {
            markerRepository.Where(this.Tile, function (markers) {
                _self.Map.Markers(markers);
            });
        }.bind(this);

    this.Timespan = new Timespan();
    this.Map = new Map();
    this.FeaturedEvent = ko.observable();
    this.Tile = new Tile(this.Map.Bounds(), this.Timespan);
    this.Initialize = function () {
        this.Tile.Bounds(this.Map.Bounds());
        this.Map.Bounds.subscribe(function (val) {
            _self.Tile.Bounds(val);
            refresh();
        });
        this.Tile.Timespan.Start.subscribe(refresh);
        this.Tile.Timespan.End.subscribe(refresh);
    }
}

Mapp.prototype = {
    Jump: function (address) {
        var _self = this;
        new GeocodeRepository().FromAddress(
            address,
            $.Deferred().done(function (geocodes) { _self.Map.Center(geocodes[0]); })
        );
    }
}