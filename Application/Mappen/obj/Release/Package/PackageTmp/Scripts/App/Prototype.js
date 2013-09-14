if (!window.history.pushState)
    window.history.pushState = function () { }

/* Array filter */

if (!Array.prototype.filter)
{
  Array.prototype.filter = function(fun /*, thisp*/)
  {
    "use strict";
 
    if (this == null)
      throw new TypeError();
 
    var t = Object(this);
    var len = t.length >>> 0;
    if (typeof fun != "function")
      throw new TypeError();
 
    var res = [];
    var thisp = arguments[1];
    for (var i = 0; i < len; i++)
    {
      if (i in t)
      {
        var val = t[i]; // in case fun mutates this
        if (fun.call(thisp, val, i, t))
          res.push(val);
      }
    }
 
    return res;
  };
}

/* Array forEach */

if (!Array.prototype.forEach) {
    Array.prototype.forEach = function (fn, scope) {
        for (var i = 0, len = this.length; i < len; ++i) {
            fn.call(scope, this[i], i, this);
        }
    }
}

/* Array map*/
if (!Array.prototype.map) {
    Array.prototype.map = function (callback, thisArg) {
        var T, A, k;
        if (this == null) {
            throw new TypeError(" this is null or not defined");
        }
        var O = Object(this);
        var len = O.length >>> 0;
        if (typeof callback !== "function") {
            throw new TypeError(callback + " is not a function");
        }
        if (thisArg) {
            T = thisArg;
        }
        A = new Array(len);
        k = 0;
        while (k < len) {

            var kValue, mappedValue;
            if (k in O) {
                kValue = O[k];
                mappedValue = callback.call(T, kValue, k, O);
                A[k] = mappedValue;
            }
            k++;
        }

        return A;
    };
}

/* String trim */

if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    };
}

String.prototype.truncate = function (len) {
    var ellipses = this.length > len;
    return this.slice(0, len) + (ellipses ? "..." : "");
}

Array.prototype.remove = function (val) {
    while (this.indexOf(val) !== -1) {
        this.splice(this.indexOf(val), 1);
    }
    return val;
}

/* Console */

if (!console)
    var console = {
        log: function () { },
        dir: function () { }
    }

/* JSON static object */

