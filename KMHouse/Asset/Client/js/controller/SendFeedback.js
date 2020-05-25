var feedback = {
    init: function () {
        feedback.regEvents();
    },
    regEvents: function () {
        $('#submit').off('click').on('click', function (e) {
            e.preventDefault(e);

                var name = $('#customername').val();
                var email = $('#customerEmail').val();
                var phone = $('#contactPhone').val();
                var address = $('#contactAddress').val();
                var title = $('#contactSubject').val();
                var message = $('#contactMessage').val();
                $.ajax({
                    url: '/Info/SendFeedback',
                    type: 'GET',
                    data: {
                        name: name,
                        email: email,
                        phone: phone,
                        address: address,
                        title: title,
                        message: message
                    },
                    dataType: 'json',
                    success: function (res) {
                        if (res.status == true) {
                            $('#info').html('<span class="text-info">Đã gửi thành công!</span>');

                            setTimeout(function () {
                                $('#info').html("");
                            }, 3000);
                        }
                        else {
                            $('#info').html('<span class="text-danger">Không gửi được. Vui lòng gọi Hotline</span>');
                            setTimeout(function () {
                                $('#info').html("");
                            }, 3000);
                        }
                    }
                })
        })

    }
}
feedback.init();