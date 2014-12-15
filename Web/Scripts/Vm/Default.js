define(['knockout', 'mapping', 'jquery', 'helper', 'auth', 'config'], function (ko, mapping, jquery, helper, Auth, config) {
    

    function RegisterData() {
        this.name = ko.observable('');
        this.password1 = ko.observable('');
        this.password2 = ko.observable('');

        this.register = function () {
           // var a = this.ctx;
        };
    };

    //Viewmodel definition
    function Default(ctx) {
        self = this;

        self.master = "Default";
        self.auth = false;
        self.data = null;

        this.registerData =  {
            Password:  ko.observable(''),
            Confirm: ko.observable(''),
            register: function () {
                jquery.ajax({
                    type: 'PUT',
                    url: config.apiUrl + 'Identity',
                    data: ko.mapping.toJS(self.registerData),
                    success: function (data, textStatus, xhr) {
                        alert('47');
                    }
                });
            }
        }
    };

    //Loading views and binding data
    Default.prototype.loaded = function (ctx) {
        ko.mapping = mapping;
        self = this;
        jquery.ajax({
            type: 'GET',
            url: config.apiUrl+ 'Identity',
            success: function (data, textStatus, xhr) {
                self.rawData = data;
                ko.mapping.fromJS(self.rawData, {}, self.registerData);
                var a = self
                helper.loadView(ctx, '#content', 'Register', self.registerData);
            }
        });
        //helper.loadView(ctx, '#login', 'Login', new LoginData(ctx));
        
    };


    return Default;
});