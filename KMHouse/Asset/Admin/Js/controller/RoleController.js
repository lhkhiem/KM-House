var roleconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var roleController = {
    init: function () {
        roleController.loadData();
        roleController.registerEvent();
    },
    registerEvent: function () {
        $('#frmRole').validate({
            rules: {
                txtName: {
                    required: true,
                },
            },
            messages: {
                txtName: {
                    required: "Bạn phải nhập tên",
                },
            }
        });
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            roleController.loadData(true);
        });
        $('#frmRole').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                if ($('#frmRole').valid()) {
                    roleController.saveData();
                }
            }
            });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            $('#txtID').removeAttr('disabled');
            roleController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmRole').valid()) {
                roleController.saveData();
            }
        });
        $('#sltPageSize').off('change').on('change', function () {
            roleconfig.pageSize = $(this).val();
            roleController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            roleController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            roleController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            $('#txtID').attr('disabled', true);
            var id = $(this).data('id');
            roleController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    roleController.deleteRole(id);
                }
            });
        });

    },
    deleteRole: function (id) {
        $.ajax({
            url: '/Role/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    roleController.loadData(true);
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
    loadDetail: function (id) {
        $.ajax({
            url: '/Role/GetDetail',
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
        var role = {
            Name: name,
            ID: id
        }
        $.ajax({
            url: '/Role/SaveData',
            data: {
                strRole: JSON.stringify(role)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    $('#modalAddUpdate').modal('hide');
                    roleController.loadData(true);
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
        $('#txtID').val('');
        $('#txtName').val('');
    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        
        $.ajax({
            url: '/Role/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type:type,
                pageIndex: roleconfig.pageIndex,
                pageSize: roleconfig.pageSize
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
                            Name: item.Name
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent+" / "+ response.total +' tổng số bản ghi');
                    roleController.paging(response.total, function () {
                        roleController.loadData();
                    }, changePageSize);
                    roleController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / roleconfig.pageSize);

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
                roleconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
roleController.init();