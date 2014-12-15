define([], function () {
   
    function RegisteredMaster() {
        //Singleton implementation        
        if (arguments.callee._singletonInstance)
            return arguments.callee._singletonInstance;
        arguments.callee._singletonInstance = this;
               
    };
    RegisteredMaster.prototype.loaded = function (ctx) {
        
    };
    return RegisteredMaster;
});