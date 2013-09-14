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

var hours;
ko.bindingHandlers.timeline = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var model = ko.utils.unwrapObservable(valueAccessor());

        hours = Array.range(-12, 12);
        var date = model.Start();
        hours = hours.map(function (val) {
            var newDate = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours() + val, 0, 0, 0);
            return newDate;
        });
        hours = ko.observableArray(hours);


        function ui() {
            ko.renderTemplate("Timeline", { Hours: hours, Timespan: model }, {}, element)
            var jEl = $(element),
                timeline = jEl.find(".Timeline"),
                slider = jEl.find(".Slider"),
                helper = jEl.find(".Helper"),
                timelineWidth = timeline.width(),
                viewport = jEl.find(".Viewport"),
                offset,
                sliderLeft,
                date = model.Start();
            slider.css("left", -(21 + date.getMinutes() + (6 * 60)) + "px");

            var queue = false;
            function update() {
                if (!queue) {
                    queue = true;
                    window.setTimeout(function () {
                        var minutes = (viewport.position().left - sliderLeft) % 60,
                            hourOffset = Math.floor((viewport.position().left - sliderLeft) / 60),
                            _hours = hours(),
                            first = _hours[0],
                            start = new Date(first.getTime() + (minutes * 60 * 1000) + (hourOffset * 60 * 60 * 1000));
                        var end = new Date(start);
                        end.setHours(start.getHours() + Math.floor(viewport.width() / 60));
                        model.Start(start);
                        model.End(end);
                        queue = false;
                    }, 50);
                }
            }
            sliderLeft = slider.position().left;

            jEl.css({ "text-align": "center" });

            viewport.draggable({
                axis: "x",
                containment: "parent",
                start: function () {
                    sliderLeft = slider.position().left;
                },
                drag: update
            });
            var timeline = "gooch";
            helper.draggable({
                axis: "x",
                start: function (e, ui) {
                    timelineWidth = $(".Timeline").width();
                    offset = helper.position().left - slider.position().left;
                },
                drag: function () {
                    sliderLeft = slider.position().left;
                    if (sliderLeft > 0) {
                        var _hours = hours(),
                            first = _hours[0];
                        hours.pop();
                        var myDate = new Date(first.getFullYear(), first.getMonth(), first.getDate(), first.getHours() - 1);
                        hours.unshift(myDate);
                        offset += 60;
                    }
                    var right = (sliderLeft + slider.width()) - timelineWidth;
                    if (right < 60) {
                        var _hours = hours(),
                            last = _hours[_hours.length - 1];
                        hours.unshift();
                        var date = new Date(last.getFullYear(), last.getMonth(), last.getDate(), last.getHours() + 1);
                        hours.push(date);
                    }

                    slider.css("left", helper.position().left - offset + "px");
                    update();
                },
                stop: function () {
                    helper.css("left", "0px");
                }
            });

        }

        window.setTimeout(ui, 0);

    },
    update: function (element, valueAccessor, allBindingsAccessor) { }
}

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