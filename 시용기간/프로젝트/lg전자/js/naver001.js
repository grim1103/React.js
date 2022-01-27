$(document).ready(function(){
   
    var ele = document.getElementById('a');
    var eleCount = ele.childElementCount;
    // var d =Number(eleCount)-3;
     
    var count=0;

    console.log('count');
    console.log('eleCount');
    console.log(count);
    console.log(eleCount);

    if(count==0){
        $('.left_bt').css('display','none');
    }else{
        $('.left_bt').css('display','block');
    }
    
    if(eleCount==count){
        $('.right_bt').css('display','none');
    }else{
        $('.right_bt').css('display','block');
    }
   
    $('.left_bt').click(function(){
        
        count= count-1;
        if(count==0){
            $('.left_bt').css('display','none');
        }else{
            $('.left_bt').css('display','block');
        }
        
        if(eleCount==count){
            $('.right_bt').css('display','none');
        }else{
            $('.right_bt').css('display','block');
        }
            
            
            var box=$('.bord').children().eq(count);
            box.css('display','block');
            $('.bord_box').not(box).css('display', 'none');
            console.log(count);

    });
    $('.right_bt').click(function(){
        count= count+1;
        if(count==0){
            $('.left_bt').css('display','none');
        }else{
            $('.left_bt').css('display','block');
        }
        var a= eleCount-3;
        if(a==count){
            $('.right_bt').css('display','none');
        }else{
            $('.right_bt').css('display','block');
        }
            
            console.log(count);
            var box=$('.bord').children().eq(count);
            box.css('display','block');
            $('.bord_box').not(box).css('display', 'none');
        
    });
    var img="<div class='sub_img'><img src='img/logo.png' width='125px' height='65px' alt=''></div>";
    var btn="<div class='sub_bt'><button>구독</button><button>기사보기</button></div>";
    $('.bord_contents_items').mouseover(function(){
        

        
        $(this).children().remove();

        $(this).append(btn);
        
    });
    $('.bord_contents_items').mouseleave(function(){

        $(this).children().remove();
        $(this).append(img);
 

    });
});