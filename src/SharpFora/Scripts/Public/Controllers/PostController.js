function PostController() {
    var $ctrl = this,
        post = $ctrl.post;

    $ctrl.edit = function () {
        alert(post.id);
    };
    $ctrl.delete = function () {
    };
}