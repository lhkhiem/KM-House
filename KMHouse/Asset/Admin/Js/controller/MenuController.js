var Menuconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var menuController = {
    init: function () {
        menuController.loadData();
        menuController.registerEvent();
    },
    registerEvent: function () {
        $('#frmMenu').validate({
            rules: {
                txtText: {
                    required: true,
                },
                txtLink: {
                    required: true,
                }
            },
            messages: {
                txtText: {
                    required: " Text bắt buộc phải có",
                },
                txtLink: {
                    required: "Link bắt buộc phải có",
                }
            }
        });
        $('#sltMenuType').off('click').on('click', function (e) {
            menuController.loadData(true);
        });
        $('.sltOrder').off('change').on('change', function (e) {
            e.preventDefault();
            var slt = $(this);
            var id = slt.data('id');
            var order = slt.val();
            var menuType = $('#sltMenuType').val();
            $.ajax({
                url: "/Menu/ChangeOrder",
                data: {
                    id: id,
                    order: order,
                    menuType: menuType
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        menuController.loadData(true);
                        $('#status').text('Change order susses');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                    }
                    else {
                        menuController.loadData(true);
                        $('#status').text('Not success');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);

                    }
                }
            });
        });
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "Menu/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        //btn.text('Kích hoạt');
                        btn.removeClass('lable-danger');
                        btn.addClass('lable-success');
                        menuController.loadData();
                    }
                    else {
                        //btn.text('Khóa');
                        btn.removeClass('lable-success');
                        btn.addClass('lable-danger');
                        menuController.loadData();
                    }
                }
            });
        });
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            menuController.loadData(true);
        });
        $('#frmMenu').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                if ($('#frmMenu').validate()) {
                    menuController.saveData();
                }
            }
            });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            menuController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmMenu').valid()) {
                //alert("da valid");
                menuController.saveData();
            }
        });
        $('#sltPageSize').off('change').on('change', function () {
            Menuconfig.pageSize = $(this).val();
            menuController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            menuController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            menuController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            menuController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    menuController.deleteMenu(id);
                }
            });
        });

    },
    deleteMenu: function (id) {
        $.ajax({
            url: '/Menu/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    menuController.loadData(true);
                    $('#status').text('Deleted');
                    window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                }
                else if (response.status == false) {
                    bootbox.alert(response.message);
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
    loadDetail: function (id) {
        $.ajax({
            url: '/Menu/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#txtID').val(data.ID);
                    $('#txtText').val(data.Text);
                    $('#txtLink').val(data.Link);
                    $('#sltTarget').val(data.Target);
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
    saveData: function () {
        var id = $('#txtID').val();
        var text = $('#txtText').val();
        var link = $('#txtLink').val();
        var target = $('#sltTarget').val();
        var menuType = $('#sltMenuType').val();

        var menu = {
            ID:id,
            Text: text,
            Link: link,
            Target: target,
            MenuTypeID:menuType
        }
        $.ajax({
            url: '/Menu/SaveData',
            data: {
                strMenu: JSON.stringify(menu)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    $('#modalAddUpdate').modal('hide');
                    menuController.loadData(true);
                    if (response.action == 'insert') {
                        $('#status').text('Add new success');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                    }
                    else {
                        $('#status').text('Updated');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                    }
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
    resetForm: function () {
        $('#txtID').val('0');
        $('#txtText').val('');
        $('#txtLink').val('');

    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        var menuType = $('#sltMenuType').val();
        $.ajax({
            url: '/Menu/LoadData',
            type: 'GET',
            data: {
                menuType:menuType,
                keyword: keyword,
                type:type,
                pageIndex: Menuconfig.pageIndex,
                pageSize: Menuconfig.pageSize
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
                            Text: item.Text,
                            Link: item.Link,
                            Target: item.Target,
                            Order:item.DisplayOrder,
                            Status: item.Status == true ? "<a data-id=" + item.ID + " class=\"label label-success btn-active\">Actived</a>" : "<a data-id=" + item.ID + " class=\"label label-danger btn-active\">Locked</a>",
                            MenuTypeID:item.MenuTypeID
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent+" / "+ response.total +' tổng số bản ghi');
                    menuController.paging(response.total, function () {
                        menuController.loadData();
                    }, changePageSize);
                    menuController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / Menuconfig.pageSize);

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
                Menuconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
menuController.init();