define(['knockout','auth'], function (ko, auth) {
    return function app() {
        this.login = ko.observable("");
        this.password = ko.observable("");
        this.authenticate = function () {
            (new auth()).Authenticate(this.login(), this.password());
        };
        
    };
});