// 상수인 변수
var video_height       = 650;     //  영상 높이
var video_height_half  = 325;     //  영상 높이 반  

// 변수 
var video_arr = [];               //  영상 리스트 배열
var lastScrollTop      = 0;       //  마지막 스크롤 탑 

// 스크롤 이벤트 발생
$(window).scroll(function(event){
    
    var st      = $(this).scrollTop();                  // 스크롤 top 값                                                         
    var win_hei =  st + $(window).height();             // 현재스크롤 top 값 +  브라우저 크기                                          
    var show_min_top = st - video_height_half;          // 스크롤 top - 비디오 크기 반 
    var show_max_top = win_hei + video_height_half;     // 브라우저 bottom 값 + 비디오 크기 반 

    if (st > lastScrollTop){ // scroll down
        fn_view_check(st,win_hei,video_arr);
        fn_init_video();
        fn_show_list_controll(show_min_top,show_max_top ,'down');
    }else{ //scroll up 
        fn_view_check(st,win_hei,video_arr);
        fn_init_video();
        fn_show_list_controll(show_min_top,show_max_top,'up');
    }

     lastScrollTop = st;
});


// 페이지 로드시 실행시킬 뷰 선택
$(document).ready(function(){
     var start_view = $(window).scrollTop() + $(window).height() - ($(window).height() / 3);   

    $('.video').each(function (index, item) {   
        $(this).addClass('video_'+Number(index+1));
        video_arr.push('video_'+Number(index+1));
    })

    for(a=1; a<video_arr.length+1; a++) 
    {
        if($('.video_' + a).offset().top < start_view  ){ 
            $('.video_' + a ).get(0).play();  
            $('.video_' + a).addClass('play');
        }else{
            $('.video_' + a ).get(0).pause(); 
        }
    }
});


function fn_view_check(st,win_hei,video_arr){
    for(a=1; a<video_arr.length+1; a++)  
    {
        var item  = $('.video_' + a).offset().top + video_height;
        var video = $('.video_' + a).get(0);
        if( st < item && item < win_hei + video_height){ 
            $('.video_' + a).addClass('show'); 
        }else{
            $('.video_' + a).removeClass('show');
        } 
    }
}

function fn_init_video(){
    $('.video').each(function (index, item) {  
        var video = $(this).get(0);
        if(!$(this).hasClass('show'))
        {
            $(this).get(0).currentTime = 0;
            $(this).get(0).pause();
        }else{
            if(!video.pause)
            {
                video.get(0).play(); 
            }
        }

    });
}

function fn_show_list_controll(show_min_top,show_max_top,state){
    $('.video').each(function (index, item) {  
        if($(this).hasClass('show'))
        {
            if( "down" == state )
            {
                var top_y = $(this).offset().top ;
                if(top_y<=show_min_top)
                {
                    $(this).get(0).pause();
                }else{
                    if(!$(this).prop("ended")){
                        $(this).get(0).play();
                    }
                }
              
            }else{
            
                var top_y = $(this).offset().top + video_height;
                if(show_max_top <= top_y)
                {
                    $(this).get(0).pause();
                }else{
                    if(!$(this).prop("ended")){
                        $(this).get(0).play();
                    }
                }
            }
        }
    });
}