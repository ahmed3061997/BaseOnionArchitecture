import 'js-loading-overlay';

export class LoadingOverlayHelper {
    public static showLoading(spinnerIcon: string = 'ball-atom') {
        JsLoadingOverlay.show({
            'overlayBackgroundColor': '#a9a9a9',
            'overlayOpacity': 0.6,
            'spinnerIcon': spinnerIcon,
            'spinnerColor': '#696cff',
            'spinnerSize': '2x',
            'overlayIDName': 'overlay',
            'spinnerIDName': 'spinner',
            'spinnerZIndex': 99999,
            'overlayZIndex': 99998
        });
    }

    public static hideLoading() {
        JsLoadingOverlay.hide();
    }
}