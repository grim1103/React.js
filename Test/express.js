var express = require('express');
var app = express();

app.use(express.static('public'));

app.get('/user', function (req, res) {
   res.sendFile( __dirname + "/" + "index.html" );
})

app.get('/process_get', function (req, res) {
//   Prepare output in JSON format
    response = {
          first_name:req.query.first_name,
                last_name:req.query.last_name
                   };
                      console.log(response);
                         res.end(JSON.stringify(response));
                         })


app.get('/', function(req,res){
        console.log("api.minokuma.kro.kr");
        res.send('api.minokuma.kro.kr');
})

var server = app.listen(3306, function(){
        var host = server.address().address
        var port = server.address().port

        console.log("Running http://api.minokuma.kro.kr", host, port)
})