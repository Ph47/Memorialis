define(['jquery', 'auth', 'sammy', 'helper','masterLoader'], function (jquery, Auth, sammy, helper, MasterLoader) {
    function App() {
        //Singleton implementation        
        if (arguments.callee._singletonInstance)
            return arguments.callee._singletonInstance;
        arguments.callee._singletonInstance = this;        

        this.auth = new Auth();        

        //routing configuration        
        this.sammy = new sammy(this.Route);

        self = this;
        //run router on document ready
        jquery(this.RestoreSession);
               
    };

    App.prototype.RestoreSession = function () {
        (new Auth()).ContinueSession(self.RunRouter);
    };

    App.prototype.RunRouter = function (success) {
        self.sammy.run();
    };

    //Routing rules
    App.prototype.Route = function () {
        //        this.get('#/:vm', function () {
        this.get(/\/#\/([\w\d-]+)(.*)/g, function () {
            var ctx = this;
            
            //get view name
            ctx.viewName = this.params.splat[0];

            //load approproative view script
            require([ctx.viewName], function (Vm) {
                var app = new App();
                var self = ctx;

                //init master
                self.masterLoader = new MasterLoader();
                self.masterLoader.setContext(self);

                //init viewmodel
                self.viewModel = new Vm(self);

                //authenticate if view required
                if (self.viewModel.auth && !app.auth.isAuthenticated()) {
                    if (self.params.redirect == undefined) {
                        self.redirect('#/Default?redirect=' + encodeURIComponent(self.path.substring(1)));
                    } else {
                        self.redirect('#/Default?redirect=' + self.params.redirect);
                    }
                    
                }

                //master load callback
                self.masterLoader.prepared = function (master) {
                    self.master = master;
                    self.master.loaded(self);
                    self.viewModel.loaded(self);
                }

                //set master
                self.masterLoader.set(self.viewModel.master);
            });
        });

        //default route
        //TODO: make somethink like 404
        this.get(/.*/, function () {
            if (!(new App()).auth.isAuthenticated()) {
                this.redirect('#/Default');
            } else {
                this.redirect('#/Wall');
            }
        });
    }      

    var app = new App();

    //Create App instance
    return App();
});