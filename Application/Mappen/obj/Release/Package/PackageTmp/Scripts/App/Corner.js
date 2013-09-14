var Corner = function (date, geocode) {
    this.Date = date;
    this.Geocode = geocode;
}

Corner.prototype = {
    ToJson: function () {
        var _self = this;
        return {
            Date: _self.Date,
            Geocode: _self.Geocode.ToJson()
        };
    }
}