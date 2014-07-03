	/* ---------------------------------------------------------------------- */
	/*	Contact Form
	/* ---------------------------------------------------------------------- */

$(function(){

$("#btnSend").click(function(){

$.ajax({type:'POST', url: './php/contact.php', data:$('#frmContact').serialize(), success: function(response) {
$("#spanMessage").html('Please Wait...');


	 if(parseInt(response)>0)
	   {
		 $("#spanMessage").html('<div class="alert alert-success"><button type="button" class="close" data-dismiss="alert">&times;</button><strong>Well done!</strong> Your message has been sent.</div>');
	   }
	   else{
		 alert(response);
		 $("#spanMessage").html('<div class="alert alert-error"><button type="button" class="close" data-dismiss="alert">&times;</button><strong>Error! </strong> Somthing Wrong</div>');
	   }
	   
		 
}});


});

	/* ---------------------------------------------------------------------- */
	/*	Email Subscriber
	/* ---------------------------------------------------------------------- */
	
$("#btnSubscribe").click(function(){

$.ajax({type:'POST', url: './php/subscribe.php', data:$('#frmSubscribe').serialize(), success: function(response) {



	 if(parseInt(response)>0)
	   {
		 alert('Successfully sent...');
	   }
	   else{
		 alert(response);
		
	   }
	   
		 
}});


});


	/* ---------------------------------------------------------------------- */
	/*	Blog Like Button
	/* ---------------------------------------------------------------------- */


$('.boxlike').each(function(index){

var likebtnID = $(this).attr('id');
var likebtnArray = likebtnID.split('-');
var pageID = likebtnArray[1];


$.ajax({type:'GET', url: './php/get-likes.php', data:{'pageid':pageID}, success: function(response) {
		 
		   $('#boxlike-'+pageID).html(response);
		 
		 }
		 });
});

$(".likebtn").click(function(){

var likebtn = $(this);
var likebtnID = $(likebtn).attr('id');
var likebtnArray = likebtnID.split('-');
var pageID = likebtnArray[1];
$.ajax({type:'GET', url: './php/like.php', data:{'pageid':pageID}, success: function(response) {

	 if(parseInt(response)>0)
	   {
		 $.ajax({type:'GET', url: './php/get-likes.php', data:{'pageid':pageID}, success: function(response) {
		 
		   $('#boxlike-'+pageID).html(response);
		 
		 }
		 });
	   }
	   else{
		 alert(response);
		
	   } 	 
}
});

});

});




	/* ---------------------------------------------------------------------- */
	/*	Accordion
	/* ---------------------------------------------------------------------- */
	
	(function() {

		var $container = $('.acc-container'),
			$trigger   = $('.acc-trigger');

		$container.hide();
		$trigger.first().addClass('active').next().show();

		var fullWidth = $container.outerWidth(true);
		$trigger.css('width', fullWidth);
		$container.css('width', fullWidth);
		
		$trigger.on('click', function(e) {
			if( $(this).next().is(':hidden') ) {
				$trigger.removeClass('active').next().slideUp(300);
				$(this).toggleClass('active').next().slideDown(300);
			}
			e.preventDefault();
		});

		// Resize
		$(window).on('resize', function() {
			fullWidth = $container.outerWidth(true)
			$trigger.css('width', $trigger.parent().width() );
			$container.css('width', $container.parent().width() );
		});

	})();
	
	/* ---------------------------------------------------------------------- */
	/*	Twitter (Latest Tweet)
	/* ---------------------------------------------------------------------- */

function tz_format_twitter(twitters) {
  var statusHTML = [];
  for (var i=0; i<twitters.length; i++){
    var username = twitters[i].user.screen_name;
    var status = twitters[i].text.replace(/((https?|s?ftp|ssh)\:\/\/[^"\s\<\>]*[^.,;'">\:\s\<\>\)\]\!])/g, function(url) {
      return '<a href="'+url+'">'+url+'</a>';
    }).replace(/\B@([_a-z0-9]+)/ig, function(reply) {
      return  reply.charAt(0)+'<a href="http://twitter.com/'+reply.substring(1)+'">'+reply.substring(1)+'</a>';
    });
    statusHTML.push('<span>'+status+'</span> <br/><b><em>'+relative_time(twitters[i].created_at)+'</em></b>');
  }
  return statusHTML.join('');
}

function relative_time(time_value) {
  var values = time_value.split(" ");
  time_value = values[1] + " " + values[2] + ", " + values[5] + " " + values[3];
  var parsed_date = Date.parse(time_value);
  var relative_to = (arguments.length > 1) ? arguments[1] : new Date();
  var delta = parseInt((relative_to.getTime() - parsed_date) / 1000);
  delta = delta + (relative_to.getTimezoneOffset() * 60);

  if (delta < 60) {
    return 'less than a minute ago';
  } else if(delta < 120) {
    return 'about a minute ago';
  } else if(delta < (60*60)) {
    return (parseInt(delta / 60)).toString() + ' minutes ago';
  } else if(delta < (120*60)) {
    return 'about an hour ago';
  } else if(delta < (24*60*60)) {
    return 'about ' + (parseInt(delta / 3600)).toString() + ' hours ago';
  } else if(delta < (48*60*60)) {
    return '1 day ago';
  } else {
    return (parseInt(delta / 86400)).toString() + ' days ago';
  }
}

					$(document).ready(function(){
					$.getJSON('http://api.twitter.com/1/statuses/user_timeline/webnus.json?count=1&callback=?', function(tweets){
					$("#twitter").html(tz_format_twitter(tweets));
					}); });

	/* ---------------------------------------------------------------------- */
	/*	Scroll to top
	/* ---------------------------------------------------------------------- */

  jQuery(document).ready(function(){ 
 
        jQuery(window).scroll(function(){
            if (jQuery(this).scrollTop() > 100) {
                jQuery('.scrollup').fadeIn();
            } else {
                jQuery('.scrollup').fadeOut();
            }
        }); 
 
        jQuery('.scrollup').click(function(){
            jQuery("html, body").animate({ scrollTop: 0 }, 700);
            return false;
        });
 
    });
