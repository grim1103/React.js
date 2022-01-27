



$(document).ready(function(){
    $('.bottom_list_items').mouseover(function(){
        $(this).css('border-bottom','5px solid black');   
        $(this).children('.product').css('display','block');
    });

    $('.bottom_list_items').mouseleave(function(){
                
        $(this).css('border-bottom','none');
        $(this).children('.product').css('display','none');  
        
    });

    $('.bottom_list_items').mouseleave(function(){
        $(this).css('border-bottom','none');
        $(this).children('.product').css('display','none');
        // $(this).css('font-weight','none'); 
        
        var a=$('.product_list_bottom').children();
        console.log(a);
        // a.css('display','block');
    });


    $('.product_list_top_item').mouseover(function(){
        $(this).css('border-bottom','2px solid black');
        
        var index=$(this).data('index');
        var a=$('.product_list_bottom').children().eq(index);
        a.css('display','block');
        $('.product_list_bottom_items').not(a).css('display','none');
    });
    $('.product_list_top_item').mouseleave(function(){
        $(this).css('border-bottom','none');
        
    });

    //회사소개 메뉴 나태내기
    $('.conponyInt').mouseover(function(){
        console.log('hi');
        $('.conponyInt_list').css('display','block');
        
    });
    $('.conponyInt').mouseleave(function(){
        $('.conponyInt_list').css('display','none');
    
    });

    //회사소개 메뉴 없애기
    $('.conponyInt_list_items').hover(function(){
        $(this).css('background-color' , '#D3D3D3');
    });
    $('.conponyInt_list_items').mouseleave(function(){
        $(this).css('background-color' , 'white');
    });

    $('.conponyInt_list_items').hover(function(){
        $(this).css('background-color' , '#D3D3D3');
    });
    $('.conponyInt_list_items').mouseleave(function(){
        $(this).css('background-color' , 'white');
    });

    //우측 하단 버튼
    var fix_btn1= true;
    $('.fix_btn1').click(function(){
        $('.fix_btn1_p').toggleClass('change');
        if(fix_btn1==true){
            console.log('checkkkk');
            // $('.fix_btn1_p').css('background','url(https://www.lge.co.kr/lg5-common/images/icons/icon-more-plus-x.png)').css('background-size','40px');
            fix_btn1=false;
            $('.fix_btn1_list').css('display', 'block');
            
        }else{
            console.log('check');
            // $('.fix_btn1_p').css('background','url(https://www.lge.co.kr/lg5-common/images/icons/icon_img_floating_40.svg)').css('background-size','40px');
            fix_btn1=true;
            $('.fix_btn1_list').css('display', 'none');
        }   
    });
    $('.fix_btn2').click(function(){
        $('html,body').stop().animate({scrollTop:0},300);
    });

    //메뉴전체보기 클릭
    $('.all_menu_items').on('click',function(){
        $(this).toggleClass('change');
    });
    var open=true;
    $('.all_menu_items').click(function(){
        
        console.log($(this.after()));
        if(open==true){
            $('.footer_open').css('display','block');
            open=false;
        }else{
            $('.footer_open').css('display','none');
            open=true;
        }
    });
    var lenguege=true;
    $('.middle_items').click(function(){
        if(lenguege==true){
            $('.lenguege').css('display','block');
            lenguege=false;
            $('.middle_items').children('img').css({
                'transform':'rotate(180deg)'
            });
        }else{
            $('.lenguege').css('display','none');
            lenguege=true;
            $('.middle_items').children('img').css({
                'transform':'rotate(0deg)'
            });
        }
    });
    
});



