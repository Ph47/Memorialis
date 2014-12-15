define(['knockout', 'jquery' ,'helper'], function (ko, jquery, helper) {
        
    //Viewmodel definition
    function Wall(ctx) {
        this.master = 'Registered';
        this.auth = true;
    };

    //Loading views and binding data
    Wall.prototype.loaded = function (ctx) {
        helper.loadView(ctx, '#content', 'Default', null);
    };

    return Wall;
});