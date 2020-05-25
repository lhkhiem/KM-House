var newsConfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var newsController = {
    init: function () {
        newsController.loadData();
        newsController.registerEvent();
    },
    registerEvent: function () {
        //Code cuả trang Index----------------------------------------
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            newsController.loadData(true);
        });
        $('.btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            newsController.loadData(true);
        });
        $('#sltPageSize').off('change').on('change', function () {
            newsConfig.pageSize = $(this).val();
            newsController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            newsController.loadData(true);
        });
        $('#ckStatus').on('ifChecked', function (e) {
            newsController.loadData(true);
        });
        $('#ckStatus').on('ifUnchecked', function (e) {
            newsController.loadData(true);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    newsController.deleteUnit(id);
                }
            });
        });

        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/News/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        //btn.text('Kích hoạt');
                        btn.removeClass('lable-danger');
                        btn.addClass('lable-success');
                        newsController.loadData();
                    }
                    else {
                        //btn.text('Khóa');
                        btn.removeClass('lable-success');
                        btn.addClass('lable-danger');
                        newsController.loadData();
                    }
                }
            });
        });
        //Code của trang Create và Edit--------------------------------------------------------
        $('#txtMetaTitle').off('click').on('click', function (e) {
            var str = $('#txtName').val();
            newsController.convertString(str);
        });
        $('#txtName').off('keyup').on('keyup', function (e) {
            var str = $('#txtName').val();
            newsController.convertString(str);
        });
    },
    convertString: function (str) {

        $.ajax({
            url: '/News/ConvertString',
            data: {
                str: str
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                $('#txtMetaTitle').val(response.str);
            }
        });
    },
    dateFormat: function (d) {
        return ((d.getMonth() + 1) + "").padStart(2, "0")
            + "/" + (d.getDate() + "").padStart(2, "0")
            + "/" + d.getFullYear();
    },
    deleteUnit: function (id) {
        $.ajax({
            url: '/News/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    newsController.loadData(true);
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
        var status = $('#ckStatus').prop('checked');
        $.ajax({
            url: '/News/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type: type,
                status: status,
                pageIndex: newsConfig.pageIndex,
                pageSize: newsConfig.pageSize
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
                            Image: item.Image,
                            CreateDate: newsController.dateFormat(new Date(parseInt((item.CreateDate).match(/\d+/)[0]))),
                            CreateBy: item.CreateBy,
                            NewsCategoryName: item.NewsCategoryName,
                            Status: item.Status == true ? "<a data-id=" + item.ID + " class=\"label label-success btn-active\">Active</a>" : "<a data-id=" + item.ID + " class=\"label label-danger btn-active\">Lock</a>"
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent + " / " + response.total + ' tổng số bản ghi');
                    newsController.paging(response.total, function () {
                        newsController.loadData();
                    }, changePageSize);
                    newsController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / newsConfig.pageSize);

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
                newsConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
newsController.init();