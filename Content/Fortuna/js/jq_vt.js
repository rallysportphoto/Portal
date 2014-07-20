/*
   Move comments form
*/

var slider_auto = 0;

function move_form_to(ee)
{
   var e = $("#form_holder").html();
   var tt = $("#form_holder .share_comment").text();
   $("#form_holder").slideUp(500, function () {
      $("#form_holder").remove();
      
      ee.append('<div id="form_holder">'+e+'</div>');
      $("#form_holder .share_comment").html(tt);
      $("#form_holder [valed]").removeAttr('valed');
      $("#form_holder .do_clear").attr('remove', 1);
      
      Cufon('#form_holder .share_comment', {
         color: '-linear-gradient(#b8b4b1, #edebe8)', textShadow: '1px 1px #000'
      });
      
      $(".formError").remove();
      
      $("#form_holder").hide().slideDown(500);
      
      upd_ev();
   });
}

function upd_ev()
{
   /* 
      Валидация форм
   */
   
   $("[placeholder]").each(function () {
      $(this).val( $(this).val().replace( $(this).attr("placeholder"), "" ) );
      $(this).placeholder();
   });
   $("form .go_submit").unbind().click(function () {
      var e=$(this).parents("form");
      e.find("input:text, textarea").each(function () {
         $(this).unbind();
         $(this).val( $(this).val().replace( $(this).attr("placeholder"), "" ) );
      });
      if (!e.attr("valed")) e.validationEngine();
      e.attr("valed", "1");
      e.submit(); 
      e.find("input, textarea").each(function () {
         $(this).placeholder();
      });      
      return false;
   });
   $("form .do_clear").unbind().click(function () {
      $(this).parents("form").find("input, textarea").each(function () {
         $(this).val("").placeholder();
      });
      $(".formError").remove();
      
      if ($(this).attr("remove") && !$(this).parents("#form_prev_holder").length) 
      {
         move_form_to( $("#form_prev_holder") );
         $("#form_holder .do_clear").removeAttr('remove');
      }
      
      return false;
   });
   
   /*
      End: Валидация форм
   */
}

