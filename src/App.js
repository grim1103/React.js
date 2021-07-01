import React, {Component} from 'react';
import './App.css';
import TOC from './conponents/TOC';
import Subject from './conponents/Subject';
import Content from './conponents/Content';
import Control from './conponents/Control';


class App extends Component{
  constructor(props){
    super(props);
    //초기화를 담당
    this.state = {
      mode:'read',
      selected_content_id:3,      
      subject:{title:'WEB', sub:'Goooooooodjob'},
      welcome:{title:'welcome',desc:'Hello, React!!'},
      contents:[
        {id:1, title:'HTML', desc:'HTML is HyperText....'},
        {id:2, title:'CSS', desc:'CSS is....'},
        {id:3, title:'JavaScript', desc:'JavaScript is....'},
      ]
    }
  };

  render(){
    console.log('App render');
    var _title, _desc=null;
    if(this.state.mode==='welcome'){
      _title=this.state.welcome.title;
      _desc=this.state.welcome.desc;
    } else if(this.state.mode === 'read'){
      var i= 0;
      while(i < this.state.contents.length){
        var data = this.state.contents[i];
        if(data.id === this.state.selected_content_id){
          _title=data.title;
          _desc=data.desc;
          break;
        }
        i= i+ 1;
      }
      // _title=this.state.contents[0].title;
      // _desc=this.state.contents[0].desc;
    }
    return (
          <div className="App">
            <Subject 
              title={this.state.subject.title} 
              sub={this.state.subject.sub}
              onChangePage={function(){
               this.setState({mode:"welcome"});
              }.bind(this)}
              >
              
            </Subject>
        {/* <header>
            <h1><a href="/" onClick={function(e){
              console.log(e);
              e.preventDefault();
             // this.state.mode='welcome';
              //클릭시 mode를 수정하도록 사용
              this.setState({
                mode:'welcome'
              });
            }.bind(this)}>{this.state.subject.title}</a></h1>
            {this.state.subject.sub}
        </header> */}
            <TOC onChangePage={function(id){
             
              this.setState({
                mode:'read',
                //id가 문자로 되어있으니 숫자로 변환
                selected_content_id:Number(id)
              });
            }.bind(this)} data={this.state.contents}>
            </TOC>
            <Control onChangeMode={function(_mode){
              this.setState({
                mode:_mode
              });
            }}></Control>
            console.log(mode);
            <Content title={_title} desc={_desc}></Content>
          </div>
        );
  }
}

export default App;