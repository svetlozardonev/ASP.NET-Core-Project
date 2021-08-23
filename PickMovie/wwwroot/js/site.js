function authGet(path, returnUrl) {
        $.ajax({
            type: 'GET',
            url: path,
            data: { returnUrl: returnUrl },
            success: function (res) {
                $('#form-modal .modal-body').html(res);
                $('#form-modal').modal('show');
            },
            error: function (err) {
                console.log(err);
            }
        })
}

authPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (!res.isValid) {
                    $('#form-modal .modal-body').html(res.html);
                }
                else {
                    window.location.href = res.redirectUrl;
                    $('#form-modal').modal('hide');
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

$(function () {
    console.log('here');
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});