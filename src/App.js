import React, {Component} from 'react';
import './App.css';
import TOC from './conponents/TOC';
import Subject from './conponents/Subject';
import Content from './conponents/Content';


class App extends Component{
  constructor(props){
    super(props);
    //초기화를 담당
    this.state = {
      subject:{title:"GOOD", sub:"Goooooooodjob"},
      contents:[
        {id:1, title:'HTML', desc:'HTML is HyperText....'},
        {id:2, title:'CSS', desc:'CSS is....'},
        {id:3, title:'JavaScript', desc:'JavaScript is....'},
        {id:4, title:'JavaScript', desc:'JavaScript is....'}
      ]
    }
  };

  render(){
    return (
          <div className="App">
            <Subject 
              title={this.state.subject.title} 
              sub={this.state.subject.sub}>
            </Subject>
            <TOC data={this.state.contents}></TOC>
            <Content title="HTML" desc="HTML is HyperText Markup Language."></Content>
          </div>
        );
  }
}

export default App;