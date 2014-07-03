	$(document).ready(function (){  
	  //Usage
	  $(".menu ul li, .portfoliofilter ul li").menu({
			itemLink: 0
	  });
	}); 

	$(window).load(function(){
		$('.flexslider, .widget-slider, .post-slider').flexslider({
			animation: "fade",
			directionNav: false, 
			controlNav: true,
			start: function(slider){
			$('body').removeClass('loading');
			}
		});
	});

	//pretty photo
	jQuery('a[data-gal]').each(function() {
	    jQuery(this).attr('rel', jQuery(this).data('gal'));
	});  	
	jQuery("a[data-gal^='prettyPhoto']").prettyPhoto({ animationSpeed: 'slow', slideshow: false, overlay_gallery: false, theme: 'light_square', social_tools: false, deeplinking: false });
	jQuery("a[rel^='prettyPhoto']").prettyPhoto({ animationSpeed: 'slow', slideshow: true, overlay_gallery: true, theme: 'light_square', social_tools: false, deeplinking: false });
	
	// dmtop
	jQuery(window).scroll(function(){
		if (jQuery(this).scrollTop() > 1) {
			jQuery('.dmtop').css({bottom:"25px"});
		} else {
			jQuery('.dmtop').css({bottom:"-100px"});
		}
	});
	jQuery('.dmtop').click(function(){
		jQuery('html, body').animate({scrollTop: '0px'}, 800);
		return false;
	});

// Skills Bar
    var maxWidth = 240;
    $('.skill-level').each(function () {
        animateSkill($(this));
    });
    function animateSkill(skillLevelContainer) {
        var level = parseInt(skillLevelContainer.attr('data-value'));
        var w = maxWidth * level / 100;
        skillLevelContainer.animate({ width: w }, {
            duration: 3000,
            step: function (currentWidth) {
                var percent = parseInt(currentWidth / maxWidth * 100);
                if (isNaN(percent))
                    percent = 0;
                $(this).parent().find('.skill-percent').html(percent + '%');
            }
        });
    }
    var bars = $('.bars')
    $('#skills li').click(function () {
        var previousSection = $($('#skills .selected a').attr('href'));
        $('#skills li').removeClass('selected');
        $(this).addClass('selected');
        previousSection.hide();
        var currentSection = $($(this).find('a').attr('href')).show();
        currentSection.find('.skill-level').css('width', '0px').each(function () {
            animateSkill($(this));
        });
        return false;
    });


$(".menu ul").tinyNav({
  active: 'active', // String: Set the "active" class
  header: '-- Main Menu --', // String: Specify text for "header" and show header instead of the active item
});

	