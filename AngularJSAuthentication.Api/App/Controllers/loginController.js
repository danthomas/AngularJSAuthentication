(function () {
    angular.module('app').controller('loginController', function ($scope, $location, $state, accountService) {
        $scope.model = {
            username: 'dan',
            password: 'password'
        };

        $scope.login = function () {
            accountService.login($scope.model).then(function (response) {
                $state.go('home');
            });
        }

        $scope.authExternalProvider = function (provider) {

            var redirectUri = 'http://localhost:53768/authcomplete.html';//$location.protocol + '//' + $location.host + '/authcomplete.html';

            var externalProviderUrl = "api/account/externalLogin?provider=" + provider
                                                                        + "&response_type=token&client_id=" + 'xxx'
                                                                        + "&redirect_uri=" + redirectUri;
            window.$windowScope = $scope;

            var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
        };

        $scope.authCompletedCB = function(fragment) {

            $scope.$apply(function() {

                if (fragment.haslocalaccount == 'False') {

                    accountService.logout();

                    accountService.externalAuthData = {
                        provider: fragment.provider,
                        userName: fragment.external_user_name,
                        externalAccessToken: fragment.external_access_token
                    };

                    $location.path('/associate');

                } else {
                    //Obtain access token and redirect to orders
                    var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                    accountService.obtainAccessToken(externalData).then(function(response) {

                            $location.path('/accountList');

                        },
                        function(err) {
                            $scope.message = err.error_description;
                        });
                }

            });
        };
    });
})();