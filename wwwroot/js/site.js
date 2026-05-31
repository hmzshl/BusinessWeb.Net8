function isDevice() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}
window.getBrowserDimension = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};
window.getScaleLevel = function () {
    return window.devicePixelRatio;
};
window.getScreenHeight = function () {
    return window.screen.height;
};
window.bwFavorites = {
    get: function (key) {
        try { return localStorage.getItem(key); } catch (e) { return null; }
    },
    set: function (key, value) {
        try { localStorage.setItem(key, value); } catch (e) { }
    }
};