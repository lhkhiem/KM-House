var aboutController = {
    init: function () {
        aboutController.registerEvent();
    },
    registerEvent: function () {
        $('#txtMetaTitle').off('click').on('click', function (e) {
            var str = $('#txtName').val();
            aboutController.convertString(str);
        });
    },
    convertString: function (str) {
        
        $.ajax({
            url: '/About/ConvertString',
            data: {
                str: str
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                $('#txtMetaTitle').val(response.str);
            }
        });
    }
}
aboutController.init();