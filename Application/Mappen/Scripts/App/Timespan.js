var Timespan = function (start, end) {
    var _self = this;
    if (start && end) {
        this.Start = ko.observable(start);
        this.End = ko.observable(end);
    } else {
        var date = new Date();
        this.Start = ko.observable(date);

        date = new Date();
        date.setHours(date.getHours() + 2);
        this.End = ko.observable(date);
    }
    this.MarkerFormat = ko.computed(function () {
        var start = _self.Start(), end = _self.End(), sameDay = start.getDate() == end.getDate();
        return sameDay ?
                start.MinuteFormat() + " - " + end.MinuteFormat() :
                start.DateFormat() + " " + start.MinuteFormat() + " - " + end.DateFormat() + " " + end.MinuteFormat();
    });
}
Timespan.prototype = {
    Contains: function (date) {
        return date <= this.End() && date >= this.Start();
    },
    ContainsSpan: function (span) {
        return span.Start() < this.End() && span.End() > this.Start();
    },
    ProcessJson: function (json) {
        this.Start(Date.FromJson(json.Start));
        this.End(Date.FromJson(json.End));
    },
    Copy: function () {
        return new Timespan(this.Start(), this.End());
    }
}
Timespan.FromJson = function (json) {
    return new Timespan(Date.FromJson(json.Start), Date.FromJson(json.End));
}