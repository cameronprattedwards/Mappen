var dataBound = false;
var Mapp = function () {
    var markerRepository = new MarkerRepository(),
        _self = this,
        events = [],
        queue = false,
        loading = ko.observable(false),
        refresh = function () {
            loading(true);
            window.setTimeout(function () {
                if (loading())
                    _self.Loading(true);
            }, 100);
            markerRepository.Where(this.Tile, function (markers) {
                _self.Map.Markers(markers);
                loading(false);
                _self.Loading(false);
            });
        }.bind(this);

    this.Loading = ko.observable(false);
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
        if (address instanceof Date) {
            this.Tile.Timespan.Start(address);
            this.Tile.Timespan.End(new Date(address.getFullYear(), address.getMonth(), address.getDate(), address.getHours() + 2));
        } else {
            var _self = this;
            new GeocodeRepository().FromAddress(
                address,
                $.Deferred().done(function (geocodes) { _self.Map.Center(geocodes[0]); })
            );
        }
    }
}