app.factory("Topics", [$http, function ($http) {
    return {
        top: function (amount) {
            return $http
                .get("")
                .success(function (data) {
                    return data;
                })
                .failure(function (er) {
                    return er;
                });
        }
    }
}]);