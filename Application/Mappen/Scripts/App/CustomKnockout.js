ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var initModel = ko.utils.unwrapObservable(valueAccessor()),
            obsModel = valueAccessor();
        $(element).datepicker({
            onSelect: function (a, b, c) {
                obsModel($(element).datepicker("getDate"));
                return true;
            },
            defaultDate: initModel,
            altField: "#DPAltField",
            hideIfNoPrevNext: true,
        });
    }
};

ko.bindingHandlers.mapVisible = {
    update: function (element, valueAccessor) {
        var value = valueAccessor(),
            valueUnwrapped = ko.utils.unwrapObservable(value),
            check = ko.utils.unwrapObservable(valueUnwrapped.check),
            map = ko.utils.unwrapObservable(valueUnwrapped.map),
            center = ko.utils.unwrapObservable(valueUnwrapped.center);

        if (check) {
            $(element).show();
            google.maps.event.trigger(map, "resize");
            map.setCenter(center);
        } else {
            $(element).hide();
        }

    }
}

ko.bindingHandlers.fadeVisible = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(), allBindings = allBindingsAccessor(),
            jsBindings = ko.toJS(allBindings);

        var valueUnwrapped = ko.utils.unwrapObservable(value);

        var duration = allBindings.fadeDuration || 250;

        var complete = allBindings.fadeComplete || function () { }

        if (valueUnwrapped == true)
            $(element).animate({opacity: 1}, duration, complete);
        else
            $(element).animate({opacity: 1}, duration, complete);
    }
};


ko.bindingHandlers.slideVisible = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(), allBindings = allBindingsAccessor(),
            jsBindings = ko.toJS(allBindings);

        var valueUnwrapped = ko.utils.unwrapObservable(value);

        var duration = allBindings.slideDuration || 250;

        var complete = allBindings.slideComplete || function () { }

        if (valueUnwrapped == true)
            $(element).slideDown(duration, complete);
        else
            $(element).slideUp(duration, complete);
    }
};


ko.bindingHandlers.html = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var model = ko.utils.unwrapObservable(valueAccessor());
        $(element).html(model);
    }
}

function Timeline(element, model) {
    var _self = this;
    this.Model = model;
    this.Hours = ko.observableArray();
    ko.renderTemplate("Timeline", { Hours: _self.Hours, Timespan: _self.Model }, {}, element)

    this.Element = $(element);
    this.Viewport = this.Element.find(".Viewport");
    this.Slider = this.Element.find(".Slider");
    this.SliderLeft = this.Slider.position().left;
    this.Helper = this.Element.find(".Helper");
    this.Model.Start.subscribe(_self.Read.bind(_self));
    this.Read();
    this.StartCheck = this.Model.Start();
    this.Queue = false;

    var refreshSliderLeft = function () {
        _self.SliderLeft = _self.Slider.position().left;
    }

    this.Viewport.draggable({
        axis: "x",
        containment: "parent",
        start: refreshSliderLeft,
        drag: _self.Write.bind(_self)
    });

    var offset, timelineWidth;
    this.Helper.draggable({
        axis: "x",
        start: function () {
            offset = _self.Helper.position().left - _self.Slider.position().left;
            timelineWidth = $(".Timeline").width();
        },
        drag: function () {
            refreshSliderLeft();
            if (_self.SliderLeft > 0) {
                var _hours = _self.Hours(),
                    first = _hours[0];
                _self.Hours.pop();
                var myDate = new Date(first.getFullYear(), first.getMonth(), first.getDate(), first.getHours() - 1);
                _self.Hours.unshift(myDate);
                offset += 60;
            }
            var right = (_self.SliderLeft + _self.Slider.width()) - timelineWidth;
            if (right < 60) {
                var _hours = _self.Hours(),
                    last = _hours[_hours.length - 1];
                _self.Hours.unshift();
                var date = new Date(last.getFullYear(), last.getMonth(), last.getDate(), last.getHours() + 1);
                _self.Hours.push(date);
            }
            _self.Slider.css("left", _self.Helper.position().left - offset + "px");
            _self.Write();
        },
        stop: function () {
            _self.Helper.css("left", "0px");
        }
    });
}

Timeline.prototype = {
    Read: function () {
        if (this.Model.Start() !== this.StartCheck) {
            var start = this.Model.Start(),
                hours = Array
                            .range(-12, 12)
                            .map(function (val) {
                                return new Date(
                                    start.getFullYear(),
                                    start.getMonth(),
                                    start.getDate(),
                                    start.getHours() + val
                                );
                            });
            this.Hours(hours);
            this.Viewport.css("left", "0px");
            var left = 339,
                minutes = start.getMinutes(),
                back = 12 * 60,
                all = left - minutes - back;
            this.Slider.css("left", all + "px");
        }
    },
    Write: function () {
        if (!this.Queue) {
            this.Queue = true;
            window.setTimeout(function () {
                var minutes = (this.Viewport.position().left - this.SliderLeft) % 60,
                    hourOffset = Math.floor((this.Viewport.position().left - this.SliderLeft) / 60),
                    _hours = this.Hours(),
                    start = new Date(_hours[0].getTime() + (minutes * 60 * 1000) + (hourOffset * 60 * 60 * 1000)),
                    end = new Date(start);
                end.setHours(start.getHours() + Math.floor(this.Viewport.width() / 60));
                this.StartCheck = start;
                this.Model.Start(start);
                this.Model.End(end);
                this.Queue = false;
            }.bind(this), 50);
        }
    }
};

