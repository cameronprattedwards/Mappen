function Jumper() {
    this.Address = ko.observable();
}

Jumper.prototype = {
    Jump: function (mapp) {
        mapp.Jump(this.Address());
    }
}