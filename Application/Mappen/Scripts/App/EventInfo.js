var EventInfo = function () {
    var _self = this;
    this.Name = ko.observable();
    this.Description = ko.observable();
    this.DisplayDescription = ko.computed(function () {
        var description = _self.Description();
        return description ? description.split(/\n+/) : [];
    });
    this.Parking = ko.observable();
    this.Website = ko.observable();
    this.Email = ko.observable();
    this.Phone = ko.observable();
    this.Category = ko.observable();
    this.Tags = ko.observableArray();
    this.PhotoUrl = ko.observable();
}

EventInfo.prototype = {
    ProcessJson: function (json) {
        this.Name(json.Name);
        this.Description(json.Description);
        this.Parking(json.Parking);
        this.Website(json.Website);
        this.Email(json.Email);
        this.Phone(json.Phone);
        if (json.Category)
            this.Category(json.Category.Name);
        this.Tags(json.Tags);
        this.PhotoUrl(json.PhotoUrl);
    }
}