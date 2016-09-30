var abp = abp || {};
(function() {
    if (!moment || !moment.tz) {
        return;
    }

    /* DEFAULTS *************************************************/

    abp.timing = abp.timing || {};

    /* FUNCTIONS **************************************************/

    abp.timing.convertToUserTimezone = function(date) {
        const momentDate = moment(date);
        const targetDate = momentDate.clone().tz(abp.timing.timeZoneInfo.iana.timeZoneId);
        return targetDate;
    };

})();