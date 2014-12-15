define(['knockout', 'jquery' ,'helper'], function (ko, jquery, helper) {



    //Viewmodel definition
    function Dev(ctx) {
        this.master = 'Registered';
        this.auth = true;
        this.devLogsUrl = '/api';
    };

    //Loading views and binding data
    Dev.prototype.loaded = function (ctx) {
        helper.loadView(ctx, '#content', 'Log');
    };

    return Dev;
});