define(['knockout', 'jquery' ,'helper'], function (ko, jquery, helper) {
        
    //Viewmodel definition
    function Login(ctx) {
        this.master = 'Default';
        this.auth = false;
    };

    //Loading views and binding data
    Login.prototype.loaded = function (ctx) {
        helper.loadView(ctx, '#content', 'Default', null);
    };

    return Login;
});