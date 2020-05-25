var credentialconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var credentialController = {
    init: function () {
        credentialController.loadData();
        credentialController.registerEvent();
    },
    registerEvent: function () {
        
        $('#btnAddNew').off('click').on('click', function () {
            $('#frmRole').submit();
        });
        $('#btnClose').off('click').on('click', function () {
            window.close();
        });
        $('#sltPageSize').off('change').on('change', function () {
            credentialconfig.pageSize = $(this).val();
            credentialController.loadData(true);
        })
        $('.btn-delete').off('click').on('click', function () {
            var userGroupId = $('#txtUserGroup').val();
            var roleId = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    credentialController.deleteCredential(userGroupId,roleId);
                }
            });
        });

    },
    deleteCredential: function (userGroupId, roleId) {
        $.ajax({
            url: '/Credential/Delete',
            data: {
                userGroupId: userGroupId,
                roleId: roleId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    credentialController.loadData(true);
                    $('#status').text('Deleted');
                    window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                }
                else if (response.status == false) {
                    bootbox.alert(response.message);
                }
                else {
                    bootbox.alert('Nhóm tài khoản đã đã được sử dụng. Vui lòng kiểm tra lại!');
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadData: function (changePageSize) {
        var userGroup = $('#txtUserGroup').val();
        $.ajax({
            url: '/Credential/LoadData',
            type: 'GET',
            data: {
                userGroup: userGroup,
                pageIndex: credentialconfig.pageIndex,
                pageSize: credentialconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            RoleID:item.RoleID,
                            UserGroupName: item.UserGroupName,
                            RoleName: item.RoleName,
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent+" / "+ response.total +' tổng số bản ghi');
                    credentialController.paging(response.total, function () {
                        credentialController.loadData();
                    }, changePageSize);
                    credentialController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / credentialconfig.pageSize);

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
                credentialconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
credentialController.init();