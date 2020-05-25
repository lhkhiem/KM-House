var unitconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var unitController = {
    init: function () {
        unitController.loadData();
        unitController.registerEvent();
    },
    registerEvent: function () {
        $('#frmUnit').validate({
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
            unitController.loadData(true);
        });
        $('#frmUnit').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                if ($('#frmUnit').valid()) {
                    unitController.saveData();
                }
            }
            });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            unitController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmUnit').valid()) {
                unitController.saveData();
            }
        });
        $('#sltPageSize').off('change').on('change', function () {
            unitconfig.pageSize = $(this).val();
            unitController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            unitController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            unitController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            unitController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    unitController.deleteUnit(id);
                }
            });
        });

    },
    deleteUnit: function (id) {
        $.ajax({
            url: '/Unit/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == 1) {
                    unitController.loadData(true);
                    $('#status').text('Deleted');
                    window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                }
                else if (response.status == 0){
                    bootbox.alert("Có lỗi xảy ra. Vui lòng thử lại");
                }
                else
                    bootbox.alert("Có Sản phẩm đang dùng. Vui lòng thử lại");
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Unit/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#hidID').val(data.ID);
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
        var id = parseInt($('#hidID').val());
        var unit = {
            Name: name,
            ID: id
        }
        $.ajax({
            url: '/Unit/SaveData',
            data: {
                strUnit: JSON.stringify(unit)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    $('#modalAddUpdate').modal('hide');
                    unitController.loadData(true);
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
        $('#hidID').val('0');
        $('#txtName').val('');
    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        
        $.ajax({
            url: '/Unit/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type:type,
                pageIndex: unitconfig.pageIndex,
                pageSize: unitconfig.pageSize
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
                    unitController.paging(response.total, function () {
                        unitController.loadData();
                    }, changePageSize);
                    unitController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / unitconfig.pageSize);

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
                unitconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
unitController.init();