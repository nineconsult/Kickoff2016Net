define(function() {
    return {
        load: function(name, req, onload, config) {
            const url = abp.appPath + "api/AbpServiceProxies/Get?name=" + name;
            req([url], function(value) {
                onload(value);
            });
        }
    };
});