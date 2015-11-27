(function () {
    var app = angular.module('app');

    app.factory('authInterceptorService', function ($q, $location, localStorageService) {

        var request = function (config) {

            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');

            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }

        var responseError = function (rejection) {
            if (rejection.status === 401) {
                //can't use $state as it gives circular reference error
                $location.path('/login');
            }

            return $q.reject(rejection);
        }

        return {
            request: request,
            responseError: responseError
        };
    });
})();