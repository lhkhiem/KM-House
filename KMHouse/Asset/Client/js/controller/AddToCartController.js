var addCart = {
    init: function () {
        addCart.regEvents();
    },
    regEvents: function () {
       
        $('.addCart').off('click').on('click', function () {
            var id = $(this).data('id');
            var quantity = $('#quantity').val();
            if (quantity == null) quantity = 1;
            $.ajax({
                url: '/Cart/AddItem',
                type: 'GET',
                data: {
                    productId:id,
                    quantity: quantity
                },
                dataType: 'json',
                success: function (res) {
                    if (res.status == 1) {
                        var data = res.miniCart;

                        var box = bootbox.dialog({
                            message: '<p class="text-center mb-0">Đã thêm thành công</p><p class="text-center mb-0"><i class="fa fa-check-circle" style="font-size:72px;"></i></p>',
                            size: "small",
                            closeButton: false,
                        }).find('.modal-content').css({
                            'background-color': '#f8f8f8',
                            'margin-top': function () {
                                return window.innerHeight / 2 - 20 + "px";
                            }
                        });
                        setTimeout(function () {
                            box.modal('hide');
                            window.location.reload();
                        }, 1000);
                    }
                    else if (res.status == 0) {
                        var box = bootbox.dialog({
                            message: '<p class="text-center mb-0">Vui lòng đăng nhập!</p><p class="text-center mb-0"><i class="fa fa-info" style="font-size:72px;"></i></p>',
                            size: "small",
                            closeButton: false,
                        });
                        setTimeout(function () {
                            box.modal('hide');
                        }, 1000);
                    }
                    else {
                        var box = bootbox.dialog({
                            message: '<p class="text-center mb-0">Sản phẩm chưa có giá,</br>Vui lòng liên hệ shop!</p><p class="text-center mb-0"><i class="fa fa-info" style="font-size:72px;"></i></p>',
                            size: "small",
                            closeButton: false,
                        });
                        setTimeout(function () {
                            box.modal('hide');
                        }, 2000);
                    }
                }
            })
        })
        
    }
}
addCart.init();