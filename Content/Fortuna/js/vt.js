$(document).ready(function() {

   $("#mainmenu > li").each(function () {
      alert ( $(this).children("div").length );
      if ( $(this).children("div").length )
         $(this).addClass("parent");
   });

	$("li.parent").hover(function (event) {
		//$(this).children("div").animate({height:'toggle'},250);
		$(this).children("div").animate({height:'300px'},250);
	});

	$(".toggle a.question").click(function (event) {
		event.preventDefault(); 
		$(this).toggleClass("act");
		$(this).next("div.answer").slideToggle("fast");
	});
	

	
    $("li.parent div ul li a").hover(function (event) {
		event.preventDefault();
		$(this).children("i").animate({width:'toggle'},100);
	});


	$("div#top_line ul li a").click(function (event) {
		event.preventDefault(); 
		$(this).prev("div").animate({height:'toggle'},250);
	});


});
