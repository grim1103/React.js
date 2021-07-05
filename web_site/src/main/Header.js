import { Component } from 'react';
class Header extends Component {
  
    render(){
      var id=this.props.id;

      console.log('헤더');
      console.log("test ID===" + this.props.id);

      var logInOUt= null;

      if(id === undefined){
          logInOUt=  <input onClick={function(e){
              e.preventDefault();
              this.props.onChangeMode('logIn');
              }.bind(this)}
              type="button" value="로그인"/>
      }else{
        logInOUt= <input onClick={function(e){
          e.preventDefault();
          this.props.onChangeMode('default',undefined);
                  }.bind(this)}
        type="button" value="로그아웃"
        />
      }
      return(
      <div>
        <div>
          <input onClick={function(e){
            e.preventDefault();
            this.props.onChangeMode('default');
            }.bind(this)}
            type="button" value="go Main"/>

          {logInOUt}

          <input onClick={function(e){
            e.preventDefault();
            this.props.onChangeMode('inFome');
            }.bind(this)}
            type="button" value="입력"/>

          {/* <input onClick={function(e){
            e.preventDefault();
            this.props.onChangeMode('logIn');
            }.bind(this)}
            type="button" value="로그인"/> */}
            
          {/* <input onClick={function(e){
            e.preventDefault();
            this.props.onChangeMode('default');
            }.bind(this)}
          type="button" value="로그아웃"
          /> */}
        </div>
      </div>
            
            );
            
    }
   
  }

  export default Header ;