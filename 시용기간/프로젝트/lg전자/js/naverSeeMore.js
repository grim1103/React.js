// $(document).ready(function(){

//     var seeMore= document.getElementById('seeMore').childElementCount;
//     var count=2;
//     var height=Number($('.seeMore').css('height').replace('px',''));
//     var seeMoreheight=Number($('.seeMore_place').css('height').replace('px',''));
//     var seeMorepadding=Number($('.seeMore_place').css('padding-top').replace('px',''));
//     var heightAdd = seeMoreheight+seeMorepadding;
//     console.log(seeMore);
    
//         $('.moreBt').click(function(){
//             console.log('click');
//             if(count==seeMore){
//                 $('.moreBt').css('display','none')
//             }
//             count=count+1;
            
//             console.log(seeMorepadding);
//             console.log(heightAdd);
            
//             height=height+heightAdd;
//             $('.seeMore').css('height', height+'px' );
//         });
    
// });

$(document).ready(function(){
   
        var count=2;
        var eq=1;
        var seeMore= document.getElementById('seeMore').childElementCount;
        console.log(seeMore);
        
        // var height=Number($('.seeMore').css('height').replace('px',''));
        
        // var seeMoreheight=Number($(this).css('height').replace('px',''));
        // var seeMorepadding=Number($(this).css('padding-top').replace('px',''));
        // var heightAdd = seeMoreheight+seeMorepadding;
        $('.moreBt').click(function(){
            console.log('click');
            if(count==seeMore){
                $('.moreBt').css('display','none')
            }

            $('.seeMore').children().eq(eq).css('display','block');
            console.log($('.seeMore').children().eq(count));
            eq=eq+1;
            count=count+1;
            
            
           
        });
    
        
});