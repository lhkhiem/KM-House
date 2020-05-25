var MenuTypeconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var menutypeController = {
    init: function () {
        menutypeController.loadData();
        menutypeController.registerEvent();
    },
    registerEvent: function () {
        $('#frmMenuType').validate({
            rules: {
                txtID: {
                    required: true,
                },
                txtName: {
                    required: true,
                }
            },
            messages: {
                txtID: {
                    required: "ID bắt buộc phải có",
                },
                txtName: {
                    required: "Bạn phải nhập tên",
                },
            }
        });
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            menutypeController.loadData(true);
        });
        $('#frmMenuType').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                if ($('#frmMenuType').valid()) {
                    menutypeController.saveData();
                }
            }
            });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            menutypeController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmMenuType').valid()) {
                menutypeController.saveData();
            }
        });
        $('#sltPageSize').off('change').on('change', function () {
            MenuTypeconfig.pageSize = $(this).val();
            menutypeController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            menutypeController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            menutypeController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            $('#txtID').attr('disabled',true);
            var id = $(this).data('id');
            menutypeController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    menutypeController.deleteMenuType(id);
                }
            });
        });

    },
    deleteMenuType: function (id) {
        $.ajax({
            url: '/MenuType/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    menutypeController.loadData(true);
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
            url: '/MenuType/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#txtID').val(data.ID);
                    $('#txtName').val(data.Name);
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
        var name = $('#txtName').val();
        var id = $('#txtID').val();
        var menuType = {
            Name: name,
            ID: id,
        }
        $.ajax({
            url: '/MenuType/SaveData',
            data: {
                strMenuType: JSON.stringify(menuType)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    $('#modalAddUpdate').modal('hide');
                    menutypeController.loadData(true);
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
        $('#txtName').val('');

    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        
        $.ajax({
            url: '/MenuType/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type:type,
                pageIndex: MenuTypeconfig.pageIndex,
                pageSize: MenuTypeconfig.pageSize
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
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent+" / "+ response.total +' tổng số bản ghi');
                    menutypeController.paging(response.total, function () {
                        menutypeController.loadData();
                    }, changePageSize);
                    menutypeController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / MenuTypeconfig.pageSize);

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
                MenuTypeconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
menutypeController.init();