ko.bindingHandlers.timeline = {
    init: function (element, valueAccessor) {
        window.setTimeout(function(){
            var timeline = new Timeline(element, ko.utils.unwrapObservable(valueAccessor()));
        }, 0);
    }
}

//var hours;
//ko.bindingHandlers.timeline = {
//    init: function (element, valueAccessor, allBindingsAccessor) {
//        var model = ko.utils.unwrapObservable(valueAccessor());

//        function setHours(date) {
//            hours = Array.range(-12, 12);
//            hours = hours.map(function (val) {
//                var newDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + val, 0, 0, 0);
//                return newDate;
//            });
//            hours = ko.observableArray(hours);
//        }
//        setHours(model.Start());
        

//        function ui() {
//            ko.renderTemplate("Timeline", { Hours: hours, Timespan: model }, {}, element)
//            var jEl = $(element),
//                timeline = jEl.find(".Timeline"),
//                slider = jEl.find(".Slider"),
//                helper = jEl.find(".Helper"),
//                timelineWidth = timeline.width(),
//                viewport = jEl.find(".Viewport"),
//                offset,
//                sliderLeft,
//                date = model.Start();
//            slider.css("left", -(21 + date.getMinutes() + (6 * 60)) + "px");

//            function dateSetFromWithout() {
//                date = model.Start();
//                setHours(model.Start());
//                slider.css("left", -(21 + date.getMinutes() + (7 * 60)) + "px");
//                viewport.css("left", "0px");
//            }

//            model.BackdoorStart.subscribe(function (val) {
//                dateSetFromWithout();
//            });

//            var queue = false;
//            function update() {
//                if (!queue) {
//                    queue = true;
//                    window.setTimeout(function () {
//                        var minutes = (viewport.position().left - sliderLeft) % 60,
//                            hourOffset = Math.floor((viewport.position().left - sliderLeft) / 60),
//                            _hours = hours(),
//                            first = _hours[0],
//                            start = new Date(first.getTime() + (minutes * 60 * 1000) + (hourOffset * 60 * 60 * 1000));
//                        var end = new Date(start);
//                        end.setHours(start.getHours() + Math.floor(viewport.width() / 60));
//                        model.Start(start);
//                        model.End(end);
//                        queue = false;
//                    }, 50);
//                }
//            }
//            sliderLeft = slider.position().left;

//            jEl.css({ "text-align": "center" });

//            viewport.draggable({
//                axis: "x",
//                containment: "parent",
//                start: function () {
//                    sliderLeft = slider.position().left;
//                },
//                drag: update
//            });
//            var timeline = "gooch";
//            helper.draggable({
//                axis: "x",
//                start: function (e, ui) {
//                    timelineWidth = $(".Timeline").width();
//                    offset = helper.position().left - slider.position().left;
//                },
//                drag: function () {
//                    sliderLeft = slider.position().left;
//                    if (sliderLeft > 0) {
//                        var _hours = hours(),
//                            first = _hours[0];
//                        hours.pop();
//                        var myDate = new Date(first.getFullYear(), first.getMonth(), first.getDate(), first.getHours() - 1);
//                        hours.unshift(myDate);
//                        offset += 60;
//                    }
//                    var right = (sliderLeft + slider.width()) - timelineWidth;
//                    if (right < 60) {
//                        var _hours = hours(),
//                            last = _hours[_hours.length - 1];
//                        hours.unshift();
//                        var date = new Date(last.getFullYear(), last.getMonth(), last.getDate(), last.getHours() + 1);
//                        hours.push(date);
//                    }

//                    slider.css("left", helper.position().left - offset + "px");
//                    update();
//                },
//                stop: function () {
//                    helper.css("left", "0px");
//                }
//            });

//        }

//        window.setTimeout(ui, 0);

//    },
//    update: function (element, valueAccessor, allBindingsAccessor) { }
//}

ko.bindingHandlers.map = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var model = ko.utils.unwrapObservable(valueAccessor()),
            subscription;
        model.Map = new google.maps.Map(element, {
            center: model.Center() ? model.Center() : new google.maps.LatLng(40.758, -73.917),
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            minZoom: 10,
            zoom: 5
        });
        model.InfoWindow = new google.maps.InfoWindow({});
        google.maps.event.addListener(model.Map, "tilesloaded", function () {
            model.Initialize.call(model);
        });
        subscription = model.Center.subscribe(function (center) {
            console.log("Centering");
            model.Map.setCenter(center);
        });
    }
}