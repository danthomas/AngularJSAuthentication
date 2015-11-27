(function () {
    var app = angular.module('app');

    app.factory('accountService', function ($http, $q, $state, localStorageService, globalData) {

        var logout = function () {

            localStorageService.remove('authorizationData');

            globalData.isAuth = false;
            globalData.userName = "";

            var deferred = $q.defer();

            deferred.resolve();

            return deferred.promise;
        };

        var login = function (model) {
            var data = "grant_type=password&username=" + model.username + "&password=" + model.password;

            var deferred = $q.defer();

            $http.post('/token', data,
            {
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (response) {

                localStorageService.set('authorizationData', { token: response.access_token, username: model.username });

                globalData.isAuth = true;
                globalData.userName = model.username;

                deferred.resolve(response);

            }).error(function (err) {
                logout();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        var list = function (data) {

            var deferred = $q.defer();

            $http.post('/api/account/list', data,
                {
                }).success(function () {

                }).error(function (err) {
                    deferred.reject(err);
                });

            return deferred.promise;
        };

        var fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                globalData.isAuth = true;
                globalData.userName = authData.userName;
            }
        };

        var obtainAccessToken = function (externalData) {

            var deferred = $q.defer();

            $http.get('api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

                localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName });

                globalData.isAuth = true;
                globalData.userName = response.userName;
                
                deferred.resolve(response);

            }).error(function (err) {
                logout();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        return {
            login: login,
            logout: logout,
            list: list,
            fillAuthData: fillAuthData,
            obtainAccessToken: obtainAccessToken
        };
    });
})();