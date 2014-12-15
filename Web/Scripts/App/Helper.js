define(['knockout', 'jquery','config'], function (ko, jquery, config) {
    function Helper() {
        //Singleton implementation
        if (arguments.callee._singletonInstance)
            return arguments.callee._singletonInstance;
        arguments.callee._singletonInstance = this;


    };

    Helper.prototype.getParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&#]" + name + "=([^&#]*)"),
            results = regex.exec(window.location);
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    Helper.prototype.loadView = function (ctx, selector, view, model) {
        jquery(selector).load(config.htmlPath + '/' + ctx.viewName + '/' + view + '.html', function () {
            jquery(selector).removeClass().addClass(ctx.viewName.camelize() + view);
            ko.cleanNode(jquery(selector)[0]);
            ko.applyBindings(model, jquery(selector)[0]);
        });
    }
    
    Helper.prototype.masterLoadView = function (ctx, selector, view, model) {
        jquery(selector).load(config.htmlPath + '/Master/' + view + '.html', function () {
            jquery(selector).removeClass().addClass('Master'.camelize() + view);
            ko.cleanNode(jquery(selector)[0]);
            ko.applyBindings(model, jquery(selector)[0]);
        });
    }

    String.prototype.camelize = function () {
        return this.replace (/(?:^|[-])(\w)/g, function (c) {
            return c ? c.toLowerCase () : '';
        });
    }

    return new Helper();
});