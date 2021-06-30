import React, {Component} from 'react';
import './App.css';

class Subject extends Comment{
  render(){
    return(
      <header>
       <h1>Web</h1>
        World Wide Web
      </header>
    );
  }
}

class App extends Component{
  render(){
    return (
          <div className="App">
            <Subject></Subject>
          </div>
        );
  }
}

export default App;