import { Component } from 'react';
import Img from '../img/exe.png'
class Body extends Component {
    render(){
      console.log('바디');
      return (
        <div>
          <p>사진 및 메인 화면 넣을 자리</p>
          {/* <img src={Img} alt="profile" /> */}
        </div>
        );
    }
   
  }

  export default Body;