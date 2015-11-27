(function () {
    angular.module('app').controller('accountListController', function ($scope, $state, accountService) {
        accountService.list().then(function (response) {
            
        });
    });
})();