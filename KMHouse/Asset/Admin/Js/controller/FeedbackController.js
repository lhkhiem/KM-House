var feedbackConfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var feedbackController = {
    init: function () {
        feedbackController.loadData();
        feedbackController.registerEvent();
    },
    registerEvent: function () {

        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            feedbackController.loadData(true);
        });
        $('.btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            feedbackController.loadData(true);
        });
        $('#sltPageSize').off('change').on('change', function () {
            feedbackConfig.pageSize = $(this).val();
            feedbackController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            feedbackController.loadData(true);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    feedbackController.deleteFeedback(id);
                }
            });
        });
        $('.btn-info').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Feedback/ShowContent",
                data: { id: id },
                dataType: "json",
                type: "GET",
                success: function (response) {
                    if (response.status == true) {
                        bootbox.alert({
                            message: '<span class="text-info">' + response.data.Content + '</span>',
                            title: '<span class="text-danger">Nội dung</span>',

                        });                    
                    }
                    else {
                    }
                }
            });

        });
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Feedback/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        //btn.text('Kích hoạt');
                        btn.removeClass('lable-danger');
                        btn.addClass('lable-success');
                        feedbackController.loadData();
                    }
                    else {
                        //btn.text('Khóa');
                        btn.removeClass('lable-success');
                        btn.addClass('lable-danger');
                        feedbackController.loadData();
                    }
                }
            });
        });
    },
    deleteFeedback: function (id) {
        $.ajax({
            url: '/Feedback/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    feedbackController.loadData(true);
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
            url: '/Feedback/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type: type,
                pageIndex: feedbackConfig.pageIndex,
                pageSize: feedbackConfig.pageSize
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
                            Email: item.Email,
                            Content: item.Content.length > 60 ? item.Content.substring(0, 60)+'...' : item.Content,
                            Status: item.Status == true ? "<a data-id=" + item.ID + " class=\"label label-success btn-active\">Đã xem</a>" : "<a data-id=" + item.ID + " class=\"label label-danger btn-active\">Chưa xem</a>"
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent + " / " + response.total + ' tổng số bản ghi');
                    feedbackController.paging(response.total, function () {
                        feedbackController.loadData();
                    }, changePageSize);
                    feedbackController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / feedbackConfig.pageSize);

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
                feedbackConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
feedbackController.init();