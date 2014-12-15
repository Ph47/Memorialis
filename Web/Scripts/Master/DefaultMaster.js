define(['knockout', 'helper', 'auth'], function (ko, helper, Auth) {
    "use strict";

    var me;
   
    function DefaultMaster() {
        //Singleton implementation        
        if (me)
            return me;
        me = this;



        this.loginData = {
            loginData: this.loginData,
            name: ko.observable(''),
            password: ko.observable(''),
            login: function () {
                (new Auth()).Authenticate(this.name(), this.password(), this.loginComplete);
            },
            loginComplete: function(success)
            {
                me.loginData.clean();
                var url = '#/';
                var redirect = me.ctx.params.redirect;
                if (success && redirect) url = redirect;
                if (!success) url += 'Login' + redirect ? ('?redirect=' + redirect) : '';               
                me.ctx.redirect(url);
            },
            clean: function () {
                this.name('');
                this.password('');
            }
        }
               
    };

    //Loading views and binding data
    DefaultMaster.prototype.loaded = function (ctx) {
        me.ctx = ctx;
        helper.masterLoadView(ctx, '#login', 'Login', this.loginData);
    };

   
    return DefaultMaster;
});