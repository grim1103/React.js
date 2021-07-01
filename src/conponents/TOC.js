import React, {Component} from 'react';

class TOC extends Component{
    render(){
      var lists  =[];
      var data = this.props.data;
      var i = 0 ;
      while(i < data.lenth){

        lists.push(<li key={data[i].id}><a href={"/content/"+data[i].id}>{data[i].title}</a></li>);
        
        i=i+1;

      }

      return(
        <nav>
            <ul>
              <li><a href="HTML is HyperText...." >HTML</a></li>
              {lists}
            </ul>
          </nav>
      );
    }
  }

  export default TOC;