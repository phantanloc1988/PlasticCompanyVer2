
var createProduct = {
    init: function () {
        createProduct.validate();
        this.selectCategory();
    },

    submitForm: function () {
        var locationDescription = $('#create-product-form .note-editable');

        var name = $('input[name="txtName"]').val();
        var sku = $('input[name="txtSku"]').val();
        var price = $('input[name="numPrice"]').val();
        var description = locationDescription.html();
        var descriptionImages = locationDescription.find('p img');
        var imageFiles = $('#formFile').get(0).files;
        var categoryLevel1 = $('select[name="level1"]').val();
        var categoryLevel2 = $('select[name="level2"]').val();
        var category = categoryLevel2 !== "0" ? categoryLevel2 : categoryLevel1;

        //Add Info Product
        var product = {
            Name: name,
            Sku: sku,
            Price: parseInt(price),
            Description: description,
            ProductCategoryId: parseInt(category),
        };

        var data = new FormData();

        data.append("Product", JSON.stringify(product));

        //Add Image File
        data.append("MainImage", imageFiles[0])
               
        //Add Des Image file                
        if (descriptionImages.length >= 1) {
            for (var i = 0; i < descriptionImages.length; i++) {
                var name = "DesImage" + i;
                var uri = descriptionImages[i].currentSrc;
                var fileName = descriptionImages[0].dataset.filename;

                // convert uri to File
                var file = this.convertToFile(uri, fileName);

                data.append(name, file)                
            };
        } 

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
                    type:"POST",
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
    }
}

var textEditor = {
    init: function () {
        console.log('edit')
        this.editor();
    },

    editor: function () {
        $('#summernote').summernote();
    }
}

textEditor.init();
createProduct.init();