define('config', ['serverConfig'], function (serverConfig) {
    this.server = serverConfig;

    this.oauthAuthorizeUrl =
        this.server.authorizeEndpointPath +
        '?response_type=code&client_id=' +
        this.server.clientId +
        '&redirect_uri=' +
        encodeURIComponent(this.server.projectUrl + this.server.echoUrl.substring(1));

    this.apiGetTokenUrl = '/api/Token/Get?code={code}';
    this.apiRefreshTokenUrl = '/api/Token/Refresh?token={code}';
    this.apiUrl = '/api/';
    this.htmlPath = '/Static/Html';
    this.masterHtmlPath = htmlPath + '/Master/{name}.html';
    


    return this;
});