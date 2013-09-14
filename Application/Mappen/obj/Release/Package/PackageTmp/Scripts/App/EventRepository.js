var EventRepository = function () { }

EventRepository.prototype = (function(){
    var myDeferred = function(deferred){
        return $.Deferred()
            .done(function (data) {
                deferred.resolve(data.map(function (event) {
                    return MEvent.FromJson(event);
                }));
            })
            .fail(function (errors) {
                deferred.reject(errors);
            });
    }

    return{
        WherePlace: function (geocode, time, deferred) {
            var data = {
                lat: geocode.lat(),
                lng: geocode.lng(),
                time: time.AspFriendly()
            };
            API.GET("Events/me/Circle", data, myDeferred(deferred));
        },
        WhereTerm: function (term, geocode, deferred) {
            var data = {
                q: term,
                lat: geocode.lat(),
                lng: geocode.lng()
            };
            API.GET("Events/me/Search", data, myDeferred(deferred));
        }
    }
}());