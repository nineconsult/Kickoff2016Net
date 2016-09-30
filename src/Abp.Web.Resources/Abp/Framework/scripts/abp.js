﻿var abp = abp || {};
(function ($) {

    /* Application paths *****************************************/

    //Current application root path (including virtual directory if exists).
    abp.appPath = abp.appPath || '/';

    abp.pageLoadTime = new Date();

    //Converts given path to absolute path using abp.appPath variable.
    abp.toAbsAppPath = function (path) {
        if (path.indexOf('/') === 0) {
            path = path.substring(1);
        }

        return abp.appPath + path;
    };

    /* LOCALIZATION ***********************************************/
    //Implements Localization API that simplifies usage of localization scripts generated by Abp.

    abp.localization = abp.localization || {};

    abp.localization.localize = function (key, sourceName) {
        sourceName = sourceName || abp.localization.defaultSourceName;

        var source = abp.localization.values[sourceName];

        if (!source) {
            abp.log.warn('Could not find localization source: ' + sourceName);
            return key;
        }

        var value = source[key];
        if (value === undefined) {
            return key;
        }

        var copiedArguments = Array.prototype.slice.call(arguments, 0);
        copiedArguments.splice(1, 1);
        copiedArguments[0] = value;

        return abp.utils.formatString.apply(this, copiedArguments);
    };

    abp.localization.getSource = function (sourceName) {
        return function (key) {
            var copiedArguments = Array.prototype.slice.call(arguments, 0);
            copiedArguments.splice(1, 0, sourceName);
            return abp.localization.localize.apply(this, copiedArguments);
        };
    };

    abp.localization.isCurrentCulture = function (name) {
        return abp.localization.currentCulture
            && abp.localization.currentCulture.name
            && abp.localization.currentCulture.name.indexOf(name) === 0;
    };

    abp.localization.defaultSourceName = undefined;
    abp.localization.abpWeb = abp.localization.getSource('AbpWeb');

    /* AUTHORIZATION **********************************************/
    //Implements Authorization API that simplifies usage of authorization scripts generated by Abp.

    abp.auth = abp.auth || {};

    abp.auth.allPermissions = abp.auth.allPermissions || {};

    abp.auth.grantedPermissions = abp.auth.grantedPermissions || {};

    //Deprecated. Use abp.auth.isGranted instead.
    abp.auth.hasPermission = function (permissionName) {
        return abp.auth.isGranted.apply(this, arguments);
    };

    //Deprecated. Use abp.auth.isAnyGranted instead.
    abp.auth.hasAnyOfPermissions = function () {
        return abp.auth.isAnyGranted.apply(this, arguments);
    };

    //Deprecated. Use abp.auth.areAllGranted instead.
    abp.auth.hasAllOfPermissions = function () {
        return abp.auth.areAllGranted.apply(this, arguments);
    };

    abp.auth.isGranted = function (permissionName) {
        return abp.auth.allPermissions[permissionName] != undefined && abp.auth.grantedPermissions[permissionName] != undefined;
    };

    abp.auth.isAnyGranted = function () {
        if (!arguments || arguments.length <= 0) {
            return true;
        }

        for (var i = 0; i < arguments.length; i++) {
            if (abp.auth.isGranted(arguments[i])) {
                return true;
            }
        }

        return false;
    };

    abp.auth.areAllGranted = function () {
        if (!arguments || arguments.length <= 0) {
            return true;
        }

        for (var i = 0; i < arguments.length; i++) {
            if (!abp.auth.isGranted(arguments[i])) {
                return false;
            }
        }

        return true;
    };

    /* FEATURE SYSTEM *********************************************/
    //Implements Features API that simplifies usage of feature scripts generated by Abp.

    abp.features = abp.features || {};

    abp.features.allFeatures = abp.features.allFeatures || {};

    abp.features.get = function (name) {
        return abp.features.allFeatures[name];
    }

    abp.features.getValue = function (name) {
        var feature = abp.features.get(name);
        if (feature === undefined) {
            return undefined;
        }

        return feature.value;
    }

    abp.features.isEnabled = function (name) {
        var value = abp.features.getValue(name);
        return value === 'true' || value === 'True';
    }

    /* SETTINGS **************************************************/
    //Implements Settings API that simplifies usage of setting scripts generated by Abp.

    abp.setting = abp.setting || {};

    abp.setting.values = abp.setting.values || {};

    abp.setting.get = function (name) {
        return abp.setting.values[name];
    };

    abp.setting.getBoolean = function (name) {
        var value = abp.setting.get(name);
        return value === 'true' || value === 'True';
    };

    abp.setting.getInt = function (name) {
        return parseInt(abp.setting.values[name]);
    };

    /* REALTIME NOTIFICATIONS ************************************/

    abp.notifications = abp.notifications || {};

    abp.notifications.severity = {
        INFO: 0,
        SUCCESS: 1,
        WARN: 2,
        ERROR: 3,
        FATAL: 4
    };

    abp.notifications.userNotificationState = {
        UNREAD: 0,
        READ: 1
    };

    abp.notifications.getUserNotificationStateAsString = function (userNotificationState) {
        switch (userNotificationState) {
            case abp.notifications.userNotificationState.READ:
                return 'READ';
            case abp.notifications.userNotificationState.UNREAD:
                return 'UNREAD';
            default:
                abp.log.warn('Unknown user notification state value: ' + userNotificationState)
                return '?';
        }
    };

    abp.notifications.getUiNotifyFuncBySeverity = function (severity) {
        var result;
        switch (severity) {
            case abp.notifications.severity.SUCCESS:
                result = abp.notify.success;
            case abp.notifications.severity.WARN:
                result = abp.notify.warn;
            case abp.notifications.severity.ERROR:
                result = abp.notify.error;
            case abp.notifications.severity.FATAL:
                result = abp.notify.error;
            case abp.notifications.severity.INFO:
            default:
                result = abp.notify.info;
        }
        return result;
    };

    abp.notifications.messageFormatters = {};

    abp.notifications.messageFormatters['Abp.Notifications.MessageNotificationData'] = function (userNotification) {
        return userNotification.notification.data.message;
    };

    abp.notifications.messageFormatters['Abp.Notifications.LocalizableMessageNotificationData'] = function (userNotification) {
        var localizedMessage = abp.localization.localize(
            userNotification.notification.data.message.name,
            userNotification.notification.data.message.sourceName
        );

        if (userNotification.notification.data.properties) {
            if ($) {
                //Prefer to use jQuery if possible
                $.each(userNotification.notification.data.properties, function (key, value) {
                    localizedMessage = localizedMessage.replace('{' + key + '}', value);
                });
            } else {
                //alternative for $.each
                var properties = Object.keys(userNotification.notification.data.properties);
                for (var i = 0; i < properties.length; i++) {
                    localizedMessage = localizedMessage.replace('{' + properties[i] + '}', userNotification.notification.data.properties[properties[i]]);
                }
            }
        }

        return localizedMessage;
    };

    abp.notifications.getFormattedMessageFromUserNotification = function (userNotification) {
        var formatter = abp.notifications.messageFormatters[userNotification.notification.data.type];
        if (!formatter) {
            abp.log.warn('No message formatter defined for given data type: ' + userNotification.notification.data.type)
            return '?';
        }

        if (!abp.utils.isFunction(formatter)) {
            abp.log.warn('Message formatter should be a function! It is invalid for data type: ' + userNotification.notification.data.type)
            return '?';
        }

        return formatter(userNotification);
    }

    abp.notifications.showUiNotifyForUserNotification = function (userNotification, options) {
        var message = abp.notifications.getFormattedMessageFromUserNotification(userNotification);
        var uiNotifyFunc = abp.notifications.getUiNotifyFuncBySeverity(userNotification.notification.severity);
        uiNotifyFunc(message, undefined, options);
    }

    /* LOGGING ***************************************************/
    //Implements Logging API that provides secure & controlled usage of console.log

    abp.log = abp.log || {};

    abp.log.levels = {
        DEBUG: 1,
        INFO: 2,
        WARN: 3,
        ERROR: 4,
        FATAL: 5
    };

    abp.log.level = abp.log.levels.DEBUG;

    abp.log.log = function (logObject, logLevel) {
        if (!window.console || !window.console.log) {
            return;
        }

        if (logLevel !== undefined && logLevel < abp.log.level) {
            return;
        }

    };

    abp.log.debug = function (logObject) {
        abp.log.log("DEBUG: ", abp.log.levels.DEBUG);
        abp.log.log(logObject, abp.log.levels.DEBUG);
    };

    abp.log.info = function (logObject) {
        abp.log.log("INFO: ", abp.log.levels.INFO);
        abp.log.log(logObject, abp.log.levels.INFO);
    };

    abp.log.warn = function (logObject) {
        abp.log.log("WARN: ", abp.log.levels.WARN);
        abp.log.log(logObject, abp.log.levels.WARN);
    };

    abp.log.error = function (logObject) {
        abp.log.log("ERROR: ", abp.log.levels.ERROR);
        abp.log.log(logObject, abp.log.levels.ERROR);
    };

    abp.log.fatal = function (logObject) {
        abp.log.log("FATAL: ", abp.log.levels.FATAL);
        abp.log.log(logObject, abp.log.levels.FATAL);
    };

    /* NOTIFICATION *********************************************/
    //Defines Notification API, not implements it
    abp.notify = abp.notify || {};

    abp.notify.success = function (message, title, options) {
        abp.log.warn('abp.notify.success is not implemented! Message = ' + message + '. Title = ' + title + '. Options = ' + options);
    };

    abp.notify.info = function (message, title, options) {
        abp.log.warn('abp.notify.info is not implemented! Message = ' + message + '. Title = ' + title + '. Options = ' + options);
    };

    abp.notify.warn = function (message, title, options) {
        abp.log.warn('abp.notify.warn is not implemented! Message = ' + message + '. Title = ' + title + '. Options = ' + options);
    };

    abp.notify.error = function (message, title, options) {
        abp.log.warn('abp.notify.error is not implemented! Message = ' + message + '. Title = ' + title + '. Options = ' + options);
    };

    /* MESSAGE **************************************************/
    //Defines Message API, not implements it

    abp.message = abp.message || {};

    var showMessage = function (message, title) {


        if (!$) {
            abp.log.warn('abp.message can not return promise since jQuery is not defined! Message = ' + message + '. Title = ' + title);
            return null;
        }

        return $.Deferred(function ($dfd) {
            $dfd.resolve();
        });
    };

    abp.message.info = function (message, title) {
        abp.log.warn('abp.message.info is not implemented!');
        return showMessage(message, title);
    };

    abp.message.success = function (message, title) {
        abp.log.warn('abp.message.success is not implemented!');
        return showMessage(message, title);
    };

    abp.message.warn = function (message, title) {
        abp.log.warn('abp.message.warn is not implemented!');
        return showMessage(message, title);
    };

    abp.message.error = function (message, title) {
        abp.log.warn('abp.message.error is not implemented!');
        return showMessage(message, title);
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {
        abp.log.warn('abp.message.confirm is not implemented!');

        if (titleOrCallback && !(typeof titleOrCallback === 'string')) {
            callback = titleOrCallback;
        }

        var result = confirm(message);
        callback && callback(result);

        if (!$) {
            abp.log.warn('abp.message can not return promise since jQuery is not defined!');
            return null;
        }

        return $.Deferred(function ($dfd) {
            $dfd.resolve();
        });
    };

    /* UI *******************************************************/

    abp.ui = abp.ui || {};

    /* UI BLOCK */
    //Defines UI Block API, not implements it

    abp.ui.block = function (elm) {
        abp.log.warn('abp.ui.block is not implemented! elm = ' + elm);
    };

    abp.ui.unblock = function (elm) {
        abp.log.warn('abp.ui.unblock is not implemented! elm = ' + elm);
    };

    /* UI BUSY */
    //Defines UI Busy API, not implements it

    abp.ui.setBusy = function (elm, optionsOrPromise) {
        abp.log.warn('abp.ui.setBusy is not implemented! elm = ' + elm + '. optionsOrPromise = ' + optionsOrPromise);
    };

    abp.ui.clearBusy = function (elm) {
        abp.log.warn('abp.ui.clearBusy is not implemented! elm = ' + elm);
    };

    /* SIMPLE EVENT BUS *****************************************/

    abp.event = (function () {

        var _callbacks = {};

        var on = function (eventName, callback) {
            if (!_callbacks[eventName]) {
                _callbacks[eventName] = [];
            }

            _callbacks[eventName].push(callback);
        };

        var off = function (eventName, callback) {
            var callbacks = _callbacks[eventName];
            if (!callbacks) {
                return;
            }

            var index = -1;
            for (var i = 0; i < callbacks.length; i++) {
                if (callbacks[i] === callback) {
                    index = i;
                    break;
                }
            }

            if (index < 0) {
                return;
            }

            _callbacks[eventName].splice(index, 1);
        };

        var trigger = function (eventName) {
            var callbacks = _callbacks[eventName];
            if (!callbacks || !callbacks.length) {
                return;
            }

            var args = Array.prototype.slice.call(arguments, 1);
            for (var i = 0; i < callbacks.length; i++) {
                callbacks[i].apply(this, args);
            }
        };

        // Public interface ///////////////////////////////////////////////////

        return {
            on: on,
            off: off,
            trigger: trigger
        };
    })();


    /* UTILS ***************************************************/

    abp.utils = abp.utils || {};

    /* Creates a name namespace.
    *  Example:
    *  var taskService = abp.utils.createNamespace(abp, 'services.task');
    *  taskService will be equal to abp.services.task
    *  first argument (root) must be defined first
    ************************************************************/
    abp.utils.createNamespace = function (root, ns) {
        var parts = ns.split('.');
        for (var i = 0; i < parts.length; i++) {
            if (typeof root[parts[i]] === 'undefined') {
                root[parts[i]] = {};
            }

            root = root[parts[i]];
        }

        return root;
    };

    /* Formats a string just like string.format in C#.
    *  Example:
    *  _formatString('Hello {0}','Halil') = 'Hello Halil'
    ************************************************************/
    abp.utils.formatString = function () {
        if (arguments.length < 1) {
            return null;
        }

        var str = arguments[0];

        for (var i = 1; i < arguments.length; i++) {
            var placeHolder = '{' + (i - 1) + '}';
            str = str.replace(placeHolder, arguments[i]);
        }

        return str;
    };

    abp.utils.toPascalCase = function (str) {
        if (!str || !str.length) {
            return str;
        }

        if (str.length === 1) {
            return str.charAt(0).toUpperCase();
        }

        return str.charAt(0).toUpperCase() + str.substr(1);
    }

    abp.utils.toCamelCase = function (str) {
        if (!str || !str.length) {
            return str;
        }

        if (str.length === 1) {
            return str.charAt(0).toLowerCase();
        }

        return str.charAt(0).toLowerCase() + str.substr(1);
    }

    abp.utils.truncateString = function (str, maxLength) {
        if (!str || !str.length || str.length <= maxLength) {
            return str;
        }

        return str.substr(0, maxLength);
    };

    abp.utils.truncateStringWithPostfix = function (str, maxLength, postfix) {
        postfix = postfix || '...';

        if (!str || !str.length || str.length <= maxLength) {
            return str;
        }

        if (maxLength <= postfix.length) {
            return postfix.substr(0, maxLength);
        }

        return str.substr(0, maxLength - postfix.length) + postfix;
    };

    abp.utils.isFunction = function (obj) {
        if ($) {
            //Prefer to use jQuery if possible
            return $.isFunction(obj);
        }

        //alternative for $.isFunction
        return !!(obj && obj.constructor && obj.call && obj.apply);
    };

    /* TIMING *****************************************/
    abp.timing = abp.timing || {};

    abp.timing.utcClockProvider = (function () {

        var toUtc = function (date) {
            return Date.UTC(
                date.getUTCFullYear()
                , date.getUTCMonth()
                , date.getUTCDate()
                , date.getUTCHours()
                , date.getUTCMinutes()
                , date.getUTCSeconds()
                , date.getUTCMilliseconds()
            );
        }

        var now = function () {
            return new Date();
        };

        var normalize = function (date) {
            if (!date) {
                return date;
            }

            return new Date(toUtc(date));
        };

        // Public interface ///////////////////////////////////////////////////

        return {
            now: now,
            normalize: normalize
        };
    })();

    abp.timing.localClockProvider = (function () {

        var toLocal = function (date) {
            return new Date(
                date.getFullYear()
                , date.getMonth()
                , date.getDate()
                , date.getHours()
                , date.getMinutes()
                , date.getSeconds()
                , date.getMilliseconds()
            );
        }

        var now = function () {
            return toLocal(new Date());
        }

        var normalize = function (date) {
            if (!date) {
                return date;
            }

            return toLocal(date);
        }

        // Public interface ///////////////////////////////////////////////////

        return {
            now: now,
            normalize: normalize
        };
    })();

    abp.timing.convertToUserTimezone = function (date) {
        var localTime = date.getTime();
        var utcTime = localTime + (date.getTimezoneOffset() * 60000);
        var targetTime = parseInt(utcTime) + parseInt(abp.timing.timeZoneInfo.windows.currentUtcOffsetInMilliseconds);
        return new Date(targetTime);
    };

    /* CLOCK *****************************************/
    abp.clock = abp.clock || {};

    abp.clock.now = function () {
        if (abp.clock.provider) {
            return abp.clock.provider.now();
        }

        return new Date();
    }

    abp.clock.normalize = function (date) {
        if (abp.clock.provider) {
            return abp.clock.provider.normalize(date);
        }

        return date;
    }

    abp.clock.provider = abp.timing.localClockProvider;

})(jQuery);