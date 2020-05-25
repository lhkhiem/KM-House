var orderconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var orderController = {
    init: function () {
        orderController.loadData();
        orderController.registerEvent();
    },
    registerEvent: function () {
        $('.btn-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Order/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        //btn.text('Kích hoạt');
                        btn.removeClass('lable-danger');
                        btn.addClass('lable-success');
                        orderController.loadData();
                    }
                    else {
                        //btn.text('Khóa');
                        btn.removeClass('lable-success');
                        btn.addClass('lable-danger');
                        orderController.loadData();
                    }
                }
            });
        });
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            orderController.loadData(true);
        });
        $('#sltPageSize').off('change').on('change', function () {
            orderconfig.pageSize = $(this).val();
            orderController.loadData(true);
        });
        $('#slStatus').off('change').on('change', function () {
            orderController.loadData(true);
        });
        $('#btnSearch').off('click').on('click', function () {
            orderController.loadData(true);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    orderController.deleteOrder(id);
                }
            });
        });

    },
    dateFormat:function(d) {
        return ((d.getMonth() + 1) + "").padStart(2, "0")
            + "/" + (d.getDate() + "").padStart(2, "0")
            + "/" + d.getFullYear();
    },
    deleteOrder: function (id) {
        $.ajax({
            url: '/Admin/Order/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    orderController.loadData(true);
                    $('#status').text('Deleted');
                    window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        var status = $('#slStatus').val();
        $.ajax({
            url: '/Admin/Order/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type: type,
                status:status,
                pageIndex: orderconfig.pageIndex,
                pageSize: orderconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        var status;
                        if (item.Status == 1) status = "<a data-id=" + item.ID + " class=\"label label-info btn-status\">Đơn mới</a>";
                        else if (item.Status == 2) status = "<a data-id=" + item.ID + " class=\"label label-danger btn-status\">Đang giao</a>";
                        else status = "<a data-id=" + item.ID + " class=\"label label-success btn-status\">Đã giao</a>";
                        html += Mustache.render(template, {
                            ID:item.ID,
                            NameAccount: item.NameAccount,
                            ShipName: item.ShipName,
                            ShipMobile: item.ShipMobile,
                            ShipEmail: item.ShipEmail,
                            Phone: item.Phone,
                            CreateDate: orderController.dateFormat(new Date(parseInt((item.CreateDate).match(/\d+/)[0]))),
                            Status:status
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent+" / "+ response.total +' tổng số bản ghi');
                    orderController.paging(response.total, function () {
                        orderController.loadData();
                    }, changePageSize);
                    orderController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / orderconfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                orderconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
orderController.init();