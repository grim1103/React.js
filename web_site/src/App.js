import { Component } from 'react';
import Body from './main/Body';
import Footer from './main/Footer';
import Header from './main/Header';
import LogIn from './main/LogIn';
import Infome from './main/Infome';


class App extends Component {
  constructor(props){
    super(props);
    this.state = {
     mode:'default',
     id:null
    }
  };

  render(){ 
  console.log('App'); 
  var _header,_article = null;
  console.log('App의 ID'+this.state.id);
  _header=<Header onChangeMode={
    function(_mode){
      this.setState({
        mode:_mode
      }
      );
    }.bind(this)
     }></Header>

  if(this.state.mode === "logIn"){

    _article=<LogIn onSubmit={function(_id,_mode){
      this.setState({
        
        id:_id,
        mode:_mode
      });
      
    }.bind(this)}>
      </LogIn>
  }else if(this.state.mode === 'inFome'){
    _article=<Infome></Infome>
  }
  else{
    //로그인 했을때
    if(this.state.id === null){
      _header=
      <div>
        <Header onChangeMode={function(_mode){
          this.setState({
              mode:_mode
            }
          );
        }.bind(this)
         }></Header>
          
          
      </div>
      //로그아웃 또는 로그인 전
    }else{
      _header=
      <div>
        <Header onChangeMode={function(_mode){
          this.setState({
            mode:_mode       
          }
          );
        }.bind(this)
         } id={this.state.id}></Header>
         <p>{this.state.id} 님 하이 </p>
      </div>
    }
    _article=<Body></Body>
  }

  
  

    return (
      <div>

          {_header} 

          <div>{_article}</div>

          <Footer></Footer>
      </div>

      );
  }
 
}
export default App;
