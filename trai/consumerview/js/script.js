function CheckAll(id, pID) {
    $("#" + pID + " :checkbox").prop('checked', $('#' + id).prop('checked'));
  }

  $(document).ready(function(){
    //console.log('loading');
    // $.mobile.loading( "hide" );
    $('.card-result-expand').click(function(){
        console.log(this);
        if(this.attr('aria-expanded') == false){
            $('card-result-plus-sign').css('display', 'block');
        }
     });
    $('#type-of-device > ul > li:nth-child(1) > input[type="radio"]').on('click',function(){ 
        $('#type-mobiles').css('display','block');
     });
    $('#type-of-device > ul > li:nth-child(2) > input[type="radio"]').on('click',function(){ 
       $('#type-mobiles').css('display','none');
    });
    $('a.sort-by-filter').on('click',function(){
        var t = $(this);
        $('a.sort-by-filter').removeClass('active');
        t.addClass('active');
    });
	
/*    $(".check2").click(function(){
		$(".panel2").css('display', 'none');
        $("#try").slideToggle();
		
		if($('#try').css('display') == 'block'){
				
			$("#expand").html('view less..');
		}
		else {
			$("#expand").html('view more..');
		}
		
    });*/
	
	$('.op').click(function(){

       $('.slide2').slideToggle('slow');
       if($(this).text() == 'View less...'){
           $(this).text('View more...');
       } else {
           $(this).text('View less...');
       }
});
	
	
	$('.check2').click(function(){

       $('.panel2').slideToggle('slow');
       if($(this).text() == 'View less...'){
           $(this).text('View more...');
       } else {
           $(this).text('View less...');
       }
});
  });
  


  $('a').click(function () {
      if ($(this).find('i').hasClass('fa fa-sort-down')) {
          $(this).find('i').removeClass('fa fa-sort-down').addClass('fa fa-sort-up');
      } else {
          $(this).find('i').removeClass('fa fa-sort-up').addClass('fa fa-sort-down');
      }
  });