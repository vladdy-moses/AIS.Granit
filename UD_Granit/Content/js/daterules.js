$.validator.methods.date = function (value, element) {
    if (!value)
        return false;
    var splitVal = value.split(' ');
    var date = splitVal[0].split('.');
    var time = (splitVal.length == 2 ? splitVal[1] : '0:0').split(':');
    return this.optional(element) || !/Invalid|NaN/.test(new Date(date[2], date[1], date[0], time[0], time[1], 0, 0));
};