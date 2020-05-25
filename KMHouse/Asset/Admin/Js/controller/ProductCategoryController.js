var productCategoryConfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var productCategoryController = {
    init: function () {
        productCategoryController.loadData();
        productCategoryController.registerEvent();
    },
    registerEvent: function () {
        $('#frmProductCategory').validate({
            rules: {
                txtName: {
                    required: true,
                }

            },
            messages: {
                txtName: {
                    required: " Tên danh mục bắt buộc phải có",
                }
            }
        });
        $('.sltOrder').off('change').on('change', function (e) {
            e.preventDefault();
            var slt = $(this);
            var id = slt.data('id');
            var order = slt.val();
            $.ajax({
                url: "/ProductCategory/ChangeOrder",
                data: {
                    id: id,
                    order: order,
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        productCategoryController.loadData(true);
                        $('#status').text('Change order susses');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                    }
                    else {
                        productCategoryController.loadData(true);
                        $('#status').text('Not success');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);

                    }
                }
            });
        });
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            productCategoryController.loadData(true);
        });
        $('#frmProductCategory').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                if ($('#frmProductCategory').validate()) {
                    productCategoryController.saveData();
                }
            }
        });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            productCategoryController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmProductCategory').valid()) {
                //alert("da valid");
                productCategoryController.saveData();
            }
        });
        $('#sltPageSize').off('change').on('change', function () {
            productCategoryConfig.pageSize = $(this).val();
            productCategoryController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            productCategoryController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(10);
            productCategoryConfig.pageSize = $('#sltPageSize').val();
            productCategoryController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            productCategoryController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    productCategoryController.deleteProductCategory(id);
                }
            });
        });
        $('#txtMetaTitle').off('click').on('click', function (e) {
            var str = $('#txtName').val();
            productCategoryController.convertString(str);
        });
        $('#txtName').off('keyup').on('keyup', function (e) {
            var str = $(this).val();
            productCategoryController.convertString(str);
        });
    },
    convertString: function (str) {

        $.ajax({
            url: '/ProductCategory/ConvertString',
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
    deleteProductCategory: function (id) {
        $.ajax({
            url: '/ProductCategory/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    productCategoryController.loadData(true);
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
            url: '/ProductCategory/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#sltParent').val(data.ParentID),
                    $('#txtID').val(data.ID);
                    $('#txtName').val(data.Name);
                    $('#txtMetaTitle').val(data.MetaTitle);
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
        var name = $('#txtName').val();
        var metaTitle = $('#txtMetaTitle').val();
        var parent = $('#sltParent').val();

        var productCategory = {
            ID: id,
            Name: name,
            MetaTitle: metaTitle,
            ParentID: parent,
        }
        $.ajax({
            url: '/ProductCategory/SaveData',
            data: {
                strProductCategory: JSON.stringify(productCategory)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    $('#modalAddUpdate').modal('hide');
                    productCategoryController.loadData(true);
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
        $('#txtMetaTitle').val('');
        $('#sltParent').val('Không có');

    },
    dateFormat: function (d) {
        return ((d.getMonth() + 1) + "").padStart(2, "0")
            + "/" + (d.getDate() + "").padStart(2, "0")
            + "/" + d.getFullYear();
    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        //alert(productCategoryConfig.pageSize);
        $.ajax({
            url: '/ProductCategory/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type: type,
                pageIndex: productCategoryConfig.pageIndex,
                pageSize: productCategoryConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    var template2 = $('#data-template2').html();
                    $.each(data, function (i, item) {
                        if (item.ParentID == null) {
                            html += Mustache.render(template, {
                                ID: item.ID,
                                Name: (item.ParentID == null) ? item.Name : ('-- ' + item.Name),
                                Order: item.DisplayOrder,
                                MetaTitle: item.MetaTitle,
                                CreateDate: productCategoryController.dateFormat(new Date(parseInt((item.CreateDate).match(/\d+/)[0]))),
                                CreateBy: item.CreateBy,
                            });
                        }
                        else {
                            html += Mustache.render(template2, {
                                ID: item.ID,
                                Name: (item.ParentID == null) ? item.Name : ('-- ' + item.Name),
                                MetaTitle: item.MetaTitle,
                                CreateDate: productCategoryController.dateFormat(new Date(parseInt((item.CreateDate).match(/\d+/)[0]))),
                                CreateBy: item.CreateBy,
                            });
                        }
                        

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent + " / " + response.total + ' tổng số bản ghi');
                    productCategoryController.paging(response.total, function () {
                        productCategoryController.loadData();
                    }, changePageSize);
                    productCategoryController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / productCategoryConfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "|<<",
            next: ">",
            last: ">>|",
            prev: "<",
            visiblePages: 10,
            onPageClick: function (event, page) {
                productCategoryConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
productCategoryController.init();