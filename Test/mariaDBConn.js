const mariadb = require('mariadb');

 
const pool = mariadb.createPool({
    host: 'api.minokuma.kro.kr', port:'3306',
    user: 'minokuma', password: '5658'
});
 
async function GetUserList(){
    let conn, rows;
    try{
        conn = await pool.getConnection();
        conn.query('USE test');
        rows = await conn.query('select * from event');
    }
    catch(err){
        throw err;
    }
    finally{
        if (conn) conn.end();
        return rows[0];
    }
}
 
module.exports = {
    getUserList: GetUserList
}