$(document).ready(function () {

   $(".comment .comments_count").click(function () {
   
      move_form_to( $(this).parent().parent() );
      
      return false;
   });

   /* Всплывающее окошко для навигационной ленты */

   var popup_options = {
      jump_height:        30,
      show_duration:      300,
      hide_duration:      300,
      tout:               200,
      top:                -131   // почему-то $("ul.pxs_thumbnails div").position().top дает неправильное значение 
   };
   
   if (!$.browser.msie)
   {
      $("ul.pxs_thumbnails li div").css({
         display:       'none',
         opacity:       0
      });
   }
   
   var touts    = new Array();
   var cur_elem = null;
   var n=0;
   $("ul.pxs_thumbnails li").each(function () {
      $(this).attr("n", ++n);
   });
   
   $("ul.pxs_thumbnails li").hover(function () {
      cur_elem = $("div", this);
      touts[ parseInt($(this).attr("n")) ] = setTimeout(function () {
         if ($.browser.msie)
         {
            cur_elem.show().css({
               top:        popup_options.top+"px"
            });
         }
         else
         {
            cur_elem.show().css({
               opacity:    0,
               top: ( popup_options.top - popup_options.jump_height )+"px"
            }).animate({
               opacity:    1,
               top:        popup_options.top+"px"
            }, {
               duration:   popup_options.show_duration,
               queue:      false,
               complete:   function () {
                  //$(this).hide();
               }
            });
         }
      }, popup_options.tout);
   }, function () {
   
      if (touts[ parseInt($(this).attr("n")) ]) clearTimeout(touts[ parseInt($(this).attr("n")) ]);
   
      if ($.browser.msie)
      {
         $("div", this).hide();
      }
      else
      {
         $("div", this).animate({
            opacity:    0
         }, {
            duration:   popup_options.hide_duration,
            queue:      false,
            complete:   function () {
               $(this).hide();
            }
         });
      }   
   });
   
   /* End: Всплывающее окошко для навигационной ленты */
   
   
   $(window).resize(function () {
      $(".pxs_slider").children().eq(0).css('margin-left', '0px');
   });
   
   
   /* Слайдер, перенес инициализацию из slider.js, добавляю выезжание стрелки */
   
   if ($('#pxs_container').length)
   {
   
      var arrow_duration  = 300;
      var arrow_prev      = $( $(".pxs_slider").children()[0] );
      var arrow_left_init = "-"+$("#pxs_container div.desc").width()+"px";
      
      $(".pxs_slider").children().eq(0).css('margin-left', '-10px');
      
      $("#pxs_container div.desc:gt(0)").css({
         left: arrow_left_init
      });
      
	   $('#pxs_container').parallaxSlider({
	      auto: slider_auto,
	      animDone: function (parent) {
	         $("div.desc", parent).show().animate({
	            left: '0px'
	         }, {
	            duration: arrow_duration,
	            queue: false, 
	            complete: function () {
	               $("div.desc", arrow_prev).hide().css('left', arrow_left_init);
	               arrow_prev = parent;
	            }
	         });
	      }
	   });
	
	}
   
   /* End: Слайдер */
   
   
   
   
   
   /* 
      Блоки с картинками и на них текст - добавление fade 
   */
   
   var blocks_speed_fade_in  = 300;
   var blocks_speed_fade_out = 300;
   
   $(".col_1-3 .desc").css({
      display: 'block',
      opacity: 0
   });
   
   $(".col_1-3").hover(function () {
      $(".desc", this).animate({
         opacity: 1
      }, {
         duration: blocks_speed_fade_in,
         queue: false,
         complete: function () {
            if ($.browser.msie) this.style.removeAttribute('filter');
         }
      });
   }, function () {
      $(".desc", this).animate({
         opacity: 0
      }, {
         duration: blocks_speed_fade_out,
         queue: false
      });
   });
   
   /*
      End: Блоки с картинками и на них текст - добавление fade 
   */
   
   
   
   
   upd_ev();
   
   
   /* 
      Цвет меню 
   */
   
   $("#mainmenu li.act").addClass("active");
   
   var n=0;
   $("#mainmenu > li").each(function () {
      $(this).find("a:eq(0)").attr("id", "m"+(++n));
   });
   $("#mainmenu > li").mouseover(function () {
      if ( $(this).hasClass('active') )
         return;
      Cufon.replace('#'+$(this).find("a:eq(0)").attr("id"), {
		      color: '-linear-gradient(#950d38, #dc3560)', textShadow: '1px 1px #000'
      });
   });
   $("#mainmenu > li").mouseout(function () {
      if ( $(this).hasClass('active') )
         return;
      Cufon.replace('#'+$(this).find("a:eq(0)").attr("id"), {
		   color: '-linear-gradient(#b8b4b1, #edebe8)', textShadow: '1px 1px #000',
		   hover: {
			   color: '-linear-gradient(#950d38, #dc3560)', textShadow: '1px 1px #000'
		   }
      });
   });
      
   /*
      End: цвет меню
   */
   
   if (1)
   {
      var popup_options2 = { top: 25 };
      
      var touts2    = new Array();
      var cur_elem2 = null;
      var n2=0;
      $("#mainmenu > li").each(function () {
         if ( !$(this).children("div").length )
            return;
         $(this).attr("n", ++n2).addClass("parent");
      });
      
      $("#mainmenu > li.parent").hover(function () {
         cur_elem2 = $("div", this);
         touts2[ parseInt($(this).attr("n")) ] = setTimeout(function () {
            if ($.browser.msie)
            {
               cur_elem2.show().css({
                  display:    'block',
                  top:        popup_options2.top+"px"
               });
            }
            else
            {
               cur_elem2.css({
                  opacity:    0,
                  display:    'block',
                  top: ( popup_options2.top + popup_options.jump_height )+"px"
               }).animate({
                  opacity:    1,
                  top:        popup_options2.top+"px"
               }, {
                  duration:   popup_options.show_duration,
                  queue:      false
               });
            }
         }, popup_options.tout);
      }, function () {
      
         if (touts2[ parseInt($(this).attr("n")) ]) clearTimeout(touts2[ parseInt($(this).attr("n")) ]);
      
         if ($.browser.msie)
         {
            $("div", this).hide();
         }
         else
         {
            $("div", this).animate({
               opacity:    0
            }, {
               duration:   popup_options.hide_duration,
               queue:      false,
               complete:   function () {
                  $(this).hide();
               }
            });
         }   
      });
   }
   
   $(".gal").attr("rel", "gal[g]");
   if ($.prettyPhoto && $(".gal").length)
   {
      $(".gal").each(function () {
         $(this).attr("rel", "gal[g]")
            .attr("title",  $(this).find("h4").text() );
      });
      $("a[rel=gal\\[g\\]]").prettyPhoto({
         theme: 'light_rounded',
         gallery_markup: ''
      });
   }
   
   $(".sh").each(function () {
      var now = 0;
      var maxnow = $(this).children(".item").length-1;
      var ee = $(this);
      $(this).parent().find(".larr, .rarr").click(function () {
         var the_now = now;
         if ( !$(this).hasClass('larr') ) now++; else now--;
         if (now<0) now = maxnow;
         if (now>maxnow) now=0;
         var now_h = ee.height();
         //$(".widget_arr").hide();
         ee.find(".item:eq("+the_now+")").fadeOut(300, function () {
            var gg = ee.find(".item:eq("+now+")");
            gg.show();
            ee.css({ height: 'auto' });
            var new_h = ee.height();
            gg.hide();
            ee.css({ height: now_h }).animate({ height: new_h }, { duration: 300, complete: function () {
               //$(".widget_arr").show();
            } });
            gg.fadeIn(300);
         });
         return false;
      });
   });
   
});

