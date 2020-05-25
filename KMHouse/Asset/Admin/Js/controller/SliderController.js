var sliderConfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var sliderController = {
    init: function () {
        sliderController.loadData();
        sliderController.registerEvent();
    },
    registerEvent: function () {
        $('#frmSlider').validate({
            rules: {
                txtName: {
                    required: true,
                }

            },
            messages: {
                txtName: {
                    required: "Bạn phải nhập tên",
                },
            }
        });
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            sliderController.loadData(true);
        });
        $('#frmSlider').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                if ($('#frmSlider').valid()) {
                    sliderController.saveData();
                }
            }
            });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            sliderController.resetForm();
        });
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Slider/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        //btn.text('Kích hoạt');
                        btn.removeClass('lable-danger');
                        btn.addClass('lable-success');
                        sliderController.loadData();
                    }
                    else {
                        //btn.text('Khóa');
                        btn.removeClass('lable-success');
                        btn.addClass('lable-danger');
                        sliderController.loadData();
                    }
                }
            });
        });
        $('#btnSave').off('click').on('click', function () {
            if ($('#frmSlider').valid()) {
                sliderController.saveData();
            }
        });
        $('#sltPageSize').off('change').on('change', function () {
            sliderConfig.pageSize = $(this).val();
            sliderController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            sliderController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            sliderController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            
            $('#txtID').attr('disabled',true);
            var id = $(this).data('id');
            sliderController.loadDetail(id);
            $('#modalAddUpdate').modal('show');
        });
        $('#modalAddUpdate').hide(function () {
            $('#imgSlide').attr('src', '');
        })
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    sliderController.deleteSlider(id);
                }
            });
        });
        //$('#txtImage').off('change').on('change', function () {
        //    $('#imgSlide').attr('src',$(this).val());
        //});

        $('#btnImage').off('click').on('click', function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                $('#txtImage').val(url);
                $('#imgSlide').attr('src',url);
            };
            finder.popup();
        });

        $('.sltOrder').off('change').on('change', function (e) {
            e.preventDefault();
            var slt = $(this);
            var id = slt.data('id');
            var order = slt.val();
            $.ajax({
                url: "/SLider/ChangeOrder",
                data: {
                    id: id,
                    order: order
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        sliderController.loadData(true);
                        $('#status').text('Change order susses');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                    }
                    else {
                        sliderController.loadData(true);
                        $('#status').text('Not success');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);

                    }
                }
            });
        });
    },
    deleteSlider: function (id) {
        $.ajax({
            url: '/Slider/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    sliderController.loadData(true);
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
            url: '/Slider/GetDetail',
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
                    $('#txtTitle').val(data.Title);
                    $('#txtSeoTitle').val(data.SeoTitle);
                    $('#txtDescription').val(data.Description);
                    $('#txtLink').val(data.Link);
                    $('#txtImage').val(data.Image);
                    $('#imgSlide').attr('src', data.Image);
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
        var title = $('#txtTitle').val();
        var seoTitle = $('#txtSeoTitle').val();
        var description = $('#txtDescription').val();
        var link = $('#txtLink').val();
        var image = $('#txtImage').val();
        var slide = {
            ID: id,
            Name: name,
            Title: title,
            SeoTitle: seoTitle,
            Description: description,
            Link: link,
            Image: image

        }
        $.ajax({
            url: '/Slider/SaveData',
            data: {
                strSlide: JSON.stringify(slide)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    $('#modalAddUpdate').modal('hide');
                    sliderController.loadData(true);
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
        $('#txtTitle').val('');
        $('#txtSeoTitle').val('');
        $('#txtDescription').val('');
        $('#txtLink').val('');
        $('#txtImage').val('');
        $('#imgSlide').attr('src', '');
    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        sliderConfig.pageSize = $('#sltPageSize').val();
        $.ajax({
            url: '/Slider/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type:type,
                pageIndex: sliderConfig.pageIndex,
                pageSize: sliderConfig.pageSize
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
                            Title:item.Title,
                            SeoTitle:item.SeoTitle,
                            Description: item.Description,
                            Link: item.Link,
                            Image: item.Image,
                            Order: item.DisplayOrder,
                            Status: item.Status == true ? "<a data-id=" + item.ID + " class=\"label label-success btn-active\">Active</a>" : "<a data-id=" + item.ID + " class=\"label label-danger btn-active\">Lock</a>"
                        });

                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent+" / "+ response.total +' tổng số bản ghi');
                    sliderController.paging(response.total, function () {
                        sliderController.loadData();
                    }, changePageSize);
                    sliderController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / sliderConfig.pageSize);

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
                sliderConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
sliderController.init();