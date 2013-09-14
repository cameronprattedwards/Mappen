var Address = function () {
    this.Name = ko.observable();
    this.Street1 = ko.observable();
    this.Street2 = ko.observable();
    this.Street3 = ko.observable();
    this.City = ko.observable();
    this.State = ko.observable();
    this.Zip = ko.observable();
    this.Geocode = ko.observable();
}
Address.prototype = {
    ProcessJson: function (json) {
        this.Name(json.Name);
        this.Street1(json.Street1);
        this.Street2(json.Street2);
        this.Street3(json.Street3);
        this.City(json.City);
        this.State(json.State);
        this.Zip(json.Zip);
        this.Geocode(google.maps.LatLng.FromJson(json.Geocode));
    }
}