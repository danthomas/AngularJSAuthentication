(function () {
    var app = angular.module('app', ['ui.router', 'LocalStorageModule', 'angular-loading-bar']);

    app.config(function ($stateProvider, $urlRouterProvider, $httpProvider) {

        $httpProvider.interceptors.push('authInterceptorService');

        $stateProvider.state('login', { url: '/login', templateUrl: 'Templates/Login.html', controller: 'loginController' });
        $stateProvider.state('home', { url: '/home', templateUrl: 'Templates/Home.html', controller: 'homeController' });
        $stateProvider.state('accountList', { url: '/accountList', templateUrl: 'Templates/AccountList.html', controller: 'accountListController' });

        $urlRouterProvider.otherwise('home');
    });

    app.value('globalData', {
        isAuth: false,
        userName: ""
    });

    app.run(function (accountService) {
        accountService.fillAuthData();
    });
})();