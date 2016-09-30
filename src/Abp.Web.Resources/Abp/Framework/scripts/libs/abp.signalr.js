var abp = abp || {};
(function($) {

    //Check if SignalR is defined
    if (!$ || !$.connection) {
        return;
    }

    //Create namespaces
    abp.signalr = abp.signalr || {};
    abp.signalr.hubs = abp.signalr.hubs || {};

    //Get the common hub
    abp.signalr.hubs.common = $.connection.abpCommonHub;

    var commonHub = abp.signalr.hubs.common;
    if (!commonHub) {
        return;
    }

    //Register to get notifications
    commonHub.client.getNotification = function(notification) {
        abp.event.trigger("abp.notifications.received", notification);
    };

    //Connect to the server
    abp.signalr.connect = function() {
        $.connection.hub.start().done(function() {
            
            abp.event.trigger("abp.signalr.connected");
            commonHub.server.register().done();
        });
    };

    if (abp.signalr.autoConnect === undefined) {
        abp.signalr.autoConnect = true;
    }

    if (abp.signalr.autoConnect) {
        abp.signalr.connect();
    }

})(jQuery);