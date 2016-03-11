function RegisterController() {
    var $ctrl = this;

    $ctrl.step = 1;
    $ctrl.setStep = function (step) {
        $ctrl.step = step;
    }
}