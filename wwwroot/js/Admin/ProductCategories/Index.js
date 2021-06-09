
var createProductCate = {
    init: function () {
        this.selectLevelOneBtn();        
    },

    selectLevelOneBtn : function () {
        $('#select-list-level-one').change(function () {
            var valueSelect = $(this).val();
            
            if (valueSelect !== 0) {
                $(this).parents('form').find('.prop-name').removeClass('d-none');
                
            }
        });
    },
}

var displayProductCate = {
    init: function () {
        this.editButtonLevel1();
        this.editButtonLevel2();
        this.selectCategoryLevel1();
    },

    selectCategoryLevel1: function () {
        $('body').on('click', '.display-categories .level-1 ul li', function () {
            var id = parseInt($(this).attr('data-id'));

            //change color selected item
            $(this).parent().children('.selected-item').removeClass('selected-item');
            $(this).addClass('selected-item');
            

            // call ajax
            $.ajax({
                type: "POST",
                url: "/Admin/ProductCategories/GetChildrenOfCateById",
                data: { id: id },
                success: function (res) {
                    console.log(res.length);
                    if (res.length > 0) {
                        $('.display-categories .level-2').removeClass('d-none');
                        $('.display-categories .level-2 ul').html('').append(res);
                    }
                    else {
                        $('.display-categories .level-2').addClass('d-none');
                    }
                    
                },
            });
        })
    },

    editButtonLevel1: function () {
        $('body').on('click', '.display-categories .level-1 .edit-button', function () {
            var id = parseInt($(this).attr('data-id'));
            console.log('delete-btn click')

            $.ajax({
                type:"POST",
                url: "/Admin/ProductCategories/GetCategoryById",
                data: { id: id },               
                success: function (res) {                    
                    var name = res.name;
                    var id = parseInt(res.productCategoryId);
                    var level = parseInt(res.level);                  

                    var nameInput = $('#modal-edit-level-one-category form .name-input');
                    var levelInput = $('#modal-edit-level-one-category form .level-input');
                    var idInput = $('#modal-edit-level-one-category form .id-input');


                    nameInput.attr('placeholder', name);
                    nameInput.attr('value', name);
                    idInput.attr('value',id)
                    levelInput.attr('value', level)
                    $('#modal-edit-level-one-category').modal('show');
                },               
            });
        });

    },

    editButtonLevel2: function () {
        $('body').on('click', '.display-categories .level-2 .edit-button', function () {
            var id = parseInt($(this).attr('data-id'));
            console.log('delete-btn click')

            $.ajax({
                type: "POST",
                url: "/Admin/ProductCategories/GetCategoryById",
                data: { id: id },
                success: function (res) {
                    var name = res.name;
                    var id = parseInt(res.productCategoryId);
                    var level = parseInt(res.level);
                    var parentID = parseInt(res.parentId);

                    var nameInput = $('#modal-edit-level-two-category form .name-input');
                    var levelInput = $('#modal-edit-level-two-category form .level-input');
                    var idInput = $('#modal-edit-level-two-category form .id-input');
                    var parentCateSelectlist = $('#modal-edit-level-two-category form select').val(parentID);


                    nameInput.attr('placeholder', name);
                    nameInput.attr('value', name);
                    idInput.attr('value', id);
                    levelInput.attr('value', level);
                    parentCateSelectlist.attr('selected',"selected")
                    
                    $('#modal-edit-level-two-category').modal('show');
                },
            });
        });

    }
} 

createProductCate.init();
displayProductCate.init();