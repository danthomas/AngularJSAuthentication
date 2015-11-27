(function () {
    angular.module('app').controller('appController', function ($scope, $state, accountService, globalData) {

        $scope.globalData = globalData;

        $scope.goto = function(state) {
            $state.go(state);
        };

        $scope.logout = function () {
            accountService.logout().then(function () {
                $state.go('home');
            });
        };

        $scope.login = function () {
            $state.go('login');
        };

        $scope.hide = function (isAuth) {
            return globalData.isAuth === isAuth;
        };
    });
})();