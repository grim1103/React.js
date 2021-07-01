import React, {Component} from 'react';

class Control extends Component{
    render(){
      console.log('Content render');
      return(
        <ul>
        <li><a href="/create" onclick={function(e){
          e.preventDefault();
          this.onChangeMode('create');
        }.bind(this)}>create</a></li>
        <li><a href="/update" onclick={function(e){
          e.preventDefault();
          this.onChangeMode('update');
        }.bind(this)}>update</a></li>
        <li><input onclick={function(e){
          e.preventDefault();
          this.onChangeMode('delete');
        }.bind(this)} type="button" value="delete"/></li>
      </ul>
      );
    }
  }
  
  export default Control;