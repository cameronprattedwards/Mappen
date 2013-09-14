function Jumper() {
    this.Address = ko.observable();
    this.Date = ko.observable(new Date());
    this.Date.subscribe(function (val) {
        app.Mapp.Jump(val);
    });
}

Jumper.prototype = {
    Jump: function (mapp) {
        mapp.Jump(this.Address());
    }
}