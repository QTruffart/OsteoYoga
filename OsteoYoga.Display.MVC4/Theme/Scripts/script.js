// Flex Slider
$(window).load(function() {
  $('.circleSlider').flexslider({
    animation: "fade",
    slideshow: true,
    touch: true,
    start: function(slider){
      $('body').removeClass('loading');
    }
  });
  $('.rectSlider, .projectSlider').flexslider({
    animation: "slide",
    slideshow: true,
    touch: true,
    start: function(slider){
      $('body').removeClass('loading');
    }
  });
});
$('body').addClass('loading');
$(document).ready(function () {

    $('#topLink').click(function (event) {
        //prevent the default action for the click event
        event.preventDefault();

        //goto that anchor by setting the body scroll top to anchor top
        $('html, body').animate({ scrollTop: 0 }, 300);
    });


    // Mobile Menu
    $mobileMenu = $('#mobileMenu');

    $("#mainMenu > ul > li").each(function () {
        var el = $(this);
        var link = el.find('a');
        var hasChildren = el.find("ul"),
      	children = el.find("li");

        if (hasChildren.length) {
            $("<optgroup />", {
                "label": el.find("> a").text()
            }).appendTo($mobileMenu);

            children.each(function () {
                $("<option />", {
                    'value': $(this).find('a').attr('href'),
                    "text": " - " + $(this).text()
                }).appendTo($mobileMenu);
            });
        } else {
            $("<option />", {
                "value": link.attr("href"),
                "text": link.text()
            }).appendTo($mobileMenu);
        }
    });

    $mobileMenu.on('change', function () {
        window.location = $(this).find("option:selected").val();
    });

    // contact form via Ajax
    var contactForm = $('#contactForm');
    contactForm.submit(submitForm);

    $("#senderEmail, #message").bind("click", function () {
        if ($(this).hasClass('error')) {
            $(this).val("");
            $(this).removeClass('error');
        }
    });

    function isEmail(email) {
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }

    function submitForm() {
        // Are all the fields filled in?	
        var errors = false;
        if (!isEmail($('#senderEmail').val())) {
            $('#senderEmail').val("This is not a valid email.").removeClass().addClass("error");
            errors = true;
        }
        if (!$('#message').val()) {
            $("#message").val("Please complete this field.").removeClass().addClass("error");
            errors = true;
        }
        if (!errors) {
            // Yes; submit the form to the PHP script via Ajax 
            $.ajax({
                url: contactForm.attr('action') + "?ajax=true",
                type: contactForm.attr('method'),
                data: contactForm.serialize(),
                success: submitFinished
            });
        }
        // Prevent the default form submission occurring
        return false;
    }

    // Handle the Ajax response	
    function submitFinished(response) {
        response = $.trim(response);
        $('#response').hide();
        if (response == "success") {
            // Form submitted successfully	        		
            $(':input', '#contactForm').not(':button, :submit, :reset, :hidden').val('');
            $('#response').removeClass().addClass('alert-success').html("<i class='icon-ok'></i> Thank you for your message!").fadeIn("slow").delay(5000).fadeOut("slow");
        } else {
            // Form submission failed: Display the failure message,
            $('#response').removeClass().addClass('alert-error').html("<i class='icon-remove'></i> There was an error in sending the message.").fadeIn("slow").delay(5000).fadeOut("slow");
        }
    }

    // isotope init
    var $container = $('.workList');
    var animationSpeed = 200;


    // work filter

    $('.workFilter a').click(function () {
        var $this = $(this);
        // don't proceed if already selected
        if ($this.hasClass('selected')) {
            return false;
        }
        var $filterSet = $this.parents('.workFilter');
        $filterSet.find('.selected').removeClass('selected');
        $this.addClass('selected');

        var selector = $(this).attr('data-filter');
        $container.isotope({ filter: selector });
        return false;
    });

    // Tool Tips
    $('.pageNav a, .socialMedia a').tooltip();


    // Accordion
    $('.accordion-group').click(function () {
        $(this).siblings().removeClass('active');
        $(this).toggleClass('active');
    });

    
});