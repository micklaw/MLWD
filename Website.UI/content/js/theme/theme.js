var theme = (function() {

    function owlOptions(extensions, count) {

        var base = {
            items: count,
            itemsCustom: false,
            itemsDesktop: [1199, count],
            itemsDesktopSmall: [980, count],
            itemsTablet: [768, count],
            itemsTabletSmall: false,
            itemsMobile: [479, 1],
            singleItem: false,
            startDragging: true,
            autoPlay: 6000
        };

        $.extend(base, extensions);

        return base;
    }
    
    var $window = $(window);
    var $document = $(document);

    $window.load(function() {

        $('.main-flex-slider').flexslider({
            slideshowSpeed: 5000,
            directionNav: false,
            animation: "fade"
        });

        //OWL CAROUSEL
        $("#clients-slider").owlCarousel({
            autoPlay: 3000,
            pagination: false,
            items: 4,
            itemsDesktop: [1199, 3],
            itemsDesktopSmall: [991, 2]
        });

        $window.scroll(function() {
            if ($(this).scrollTop() > 100) {
                $('.transparent-header').css("background", "#252525");
            } else {
                $('.transparent-header').css("background", "transparent");
            }
        });
        
    });
    
    $document.ready(function() {

        $window.stellar({
            horizontalScrolling: false,
            responsive: true
        });
        
        $('.js-activated').dropdownHover({
            instantlyCloseOthers: false,
            delay: 0
        }).dropdown();

        $("#work-carousel").owlCarousel(owlOptions({
            autoPlay: 6000
        }, 3));

        $("#news-carousel").owlCarousel(owlOptions({
            autoPlay: 4000
        }, 4));

        $("#testi-carousel").owlCarousel(owlOptions({
            autoPlay: 4000
        }, 1));

        var wow = new WOW(
            {
                boxClass: 'wow',
                animateClass: 'animated',
                offset: 100,
                mobile: false
            });

        wow.init();

        $('#grid').mixitup();

        $("[data-toggle=popover]").popover();

        $("[data-toggle=tooltip]").tooltip();

    });

})();