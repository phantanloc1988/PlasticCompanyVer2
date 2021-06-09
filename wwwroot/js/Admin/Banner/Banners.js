var banner = {
    init: function () {
        this.selectBanner();
    },

    selectBanner: function () {
        $('body').on('change', '#banners-list .banner-input', function () {
            var files = $(this).get(0).files;
            var id = $(this).attr('data-id');
            var data = new FormData;
            data.append("id", id);
            data.append("image", files[0]);

            $.ajax({
                url: '/Admin/Banner/Create',
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
        })
    }
}

banner.init();