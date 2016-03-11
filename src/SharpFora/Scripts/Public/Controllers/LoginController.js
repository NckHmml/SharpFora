app.controller("LoginController", [$scope, "TOTP", function ($scope, totp) {
    totp.success(function (data) {
        $scope.totp = "otpauth://totp/Example:alice@google.com?secret=" + data.token + "&issuer=Example";
    });
}]);