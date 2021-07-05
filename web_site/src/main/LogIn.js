import { Component } from 'react';

class LogIn extends Component {
      render(){
      console.log('로그인');
      return (
          <form action="./test" method="post"
          onSubmit={function(e){
            e.preventDefault();
            this.props.onSubmit(
              e.target.id.value,
              'default'
            );
          }.bind(this)}
          >
            <div>
              <div>ID: <input type="text"  id="id"/></div>
              <div>PW: <input type="text" id ="pw"/></div>
            </div>
            <div>
               <input  type="submit" value="Submit"/>
            </div>
          </form>
        );
    }
   
  }

  export default LogIn;