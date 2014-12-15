define(['jquery', 'config'], function (jquery, config) {
    function MasterLoader() {
        //Singleton implementation        
        if (arguments.callee._singletonInstance)
            return arguments.callee._singletonInstance;
        arguments.callee._singletonInstance = this;
    };

    //store Sammy context
    MasterLoader.prototype.setContext = function (ctx) {
        this.ctx = ctx;
    };

    //set master page if needed, required to call by viewmodel
    MasterLoader.prototype.set = function (name) {
        self = this;
        if (name === undefined)
            name = "Default";
        if (self.name != name) {
            self.name = name;
            self.load();
        } else {
            require([self.name + 'Master'], function (M) {
                self.prepared(new M(self.ctx));
            });
        }
        
    };    

    //load master page and master's js
    MasterLoader.prototype.load = function () {
        var self = this;
        jquery('body').load(config.masterHtmlPath.replace('{name}', self.name), function () {
            require([self.name + 'Master'], function (M) {                
                self.prepared(new M(self.ctx));
            });
        });
    };

    //stub for call back method
    MasterLoader.prototype.prepared = function () { };

    return MasterLoader;
});