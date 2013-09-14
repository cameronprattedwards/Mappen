var TileRepository = function () {
    this.Tiles = [];
}
TileRepository.prototype = (function () {
    var processing = false,
        toReturn,
        request = function (missing, callback) {
            var _self = this,
                data = missing.map(function (corner) { return corner.ToJson() }),
                deferred = $.Deferred()
                    .done(function(tiles){
                        var newTiles = tiles.map(function (val) {
                            return Tile.FromJson(val);
                        });
                        toReturn = toReturn.concat(newTiles);
                        _self.Tiles = _self.Tiles.concat(newTiles);
                        callback(toReturn);
                    })
                    .always(function(errors){
                        processing = false;
                    });

            API.POST("Tiles/me/Get", data, deferred);
        }

    return {
        GetTiles: function (tile, callback) {
            if (!processing) {
                processing = true;
                var corners = tile.Corners(),
                    missing = [],
                    _self = this;

                toReturn = [];

                for (var i = 0; i < corners.length; i++) {
                    var matches = this.Tiles.filter(function (val) {
                        return val.Contains(corners[i]);
                    });
                    if (matches.length == 0)
                        missing.push(corners[i]);
                    else
                        toReturn = toReturn.concat(matches);
                }

                if (missing.length > 0) {
                    request.call(this, missing, callback);
                } else {
                    callback(toReturn);
                    processing = false;
                }
            }
        }
    }
}());