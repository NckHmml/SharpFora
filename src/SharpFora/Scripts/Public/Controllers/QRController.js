function QRController() {
    var $ctrl = this,
        text = $ctrl.text;
    $ctrl.data = QR.create(text);
}