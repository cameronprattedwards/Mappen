var App = function () {
    var _self = this,
        eventRepository = new EventRepository(),
        geocodeRepository = new GeocodeRepository();

    this.SearchTerm = ko.observable();
    this.Mapp = new Mapp();
    this.MappMode = ko.observable(false);
    this.JumpMode = ko.observable(false);
    this.Events = ko.observableArray();
    this.NumEvents = ko.observable(20);
    this.VisibleEvents = ko.computed(function () {
        return _self.Events.slice(0, _self.NumEvents());
    });
    this.FeaturedEvent = ko.observable();
    this.Busy = ko.observable(true);
    this.Home = ko.observable();
    geocodeRepository.Here(
        $.Deferred()
        .done(function (center) {
            _self.Home(center);
            _self.Mapp.Map.Center(center);

            eventRepository.WherePlace(
                center,
                new Date(),
                $.Deferred()
                    .done(function (events) {
                        _self.Events(events);
                    })
                    .always(function () {
                        _self.Busy(false);
                    })
            );
        }));

    $(document).ready(function () {
        if (!dataBound) {
            ko.applyBindings(_self);
            dataBound = true;
        }
    });
}

App.prototype = {
    Search: function () {
        this.Busy(true);
        var repository = new EventRepository(),
            _self = this;
        repository.WhereTerm(
            this.SearchTerm(),
            this.Home(),

            $.Deferred()
                .done(function (events) {
                    _self.Events(events);
                })
                .always(function () {
                    _self.Busy(false);
                })
        );
    },
    DoJump: function () {
        this.JumpMode(true);
    },
    StopJump: function () {
        this.JumpMode(false);
    },
    DoMapp: function () {
        this.MappMode(true);
    },
    StopMapp: function () {
        this.MappMode(false);
    },
    SetFeatured: function (event) {
        this.FeaturedEvent(event);
    },
    Unfeature: function () {
        this.FeaturedEvent(null);
    },
    ShowMoreEvents: function () {
        this.NumEvents(this.NumEvents() + 20);
    }
}