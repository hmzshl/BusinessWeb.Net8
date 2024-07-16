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