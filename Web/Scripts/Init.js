//Scripts entry point
requirejs.config({
    baseUrl: 'Scripts',

    //All links to files goes here
    paths: {

        //libs
        knockout: 'Lib/knockout-3.1.0.min',
        validation: 'Lib/knockout.validation-2.0.0.min',
        mapping: 'Lib/knockout.mapping-2.4.1.min',
        jquery: 'Lib/jquery-2.1.0.min',
        //Modernizr: 'Lib/modernizr-2.8.2', //Modernizr source modified to work with requirejs
        jstorage: 'Lib/jstorage-0.4.11',
        sammy: 'Lib/sammy-0.7.5',

        //application
        app: 'App/app',
        serverConfig: '/Settings',
        config: 'App/config',
        helper: 'App/helper',
        auth: 'App/auth',
        masterLoader: 'App/MasterLoader',
        

        //viewmodels
        Default: 'Vm/Default',
        Login: 'Vm/Login',
        Wall: 'Vm/Wall',
        Dev: 'Vm/Dev',

        //masters
        DefaultMaster: 'Master/DefaultMaster',
        RegisteredMaster: 'Master/RegisteredMaster'
    }
});

//Load primary app class
require(['app']);
