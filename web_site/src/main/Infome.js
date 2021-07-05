import { Component } from 'react';
import moment from 'moment';
import 'moment/locale/ko';

class Infome extends Component {
    constructor(props){
        super(props);
        this.state = {
        contents:[
            <option value='0'>0</option>
        ]
    }
    };

  render(){
    const nowTime = moment().format('YYYY');
    var time=1800;
    var NowTime=Number(nowTime);
    
    var _select=[];
    while(time <= NowTime){

        console.log(time);

        time=time+1;

    }
    

    

    return(

    <form>
        <table border='1'>
        <tr>
            <td colspan='6' align='center'>출력일보</td>
        </tr>
        <tr>
            <td>일시:</td>
            <td><select >
            {this.state.contents}
            </select></td>
        </tr>
        <tr>
            <td>인원:</td><td colspan='5'><input type='text' />명</td>
        </tr>
        <tr>
            <td>작업자명</td><td>공정</td><td>공수</td><td>노무비</td><td>계</td><td>비고</td>
        </tr>
        <tr>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
        </tr>
        <tr>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
        </tr>
        <tr>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
        </tr>
        <tr>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
            <td><input type='text' /></td>
        </tr>
        </table>
    </form>


      );
  }

}

export default Infome ;