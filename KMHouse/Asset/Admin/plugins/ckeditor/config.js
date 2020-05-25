/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
    config.filebrowserBrowseUrl = '/Asset/Admin/plugins/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Asset/Admin/plugins/ckfinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl = '/Asset/Admin/plugins/ckfinder/ckfinder.html?type=Flash';
    config.filebrowserUploadUrl = '/Asset/Admin/plugins/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Asset/Admin/plugins/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/Asset/Admin/plugins/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Flash';
};
