var Venue;
(function () {
    var instances = {};
    Venue = function (id) {
        if (id && instances[id])
            return instances[id];

        this.Id = ko.observable();
        this.Address = new Address();
        this.Name = ko.observable();
        this.Phone = ko.observable();
        this.Email = ko.observable();
        this.SubaddressType = ko.observable();
        this.SubaddressNumber = ko.observable();

        if (id)
            instances[id] = this;
    }

    Venue.prototype = {
        ProcessJson: function (json) {
            this.Address.ProcessJson(json.Address);
            this.Name(json.Name);
            this.Phone(json.Phone);
            this.Email(json.Email);
            this.SubaddressType(json.SubaddressType);
            this.SubaddressNumber(json.SubaddressNumber);
        }
    }
}());