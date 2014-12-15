//Scripts entry point
requirejs.config({
    baseUrl: 'Scripts',

    //All links to files goes here
    paths: {
        knockout: 'lib/knockout-3.1.0.min',
        jquery: 'lib/jquery-2.1.0.min',
        app: 'app/app',
        auth: 'app/auth'
    }
});

//Load primary app class
require(['knockout', 'app'], function (ko, app) {
    ko.applyBindings(new app());
});