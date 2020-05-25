var userGroupconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var userGroupController = {
    init: function () {
        userGroupController.loadData();
        userGroupController.registerEvent();
    },
    registerEvent: function () {
        $('#frmUserGroup').validate({
            rules: {
                txtID: {
                    required: true,
                },
                txtName: {
                    required: true,
                },
                txtDescription: {
                    required: false,
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
            userGroupController.loadData(true);
        });
        $('#frmUserGroup').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                if ($('#frmUserGroup').valid()) {
                    userGroupController.saveData();
                }
            }
            });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            $('#txtID').removeAttr('disabled');
            userGroupController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmUserGroup').valid()) {
                userGroupController.saveData();
            }
        });
        $('#sltPageSize').off('change').on('change', function () {
            userGroupconfig.pageSize = $(this).val();
            userGroupController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            userGroupController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            userGroupController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            $('#txtID').attr('disabled',true);
            var id = $(this).data('id');
            userGroupController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    userGroupController.deleteUserGroup(id);
                }
            });
        });

    },
    deleteUserGroup: function (id) {
        $.ajax({
            url: '/UserGroup/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    userGroupController.loadData(true);
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
            url: '/UserGroup/GetDetail',
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
                    $('#txtDescription').val(data.Description);
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
        var description = $('#txtDescription').val();
        var userGroup = {
            Name: name,
            ID: id,
            Description: description
        }
        $.ajax({
            url: '/UserGroup/SaveData',
            data: {
                strUserGroup: JSON.stringify(userGroup)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    $('#modalAddUpdate').modal('hide');
                    userGroupController.loadData(true);
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
        $('#txtDescription').val('');

    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        
        $.ajax({
            url: '/UserGroup/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type:type,
                pageIndex: userGroupconfig.pageIndex,
                pageSize: userGroupconfig.pageSize
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
                            Description:item.Description
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent+" / "+ response.total +' tổng số bản ghi');
                    userGroupController.paging(response.total, function () {
                        userGroupController.loadData();
                    }, changePageSize);
                    userGroupController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / userGroupconfig.pageSize);

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
                userGroupconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
userGroupController.init();