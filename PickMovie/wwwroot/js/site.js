function showPopup(path, returnUrl) {
    try {
        $.ajax({
            type: 'GET',
            url: path,
            data: { returnUrl: returnUrl },
            success: function (res) {
                $('#formModal .modal-body').html(res);
                $('#formModal').modal('show');
            },
            error: function (err) {
                console.log(err);
            }
        })
    } catch (e) {
        console.log(e);
    }
}

authPost = form => {
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        success: function (res) {
            if (!res.isValid) {
                $('#formModal .modal-body').html(res.html);
            }
            else {
                $('#formModal').modal('hide');
                window.location.href = res.redirectUrl;
            }
        }
    })
}