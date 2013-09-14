var MarkerRepository = function () {
    var tileRepository = new TileRepository();
    this.Where = function (origTile, success) {
        tile = origTile.Copy();

        var bounds = tile.Bounds(),
            sw = bounds.getSouthWest(),
            span = tile.Timespan;
        tile.Bounds(bounds.extend(new google.maps.LatLng(sw.lat() + 1, sw.lng() + 1)));
        var newDate = new Date(span.Start().getTime() + 86400000);
        span.End(newDate);
        tileRepository.GetTiles(tile, function (tiles) {
            success(
                tiles
                .into(function (currTile) {
                    return currTile.Events();
                })
                .filter(function (event) {
                    return origTile.Timespan.ContainsSpan(event.Timespan);
                })
                .map(function (event) {
                    return google.maps.Marker.FromEvent(event);
                })
            );
        });
    }
}