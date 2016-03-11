// HACK: we ask for $injector instead of $compile, to avoid circular dep
app.factory("$templateCache", ["$cacheFactory", $http, "$injector", function ($cacheFactory, $http, $injector) {
    var cache = $cacheFactory("templates");
    var allTemplatePromise;

    return {
        get: function (url) {
            var fromCache = cache.get(url);

            // already have required template in the cache
            if (fromCache) {
                return fromCache;
            }

            // first template request ever - get the all template files
            if (!allTemplatePromise) {
                allTemplatePromise = $http.get("/Home/Templates").then(function (response) {
                    // compile the response, which will put everything into the cache
                    $injector.get("$compile")(response.data);
                    return response;
                });
            }

            // return the allTemplatePromise promise to all template requests
            return allTemplatePromise.then(function (response) {
                return {
                    status: response.status,
                    data: cache.get(url),
                    headers: response.headers
                };
            });
        },

        put: function (key, value) {
            cache.put(key, value);
        }
    };
}]);
