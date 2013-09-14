var API;
(function () {
    var request = function (type, url, data, deferred) {
        console.log(arguments);
        var obj = {
            url: "/api/" + url + (type.toLowerCase() == "get" ? "?" + $.param(data) : ""),
            type: type,
            data: type.toLowerCase() !== "get" ? JSON.stringify(data) : null,
            contentType: "application/json",
            success: function (response) {
                console.log("Request Success: ", response);
                if (response.Status.Status == "Success") {
                    deferred.resolve(response.Data);
                } else {
                    deferred.reject(response.Status.Messages);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                deferred.reject([errorThrown]);
            },
            complete: function (jqXHR, statusText) {
                console.log("Request complete: ", jqXHR, statusText);
            }
        }
        console.log("Requesting", obj);
        return $.ajax(obj);
    };

    var args = function (arr, type) {
        var output = [type];
        for (var i = 0; i < arr.length; i++) {
            output.push(arr[i]);
        }
        return output;
    }

    API = {
        POST: function () { request.apply(this, args(arguments, "POST")); },
        GET: function () { request.apply(this, args(arguments, "GET")); },
        PUT: function () { request.apply(this, args(arguments, "PUT")); },
        DELETE: function () { request.apply(this, args(arguments, "DELETE")); }
    }
}());