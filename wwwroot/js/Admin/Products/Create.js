var createProduct = {
    init: function () {
        createProduct.validate();
        this.selectCategory();
        this.onChangeMainImage();
    },

    submitForm: function () {
        var locationDescription = $('#create-product-form .note-editable');

        var productName = $('input[name="txtName"]').val();
        var sku = $('input[name="txtSku"]').val();
        var price = $('input[name="numPrice"]').val();
        var description = locationDescription.html();
        var descriptionImages = locationDescription.find('p img');
        var status = $('input[name="checkStatus"]').attr('checked') === 'checked' ? true : false;
        var mainImage = $('#formFile').get(0).files;
        var categoryLevel1 = $('select[name="level1"]').val();
        var categoryLevel2 = $('select[name="level2"]').val();
        var category = categoryLevel2 !== "0" ? categoryLevel2 : categoryLevel1;

        var data = new FormData();

        //Add Image File        
        data.append("MainImage", mainImage[0])

        //Add Des Image file     
        if (descriptionImages.length >= 1) {
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

        console.log(description);
        //Add Info Product
        var product = {
            Name: productName,
            Sku: sku,
            Price: parseInt(price),
            Description: description,
            ProductCategoryId: parseInt(category),
            Status: status
        };

        data.append("Product", JSON.stringify(product));

        $.ajax({
            url: '/Admin/Products/Create',
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
        $('#create-product-form').validate({
            rules: {
                txtName: { required: true },
                numPrice: { required: true },
            },
            messages: {
                txtName: { required: "Vui lòng điền tên sản phẩm" },
                numPrice: { required: "Vui lòng điền giá sản phẩm" },
            },
            submitHandler: function () {
                createProduct.submitForm()
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

    onChangeMainImage: function () {
        $('#formFile').change(function () {
            helpers.displayMainImage(this)
        })
        
    },

}

var textEditor = {
    init: function () {
        this.editor();
    },

    editor: function () {
        $('#summernote').summernote({
            heigth: 1000,
            //callbacks: {
            //    onImageUpload: function (files, editor, welEditable) {
            //        console.log(files)
            //        var a = files
            //        var b = 0;

            //        sendFile(files[0], editor, welEditable);
            //    }
            //}
        });
    }
}


var helpers = {

    uploadFile: function (file) {
        data = new FormData();
        data.append("file", file);

        $.ajax({
            data: data,
            type: "POST",
            url: "/Admin/File/SaveImageFromSummernote",
            cache: false,
            contentType: false,
            processData: false,
            success: function (url) {
                //$('#summernote').summernote("insertImage", url);
            }
        });
    },

    changeSrcImageHtml: function (path, nameFile) {
        var imageElement = descriptionImages[i];
        var newSrc = path + nameFile;
        imageElement.attr("src", newSrc)
    },

    displayMainImage: function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#display-main-image').attr('src', e.target.result);
                $('#display-main-image').parent().removeClass('d-none');
            }
            reader.readAsDataURL(input.files[0]);
        } else {
            alert('select a file to see preview');
            $('#display-main-image').attr('src', '');
        }
    },
}

textEditor.init();
createProduct.init();