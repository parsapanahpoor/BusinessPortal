jQuery(function ($) {
    'use strict';
	
	// Header Sticky
	$(window).on('scroll',function() {
		if ($(this).scrollTop() > 30){  
			$('.navbar-area').addClass("is-sticky");
		}
		else{
			$('.navbar-area').removeClass("is-sticky");
		}
	});

	// Mean Menu
	jQuery('.mean-menu').meanmenu({
		meanScreenWidth: "1199"
	});
	
	// Others Option For Responsive JS
	$(".others-option-for-responsive .dot-menu").on("click", function(){
		$(".others-option-for-responsive .container .container").toggleClass("active");
	});

	// Search Menu JS
	//$(".others-options .search-box").on("click", function(){
	//	$(".search-overlay").toggleClass("search-overlay-active");
	//});
	//$(".search-overlay-close").on("click", function(){
	//	$(".search-overlay").removeClass("search-overlay-active");
	//});

	// Language Switcher
	$(".language-option").each(function() {
        var each = $(this)
        each.find(".lang-name").html(each.find(".language-dropdown-menu a:nth-child(1)").text());
        var allOptions = $(".language-dropdown-menu").children('a');
        each.find(".language-dropdown-menu").on("click", "a", function() {
            allOptions.removeClass('selected');
            $(this).addClass('selected');
            $(this).closest(".language-option").find(".lang-name").html($(this).text());
        });
    })

	// Down Arrow JS
	//$('.navbar .navbar-nav li a, .down_arrow .scroll_down').on('click', function(e){
	//	var anchor = $(this);
	//	$('html, body').stop().animate({
	//		scrollTop: $(anchor.attr('href')).offset().top - 50
	//	}, 1500);
	//	e.preventDefault();
	//});

	// Home Slides
	$('.home-slides').owlCarousel({
		loop: true,
		nav: true,
		rtl: true,
		dots: false,
		animateOut: 'fadeOut',
		autoplayHoverPause: true,
		autoHeight:true,
		autoplay: true,
		items: 1,
		margin: 25,
		navText: [
			"<i class='ri-arrow-right-line'></i>",
			"<i class='ri-arrow-left-line'></i>"
		],
	});

	// Hero Slides
	$('.hero-slides').owlCarousel({
		loop: true,
		nav: true,
		rtl: true,
		dots: false,
		animateOut: 'fadeOut',
		smartSpeed: 1000,
		autoplayHoverPause: true,
		autoplay: true,
		autoHeight:true,
		items: 1,
		margin: 25,
		navText: [
			"<i class='ri-arrow-left-line'></i>",
			"<i class='ri-arrow-right-line'></i>"
		],
	});

	// Services Slides
	$('.services-slides').owlCarousel({
		loop: true,
		nav: true,
		rtl: true,
		dots: false,
		autoHeight: true,
		smartSpeed: 500,
		margin: 25,
		autoplayHoverPause: true,
		autoplay: true,
		navText: [
			"<i class='ri-arrow-right-line'></i>",
			"<i class='ri-arrow-left-line'></i>"
		],

		responsive: {
			0: {
				items: 1
			},
			768: {
				items: 2
			},
			1200: {
				items: 4
			}
		}
	});
	$('.services-slides-two').owlCarousel({
		loop: true,
		nav: true,
		dots: false,
		rtl: true,
		autoHeight: true,
		smartSpeed: 500,
		margin: 25,
		autoplayHoverPause: true,
		autoplay: true,
		navText: [
			"<i class='ri-arrow-left-line'></i>",
			"<i class='ri-arrow-right-line'></i>"
		],

		responsive: {
			0: {
				items: 1
			},
			768: {
				items: 2
			},
			1200: {
				items: 3
			}
		}
	});

	// Partner Slides
	$('.partner-slides').owlCarousel({
		loop: true,
		nav: false,
		dots: false,
		rtl: true,
		autoHeight: true,
		smartSpeed: 500,
		margin: 25,
		autoplayHoverPause: true,
		autoplay: true,

		responsive: {
			0: {
				items: 2
			},
			768: {
				items: 2
			},
			1200: {
				items: 3
			}
		}
	});
	$('.partner-slides-two').owlCarousel({
		loop: true,
		nav: false,
		dots: false,
		rtl: true,
		autoHeight: true,
		smartSpeed: 500,
		margin: 25,
		autoplayHoverPause: true,
		autoplay: true,

		responsive: {
			0: {
				items: 2
			},
			768: {
				items: 2
			},
			1200: {
				items: 4
			}
		}
	});

	// Pricing Slides
	$('.pricing-slides').owlCarousel({
		loop: true,
		nav: true,
		rtl: true,
		dots: false,
		smartSpeed: 500,
		margin: 25,
		autoplayHoverPause: true,
		autoplay: true,
		navText: [
			"<i class='ri-arrow-right-line'></i>",
			"<i class='ri-arrow-left-line'></i>"
		],
		responsive: {
			0: {
				items: 1
			},
			768: {
				items: 2
			},
			1200: {
				items: 2
			}
		}
	});

	// Team Slides
	$('.team-slides').owlCarousel({
		loop: true,
		nav: true,
		dots: false,
		rtl: true,
		autoHeight: true,
		smartSpeed: 500,
		margin: 25,
		autoplayHoverPause: true,
		autoplay: true,
		navText: [
			"<i class='ri-arrow-right-line'></i>",
			"<i class='ri-arrow-left-line'></i>"
		],

		responsive: {
			0: {
				items: 1
			},
			768: {
				items: 2
			},
			1200: {
				items: 4
			}
		}
	});

	// Testimonial Slides
	$('.testimonial-slides').owlCarousel({
		loop: true,
		nav: true,
		rtl: true,
		dots: false,
		animateOut: 'fadeOut',
		autoplayHoverPause: true,
		autoHeight:true,
		autoplay: true,
		items: 1,
		margin: 25,
		navText: [
			"<i class='ri-arrow-right-line'></i>",
			"<i class='ri-arrow-left-line'></i>"
		],
	});

	// Project Slides
	$('.project-slides').owlCarousel({
		loop: true,
		nav: true,
		rtl: true,
		dots: false,
		autoHeight: true,
		smartSpeed: 500,
		margin: 25,
		autoplayHoverPause: true,
		autoplay: true,
		navText: [
			"<i class='ri-arrow-right-line'></i>",
			"<i class='ri-arrow-left-line'></i>"
		],

		responsive: {
			0: {
				items: 1
			},
			768: {
				items: 2
			},
			1200: {
				items: 4
			}
		}
	});

	// Shop Slides
	$('.shop-slides').owlCarousel({
		loop: true,
		nav: false,
		rtl: true,
		dots: true,
		autoHeight: true,
		smartSpeed: 500,
		margin: 25,
		autoplayHoverPause: true,
		autoplay: true,

		responsive: {
			0: {
				items: 1
			},
			768: {
				items: 2
			},
			1200: {
				items: 3
			}
		}
	});

	// Subscribe form
	$(".newsletter-form").validator().on("submit", function (event) {
		if (event.isDefaultPrevented()) {
		// handle the invalid form...
			formErrorSub();
			submitMSGSub(false, "لطفا ایمیل خود را به درستی وارد کنید.");
		} 
		else {
			// everything looks good!
			event.preventDefault();
		}
	});
	function callbackFunction (resp) {
		if (resp.result === "success") {
			formSuccessSub();
		}
		else {
			formErrorSub();
		}
	}
	function formSuccessSub(){
		$(".newsletter-form")[0].reset();
		submitMSGSub(true, "با تشکر از شما برای اشتراک!");
		setTimeout(function() {
			$("#validator-newsletter").addClass('hide');
		}, 4000)
	}
	function formErrorSub(){
		$(".newsletter-form").addClass("animated shake");
		setTimeout(function() {
			$(".newsletter-form").removeClass("animated shake");
		}, 1000)
	}
	function submitMSGSub(valid, msg){
		if(valid){
			var msgClasses = "validation-success";
		} 
		else {
			var msgClasses = "validation-danger";
		}
		$("#validator-newsletter").removeClass().addClass(msgClasses).text(msg);
	}
	// AJAX MailChimp
	$(".newsletter-form").ajaxChimp({
		url: "https://envytheme.us20.list-manage.com/subscribe/post?u=60e1ffe2e8a68ce1204cd39a5&amp;id=42d6d188d9", // Your url MailChimp
		callback: callbackFunction
	});

	// Count Time 
	function makeTimer() {
		var endTime = new Date("September 20, 2025 17:00:00 PDT");			
		var endTime = (Date.parse(endTime)) / 1000;
		var now = new Date();
		var now = (Date.parse(now) / 1000);
		var timeLeft = endTime - now;
		var days = Math.floor(timeLeft / 86400); 
		var hours = Math.floor((timeLeft - (days * 86400)) / 3600);
		var minutes = Math.floor((timeLeft - (days * 86400) - (hours * 3600 )) / 60);
		var seconds = Math.floor((timeLeft - (days * 86400) - (hours * 3600) - (minutes * 60)));
		if (hours < "10") { hours = "0" + hours; }
		if (minutes < "10") { minutes = "0" + minutes; }
		if (seconds < "10") { seconds = "0" + seconds; }
		$("#days").html(days + "<span>روز</span>");
		$("#hours").html(hours + "<span>ساعت</span>");
		$("#minutes").html(minutes + "<span>دقیقه</span>");
		$("#seconds").html(seconds + "<span>ثانیه</span>");
	}
	setInterval(function() { makeTimer(); }, 0);

	// Input Plus & Minus Number JS
	$('.input-counter').each(function() {
		var spinner = jQuery(this),
		input = spinner.find('input[type="text"]'),
		btnUp = spinner.find('.plus-btn'),
		btnDown = spinner.find('.minus-btn'),
		min = input.attr('min'),
		max = input.attr('max');
		
		btnUp.on('click', function() {
			var oldValue = parseFloat(input.val());
			if (oldValue >= max) {
				var newVal = oldValue;
			} 
			else {
				var newVal = oldValue + 1;
			}
			spinner.find("input").val(newVal);
			spinner.find("input").trigger("change");
		});
		btnDown.on('click', function() {
			var oldValue = parseFloat(input.val());
			if (oldValue <= min) {
				var newVal = oldValue;
			} 
			else {
				var newVal = oldValue - 1;
			}
			spinner.find("input").val(newVal);
			spinner.find("input").trigger("change");
		});
	});

	// Popup Video
	$('.popup-youtube').magnificPopup({
		disableOn: 320,
		type: 'iframe',
		mainClass: 'mfp-fade',
		removalDelay: 160,
		preloader: false,
		fixedContentPos: false
	});

	// Datepicker
	$("#datepicker").datepicker();

	// Selectize
    $(".selectize-filter").selectize({
        create: true,
        sortField: 'text'
    });

	// Odometer JS
	$('.odometer').appear(function(e) {
		var odo = $(".odometer");
		odo.each(function() {
			var countNumber = $(this).attr("data-count");
			$(this).html(countNumber);
		});
	});

	// AOS JS
	AOS.init();

	// WOW Animation JS
	if($('.wow').length){
		var wow = new WOW({
			mobile: false
		});
		wow.init();
	}

	// Project More Item
	$(".project-more-item").slice(0, 6).show();
		$("#loadmore").on('click', function (e) {
		e.preventDefault();
		$(".project-more-item:hidden").slice(0, 3).slideDown();
	});

	// Services More Item
	$(".services-more-item").slice(0, 6).show();
		$("#loadmore").on('click', function (e) {
		e.preventDefault();
		$(".services-more-item:hidden").slice(0, 3).slideDown();
	});

	// Blog More Item
	$(".blog-more-item").slice(0, 3).show();
		$("#loadmore").on('click', function (e) {
		e.preventDefault();
		$(".blog-more-item:hidden").slice(0, 1).slideDown();
	});

	// Go to Top
	$(window).on('scroll', function(){
		var scrolled = $(window).scrollTop();
		if (scrolled > 600) $('.go-top').addClass('active');
		if (scrolled < 600) $('.go-top').removeClass('active');
	});  
	$('.go-top').on('click', function() {
		$("html, body").animate({ scrollTop: "0" },  500);
	});
	
	// Preloader JS
	jQuery(window).on('load',function(){
		jQuery(".preloader").fadeOut(500);
	});

}(jQuery));