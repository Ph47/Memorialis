define('auth',['jquery'], function ($) {
    function Auth(){
        this.tokenEndpoint = "https://id.memorialis.efe/connect/token";
        this.clientId = "memorialis";
        this.token = null;
    };
    Auth.prototype.Authenticate = function (login, password) {
        $.ajax({
            url: this.tokenEndpoint,
            type: "POST",
            data: {
                client_id: this.clientId,
                client_secret: '47',
                grant_type: 'password',
                scope: 'openid',
                username: login,
                password: password
            }
        });
        alert(login);
        return true;
    };
    return Auth;
});