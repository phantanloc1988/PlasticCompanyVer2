﻿
//var values = {
//    name: $('')
//}

var editProduct = {
    init: function () {
        this.showDescriptionContent();
        this.changeMainImage()
    },

    submitForm: function () {
        var locationDescription = $('#edit-product-form .note-editable');

        var productId = $('#edit-product-form').attr('data-product-id');
        var productName = $('input[name="txtName"]').val();
        var sku = $('input[name="txtSku"]').val();
        var price = $('input[name="numPrice"]').val();
        var description = locationDescription.html();
        var descriptionImages = locationDescription.find('p img');
        var status = $('input[name="checkStatus"]').attr('checked') === 'checked' ? true : false;
        var categoryLevel1 = $('select[name="level1"]').val();
        var categoryLevel2 = $('select[name="level2"]').val();
        var category = categoryLevel2 !== "0" ? categoryLevel2 : categoryLevel1;

        var data = new FormData();

        //Add Image File
        var isChangeMainImage = $('#main-image-input').attr('data-is-changed');

        if (isChangeMainImage === true) {
            var mainImage = $('#formFile').get(0).files;
            var fileName = `${Date.now().toString()}-${Math.random()}.png`;

            data.append("MainImage", mainImage[0])
        }

        //Add Des Image file
        var oldContentLength = $('#edit-product .description-content').attr('data-content-length');
        var newContentLength = $('.note-editable').html(content).length;

        var idChangeDescriptionContent = newContentLength !== oldContentLength ? true : false;

        if (idChangeDescriptionContent === true && descriptionImages.length >= 1) {
            for (var i = 0; i < descriptionImages.length; i++) {
                var name = "DesImage" + i;
                var uri = descriptionImages[i].currentSrc;
                var fileName = `${i}-${Date.now().toString()}-${Math.random()}.png`;

                // convert uri to File
                var file = this.convertToFile(uri, fileName);
                data.append(name, file)

                //Change path src image in HTML
                var imageElement = descriptionImages[i];
                imageElement.removeAttribute('src')
                var path = "Images/ProductImages/"
                var newSrc = path + fileName;
                imageElement.src = newSrc;
            };
        }        

        //Add Info Product
        var product = {
            ProductId: productId,
            Name: productName,
            Sku: sku,
            Price: parseInt(price),
            Description: description,
            ProductCategoryId: parseInt(category),
            Status: status
        };

        data.append("Product", JSON.stringify(product));

        $.ajax({
            url: '/Admin/Products/Edit',
            type: 'POST',
            contentType: false,
            processData: false,
            data: data,
            success: function (res) {
                if (res.status === "Ok") {
                    window.location.href = res.url;
                }
                window.location.href.res.url;
            }
        })
    },

    validate: function () {
        console.log('validate ok')
        $('#edit-product-form').validate({
            rules: {
                txtName: { required: true },
                numPrice: { required: true },
            },
            messages: {
                txtName: { required: "Vui lòng điền tên sản phẩm" },
                numPrice: { required: "Vui lòng điền giá sản phẩm" },
            },
            submitHandler: function () {
                editProduct.submitForm()
            }
        })
    },

    convertToFile: function convert_URI_to_file(URI, filename) {
        var arr = URI.split(','), mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
        return new File([u8arr], filename, { type: mime });
    },

    selectCategory: function () {
        $('body').on('change', '#create-product-form select', function () {

            var levelOfSelectList = $(this).attr('name');

            //level 1 selected successFully
            if (levelOfSelectList === 'level1' && $(this).val() !== "0") {
                var id = $(this).val();

                $.ajax({
                    type: "POST",
                    url: '/Admin/ProductCategories/FindChilrenOfCategory',
                    data: { id: id },
                    success: function (res) {
                        var result = [];
                        for (var i = 0; i < res.length; i++) {
                            result += `<option value=${res[i].productCategoryId}>${res[i].name}</option>`
                        }
                        // level-1 has children

                        var optionDefault = '<option value=0 selected="selected">Vui lòng chọn</option>';

                        $('#create-product-form select[name="level2"]').html('');
                        $('#create-product-form select[name="level2"]').append(optionDefault);
                        $('#create-product-form select[name="level2"]').append(result);
                        $('#create-product-form select[name="level2"]').parent().removeClass('d-none');

                        var valueOfLevel2 = $('#create-product-form select[name="level2"]').val();
                        if (valueOfLevel2 === "0") {
                            $('#create-product-form .Info').addClass('d-none');
                        }

                        // level-1 has not children
                        if (res.length === 0) {
                            $('#create-product-form select[name="level2"]').parent().addClass('d-none');
                            $('#create-product-form .Info').removeClass('d-none');

                        }
                    }
                })


            }//level 2 selected successFully
            else if (levelOfSelectList === 'level2' && $(this).val() !== "0") {
                $('#create-product-form .Info').removeClass('d-none');
            } //level 2 selected unsuccessFully
            else if (levelOfSelectList === 'level2' && $(this).val() === "0") {
                $('#create-product-form .Info').addClass('d-none');
            } else {
                $('#create-product-form .Info').addClass('d-none');
                $('#create-product-form select[name="level2"]').parent().addClass('d-none');

            }
        })
    },

    showDescriptionContent: function () {
        var content = $('#edit-product .description-content').html();

        $('#description-content-editor').summernote();
        $('.note-editable').html(content);

        var contentLength = content.length
        $('#edit-product .description-content').attr('data-content-length', contentLength);
    },

    changeMainImage: function () {
        $('#main-image-input').change(function () {
            $(this).attr('data-is-changed',true)
        });
    },
}

editProduct.init()