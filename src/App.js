import React, {Component} from 'react';
import './App.css';
import TOC from './conponents/TOC';
import Subject from './conponents/Subject';
import Content from './conponents/Content';


class App extends Component{
  render(){
    return (
          <div className="App">
            <Subject title="WEB" sub="World Wide Web zz"></Subject>
            <Subject title="React" sub="React.js"></Subject>
            <TOC></TOC>
            <Content title="HTML" desc="HTML is HyperText Markup Language."></Content>
          </div>
        );
  }
}

export default App;