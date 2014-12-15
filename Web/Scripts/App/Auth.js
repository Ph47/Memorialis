define(['config', 'jquery', 'jstorage', 'helper'], function (config, jquery, jstorage, helper) {
    function Auth() {

        //Singleton implementation
        if (arguments.callee._singletonInstance)
            return arguments.callee._singletonInstance;
        arguments.callee._singletonInstance = this;
        self = this;

        //store dictionary keys
        this.accessTokenKey = 'accessToken';
        this.refreshTokenKey = 'refreshToken';
    };
  
    //Check if authenticated (client-side)
    Auth.prototype.isAuthenticated = function () {
        return (jquery.jStorage.get(this.accessTokenKey) ? true : false);
    }

    //Session restore if refresh token present
    Auth.prototype.ContinueSession = function (callback) {
        token = jquery.jStorage.get(this.refreshTokenKey);
        if (!token) {
            callback(false);
            return;
        }        
        jquery.ajax({
            type: 'GET',
            url: config.apiRefreshTokenUrl.replace('{code}', encodeURIComponent(jquery.jStorage.get((new Auth()).refreshTokenKey))),
            success: function (tokens) {
                (new Auth()).ReciveTokens(tokens.access_token, tokens.expires_in, tokens.refresh_token);
                callback(true);
            },
            error: function () {
                callback(false);
            }
        });
    }

    //Store tokens in dictionary
    Auth.prototype.ReciveTokens = function (accessToken, accesTokenExpire, refreshToken) {
        //store tokens
        jquery.jStorage.set(this.accessTokenKey, accessToken, { TTL: accesTokenExpire * 1000 /*seconds to milliseconds*/ });
        jquery.jStorage.set(this.refreshTokenKey, refreshToken);

        //configure bearer
        jquery.ajaxSetup({
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
            }
        });

        //set refresh timeout
        var timeout = accesTokenExpire * 1000 - 60000;
        setTimeout(function () {
            jquery.ajax({
                type: 'GET',
                url: config.apiRefreshTokenUrl.replace('{code}', encodeURIComponent(jquery.jStorage.get((new Auth()).refreshTokenKey))),
                success: function (tokens) {
                    (new Auth()).ReciveTokens(tokens.access_token, tokens.expires_in, tokens.refresh_token);
                }
            });
        }, timeout)
    }

    //OAuth2 framework authentication
    Auth.prototype.Authenticate = function (userLogin, userPassword, callback) {

        //Authorize endpoint request
        jquery.ajax({
            type: 'POST',
            url: config.oauthAuthorizeUrl,
            data: {
                login: userLogin,
                password: userPassword
            },
            dataType: 'json',
            success: function (data, textStatus) {
                if (data.code) {
                    //Resource server token request
                    jquery.ajax({
                        type: 'GET',
                        //insert authorization code
                        url: config.apiGetTokenUrl.replace('{code}', encodeURIComponent(data.code)),
                        success: function (tokens) {
                            (new Auth()).ReciveTokens(tokens.access_token, tokens.expires_in, tokens.refresh_token);
                            callback(true);
                        },
                        error: function () {
                            callback(false);
                        }
                    });
                } else {
                    callback(false);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                callback(false);
            }
        });
    }
    
    return Auth;
});