var about = {
    init: function () {
        this.textEdiotor();
        this.submitForm();
    },

    submitForm: function () {
        $('#about-information #submit-button').click(function () {
            var contentElement = $('#about-information .note-editable');
            var content = contentElement.html();
            var imgElement = contentElement.find('p img');

            var data = new FormData();
            data.append('content', content);

            for (var i = 0; i < imgElement.length; i++) {

                var uri = imgElement[i].currentSrc;
                var fileName = `AboutFileName${i}.png`;
                               
                var file = helpers.convertUriToFile(uri, fileName);

                var name = `AboutImage${i}`
                data.append(name, file);
            }

            $.ajax({
                url: '/Admin/Information/About',
                type: 'POST',
                contentType: false,
                processData: false,
                data: data,
                success: function (res) {
                    console.log(res)
                }
            })
        })
    },

    textEdiotor: function () {
        $('#about-information #about-text-editor').summernote();
    }
}

var contact = {
    init: function () {
        this.textEdiotor();
        this.submitForm();
    },

    submitForm: function () {
        $('#contact-information #submit-button').click(function () {
            var contentElement = $('#contact-information .note-editable');
            var content = contentElement.html();

            $.ajax({
                url: '/Admin/Information/Contact',
                type: 'POST',
                data: { content: content},
                success: function (res) {
                    if (res === "Ok") {
                        $('#success-modal-achievement').modal('show');
                    } else {
                        $('#fail-modal-notification').modal('show')
                    }
                }
            })
        })
    },

    textEdiotor: function () {
        $('#contact-information #contact-text-editor').summernote({
            toolbar: [
                // [groupName, [list of button]]
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['font', ['strikethrough', 'superscript', 'subscript']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['height', ['height']]
            ]
        });
    }
}

var helpers = {
    convertUriToFile: function (URI, filename) {
        var arr = URI.split(','), mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
        return new File([u8arr], filename, { type: mime });
    },

    //convertUrlToBase64Image: function (src, callback, outputFormat) {
    //    var img = new Image();
    //    img.crossOrigin = 'Anonymous';
    //    img.onload = function () {
    //        var canvas = document.createElement('CANVAS');
    //        var ctx = canvas.getContext('2d');
    //        var dataURL;
    //        canvas.height = this.naturalHeight;
    //        canvas.width = this.naturalWidth;
    //        ctx.drawImage(this, 0, 0);
    //        dataURL = canvas.toDataURL(outputFormat);
    //        callback(dataURL);
    //    };
    //    img.src = src;
    //    if (img.complete || img.complete === undefined) {
    //        img.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
    //        img.src = src;
    //    }
    //},

    //getBase64Image: function (dataUrl) {
    //    alert(dataUrl)
    //    console.log('RESULT:', dataUrl)
    //}        
}

about.init();
contact.init();