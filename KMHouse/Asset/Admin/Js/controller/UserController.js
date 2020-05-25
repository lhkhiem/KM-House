var userconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var userController = {
    init: function () {
        userController.loadData();
        userController.registerEvent();
    },
    registerEvent: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        //btn.text('Kích hoạt');
                        btn.removeClass('lable-danger');
                        btn.addClass('lable-success');
                        userController.loadData();
                    }
                    else {
                        //btn.text('Khóa');
                        btn.removeClass('lable-success');
                        btn.addClass('lable-danger');
                        userController.loadData();
                    }
                }
            });
        });
        $('#ckStatus').on('ifChecked', function (e) {
            userController.loadData(true);
        });
        $('#ckStatus').on('ifUnchecked', function (e) {
            userController.loadData(true);
        });
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            userController.loadData(true);
        });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            $('#txtID').removeAttr('disabled');
            userController.resetForm();
        });
        $('#sltPageSize').off('change').on('change', function () {
            userconfig.pageSize = $(this).val();
            userController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            userController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(10);
            $('#ckStatus').iCheck('check');
            userController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            $('#txtID').attr('disabled',true);
            var id = $(this).data('id');
            userController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    userController.deleteUser(id);
                }
            });
        });

    },
    dateFormat:function(d) {
        return ((d.getMonth() + 1) + "").padStart(2, "0")
            + "/" + (d.getDate() + "").padStart(2, "0")
            + "/" + d.getFullYear();
    },
    deleteUser: function (id) {
        $.ajax({
            url: '/Admin/User/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    userController.loadData(true);
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
            url: '/Admin/User/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type: type,
                status:status,
                pageIndex: userconfig.pageIndex,
                pageSize: userconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID:item.ID,
                            Name: item.Name,
                            UserName: item.UserName,
                            UserGroup: item.UserGroup,
                            Email: item.Email,
                            Phone: item.Phone,
                            CreateDate: userController.dateFormat(new Date(parseInt((item.CreateDate).match(/\d+/)[0]))),
                            Status: item.Status == true ? "<a data-id=" + item.ID + " class=\"label label-success btn-active\">Actived</a>" : "<a data-id=" + item.ID +" class=\"label label-danger btn-active\">Locked</a>"

                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent+" / "+ response.total +' tổng số bản ghi');
                    userController.paging(response.total, function () {
                        userController.loadData();
                    }, changePageSize);
                    userController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / userconfig.pageSize);

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
                userconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
userController.init();