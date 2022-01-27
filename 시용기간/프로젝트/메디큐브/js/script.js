
        
        //최 상단 탑 베너 무한 슬라이드
        $(window).on("scroll",function(){
            var scroll_top=$(this).scrollTop();
            console.log(scroll_top);
            if(scroll_top==0){
                $('.top_header_bn').removeClass('scroll');
            }else{
        
                $('.top_header_bn').addClass('scroll'); 
            }
        });
        //카테고리에 마우스 올렸을시 해당 카테고리 목록 표시및 밑줄 
        $(document).ready(function(){
            $('.header_categorys').hover(function(){
                
                $(this).css('border-bottom','3px solid black');
                $(this).children('#header_categorys1').css('display','block');
                
                
            });
            $('.header_categorys').mouseleave(function(){
                
                $(this).css('border-bottom','none');
                $(this).children('#header_categorys1').css('display','none');
    
            });
        });
        //카테고리에 마우스 올렸을시 해당 카테고리 목록에 마우스 올렸을시 빨간색으로 변경
        $(document).ready(function(){
            $('.gnb_category').hover(function(){
                $(this).children().css('color','#c8042f');                  
            });

            $('.gnb_category').mouseleave(function(){
                $(this).children().css('color','#3c3a39');
            });
        });
        $(document).ready(function(){
            $('.gnb_list').children().hover(function(){
                $(this).children().css('color','#c8042f');                  
            });

            $('.gnb_list').children().mouseleave(function(){
                $(this).children().css('color','#3c3a39');
            });
        });
    
        

        $(document).ready(function(){
            $('.header_categorys').hover(function(){
                var index =$(this).data('index');
               if(index>1){
                var value = 0;
                   for(i=1; i<index; i++ ){
                    value +=Number($('.header_gnb').children().eq(i-1).width());
                    value +=Number($('.header_gnb').children().eq(i-1).css('margin-right').replace('px',''));
                   }
                   $(this).children().children().css('padding-left',value);
                   
               }

            });
        });

        $(document).ready(function(){
            
            setInterval(function() {
                $('#slide_wrap').animate({
                    'top':'-37px'
                    },1000,function(){
                        $('#slide_wrap').children().eq(0).clone().appendTo('#slide_wrap');
                        $('#slide_wrap').children().eq(0).remove();
                        $('#slide_wrap').css('top','0');
                        // console.log( $('#slide_wrap').children());
                    });
            }, 2000);
        });

        // $('.search_tab button').click(function(){

        //     $(this).css({color:'#fff',backgroundColor:'#111'}); //클릭한 버튼 글자색은 #ffff,바탕은 #111

        //     $('.search_tab button').not(this).css({color:'#111',backgroundColor:'#fff'}); // 클릭한 버튼 제외는 글자색은 #111,바탕은 #fff
        //     });
            
        $(document).ready(function(){
            $('.most_po_menu_items').click(function(){
               
                $(this).css('background-color','black');
                $('.most_po_menu_items').not(this).css('background-color','#f5f5f5');
                
                $(this).css('color','white');
                $('.most_po_menu_items').not(this).css('color','rgba(0,0,0,.5)');

                var X=Number($(this).data('index'));
                console.log('X=');
                console.log(X);
       
                /// most_po_product_grup.children()=swiper-wrapper
                // $('.most_po_product').children().eq(X).css('display','flex');

                

                var aa=$('.most_po_product').children().eq(X);
                //클릭시 해당 제품의 이미지만 보이게
                aa.css('z-index','3');
                $('.most_po_product_grup').not(aa).css('z-index','1');
                  
                // aa.css('display','flex');
                aa.attr('style','display:flex !important');
                // $('.most_po_product_grup').not(aa).css('display','none');
                $('.most_po_product_grup').not(aa).attr('style','display:none !important');
                
                }
            );
    
        });