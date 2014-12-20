//Scripts entry point
requirejs.config({
    baseUrl: 'js',

    //All links to files goes here
    paths: {

        //libs
        knockout: 'Knockout',
        validation: 'KnockoutValidation',
        mapping: 'KnockoutMapping',
        jquery: 'jQuery',
        jstorage: 'jStorage',
        sammy: 'Sammy',

        //application
        app: 'App',
        serverConfig: '/Settings',
        config: 'Config',
        helper: 'Helper',
        auth: 'Auth',
        masterLoader: 'MasterLoader',
        

        //viewmodels
        Default: 'Default',
        Login: 'Login',
        Wall: 'Wall',
        Dev: 'Dev',

        //masters
        DefaultMaster: 'DefaultMaster',
        RegisteredMaster: 'RegisteredMaster'
    }
});

//Load primary app class
require(['app']);
