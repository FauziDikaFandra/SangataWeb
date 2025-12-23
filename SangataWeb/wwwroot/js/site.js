// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
setTimeout(function () {
    stopBlinking = true;
}, 4000);

function blink(selector) {
    bgcolor = $(selector).css('background-color');
    $(selector).fadeOut('slow', function () {
        $(this).fadeIn('slow', function () {
            if (!stopBlinking) {
                blink(this);
            }
            else {
                $(this).fadeIn('slow').css('background-color', bgcolor);
            }
        });
    }).css("background-color", "#ffff99");
}

function setselect(ths) {
    var $this = ths;
    if ($this.hasClass('ddmulti1')) {
        $this.select2({
            width: '100%',
            dropdownAutoWidth: true,
            templateResult: function (data) {
                return formatResultMulti(data, $this.data('first'), $this.data('two'))
            }
        });
    }
    else if ($this.hasClass('ddmulti2')) {
        $this.select2({
            width: '100%',
            dropdownAutoWidth: true,
            templateResult: function (data) {
                return formatResultMulti2(data, $this.data('first'), $this.data('two'), $this.data('three'))
            }
        });
    }
    else if ($this.hasClass('ddmulti3')) {
        $this.select2({
            width: '100%',
            dropdownAutoWidth: true,
            templateResult: function (data) {
                return formatResultMulti3(data, $this.data('first'), $this.data('two'), $this.data('three'), $this.data('four'))
            }
        });
    }
    else {
        $this.select2({
            width: '100%',
            dropdownAutoWidth: true,
        });
    }
}

function formatResultMulti(data, col1, col2) {
    var city = $(data.element).data('city');
    var classAttr = $(data.element).attr('class');
    var hasClass = typeof classAttr != 'undefined';
    classAttr = hasClass ? ' ' + classAttr : '';
    var $result = $(
        '<div class="row">' +
        '<div class="col-' + col1 + classAttr + '">' + data.text + '</div>' +
        '<div class="col-' + col2 + classAttr + '">' + city + '</div>' +
        '</div>'
    );
    return $result;
}
function formatResultMulti2(data, col1, col2, col3) {
    var city = $(data.element).data('city');
    var code = $(data.element).data('code');
    var classAttr = $(data.element).attr('class');
    var hasClass = typeof classAttr != 'undefined';
    classAttr = hasClass ? ' ' + classAttr : '';
    var $result = $(
        '<div class="row">' +
        '<div class="col-' + col1 + classAttr + '">' + data.text + '</div>' +
        '<div class="col-' + col2 + classAttr + '">' + code + '</div>' +
        '<div class="col-' + col3 + classAttr + '">' + city + '</div>' +
        '</div>'
    );
    return $result;
}
function formatResultMulti3(data, col1, col2, col3, col4) {
    var city = $(data.element).data('city');
    var code = $(data.element).data('code');
    var group = $(data.element).data('group');
    var classAttr = $(data.element).attr('class');
    var hasClass = typeof classAttr != 'undefined';
    classAttr = hasClass ? ' ' + classAttr : '';
    var $result = $(
        '<div class="row">' +
        '<div class="col-' + col1 + classAttr + '">' + data.text + '</div>' +
        '<div class="col-' + col2 + classAttr + '">' + code + '</div>' +
        '<div class="col-' + col3 + classAttr + '">' + group + '</div>' +
        '<div class="col-' + col4 + classAttr + '">' + city + '</div>' +
        '</div>'
    );
    // var $result = '<div class="row">' +
    //     '<div class="col-xs-3"><b>Client</b></div>' +
    //     '<div class="col-xs-3"><b>Account</b></div>' +
    //     '<div class="col-xs-3"><b>Deal</b></div>' +
    //     '</div>';
    return $result;
}
$(document).ready(function () {

    //ini bisa
    //const button = document.getElementById('tabMaterial');
    //button.addEventListener('click', function () {

    //});
    


    //$("#tabMaterial").on('click', function () {
    //    $('#form-wizard-tab2').find("select").each(function () {
    //        $(this).select2('destroy'); 
    //        setSelect($(this));
    //    });
    //});

    //function setSelect(ths) {
    //    var $this = ths;
    //    if ($this.hasClass('ddmulti1')) {
    //        $this.select2({
    //            width: '100%',
    //            dropdownAutoWidth: true,
    //            templateResult: function (data) {
    //                return formatResultMulti(data, $this.data('first'), $this.data('two'))
    //            }
    //        });
    //    }
    //    else if ($this.hasClass('ddmulti2')) {
    //        $this.select2({
    //            width: '100%',
    //            dropdownAutoWidth: true,
    //            templateResult: function (data) {
    //                return formatResultMulti2(data, $this.data('first'), $this.data('two'), $this.data('three'))
    //            }
    //        });
    //    }
    //    else if ($this.hasClass('ddmulti3')) {
    //        $this.select2({
    //            width: '100%',
    //            dropdownAutoWidth: true,
    //            templateResult: function (data) {
    //                return formatResultMulti3(data, $this.data('first'), $this.data('two'), $this.data('three'), $this.data('four'))
    //            }
    //        });
    //    }
    //    else {
    //        $this.select2({
    //            width: '100%',
    //            dropdownAutoWidth: true,
    //        });
    //    }
    //}

    $("select").each(function () {
        setselect($(this));
    });

 
    
    const numberInput = document.querySelectorAll('input.vnumber');


    numberInput.forEach(element => {
        // Add a keypress event listener to each element
        element.addEventListener('keypress', function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;

            // Check if the key code is not a number (0-9)
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                event.preventDefault(); // Prevent the character from being entered
                return false;
            }
            return true;
        });
    });

    const textInput = document.querySelectorAll('input.vtext,textarea.vtext');

    textInput.forEach(element => {
        // Add a keypress event listener to each element
        element.addEventListener('keyup', function () {
            element.classList.remove('is-invalid', 'is-valid');
            if (element.value === "") {
                element.classList.add('is-invalid');
            }
            else {
                element.classList.add('is-valid');
            }
        });
    });
    $('select.vtext').on('select2:select', function (e) {
        $(this).removeClass('is-invalid').removeClass('is-valid');
        if ($(this).prop("selectedIndex") == 0) {
            $(this).addClass('is-invalid');
        }
        else {
            $(this).addClass('is-valid');
        }
    });
});