if (!window.JSON) {
    window.JSON = {
        parse: function (sJSON) { return eval("(" + sJSON + ")"); },
        stringify: function (vContent) {
            if (vContent instanceof Object) {
                var sOutput = "";
                if (vContent.constructor === Array) {
                    for (var nId = 0; nId < vContent.length; sOutput += this.stringify(vContent[nId]) + ",", nId++);
                    return "[" + sOutput.substr(0, sOutput.length - 1) + "]";
                }
                if (vContent.toString !== Object.prototype.toString) { return "\"" + vContent.toString().replace(/"/g, "\\$&") + "\""; }
                for (var sProp in vContent) { sOutput += "\"" + sProp.replace(/"/g, "\\$&") + "\":" + this.stringify(vContent[sProp]) + ","; }
                return "{" + sOutput.substr(0, sOutput.length - 1) + "}";
            }
            return typeof vContent === "string" ? "\"" + vContent.replace(/"/g, "\\$&") + "\"" : String(vContent);
        }
    };
}
/* Math between */
Math.between = function (val,min,max) {
    return val >= min && val <= max;
}

/* Array range*/
Array.range = function (start,end) {
    var arr = [];
    for (i = start; i <= end; i++) {
        arr.push(i);
    }
    return arr;
}

Array.prototype.shuffle = function () {
    var out = [];
    for (var i = 0; i < this.length; i++) {
        out[i] = this[i];
    }
    var len = this.length,
        i = len;
    while (i--) {

        var p = parseInt(Math.random() * len);
        var t = out[i];
        out[i] = out[p];
        out[p] = t;
    }
    return out;
};

if (!Function.prototype.bind) {
    Function.prototype.bind = function (oThis) {
        if (typeof this !== "function") {
            // closest thing possible to the ECMAScript 5 internal IsCallable function
            throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
        }

        var aArgs = Array.prototype.slice.call(arguments, 1),
            fToBind = this,
            fNOP = function () { },
            fBound = function () {
                return fToBind.apply(this instanceof fNOP && oThis
                                       ? this
                                       : oThis,
                                     aArgs.concat(Array.prototype.slice.call(arguments)));
            };

        fNOP.prototype = this.prototype;
        fBound.prototype = new fNOP();

        return fBound;
    };
}

var pad = function (len) {
    var diff = 1 + len - this.length;
    if (diff > 0)
        return new Array(diff).join("0") + this;
}

String.prototype.pad = pad;


(function () {
    Date.FromJson = function (str) {
        if (str.search("Date") !== -1)
            return new Date(parseInt(/-?\d+/.exec(str)[0]))
        else
            return new Date(str);
    }
    Date.prototype.StandardHours = function () {
        var hours = this.getHours();
        return hours == 0 ? 12 : hours > 12 ? hours - 12 : hours;
    }
    Date.prototype.Meridiem = function () {
        var hours = this.getHours();
        return hours < 12 ? "am" : "pm";
    }
    Date.prototype.HourFormat = function () {
        return this.StandardHours() + this.Meridiem();
    }

    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

    Date.prototype.StrDay = function () {
        return days[this.getDay()];
    }
    Date.prototype.ShortStrDay = function () {
        var strd = this.StrDay();
        if (!strd) {
            console.log("No Strd", this.getDay());
            return "";
        }
        return strd.slice(0, 3);
    }
    Date.prototype.StrMonth = function () {
        return months[this.getMonth()];
    }
    Date.prototype.ShortStrMonth = function () {
        var strm = this.StrMonth();
        if (!strm) {
            console.log("No Strm");
            return "";
        }
        return strm.slice(0, 3);
    }
    Date.prototype.MinuteFormat = function () {
        var hrs = this.StandardHours();
        var mins = this.getMinutes();
        mins = mins.toString();
        mins = mins.pad(2);

        var output = this.StandardHours() + ":" + this.getMinutes().toString().pad(2) + this.Meridiem();
        return output;
    }
    Date.prototype.DateFormat = function () {
        return this.ShortStrDay() + " " + this.getDate() + " " + this.ShortStrMonth() + " " + this.getFullYear();
    }
    Date.prototype.ToJson = function () {
        return "/Date(" + this.getTime().toString() + ")/";
    }
    Date.prototype.AspFriendly = function () {
        return (this.getMonth() + 1).toString().pad(2) + "/" + this.getDate().toString().pad(2) + "/" + this.getFullYear() + " " + this.getHours().toString().pad(2) + ":" + this.getMinutes().toString().pad(2) + ":" + this.getSeconds().toString().pad(2);
    }
    Date.prototype.IsToday = function () {
        var newDate = new Date();
        return this.getDate() == newDate.getDate() && this.getMonth() == newDate.getMonth() && this.getFullYear() == newDate.getFullYear();
    }
}());

google.maps.LatLng.prototype.ToJson = function () {
    var _self = this;
    return {
        Lat: _self.lat(),
        Lng: _self.lng()
    }
}

google.maps.LatLng.FromJson = function (json) {
    return new google.maps.LatLng(json.Lat, json.Lng);
}

google.maps.LatLngBounds.prototype.ToJson = function () {
    var _self = this;
    return {
        NorthEast: _self.getNorthEast().ToJson(),
        SouthWest: _self.getSouthWest().ToJson()
    }
}

google.maps.LatLngBounds.prototype.Copy = function () {
    return new google.maps.LatLngBounds(this.getSouthWest(), this.getNorthEast());
}

google.maps.LatLngBounds.FromJson = function (json) {
    var ne = google.maps.LatLng.FromJson(json.NorthEast),
        sw = google.maps.LatLng.FromJson(json.SouthWest);
    return new google.maps.LatLngBounds(sw, ne);
}

var eventMarkers = {};
google.maps.Marker.FromEvent = function (event) {
    var id = event.Id();
    if (eventMarkers[id])
        return eventMarkers[id];

    var options = {
        position: event.Venue.Address.Geocode(),
        title: event.Info.Name()
    },
    category = event.Info.Category();
    if (category)
        options.icon = "/Images/Markers/" + category + ".png";
    

    var marker = new google.maps.Marker(options);
    event.Venue.Address.Geocode.subscribe(function (val) {
        marker.setPosition(val);
    });
    event.Info.Name.subscribe(function (val) {
        marker.setTitle(val);
    });
    var el = document.createElement("div");
    ko.renderTemplate("MarkerSummary", event, {}, el);
    marker.content = el;

    eventMarkers[id] = marker;

    return marker;
}

Array.prototype.into = function (callback) {
    var output = [];
    for (var i = 0; i < this.length; i++)
        output = output.concat(callback(this[i]));
    return output;
}