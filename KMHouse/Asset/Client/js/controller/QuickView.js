var quickView = {
    init: function () {
        quickView.registerEvent();
    },
    registerEvent: function () {
        $('.quick-view-product').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            quickView.loadDetail(id);
            $('#QuickViewModal').modal('show');
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Home/QuickView',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#quick-view-image').attr('src', data.Image);
                    $('#quick-view-title').text(data.Name);
                    $('#quick-view-category').text('Danh mục: ' + data.ProductCategoryName);
                    if (data.PromotionPrice != null && data.PromotionPrice > 0) {
                        $('#price').html(
                            '<span class="new-price new-price-2">' + numeral(data.PromotionPrice).format('0,0[.]00') + ' đ</span>');
                    }
                    else {
                        $('#price').html('<span class="new-price new-price-2">' + numeral(data.Price).format('0,0[.]00') + ' đ</span>');
                    }
                    $('#quick-view-description').html(data.Description);
                    $('#btnCart').attr('data-id',data.ID);

                }
            }
        });
    }
}
quickView.init();