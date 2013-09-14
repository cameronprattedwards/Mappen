var MEvent;
(function () {
    var instances = {};
    MEvent = function (id) {
        if (id && instances[id])
            return instances[id];

        this.Id = ko.observable(id);
        this.Info = new EventInfo();
        this.Venue = new Venue();
        this.Timespan = new Timespan();

        if (id)
            instances[id] = this;
    }

    MEvent.prototype = {
        ProcessJson: function (json) {
            this.Info.ProcessJson(json.Info);
            this.Venue.ProcessJson(json.Venue);
            this.Timespan.ProcessJson(json.Timespan);
        }
    }

    MEvent.FromJson = function (json) {
        var event = new MEvent(json.Id);
        event.ProcessJson(json);
        return event;
    }
}());