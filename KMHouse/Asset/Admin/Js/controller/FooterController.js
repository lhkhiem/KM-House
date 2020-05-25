var footerconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var footerController = {
    init: function () {
        footerController.loadData();
        footerController.registerEvent();
    },
    registerEvent: function () {

        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            footerController.loadData(true);
        });
        $('.btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            footerController.loadData(true);
        });
        $('#sltPageSize').off('change').on('change', function () {
            footerconfig.pageSize = $(this).val();
            footerController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            footerController.loadData(true);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    footerController.deleteUnit(id);
                }
            });
        });
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Footer/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        //btn.text('Kích hoạt');
                        btn.removeClass('lable-danger');
                        btn.addClass('lable-success');
                        footerController.loadData();
                    }
                    else {
                        //btn.text('Khóa');
                        btn.removeClass('lable-success');
                        btn.addClass('lable-danger');
                        footerController.loadData();
                    }
                }
            });
        });
    },
    deleteUnit: function (id) {
        $.ajax({
            url: '/Footer/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    footerController.loadData(true);
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

        $.ajax({
            url: '/Footer/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type: type,
                pageIndex: footerconfig.pageIndex,
                pageSize: footerconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.ID,
                            Name: item.Name,
                            Status: item.Status == true ? "<a data-id=" + item.ID + " class=\"label label-success btn-active\">Default</a>" : "<a data-id=" + item.ID + " class=\"label label-danger btn-active\">Not use</a>"
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent + " / " + response.total + ' tổng số bản ghi');
                    footerController.paging(response.total, function () {
                        footerController.loadData();
                    }, changePageSize);
                    footerController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / footerconfig.pageSize);

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
                footerconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
footerController.init();