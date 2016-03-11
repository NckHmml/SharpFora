app.run([$http, function ($http) {
    // Set the X-Requested-With so that ASP Identity wont redirect us, and instead sends a plain 401.
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest"; 
}]);