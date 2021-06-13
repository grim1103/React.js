using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Hanyokessai.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.ComponentModel;
using MySqlX.XDevAPI.Common;
using System.Text;

namespace Hanyokessai
{
    public class DbService
    {
        public string ConnectionString { get; set; }

        public DbService(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// t_templeteからテンプレートファイルとテンプレートファイルサイズを取得する。
        /// </summary>
        /// <param name="templeteId"></param>
        public void SelectTempleteInfoByTempleteIdTa300(string templeteId)
        {
            string SQL = "SELECT file_name, file_binary, templete_file_size FROM t_templete WHERE templete_id = '" + templeteId + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string file_name = reader["file_name"].ToString();
                            string fileBinary = reader["file_binary"].ToString();
                            UInt32 templeteFileSize = (uint)reader["templete_file_size"];
                            Common.templateDownload(file_name, fileBinary, templeteFileSize);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        public Zb200Dto GetUserInfoByMailZb200(string memberId)
        {
            Zb200Dto zb200 = null;
            string SQL = "SELECT tm.member_id, tm.authority_cd, mc.comName_ryak, md1.dep1_name, md2.dep2_name, mp.pos_name, date_format(tm.entry_date, '%Y/%m/%d') as entry_date, tm.mail, tm.member_fistname, tm.member_lastname, tm.member_fistname_kana, tm.member_lastname_kana, tm.member_fistname_eng, tm.member_lastname_eng, tm.gender, tm.cellphone, tm.zipcode, tm.address, tm.stamp_cd, CONVERT(tm.stamp_image USING utf8) as stamp_image, date_format(tm.upd_date, '%Y/%m/%d %H:%i:%s') as upd_date, tm.upd_id FROM t_member tm, m_company mc , m_department1 md1, m_department2 md2, m_position mp WHERE tm.com_cd = mc.com_cd AND tm.dep1_cd = md1.dep1_cd AND tm.dep2_cd = md2.dep2_cd AND tm.pos_cd = mp.pos_cd AND tm.member_id = '" + memberId + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        zb200 = new Zb200Dto();
                        zb200.labelKaiinId = reader["member_id"].ToString();
                        zb200.labelPw = "";
                        zb200.labelKenGen = reader["authority_cd"].ToString();
                        zb200.labelKaiSyaMei = reader["comName_ryak"].ToString();
                        zb200.labelSyoZoku1 = reader["dep1_name"].ToString();
                        zb200.labelSyoZoku2 = reader["dep2_name"].ToString();
                        zb200.labelSyoKui = reader["pos_name"].ToString();
                        zb200.labelNyuSyaBi = reader["entry_date"].ToString();
                        zb200.labelKauInMail = reader["mail"].ToString();
                        zb200.labelKanJiSei = reader["member_fistname"].ToString();
                        zb200.labelKanJiMei = reader["member_lastname"].ToString();
                        zb200.labelKaNaSei = reader["member_fistname_kana"].ToString();
                        zb200.labelKaNaMei = reader["member_lastname_kana"].ToString();
                        zb200.labelEiGoSei = reader["member_fistname_eng"].ToString();
                        zb200.labelEiGoMei = reader["member_lastname_eng"].ToString();
                        zb200.labelSeiBeTu = reader["gender"].ToString();
                        zb200.labelKaInTel = reader["cellphone"].ToString();
                        zb200.labelPostNo = reader["zipcode"].ToString();
                        zb200.labelAddress = reader["address"].ToString();
                        zb200.labelOuInUm = reader["stamp_cd"].ToString();
                        zb200.labelOuInZyoHo = reader["stamp_image"].ToString();
                        zb200.labelSyuSeiBi = reader["upd_date"].ToString();
                        zb200.labelSyuSeiSya = reader["upd_id"].ToString();
                    }
                }
                conn.Close();
                return zb200;
            }
        }

        public Zb300Dto GetUserInfoByMailZb300(string email)
        {
            Zb300Dto zb300 = null;
            string SQL = "SELECT member_id, authority_cd, mail, member_fistname, member_fistname_kana, member_fistname_eng, member_lastname, member_lastname_kana,"
                + "member_lastname_eng, entry_date, com_cd, dep1_cd, dep2_cd, pos_cd, gender, cellphone, zipcode, address, stamp_cd, CONVERT(stamp_image USING utf8) as Stamp_image "
                + "FROM t_member WHERE mail = '" + email + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        zb300 = new Zb300Dto();
                        zb300.member_id = reader["member_id"].ToString();
                        zb300.txtPwd = "*******";
                        zb300.txtMail = reader["mail"].ToString();
                        zb300.txtAuthorityCode = reader["authority_cd"].ToString();
                        zb300.txtCompanyCode = reader["com_cd"].ToString();
                        zb300.txtDepartment1 = reader["dep1_cd"].ToString();
                        zb300.txtDepartment2 = reader["dep2_cd"].ToString();
                        zb300.txtPosition = reader["pos_cd"].ToString();
                        zb300.txtEnterDate = reader["entry_date"].ToString();
                        zb300.txtKanjiFirstName = reader["member_fistname"].ToString();
                        zb300.txtKanjiLastName = reader["member_lastname"].ToString();
                        zb300.txtKanaFirstName = reader["member_fistname_kana"].ToString();
                        zb300.txtKanaLastName = reader["member_lastname_kana"].ToString();
                        zb300.txtEngFirstName = reader["member_fistname_eng"].ToString();
                        zb300.txtEngLastName = reader["member_lastname_eng"].ToString();
                        zb300.txtGender = reader["gender"].ToString();
                        zb300.txtPhone = reader["cellphone"].ToString();
                        zb300.txtZipcode = reader["zipcode"].ToString();
                        zb300.txtAddress = reader["address"].ToString();
                        zb300.txtStampCode = reader["stamp_cd"].ToString();
                        zb300.txtimgStamp = reader["Stamp_image"].ToString();
                    }
                }
                conn.Close();
                return zb300;
            }
        }

        /// <summary>
        /// personテーブルをSELECT！
        /// </summary>
        public List<Data> GetData()
        {
            List<Data> list = new List<Data>();
            string SQL = "SELECT * FROM Person";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Data()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            Name = reader["Name"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Phone = reader["Phone"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
                conn.Close();
                return list;
            }

        }
        /// <summary>
        /// 会員登録INSERT
        /// </summary>
        public int InsertMemberRegister(Za300Dto za300Dto, string imgtext)
        {
            string SQL = "INSERT INTO t_member (member_id, mail, authority_cd, password, " +
                         "member_fistname, member_fistname_kana, member_fistname_eng, " +
                         "member_lastname, member_lastname_kana, member_lastname_eng, entry_date, " +
                         "com_cd, dep1_cd, dep2_cd, pos_cd, gender, cellphone, zipcode, " +
                         "address, stamp_cd, stamp_image, stamp_size," +
                         "reg_id, reg_date, del_cd)" +
                         "VALUES('"
                         + za300Dto.txtId + "','"
                         + za300Dto.txtMail + "','"
                         + za300Dto.txtAuthorityCode + "','"
                         + za300Dto.txtPwd + "','"
                         + za300Dto.txtKanjiFirstName + "','"
                         + za300Dto.txtKanaFirstName + "','"
                         + za300Dto.txtEngFirstName + "','"
                         + za300Dto.txtKanjiLastName + "','"
                         + za300Dto.txtKanaLastName + "','"
                         + za300Dto.txtEngLastName + "','"
                         + za300Dto.txtEnterDate + "','"
                         + za300Dto.txtCompanyCode + "','"
                         + za300Dto.txtDepartment1 + "','"
                         + za300Dto.txtDepartment2 + "','"
                         + za300Dto.txtPosition + "','"
                         + za300Dto.txtGender + "','"
                         + za300Dto.txtPhone + "','"
                         + za300Dto.txtZipcode + "','"
                         + za300Dto.txtAddress + "','"
                         + za300Dto.txtStampCode + "','"
                         + imgtext + "','"
                         + za300Dto.fileSize + "','"
                         + za300Dto.txtRegisterId + "','"
                         + za300Dto.txtRegisterDay + "','"
                         + za300Dto.txtDelCode + "' )";

            int insertResult = new int();

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                        insertResult = 0;

                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                        insertResult = 1;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                    insertResult = 1;

                }
                conn.Close();
            }

            return insertResult;

        }

        /// <summary>
        /// シリアル・ナンバーをSELECT！
        /// </summary>
        public String SelectSerialNumber()
        {
            string SQL = "SELECT LPAD((SELECT COUNT(member_id) + 1 as member_id_Cnt FROM t_member),7,0) as member_id_Cnt";

            String serialNumber = null;

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");

                            serialNumber = reader["member_id_Cnt"].ToString();

                        }
                        else
                        {
                            Console.WriteLine("Select fail");

                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return serialNumber;

        }

        /// <summary>
        /// 会社名をSELECT！
        /// </summary>
        public List<string> SelectcompanyNameZa300()
        {
            List<string> txtComCdList = new List<string>();
            string SQL = "SELECT com_cd, comName_eng FROM m_company;";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        while (reader.Read())
                        {
                            txtComCdList.Add(reader["com_cd"].ToString());
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return txtComCdList;

        }

        /// <summary>
        /// 所属1情報をSELECT！
        /// </summary>
        public Za300Dto SelectDepartment1ListZa300()
        {
            Za300Dto za300Dto = new Za300Dto();
            za300Dto.txtDep1CdList = new List<string>();
            za300Dto.txtDep1NameList = new List<string>();

            string SQL = "SELECT dep1_cd, dep1_name FROM m_department1;";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            za300Dto.txtDep1CdList.Add(reader["dep1_cd"].ToString());
                            za300Dto.txtDep1NameList.Add(reader["dep1_name"].ToString());
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return za300Dto;

        }

        /// <summary>
        /// 所属2情報をSELECT！
        /// </summary>
        public Za300Dto SelectDepartment2ListZa300()
        {
            Za300Dto za300Dto = new Za300Dto();
            za300Dto.txtDep2CdList = new List<string>();
            za300Dto.txtDep2CdNameList = new List<string>();

            string SQL = "SELECT dep2_cd, dep2_name FROM m_department2";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        while (reader.Read())
                        {
                            za300Dto.txtDep2CdList.Add(reader["dep2_cd"].ToString());
                            za300Dto.txtDep2CdNameList.Add(reader["dep2_name"].ToString());
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return za300Dto;

        }

        /// <summary>
        /// 職位情報をSELECT！
        /// </summary>
        public Za300Dto SelectCompanyPositionListZa300()
        {
            Za300Dto za300Dto = new Za300Dto();
            za300Dto.txtComPosCdList = new List<string>();
            za300Dto.txtComPosCdNameList = new List<string>();

            string SQL = "SELECT pos_cd, pos_name FROM m_position";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        while (reader.Read())
                        {
                            za300Dto.txtComPosCdList.Add(reader["pos_cd"].ToString());
                            za300Dto.txtComPosCdNameList.Add(reader["pos_name"].ToString());
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return za300Dto;

        }
        /// <summary>
        /// personテーブルをSELECT！
        /// </summary>
        public String SelectEmailZa300(String email)
        {
            string emailCheck = "";

            string SQL = "SELECT mail from t_member WHERE mail = '" + email + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        emailCheck = reader["mail"].ToString();
                    }
                }
                conn.Close();
                return emailCheck;
            }
        }


        /// <summary>
        /// personテーブルをSELECT！
        /// </summary>
        public Za400Dto GetUserInfoByMailZa400(String email)
        {
            Za400Dto za400 = null;
            string SQL = "SELECT member_id, mail, member_fistname, member_fistname_kana, member_fistname_eng, member_lastname, member_lastname_kana,"
                + "member_lastname_eng, entry_date, com_cd, dep1_cd, dep2_cd, pos_cd, gender, cellphone, zipcode, address, stamp_cd, CONVERT(stamp_image USING utf8) as Stamp_image "
                + "FROM t_member WHERE mail = '" + email + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        za400 = new Za400Dto();
                        za400.member_id = reader["member_id"].ToString();
                        za400.txtMail = reader["mail"].ToString();
                        za400.txtPwd = "";
                        za400.txtPwdConfirm = "";
                        za400.txtKanaFirstName = reader["member_fistname_kana"].ToString();
                        za400.txtKanaLastName = reader["member_lastname_kana"].ToString();
                        za400.txtKanjiFirstName = reader["member_fistname"].ToString();
                        za400.txtKanjiLastName = reader["member_lastname"].ToString();
                        za400.txtEngFirstName = reader["member_fistname_eng"].ToString();
                        za400.txtEngLastName = reader["member_lastname_eng"].ToString();
                        za400.txtGender = reader["gender"].ToString();
                        za400.txtPhone = reader["cellphone"].ToString();
                        za400.txtZipcode = reader["zipcode"].ToString();
                        za400.txtAddress = reader["address"].ToString();
                        za400.txtCompanyCode = reader["com_cd"].ToString();
                        za400.txtDepartment1 = reader["dep1_cd"].ToString();
                        za400.txtDepartment2 = reader["dep2_cd"].ToString();
                        za400.txtPosition = reader["pos_cd"].ToString();
                        za400.txtEnterDate = reader["entry_date"].ToString();
                        za400.txtStampCode = reader["stamp_cd"].ToString();
                        za400.txtimgStamp = reader["Stamp_image"].ToString();
                    }
                }
                conn.Close();
                return za400;
            }

        }

        /// <summary>
        /// Za600_会員情報照会SQL
        /// </summary>
        /// <returns></returns>
        public Za600Dto GetUserInfoByMailZa600(String email)
        {
            Za600Dto za600 = null;
            string SQL = "SELECT mail, member_fistname, member_fistname_kana, member_fistname_eng, member_lastname, member_lastname_kana,"
                + "member_lastname_eng, date_format(entry_date, '%Y-%m-%d') as entry_date, com_cd, dep1_cd, dep2_cd, pos_cd, gender, cellphone, zipcode, address, stamp_cd, CONVERT(stamp_image USING utf8) as stamp_image "
                + "FROM t_member WHERE mail = '" + email + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        za600 = new Za600Dto();
                        za600.labelMail = reader["mail"].ToString();
                        za600.labelKanjiName = reader["member_fistname"].ToString() + " " + reader["member_lastname"].ToString();
                        za600.labelKanaName = reader["member_fistname_kana"].ToString() + " " + reader["member_lastname_kana"].ToString();
                        za600.labelEngName = reader["member_fistname_eng"].ToString() + " " + reader["member_lastname_eng"].ToString();
                        za600.labelGender = reader["gender"].ToString();
                        za600.labelCom = reader["com_cd"].ToString();
                        za600.labelDep1 = reader["dep1_cd"].ToString();
                        za600.labelDep2 = reader["dep2_cd"].ToString();
                        za600.labelPos = reader["pos_cd"].ToString();
                        za600.labelPhone = reader["cellphone"].ToString();
                        za600.labelZip = reader["zipcode"].ToString();
                        za600.labelAdd = reader["address"].ToString();
                        za600.labelEntryDate = reader["entry_date"].ToString();
                        za600.labelStamp = reader["stamp_cd"].ToString();
                        za600.imgStamp = reader["stamp_image"].ToString();
                    }
                }
                conn.Close();
                return za600;
            }

        }

        /// <summary>
        /// Zb100_ユーザー情報管理一覧SQL
        /// </summary>
        /// <returns></returns>
        public List<Zb100Dto> GetUserInfoByMailZb100(String com)
        {

            List<Zb100Dto> zb100 = new List<Zb100Dto>();
            string SQL = "SELECT tm.member_id, tm.mail, tm.authority_cd, tm.member_fistname, tm.member_lastname, tm.com_cd, md1.dep1_name, md2.dep2_name, tm.pos_cd, date_format(tm.entry_date, '%Y-%m-%d') as entry_date "
                + "FROM t_member tm, m_department1 md1, m_department2 md2 WHERE tm.del_cd = '0' AND tm.dep1_cd = md1.dep1_cd AND tm.dep2_cd = md2.dep2_cd AND tm.com_cd = '" + com + "' "
                + "ORDER BY tm.entry_date";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        zb100.Add(new Zb100Dto()
                        {
                            labelId = reader["member_id"].ToString(),
                            labelMail = reader["mail"].ToString(),
                            labelAuth = reader["authority_cd"].ToString(),
                            labelName = reader["member_fistname"].ToString() + "　" + reader["member_lastname"].ToString(),
                            labelCom = reader["com_cd"].ToString(),
                            labelDep1 = reader["dep1_name"].ToString(),
                            labelDep2 = reader["dep2_name"].ToString(),
                            labelPos = reader["pos_cd"].ToString(),
                            labelEntryDate = reader["entry_date"].ToString(),
                        });
                    }
                }
                conn.Close();
                return zb100;
            }

        }

        /// <summary>
        /// personテーブルにINSERT！
        /// </summary>
        public void UpdateUserInfoByMailZa400(Za400Dto za400Dto, string text)
        {

            string SQL = "UPDATE t_member SET password = '" + za400Dto.txtPwd
                + "', member_fistname_kana = '" + za400Dto.txtKanaFirstName
                + "', member_lastname_kana = '" + za400Dto.txtKanaLastName
                + "', member_fistname = '" + za400Dto.txtKanjiFirstName
                + "', member_lastname = '" + za400Dto.txtKanjiLastName
                + "', member_fistname_eng = '" + za400Dto.txtEngFirstName
                + "', member_lastname_eng = '" + za400Dto.txtEngLastName
                + "', gender = '" + za400Dto.txtGender
                + "', cellphone = '" + za400Dto.txtPhone
                + "', zipcode = '" + za400Dto.txtZipcode
                + "', address = '" + za400Dto.txtAddress
                + "', dep1_cd = '" + za400Dto.txtDepartment1
                + "', dep2_cd = '" + za400Dto.txtDepartment2
                + "', pos_cd = '" + za400Dto.txtPosition
                + "', entry_date = '" + za400Dto.txtUpdateDate
                + "', stamp_cd = '" + za400Dto.txtStampCode
                + "', stamp_image = '" + text
                + "', upd_id = '" + za400Dto.txtUpdateid
                + "', upd_date = '" + za400Dto.txtUpdateDate
                + "' WHERE mail = '" + za400Dto.txtMail + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Update success");
                    }
                    else
                    {
                        Console.WriteLine("Update fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        /// <summary>
        /// personテーブルにINSERT！
        /// </summary>
        public void UpdateUserInfoByMailZb300(Zb300Dto zb300Dto)
        {

            string SQL = "UPDATE t_member SET member_fistname_kana = '" + zb300Dto.txtKanaFirstName
                + "', member_lastname_kana = '" + zb300Dto.txtKanaLastName
                + "', member_fistname = '" + zb300Dto.txtKanjiFirstName
                + "', member_lastname = '" + zb300Dto.txtKanjiLastName
                + "', member_fistname_eng = '" + zb300Dto.txtEngFirstName
                + "', member_lastname_eng = '" + zb300Dto.txtEngLastName
                + "', gender = '" + zb300Dto.txtGender
                + "', cellphone = '" + zb300Dto.txtPhone
                + "', zipcode = '" + zb300Dto.txtZipcode
                + "', address = '" + zb300Dto.txtAddress
                + "', dep1_cd = '" + zb300Dto.txtDepartment1
                + "', dep2_cd = '" + zb300Dto.txtDepartment2
                + "', pos_cd = '" + zb300Dto.txtPosition
                + "', entry_date = '" + zb300Dto.txtUpdateDate
                + "', upd_id = '" + zb300Dto.txtUpdateid
                + "', upd_date = '" + zb300Dto.txtUpdateDate
                + "' WHERE mail = '" + zb300Dto.txtMail + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Update success");
                    }
                    else
                    {
                        Console.WriteLine("Update fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        /// <summary>
        /// personテーブルにINSERT！
        /// </summary>
        public void InsertData(Data user)
        {
            string SQL = "INSERT INTO Person(id,Name,Age,Phone,Email) VALUES(" + user.id + ",'" + user.Name + "'," + user.Age + ",'" + user.Phone + "','" + user.Email + "')";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        /// <summary>
        /// セッション情報SELECT！
        /// </summary>
        public LoginInfoDto SelectLoginZa200(string txtMail, String txtPwd)
        {
            string SQL = "SELECT member_id, mail, authority_cd, com_cd FROM t_member Where mail = '" + txtMail + "'And password ='" + txtPwd + "' And del_cd = '0'";

            LoginInfoDto loginInfoDto = new LoginInfoDto();

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");


                            loginInfoDto.txtId = reader["member_id"].ToString();
                            loginInfoDto.txtMail = reader["mail"].ToString();
                            loginInfoDto.txtAuthorityCd = reader["authority_cd"].ToString();
                            loginInfoDto.txtCompanyCd = reader["com_cd"].ToString();

                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                            loginInfoDto = null;

                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                    loginInfoDto = null;

                }
                conn.Close();
            }

            return loginInfoDto;

        }

        /// <summary>
        /// select password
        /// </summary>
        public String SelectPassword(string email)
        {
            string password = null;
            string SQL = "SELECT password FROM t_member Where del_cd = '0' and mail = '" + email + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            password = reader["password"].ToString();

                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return password;
        }


        /// <summary>
        /// t_member テーブルにupdate！
        /// </summary>
        public void UpdateUserInfoByMail(string email, string userId)
        {

            string SQL = "UPDATE t_member SET authority_cd = '0', member_fistname = '******', member_fistname_kana = '******', member_fistname_eng = '******', member_lastname = '******', member_lastname_kana = '******', member_lastname_eng = '******', cellphone = '******', zipcode = '******', address = '******', upd_id = '" + userId + "', upd_date = sysdate(), del_cd = '1' WHERE del_cd = '0' and mail = '" + email + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("update success");
                    }
                    else
                    {
                        Console.WriteLine("update fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        /// <summary>
        /// 画面コードテーブルから画面名取得
        /// </summary>
        public String SelectDisplayName(string displayId)
        {
            String displayName = null;
            string SQL = "SELECT display_name FROM m_display Where display_id = '" + displayId + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            displayName = reader["display_name"].ToString();

                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return displayName;
        }


        //<summary>
        //決裁管理＿全体画面データ取得
        //</summary>
        public List<Ka100Dto> approvalReportSrch()
        {
            // SQL文作成
            string SQL = "SELECT ta.report_id, ta.templete_id, ta.status, ta.approval_mail1, ta.approval_mail2, ta.approval_mail3, ta.approval_mail4, ta.approval_mail5, ta.approval_mail6, ta.approval_mail7, ta.approval_mail8, ta.approval_mail9, ta.approval_mail10, ta.reject_mail, ta.approval_date_plan, tr.approval_mail1, tr.approval_mail2, tr.approval_mail3, tr.approval_mail4, tr.approval_mail5, tr.approval_mail6, tr.approval_mail7, tr.approval_mail8, tr.approval_mail9, tr.approval_mail10, tr.approval_mail_last " +
                "FROM t_approval ta left join t_report tr " +
                "on ta.report_id = tr.report_id and ta.templete_id = tr.templete_id " +
                ", (select max(approval_date_plan) recen_approval_date_plan from t_approval) recen_date " +
                "where ta.status = '99' and substr(ta.approval_date_plan, 1, 6) = substr(recen_date.recen_approval_date_plan, 1, 6);";

            // DB接続
            MySqlConnection conn = GetConnection();
            conn.Open();

            // SQL実行
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            using (var reader = cmd.ExecuteReader())
            { 
                List<Ka100Dto> ka100 = new List<Ka100Dto>();
                while (reader.Read())
                {
                    ka100.Add(new Ka100Dto()
                    {
                        approvedNum = reader["report_id"].ToString()
                    });
                }
                // 結果読み
                conn.Close();
                return ka100;
            }


        }

        //<summary>
        //決裁管理一覧データを取得
        //</summary>
        public List<Ka200Dto> approvalSrch(string usrSsseionId, string sqlReportList, string start, string end, bool kensakuSts, string zyokenSQL)
        {
            List<Ka200Dto> ka200 = new List<Ka200Dto>();
            string SQL = "SELECT tr.report_id, tr.templete_id, tr.report_name, ta.status, tm.com_cd, tm.mail," +
                " concat (tm.member_fistname , tm.member_lastname) as name, tr.file_name, tr.file_binary ," +
                " tr.report_file_size, date_format(ta.approval_date_plan, '%Y-%m-%d') as approval_date_plan," +
                " date_format(tr.upd_date, '%Y-%m-%d') as upd_date, tr.approval_mail1, tr.approval_mail2, tr.approval_mail3," +
                " tr.approval_mail4, tr.approval_mail5, tr.approval_mail6 , tr.approval_mail7, tr.approval_mail8," +
                " tr.approval_mail9, tr.approval_mail10, tr.approval_mail_last" +
                " FROM t_member tm," +
                " t_report tr inner join t_approval ta" +
                " on  ta.report_id = tr.report_id" +
                " and ta.templete_id = tr.templete_id" +
                " WHERE ta.reg_id = tm.member_id" +
                " and ta.report_id in ('" + sqlReportList + "') " +
                " and tr.del_cd = '0'" +
                " and ta.del_cd = '0'" +
                " and tm.del_cd = '0'" +
                " and CONCAT(approval_date_plan, '000000') between " + start + " and " + end +
                " and tm.com_cd = (select com_cd from t_member where mail = '" + usrSsseionId + "') ";

            StringBuilder sb = new StringBuilder();
            sb.Append(SQL);
            if (kensakuSts) 
            {
                sb.Append(zyokenSQL);
            }

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ka200.Add(new Ka200Dto()
                        {
                            reportId = reader["report_id"].ToString(),
                            templeteId = reader["templete_id"].ToString(),
                            txtAtTblNm = reader["report_name"].ToString(),
                            txtApvlSts = reader["status"].ToString(),
                            drpbxSrchComCd = reader["com_cd"].ToString(),
                            txtWrtMail = reader["mail"].ToString(),
                            txtWrtrNm = reader["name"].ToString(),
                            txtApvlDtPlan = reader["approval_date_plan"].ToString(),
                            dtChgeDay = reader["upd_date"].ToString(),
                            approvalMail1 = reader["approval_mail1"].ToString(),
                            approvalMail2 = reader["approval_mail2"].ToString(),
                            approvalMail3 = reader["approval_mail3"].ToString(),
                            approvalMail4 = reader["approval_mail4"].ToString(),
                            approvalMail5 = reader["approval_mail5"].ToString(),
                            approvalMail6 = reader["approval_mail6"].ToString(),
                            approvalMail7 = reader["approval_mail7"].ToString(),
                            approvalMail8 = reader["approval_mail8"].ToString(),
                            approvalMail9 = reader["approval_mail9"].ToString(),
                            approvalMail10 = reader["approval_mail10"].ToString(),
                            approvalMailLast = reader["approval_mail_last"].ToString(),
                            file_name = reader["file_name"].ToString(),
                            file_binary = System.Text.Encoding.Default.GetString((byte[])reader["file_binary"]).ToString(),
                            report_file_size = reader["report_file_size"].ToString()
                        });
                    }
                }
                conn.Close();
                return ka200;
            }
        }

        // <summary>
        //帳票リストリストを取得
        // </summary>
        public List<string> SelectReportList(string usrSsseionId, string sqlMailList)
        {
            List<string> reportID = new List<string>();
            string result = null;
            string SQL = "SELECT report_id FROM t_report" +
                " WHERE approval_mail1 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail2 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail3 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail4 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail5 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail6 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail7 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail8 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail9 in ('" + usrSsseionId + "', '" + sqlMailList + "')" +
                " or approval_mail10 in ('" + usrSsseionId + "', '" + sqlMailList + "') ";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader["report_id"].ToString();
                        reportID.Add(result);
                    }
                }
                conn.Close();
            }

                return reportID;
        }

        // <summary>
        //帳票リストの全体件数を取得
        // </summary>
        public (string ksmati, string kskyaka, string kskanryo) SelectKensu(string sqlReportList)
        {
            string ksmati = null;
            string kskyaka = null;
            string kskanryo = null;

            string SQL = "SELECT " +
                         "COUNT(CASE WHEN ta.status IN ('10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '90') THEN 1 END) AS ksmati " +
                         ", COUNT(CASE WHEN ta.status = '98' THEN 1 END) AS kskyaka " +
                         ", COUNT(CASE WHEN ta.status = '99' THEN 1 END) AS kskanryo " +
                         "FROM t_approval ta INNER JOIN t_report tr ON ta.report_id = tr.report_id " +
                         "WHERE ta.report_id IN ('" + sqlReportList + "')";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ksmati = reader["ksmati"].ToString();
                        kskyaka = reader["kskyaka"].ToString();
                        kskanryo = reader["kskanryo"].ToString();

                    }
                }
                conn.Close();
            }

            return (ksmati, kskyaka, kskanryo);
        }

        // <summary>
        //最新帳票の決裁日を取得
        // </summary>
        public string SelectNengatuList(string reportList) 
        {
            string nengatu = null;

            string SQL = "SELECT MAX(ta.approval_date_plan) as approval_date_plan " +
                "FROM t_report tr INNER JOIN t_approval ta ON ta.report_id = tr.report_id AND ta.templete_id = tr.templete_id " +
                "WHERE ta.report_id in ('" + reportList  + "') ";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nengatu = reader["approval_date_plan"].ToString();
                        
                    }
                }
                conn.Close();
            }

            return nengatu;
        }

        /// <summary>
        /// 自分が所属した押印グループ取得
        /// </summary>
        public List<String> SelectMailList(string usrSsseionId)
        {
            List<String> mailList = new List<String>();
            String ouinMail = null;
            string SQL = "SELECT mailList_mail FROM t_mailList Where mail1 = '" + usrSsseionId + "' or mail2 = '" + usrSsseionId +
                "' or mail3 = '" + usrSsseionId + "' or mail4 = '" + usrSsseionId + "' or mail5 = '" + usrSsseionId +
                "' or mail6 = '" + usrSsseionId + "' or mail7 = '" + usrSsseionId + "' or mail8 = '" + usrSsseionId +
                "' or mail9 = '" + usrSsseionId + "' or mail10 = '" + usrSsseionId + "'";
            Console.WriteLine(SQL);

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        while (reader.Read())
                        {

                            Console.WriteLine("Select success");
                            ouinMail = reader["mailList_mail"].ToString();
                            mailList.Add(ouinMail);
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return mailList;
        }

        /// <summary>
        /// 初期表示に必要なデータを取得
        /// </summary>
        /// <param name="templeteid">決裁管理一覧から取得したテンプレートID</param>
        /// <param name="reportid">決裁管理一覧から取得したレポートID</param>
        /// <param name="btn">決裁管理一覧から取得したボタン表示フラグ</param>
        /// <returns></returns>
        public Ka500Dto GetInfoBydataKa500(String templeteid, String reportid, Boolean btn)
        {
            Ka500Dto ka500 = null;
            string SQL = "select tp.templete_name, tp.templete_detail, tp.approval_place1, tp.approval_place2, tp.approval_place3, tp.approval_place4, tp.approval_place5, tp.approval_place6, tp.approval_place7, tp.approval_place8, tp.approval_place9, tp.approval_place10, tp.approval_place_last, tp.approval_status1, tp.approval_status2, tp.approval_status3, tp.approval_status4, tp.approval_status5, tp.approval_status6, tp.approval_status7, tp.approval_status8, tp.approval_status9, tp.approval_status10, tp.approval_status_last, rp.report_id, rp.templete_id, rp.report_name, rp.report_detail, rp.file_name, rp.file_binary, rp.reg_id, rp.reg_date, rp.upd_id, rp.upd_date, rp.approval_mail1, rp.approval_mail2, rp.approval_mail3, rp.approval_mail4, rp.approval_mail5, rp.approval_mail6, rp.approval_mail7, rp.approval_mail8, rp.approval_mail9, rp.approval_mail10, rp.approval_mail_last, ap.reject_mail, ap.reject_detail, ap.maker_mail, ap.approval_date1, ap.approval_date2, ap.approval_date3, ap.approval_date4, ap.approval_date5, ap.approval_date6, ap.approval_date7, ap.approval_date8, ap.approval_date9, ap.approval_date10, ap.approval_date_last, ap.maker_stamp, ap.approval_stamp1, ap.approval_stamp2, ap.approval_stamp3, ap.approval_stamp4, ap.approval_stamp5, ap.approval_stamp6, ap.approval_stamp7, ap.approval_stamp8, ap.approval_stamp9, ap.approval_stamp10, ap.approval_stamp_last, ap.approval_date_plan, ap.approval_date1, ap.approval_date1, ap.approval_date2, ap.approval_date3, ap.approval_date4, ap.approval_date5, ap.approval_date6, ap.approval_date7, ap.approval_date8, ap.approval_date9, ap.approval_date10, ap.approval_date_last, ap.status, mb.member_fistname, mb.member_lastname, mb.pos_cd, mb.dep1_cd, mb.dep2_cd, mb.com_cd"
                           + " from t_templete AS tp"
                           + " JOIN"
                           + " t_report AS rp"
                           + " ON tp.templete_id = rp.templete_id"
                           + " JOIN "
                           + " t_approval AS ap"
                           + " ON ap.templete_id = rp.templete_id"
                           + " JOIN"
                           + " t_member AS mb"
                           + " ON rp.reg_id = mb.member_id"
                           + " WHERE tp.templete_id = '" + templeteid + "' AND rp.report_id = '" + reportid + "' AND rp.del_cd = '0'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ka500 = new Ka500Dto();
                        DateTime dt = new DateTime();
                        ka500.LB_file_nm = reader["file_name"].ToString();
                        ka500.LB_templete_nm = reader["templete_name"].ToString();
                        ka500.LB_templete_detail = reader["templete_detail"].ToString();
                        ka500.LB_com_cd_reg = reader["com_cd"].ToString();
                        ka500.LB_dep_cd_reg1 = reader["dep1_cd"].ToString();
                        ka500.LB_dep_cd_reg2 = reader["dep2_cd"].ToString();
                        ka500.LB_pos_cd_reg = reader["pos_cd"].ToString();
                        DateTime.TryParse(reader["reg_date"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_reg_date_reg = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_reg_date_reg == "0001-01-01 00:00:00")
                        {
                            ka500.LB_reg_date_reg = "";
                        }
                        ka500.LB_reg_id = reader["member_fistname"].ToString() + '　' + reader["member_lastname"].ToString();
                        ka500.LB_member1 = reader["approval_status1"].ToString();
                        ka500.LB_approval_mail1 = reader["approval_mail1"].ToString();
                        ka500.LB_approval_place1 = reader["approval_place1"].ToString();
                        DateTime.TryParse(reader["approval_date1"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date1 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date1 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date1 = "";
                        }
                        ka500.LB_member2 = reader["approval_status2"].ToString();
                        ka500.LB_approval_mail2 = reader["approval_mail2"].ToString();
                        ka500.LB_approval_place2 = reader["approval_place2"].ToString();
                        DateTime.TryParse(reader["approval_date2"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date2 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date2 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date2 = "";
                        }
                        ka500.LB_member3 = reader["approval_status3"].ToString();
                        ka500.LB_approval_mail3 = reader["approval_mail3"].ToString();
                        ka500.LB_approval_place3 = reader["approval_place3"].ToString();
                        DateTime.TryParse(reader["approval_date3"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date3 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date3 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date3 = "";
                        }
                        ka500.LB_member4 = reader["approval_status4"].ToString();
                        ka500.LB_approval_mail4 = reader["approval_mail4"].ToString();
                        ka500.LB_approval_place4 = reader["approval_place4"].ToString();
                        DateTime.TryParse(reader["approval_date4"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date4 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date4 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date4 = "";
                        }
                        ka500.LB_member5 = reader["approval_status5"].ToString();
                        ka500.LB_approval_mail5 = reader["approval_mail5"].ToString();
                        ka500.LB_approval_place5 = reader["approval_place5"].ToString();
                        DateTime.TryParse(reader["approval_date5"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date5 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date5 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date5 = "";
                        }
                        ka500.LB_member6 = reader["approval_status6"].ToString();
                        ka500.LB_approval_mail6 = reader["approval_mail6"].ToString();
                        ka500.LB_approval_place6 = reader["approval_place6"].ToString();
                        DateTime.TryParse(reader["approval_date6"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date6 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date6 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date6 = "";
                        }
                        ka500.LB_member7 = reader["approval_status7"].ToString();
                        ka500.LB_approval_mail7 = reader["approval_mail7"].ToString();
                        ka500.LB_approval_place7 = reader["approval_place7"].ToString();
                        DateTime.TryParse(reader["approval_date7"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date7 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date7 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date7 = "";
                        }
                        ka500.LB_member8 = reader["approval_status8"].ToString();
                        ka500.LB_approval_mail8 = reader["approval_mail8"].ToString();
                        ka500.LB_approval_place8 = reader["approval_place8"].ToString();
                        DateTime.TryParse(reader["approval_date8"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date8 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date8 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date8 = "";
                        }
                        ka500.LB_member9 = reader["approval_status9"].ToString();
                        ka500.LB_approval_mail9 = reader["approval_mail9"].ToString();
                        ka500.LB_approval_place9 = reader["approval_place9"].ToString();
                        DateTime.TryParse(reader["approval_date9"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date9 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date9 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date9 = "";
                        }
                        ka500.LB_member10 = reader["approval_status10"].ToString();
                        ka500.LB_approval_mail10 = reader["approval_mail10"].ToString();
                        ka500.LB_approval_place10 = reader["approval_place10"].ToString();
                        DateTime.TryParse(reader["approval_date10"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date10 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date10 == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date10 = "";
                        }
                        ka500.LB_member_last = reader["approval_status_last"].ToString();
                        ka500.LB_approval_mail_last = reader["approval_mail_last"].ToString();
                        ka500.LB_approval_place_last = reader["approval_place_last"].ToString();
                        DateTime.TryParse(reader["approval_date_last"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_approval_date_last = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_approval_date_last == "0001-01-01 00:00:00")
                        {
                            ka500.LB_approval_date_last = "";
                        }
                        ka500.TXT_reject_detail = reader["reject_detail"].ToString();
                        ka500.LB_report_nm = reader["report_name"].ToString();
                        ka500.LB_report_detail = reader["report_detail"].ToString();
                        ka500.LB_approval_date_plan = DateTime.ParseExact(reader["approval_date_plan"].ToString(), "yyyyMMdd", null).ToString("yyyy-MM-dd");
                        ka500.LB_status = reader["status"].ToString();
                        if (!btn)
                        {
                            //ka500.TXT_act = "none";
                            ka500.BTN_act = "none";
                        }
                        ka500.LB_reject_mail = reader["reject_mail"].ToString();
                        ka500.LB_upd_id = reader["upd_id"].ToString();
                        ka500.report_id = reader["report_id"].ToString();
                        ka500.templete_id = reader["templete_id"].ToString();
                    }
                }
                conn.Close();
            }


            string SQL2 = "select file_name from t_templete where templete_id = '" + templeteid + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL2, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ka500.LB_templete_file_nm = reader["file_name"].ToString();
                    }
                }
                conn.Close();
            }

            string SQL3 = "select mb.member_fistname, mb.member_lastname, mb.com_cd, mb.dep1_cd, mb.dep2_cd, mb.pos_cd, rp.upd_date"
                          + " from t_member AS mb"
                          + " JOIN"
                          + " t_report AS rp"
                          + " ON rp.report_id = '" + reportid + "'"
                          + " where rp.upd_id = mb.member_id";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL3, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime dt = new DateTime();
                        ka500.LB_upd_id = reader["member_fistname"].ToString() + '　' + reader["member_lastname"].ToString();
                        ka500.LB_com_cd_upd = reader["com_cd"].ToString();
                        ka500.LB_dep_cd_upd1 = reader["dep1_cd"].ToString();
                        ka500.LB_dep_cd_upd2 = reader["dep2_cd"].ToString();
                        ka500.LB_pos_cd_upd = reader["pos_cd"].ToString();
                        DateTime.TryParse(reader["upd_date"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_upd_date = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_upd_date == "0001-01-01 00:00:00")
                        {
                            ka500.LB_upd_date = "";
                        }
                    }
                }
                conn.Close();
            }

            string SQL4 = "select mb.member_fistname, mb.member_lastname, ap.reject_date, ap.reject_detail"
                        + " from t_member AS mb"
                        + " JOIN"
                        + " t_approval AS ap"
                        + " ON ap.report_id = '" + reportid + "'"
                        + " where ap.reject_mail = mb.mail";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL4, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime dt = new DateTime();
                        ka500.LB_reject_mail = reader["member_fistname"].ToString() + reader["member_lastname"].ToString();
                        DateTime.TryParse(reader["reject_date"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ka500.LB_reject_date = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ka500.LB_reject_date == "0001-01-01 00:00:00")
                        {
                            ka500.LB_reject_date = "";
                        }
                        ka500.TXT_reject_detail = reader["reject_detail"].ToString();
                    }
                }
                conn.Close();
            }

            string SQL5 = "select dp1.dep1_name, dp2.dep2_name"
                        + " from m_department1 AS dp1"
                        + " JOIN"
                        + " m_department2 AS dp2"
                        + " ON dp1.dep1_cd = '" + ka500.LB_dep_cd_reg1 + "' AND dp2.dep2_cd = '" + ka500.LB_dep_cd_reg2 + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL5, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ka500.LB_dep_cd_reg1 = reader["dep1_name"].ToString();
                        ka500.LB_dep_cd_reg2 = reader["dep2_name"].ToString();
                    }
                }
                conn.Close();
            }

            string SQL6 = "select dp1.dep1_name, dp2.dep2_name"
                        + " from m_department1 AS dp1"
                        + " JOIN"
                        + " m_department2 AS dp2"
                        + " ON dp1.dep1_cd = '" + ka500.LB_dep_cd_upd1 + "' AND dp2.dep2_cd = '" + ka500.LB_dep_cd_upd2 + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL6, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ka500.LB_dep_cd_upd1 = reader["dep1_name"].ToString();
                        ka500.LB_dep_cd_upd2 = reader["dep2_name"].ToString();
                    }
                }
                conn.Close();
            }

            string SQL7 = "select cd_name from m_cd where cd = '" + ka500.LB_status + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL7, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ka500.LB_status = reader["cd_name"].ToString();
                    }
                }
                conn.Close();
                return ka500;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templeteid"></param>
        /// <param name="reportid"></param>
        /// <returns></returns>
        public string GetApprovalMailKa500(String templeteid, String reportid)
        {
            string status = null;
            string SQL = "select status from t_approval where report_id = '" + reportid + "' AND templete_id = '" + templeteid + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        status = reader["status"].ToString();
                    }
                }
                conn.Close();
                return status;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="status"></param>
        /// <param name="reportid"></param>
        /// <param name="mail"></param>
        /// <param name="id"></param>
        /// <param name="date"></param>
        public void ImprintApprovalKa500(string detail, string status, string reportid, string mail, string id, string date)
        {
            string SQL = "update t_approval set reject_mail = '" + mail + "', reject_detail = '" + detail + "', reject_date = '" + date + "', upd_id = '" + id + "', upd_date = '" + date + "', status = '98'"
                       + " where report_id = '" + reportid + "' AND del_cd = '0'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="date"></param>
        /// <param name="num"></param>
        /// <param name="status"></param>
        /// <param name="reportid"></param>
        public void InputApprovalKa500(string mail, string date, string num, int status, string reportid)
        {
            string SQL = "update t_approval"
                       + " JOIN t_member on t_member.mail = '" + mail + "'"
                       + " set t_approval.upd_id = t_member.member_id, t_approval.upd_date = '" + date + "', t_approval.approval_mail" + num + " = t_member.mail, t_approval.approval_date" + num + " = '" + date + "', t_approval.approval_stamp" + num + " = t_member.stamp_image, t_approval.status = '" + status + "'"
                       + " where t_approval.report_id = '" + reportid + "' AND t_approval.del_cd = '0'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportid"></param>
        /// <returns></returns>
        public List<Ka500Dto> GetApprovalList(string reportid)
        {
            List<Ka500Dto> list = new List<Ka500Dto>();
            string SQL = "select approval_mail1, approval_mail2, approval_mail3, approval_mail4, approval_mail5," +
                         "approval_mail6, approval_mail7, approval_mail8, approval_mail9, approval_mail10, " +
                         "approval_mail_last from t_approval where t_approval.report_id = '" + reportid +
                         "' AND t_approval.del_cd = '0'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        list.Add(new Ka500Dto()
                        {
                            LB_approval_mail1 = reader["approval_mail1"].ToString(),
                            LB_approval_mail2 = reader["approval_mail2"].ToString(),
                            LB_approval_mail3 = reader["approval_mail3"].ToString(),
                            LB_approval_mail4 = reader["approval_mail4"].ToString(),
                            LB_approval_mail5 = reader["approval_mail5"].ToString(),
                            LB_approval_mail6 = reader["approval_mail6"].ToString(),
                            LB_approval_mail7 = reader["approval_mail7"].ToString(),
                            LB_approval_mail8 = reader["approval_mail8"].ToString(),
                            LB_approval_mail9 = reader["approval_mail9"].ToString(),
                            LB_approval_mail10 = reader["approval_mail10"].ToString(),
                            LB_approval_mail_last = reader["approval_mail_last"].ToString()

                        });
                    }
                }
                conn.Close();
                return list;
            }

        }

        /// <summary>
        /// Ca300_帳票一覧SQL
        /// </summary>
        /// <returns></returns>
        public List<Ca300Dto> SelectCa300(string userID, string todayYmd)
        {
            List<Ca300Dto> ca300Dto = new List<Ca300Dto>();
            var i = 1;
            string SQL = "SELECT A1.report_id, A1.templete_id, A1.file_name, A1.report_name, A1.bef_report_id, A1.reg_date," +
                         "A1.upd_date, A1.file_binary, A1.report_file_size, C1.cd_short_name FROM t_report AS A1 INNER JOIN t_approval AS B1 " +
                         "ON B1.report_id = A1.report_id AND B1.templete_id = A1.templete_id INNER JOIN m_cd " +
                         "AS C1 ON B1.status = C1.cd WHERE B1.approval_date_plan = '" + todayYmd + "' AND " +
                         "A1.del_cd = '0' AND B1.del_cd = '0' AND A1.reg_id = '" + userID + "' AND B1.reg_id = '"
                         + userID + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime regDt = new DateTime();
                            string strRegDt = "";
                            DateTime updDt = new DateTime();
                            string strUpdDt = "";

                            DateTime.TryParse(reader["reg_date"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out regDt);
                            if (regDt.ToString("yyyy-MM-dd") == "0001-01-01")
                            {
                                strRegDt = "";
                            }
                            else
                            {
                                strRegDt = regDt.ToString("yyyy-MM-dd");
                            }

                            DateTime.TryParse(reader["upd_date"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out updDt);
                            if (updDt.ToString("yyyy-MM-dd") == "0001-01-01")
                            {
                                strUpdDt = "";
                            }
                            else
                            {
                                strUpdDt = updDt.ToString("yyyy-MM-dd");
                            }

                            ca300Dto.Add(new Ca300Dto()
                            {

                                formNum = i,
                                reportID = reader["report_id"].ToString(),
                                fileName = reader["file_name"].ToString(),
                                templeteID = reader["templete_id"].ToString(),
                                reportName = reader["report_name"].ToString(),
                                befReportId = reader["bef_report_id"].ToString(),
                                regYmd = strRegDt,
                                updYmd = strUpdDt,
                                approveStatus = reader["cd_short_name"].ToString(),
                                binaryFile = System.Text.Encoding.Default.GetString((byte[])reader["file_binary"]),
                                fileSize = reader["report_file_size"].ToString()
                            });
                            i = i + 1;
                            Console.WriteLine("Select success");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }
            return ca300Dto;
        }

        /// <summary>
        /// Ca100_帳票作成：会員情報取得SQL
        /// </summary>
        /// <returns></returns>
        public Ca100Dto SelectCa100MemberDto(string userID)
        {
            Ca100Dto ca100Dto = new Ca100Dto();
            string SQL = "SELECT A1.com_cd, A1.member_fistname, A1.member_lastname, B1.dep1_name, C1.dep2_name," +
                         "D1.pos_name FROM t_member as A1 inner join m_department1 as B1 On A1.dep1_cd = B1.dep1_cd " +
                         "inner join m_department2 as C1 On A1.dep2_cd = C1.dep2_cd inner join m_position as D1 " +
                         "On A1.pos_cd = D1.pos_cd where A1.del_cd='0' AND A1.member_id='" + userID + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            ca100Dto.lbAuthorName = reader["member_fistname"].ToString() + reader["member_lastname"].ToString();
                            ca100Dto.lbComName = reader["com_cd"].ToString();
                            ca100Dto.lbDep1 = reader["dep1_name"].ToString();
                            ca100Dto.lbDep2 = reader["dep2_name"].ToString();
                            ca100Dto.lbPos = reader["pos_name"].ToString();
                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }
            return ca100Dto;
        }

        /// <summary>
        /// Ca100_帳票作成：テンプレート情報取得SQL
        /// </summary>
        /// <returns></returns>
        public List<Ca100SubDto> SelectCa100DtoList(string comName)
        {
            List<Ca100SubDto> ca100SelectoLists = new List<Ca100SubDto>();
            string SQL = "Select templete_id, templete_name from t_templete where  com_cd = '" + comName +
                         "' AND del_cd = '0'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ca100SelectoLists.Add(new Ca100SubDto()
                            {
                                templeteID = reader["templete_id"].ToString(),
                                templeteName = reader["templete_name"].ToString()
                            });
                            Console.WriteLine("Select success");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return ca100SelectoLists;
        }

        /// <summary>
        /// Ca100_帳票作成：押印情報取得SQL
        /// </summary>
        /// <returns></returns>
        public Ca100Dto SelectCa100DtoApprove(string userID, string templeteID)
        {
            Ca100Dto ca100Approve = SelectCa100MemberDto(userID);

            string SQL = "Select approval_place_start, approval_place1, approval_place2, approval_place3," +
                         "approval_place4, approval_place5, approval_place6, approval_place7, approval_place8," +
                         "approval_place9, approval_place10, approval_place_last, approval_mail1,approval_mail2," +
                         "approval_mail3, approval_mail4, approval_mail5, approval_mail6, approval_mail7, " +
                         "approval_mail8, approval_mail9, approval_mail10, approval_mail_last, approval_status1," +
                         "approval_status2, approval_status3, approval_status4, approval_status5, approval_status6," +
                         "approval_status7, approval_status8, approval_status9, approval_status10, " +
                         "approval_status_last from t_templete where  templete_id= '" + templeteID +
                         "' AND del_cd = '0'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            ca100Approve.lbAprvNum1 = "決裁1";
                            ca100Approve.lbApprover1 = reader["approval_status1"].ToString();
                            ca100Approve.lbAprvPlc1 = reader["approval_place1"].ToString();
                            ca100Approve.lbAprvMail1 = reader["approval_mail1"].ToString();
                            ca100Approve.lbAprvNum2 = "決裁2";
                            ca100Approve.lbApprover2 = reader["approval_status2"].ToString();
                            ca100Approve.lbAprvPlc2 = reader["approval_place2"].ToString();
                            ca100Approve.lbAprvMail2 = reader["approval_mail2"].ToString();
                            ca100Approve.lbAprvNum3 = "決裁3";
                            ca100Approve.lbApprover3 = reader["approval_status3"].ToString();
                            ca100Approve.lbAprvPlc3 = reader["approval_place3"].ToString();
                            ca100Approve.lbAprvMail3 = reader["approval_mail3"].ToString();
                            ca100Approve.lbAprvNum4 = "決裁4";
                            ca100Approve.lbApprover4 = reader["approval_status4"].ToString();
                            ca100Approve.lbAprvPlc4 = reader["approval_place4"].ToString();
                            ca100Approve.lbAprvMail4 = reader["approval_mail4"].ToString();
                            ca100Approve.lbAprvNum5 = "決裁5";
                            ca100Approve.lbApprover5 = reader["approval_status5"].ToString();
                            ca100Approve.lbAprvPlc5 = reader["approval_place5"].ToString();
                            ca100Approve.lbAprvMail5 = reader["approval_mail5"].ToString();
                            ca100Approve.lbAprvNum6 = "決裁6";
                            ca100Approve.lbApprover6 = reader["approval_status6"].ToString();
                            ca100Approve.lbAprvPlc6 = reader["approval_place6"].ToString();
                            ca100Approve.lbAprvMail6 = reader["approval_mail6"].ToString();
                            ca100Approve.lbAprvNum7 = "決裁7";
                            ca100Approve.lbApprover7 = reader["approval_status7"].ToString();
                            ca100Approve.lbAprvPlc7 = reader["approval_place7"].ToString();
                            ca100Approve.lbAprvMail7 = reader["approval_mail7"].ToString();
                            ca100Approve.lbAprvNum8 = "決裁8";
                            ca100Approve.lbApprover8 = reader["approval_status8"].ToString();
                            ca100Approve.lbAprvPlc8 = reader["approval_place8"].ToString();
                            ca100Approve.lbAprvMail8 = reader["approval_mail8"].ToString();
                            ca100Approve.lbAprvNum9 = "決裁9";
                            ca100Approve.lbApprover9 = reader["approval_status9"].ToString();
                            ca100Approve.lbAprvPlc9 = reader["approval_place9"].ToString();
                            ca100Approve.lbAprvMail9 = reader["approval_mail9"].ToString();
                            ca100Approve.lbAprvNum10 = "決裁10";
                            ca100Approve.lbApprover10 = reader["approval_status10"].ToString();
                            ca100Approve.lbAprvPlc10 = reader["approval_place10"].ToString();
                            ca100Approve.lbAprvMail10 = reader["approval_mail10"].ToString();
                            ca100Approve.fnAprvNum = "最終決裁者";
                            ca100Approve.fnApprover = reader["approval_status_last"].ToString();
                            ca100Approve.fnAprvPlc = reader["approval_place_last"].ToString();
                            ca100Approve.fnAprvMail = reader["approval_mail_last"].ToString();
                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return ca100Approve;
        }

        /// <summary>
        /// Ca100_帳票作成：作成者押印イメージ取得SQL
        /// </summary>
        /// <returns></returns>
        public Ca100Dto SelectCa100StartStampImg(Ca100Dto ca100Dto, string mail)
        {
            Ca100Dto ca100DtoStamp = ca100Dto;
            string SQL = "Select CONVERT(stamp_image USING utf8) as imgStamp1,stamp_cd,stamp_size " +
                         "from t_member where  mail= '" + mail + "' AND del_cd = '0'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            ca100DtoStamp.startStampImg = reader["imgStamp1"].ToString();
                            ca100DtoStamp.stampCd = reader["stamp_cd"].ToString();
                            ca100DtoStamp.stampSize = Convert.ToInt32(reader["stamp_size"]);
                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return ca100DtoStamp;
        }

        /// <summary>
        /// CA100_帳票作成INSERT
        /// </summary>
        public void InsertCa100Register(Ca100Dto ca100Dto)
        {
            string SQL = "INSERT INTO t_report (report_id, templete_id, report_name, report_detail, " +
                         "file_name, file_binary, report_file_size, approval_mail1, " +
                         "approval_mail2, approval_mail3, approval_mail4, approval_mail5, " +
                         "approval_mail6, approval_mail7, approval_mail8, approval_mail9, approval_mail10," +
                         "approval_mail_last, reg_ym, reg_id, reg_date, del_cd)" +
                         "VALUES('"
                         + ca100Dto.reportID + "','"
                         + ca100Dto.selTempleteID + "','"
                         + ca100Dto.txtReportName + "','"
                         + ca100Dto.txtReportDetail + "','"
                         + ca100Dto.reportFile + "','"
                         + ca100Dto.binaryFile + "','"
                         + ca100Dto.fileSize + "','"
                         + ca100Dto.txtAprvMail1 + "','"
                         + ca100Dto.txtAprvMail2 + "','"
                         + ca100Dto.txtAprvMail3 + "','"
                         + ca100Dto.txtAprvMail4 + "','"
                         + ca100Dto.txtAprvMail5 + "','"
                         + ca100Dto.txtAprvMail6 + "','"
                         + ca100Dto.txtAprvMail7 + "','"
                         + ca100Dto.txtAprvMail8 + "','"
                         + ca100Dto.txtAprvMail9 + "','"
                         + ca100Dto.txtAprvMail10 + "','"
                         + ca100Dto.txtFnAprvMail + "','"
                         + ca100Dto.regYm + "','"
                         + ca100Dto.regID + "','"
                         + ca100Dto.regDate + "','"
                         + ca100Dto.delCd + "')";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }
                conn.Close();
            }

            string SQL1 = "INSERT INTO  t_approval (report_id, templete_id, status, maker_mail, " +
                         "maker_stamp, maker_stamp_size, " +
                         "approval_date_plan, reg_id, reg_date, del_cd) VALUES('"
                         + ca100Dto.reportID + "','"
                         + ca100Dto.selTempleteID + "','"
                         + ca100Dto.status + "','"
                         + ca100Dto.startAprvMail + "','"
                         + ca100Dto.startStampImg + "','"
                         + ca100Dto.stampSize + "','"
                         + ca100Dto.txtPlanYmd + "','"
                         + ca100Dto.regID + "','"
                         + ca100Dto.regDate + "','"
                         + ca100Dto.delCd + "')";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL1, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");

                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }
                conn.Close();
            }
        }

        /// <summary>
        /// CA400画面初期表示
        /// </summary>
        /// <param name="templeteid"></param>
        /// <param name="reportid"></param>
        /// <returns></returns>
        public Ca400Dto GetInfoBydataCa400(string templeteid, string reportid)
        {
            Ca400Dto ca400 = null;
                  //テンプレートテーブルから決裁者、押印位置などを取得
            string SQL = "select templete_name, templete_detail, approval_place1, approval_place2, approval_place3, approval_place4, approval_place5, approval_place6, approval_place7, approval_place8, approval_place9, approval_place10, approval_place_last, approval_status1, approval_status2, approval_status3, approval_status4, approval_status5, approval_status6, approval_status7, approval_status8, approval_status9, approval_status10, approval_status_last"
                       + " from t_templete where templete_id = '" + templeteid + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ca400 = new Ca400Dto();
                        ca400.templete_name = reader["templete_name"].ToString();
                        ca400.templete_detail = reader["templete_detail"].ToString();
                        ca400.approval_place1 = reader["approval_place1"].ToString();
                        ca400.approval_place2 = reader["approval_place2"].ToString();
                        ca400.approval_place3 = reader["approval_place3"].ToString();
                        ca400.approval_place4 = reader["approval_place4"].ToString();
                        ca400.approval_place5 = reader["approval_place5"].ToString();
                        ca400.approval_place6 = reader["approval_place6"].ToString();
                        ca400.approval_place7 = reader["approval_place7"].ToString();
                        ca400.approval_place8 = reader["approval_place8"].ToString();
                        ca400.approval_place9 = reader["approval_place9"].ToString();
                        ca400.approval_place10 = reader["approval_place10"].ToString();
                        ca400.approval_place_last = reader["approval_place_last"].ToString();
                        ca400.approval_status1 = reader["approval_status1"].ToString();
                        ca400.approval_status2 = reader["approval_status2"].ToString();
                        ca400.approval_status3 = reader["approval_status3"].ToString();
                        ca400.approval_status4 = reader["approval_status4"].ToString();
                        ca400.approval_status5 = reader["approval_status5"].ToString();
                        ca400.approval_status6 = reader["approval_status6"].ToString();
                        ca400.approval_status7 = reader["approval_status7"].ToString();
                        ca400.approval_status8 = reader["approval_status8"].ToString();
                        ca400.approval_status9 = reader["approval_status9"].ToString();
                        ca400.approval_status10 = reader["approval_status10"].ToString();
                        ca400.approval_status_last = reader["approval_status_last"].ToString();

                    }
                }
                conn.Close();
            }

                  //レポートテーブルからデータを取得
            string SQL2 = "select report_id, templete_id, bef_report_id, file_name, reg_id, reg_date, upd_id, upd_date, report_name, report_detail, approval_mail1, approval_mail2, approval_mail3, approval_mail4, approval_mail5, approval_mail6, approval_mail7, approval_mail8, approval_mail9, approval_mail10, approval_mail_last"
                        + " from t_report where report_id = '" + reportid + "' AND templete_id = '" + templeteid + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL2, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime dt = new DateTime();
                        ca400.report_id = reader["report_id"].ToString();
                        ca400.templete_id = reader["templete_id"].ToString();
                        ca400.bef_report_id = reader["bef_report_id"].ToString();
                        if (ca400.bef_report_id == null || ca400.bef_report_id == "")
                        {
                            ca400.lnk_show = "none";
                        }
                        ca400.file_name = reader["file_name"].ToString();
                        DateTime.TryParse(reader["reg_date"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.reg_date = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.reg_date == "0001-01-01 00:00:00")
                        {
                            ca400.reg_date = "";
                        }
                        DateTime.TryParse(reader["upd_date"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.upd_date = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.upd_date == "0001-01-01 00:00:00")
                        {
                            ca400.upd_date = "";
                        }
                        ca400.report_name = reader["report_name"].ToString();
                        ca400.report_detail = reader["report_detail"].ToString();
                        ca400.approval_mail1 = reader["approval_mail1"].ToString();
                        ca400.approval_mail2 = reader["approval_mail2"].ToString();
                        ca400.approval_mail3 = reader["approval_mail3"].ToString();
                        ca400.approval_mail4 = reader["approval_mail4"].ToString();
                        ca400.approval_mail5 = reader["approval_mail5"].ToString();
                        ca400.approval_mail6 = reader["approval_mail6"].ToString();
                        ca400.approval_mail7 = reader["approval_mail7"].ToString();
                        ca400.approval_mail8 = reader["approval_mail8"].ToString();
                        ca400.approval_mail9 = reader["approval_mail9"].ToString();
                        ca400.approval_mail10 = reader["approval_mail10"].ToString();
                        ca400.approval_mail_last = reader["approval_mail_last"].ToString();
                        ca400.reg_id = reader["reg_id"].ToString();
                        ca400.upd_id = reader["upd_id"].ToString();
                    }
                }
                conn.Close();
            }

                  //決済テーブルからデータを取得
            string SQL3 = "select approval_date_plan, status, approval_date1, approval_date2, approval_date3, approval_date4, approval_date5, approval_date6, approval_date7, approval_date8, approval_date9, approval_date10, approval_date_last, reject_mail, reject_date, reject_detail"
                        + " from t_approval where report_id = '" + reportid + "' AND templete_id = '" + templeteid + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL3, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime dt = new DateTime();
                        ca400.approval_date_plan = reader["approval_date_plan"].ToString();
                        ca400.status = reader["status"].ToString();
                        if (reader["status"].ToString() == "10" || reader["status"].ToString() == "98")
                        {
                            ca400.upd_btn = "";
                        }
                        else
                        {
                            ca400.upd_btn = "none";
                        }
                        if (reader["status"].ToString() == "10")
                        {
                            ca400.del_btn = "";
                        }
                        else
                        {
                            ca400.del_btn = "none";
                        }
                        if (reader["status"].ToString() == "98")
                        {
                            ca400.btnFlg = true;
                        }
                        else
                        {
                            ca400.btnFlg = false;
                        }
                        DateTime.TryParse(reader["approval_date1"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date1 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date1 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date1 = "";
                        }
                        DateTime.TryParse(reader["approval_date2"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date2 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date2 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date2 = "";
                        }
                        DateTime.TryParse(reader["approval_date3"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date3 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date3 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date3 = "";
                        }
                        DateTime.TryParse(reader["approval_date4"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date4 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date4 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date4 = "";
                        }
                        DateTime.TryParse(reader["approval_date5"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date5 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date5 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date5 = "";
                        }
                        DateTime.TryParse(reader["approval_date6"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date6 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date6 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date6 = "";
                        }
                        DateTime.TryParse(reader["approval_date7"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date7 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date7 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date7 = "";
                        }
                        DateTime.TryParse(reader["approval_date8"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date8 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date8 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date8 = "";
                        }
                        DateTime.TryParse(reader["approval_date9"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date9 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date9 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date9 = "";
                        }
                        DateTime.TryParse(reader["approval_date10"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date10 = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date10 == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date10 = "";
                        }
                        DateTime.TryParse(reader["approval_date_last"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.approval_date_last = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.approval_date_last == "0001-01-01 00:00:00")
                        {
                            ca400.approval_date_last = "";
                        }
                        ca400.reject_mail = reader["reject_mail"].ToString();
                        ca400.reject_detail = reader["reject_detail"].ToString();
                        DateTime.TryParse(reader["reject_date"].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt);
                        ca400.reject_date = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        if (ca400.reject_date == "0001-01-01 00:00:00")
                        {
                            ca400.reject_date = "";
                        }
                    }
                }
                conn.Close();
            }

            string SQL4 = "select mb.member_fistname, mb.member_lastname, mb.com_cd, mb.dep1_cd, mb.dep2_cd, mb.pos_cd"
                        + " from t_member AS mb"
                        + " JOIN"
                        + " t_report AS rp"
                        + " ON mb.member_id = rp.reg_id"
                        +" where mb.member_id = '" + ca400.reg_id + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL4, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ca400.reg_member_fistname = reader["member_fistname"].ToString();
                        ca400.reg_member_lastname = reader["member_lastname"].ToString();
                        ca400.reg_com_cd = reader["com_cd"].ToString();
                        ca400.reg_dep1_cd = reader["dep1_cd"].ToString();
                        ca400.reg_dep2_cd = reader["dep2_cd"].ToString();
                        ca400.reg_pos_cd = reader["pos_cd"].ToString();
                    }
                }
                conn.Close();
            }

            string SQL5 = "select mb.member_fistname, mb.member_lastname, mb.com_cd, mb.dep1_cd, mb.dep2_cd, mb.pos_cd"
                       + " from t_member AS mb"
                       + " JOIN"
                       + " t_report AS rp"
                       + " ON mb.member_id = rp.upd_id"
                       + " where mb.member_id = '" + ca400.upd_id + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL5, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ca400.upd_member_fistname = reader["member_fistname"].ToString();
                        ca400.upd_member_lastname = reader["member_lastname"].ToString();
                        ca400.upd_com_cd = reader["com_cd"].ToString();
                        ca400.upd_dep1_cd = reader["dep1_cd"].ToString();
                        ca400.upd_dep2_cd = reader["dep2_cd"].ToString();
                        ca400.upd_pos_cd = reader["pos_cd"].ToString();
                    }
                }
                conn.Close();
            }

            string SQL6 = "select mb.member_fistname, mb.member_lastname, mb.com_cd, mb.dep1_cd, mb.dep2_cd, mb.pos_cd"
                       + " from t_member AS mb"
                       + " JOIN"
                       + " t_approval AS ap"
                       + " ON mb.mail = ap.reject_mail"
                       + " where mb.mail = '" + ca400.reject_mail + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL6, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ca400.reject_member_fistname = reader["member_fistname"].ToString();
                        ca400.reject_member_lastname = reader["member_lastname"].ToString();
                        ca400.reject_com_cd = reader["com_cd"].ToString();
                        ca400.reject_dep1_cd = reader["dep1_cd"].ToString();
                        ca400.reject_dep2_cd = reader["dep2_cd"].ToString();
                        ca400.reject_pos_cd = reader["pos_cd"].ToString();
                    }
                }
                conn.Close();
            }

            //
            string SQL7 = "select dp1.dep1_name, dp2.dep2_name, pos.pos_name"
                        + " from m_department1 AS dp1"
                        + " JOIN"
                        + " m_department2 AS dp2"
                        + " ON dp1.dep1_cd = '" + ca400.reg_dep1_cd + "' AND dp2.dep2_cd = '" + ca400.reg_dep2_cd + "'"
                        + " JOIN"
                        + " m_position AS pos"
                        + " ON pos.pos_cd = '" + ca400.reg_pos_cd + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL7, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ca400.reg_dep1_cd = reader["dep1_name"].ToString();
                        ca400.reg_dep2_cd = reader["dep2_name"].ToString();
                        ca400.reg_pos_cd = reader["pos_name"].ToString();
                    }
                }
                conn.Close();
            }

                  //所属値を取得
            string SQL8 = "select dp1.dep1_name, dp2.dep2_name, pos.pos_name"
                        + " from m_department1 AS dp1"
                        + " JOIN"
                        + " m_department2 AS dp2"
                        + " ON dp1.dep1_cd = '" + ca400.upd_dep1_cd + "' AND dp2.dep2_cd = '" + ca400.upd_dep2_cd + "'"
                        + " JOIN"
                        + " m_position AS pos"
                        + " ON pos.pos_cd = '" + ca400.upd_pos_cd + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL8, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ca400.upd_dep1_cd = reader["dep1_name"].ToString();
                        ca400.upd_dep2_cd = reader["dep2_name"].ToString();
                        ca400.upd_pos_cd = reader["pos_name"].ToString();
                    }
                }
                conn.Close();
            }

            string SQL9 = "select dp1.dep1_name, dp2.dep2_name, pos.pos_name"
                        + " from m_department1 AS dp1"
                        + " JOIN"
                        + " m_department2 AS dp2"
                        + " ON dp1.dep1_cd = '" + ca400.reject_dep1_cd + "' AND dp2.dep2_cd = '" + ca400.reject_dep2_cd + "'"
                        + " JOIN"
                        + " m_position AS pos"
                        + " ON pos.pos_cd = '" + ca400.reject_pos_cd + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL9, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ca400.reject_dep1_cd = reader["dep1_name"].ToString();
                        ca400.reject_dep2_cd = reader["dep2_name"].ToString();
                        ca400.reject_pos_cd = reader["pos_name"].ToString();
                    }
                }
                conn.Close();
            }

            //ステータス値取得
            string SQL10 = "select cd_name from m_cd where cd = '" + ca400.status + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL10, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ca400.status = reader["cd_name"].ToString();
                    }
                }
                conn.Close();
                return ca400;
            }

        }

        /// <summary>
        /// テンプレートID取得
        /// </summary>
        /// <param name="reportid"></param>
        /// <returns></returns>
        public string GetTempleteIDCa400(String reportid)
        {
            string templete = null;
            string SQL = "select templete_id from t_report where report_id =  '" + reportid + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        templete = reader["templete_id"].ToString();
                    }
                }
                conn.Close();
                return templete;
            }

        }

        /// <summary>
        /// 帳票削除イベント
        /// </summary>
        /// <param name="templeteid"></param>
        /// <param name="reportid"></param>
        /// <param name="id"></param>
        /// <param name="date"></param>
        public void DeleteReportCa400(string templeteid, string reportid, string id, string date)
        {
            string SQL = "update t_report"
                       + " set upd_id = '" + id + "', upd_date = '" + date + "', del_cd = '1'"
                       + " where report_id = '" + reportid + "' AND templete_id = '" + templeteid + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        /// <summary>
        /// Ca200_帳票修正：帳票情報取得SQL
        /// </summary>
        /// <returns></returns>
        public Ca200Dto SelectCa200Dto(string reportID, string templeteID, string userId)
        {
            Ca200Dto ca200Dto = new Ca200Dto();
            string SQL = "SELECT A1.com_cd, A1.member_fistname, A1.member_lastname, A1.stamp_cd, A1.stamp_size, B1.dep1_name, C1.dep2_name," +
                         "D1.pos_name FROM t_member as A1 inner join m_department1 as B1 On A1.dep1_cd = B1.dep1_cd " +
                         "inner join m_department2 as C1 On A1.dep2_cd = C1.dep2_cd inner join m_position as D1 " +
                         "On A1.pos_cd = D1.pos_cd where A1.del_cd='0' AND A1.member_id='" + userId + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            ca200Dto.lbAuthorName = reader["member_fistname"].ToString() + reader["member_lastname"].ToString();
                            ca200Dto.lbComName = reader["com_cd"].ToString();
                            ca200Dto.lbDep1 = reader["dep1_name"].ToString();
                            ca200Dto.lbDep2 = reader["dep2_name"].ToString();
                            ca200Dto.lbPos = reader["pos_name"].ToString();
                            ca200Dto.stampCd = reader["stamp_cd"].ToString();
                            if (reader["stamp_size"].ToString() != null)
                            {
                                ca200Dto.stampSize = Convert.ToInt32(reader["stamp_size"]);
                            }
                            else
                            {
                                ca200Dto.stampSize = 0;
                            }
                            //TODO：StampImg
                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            string SQL1 = "Select report_name, report_detail, file_name, file_binary, report_file_size, approval_mail1," +
                          "approval_mail2, approval_mail3, approval_mail4, approval_mail5, approval_mail6, approval_mail7," +
                          "approval_mail8, approval_mail9, approval_mail10, approval_mail_last from t_report where " +
                          "del_cd='0' AND report_id= '" + reportID + "' AND templete_id='" + templeteID + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL1, conn);
                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            ca200Dto.lbReportName = reader["report_name"].ToString();
                            ca200Dto.lbReportDetail = reader["report_detail"].ToString();
                            ca200Dto.reportFile = reader["file_name"].ToString();
                            ca200Dto.binaryFile = reader["file_binary"].ToString();
                            if (reader["report_file_size"].ToString() == null)
                            {
                                ca200Dto.fileSize = 0;
                            }
                            else
                            {
                                ca200Dto.fileSize = Convert.ToInt32(reader["report_file_size"]);
                            }

                            ca200Dto.lbAprvMail1 = reader["approval_mail1"].ToString();
                            ca200Dto.lbAprvMail2 = reader["approval_mail2"].ToString();
                            ca200Dto.lbAprvMail3 = reader["approval_mail3"].ToString();
                            ca200Dto.lbAprvMail4 = reader["approval_mail4"].ToString();
                            ca200Dto.lbAprvMail5 = reader["approval_mail5"].ToString();
                            ca200Dto.lbAprvMail6 = reader["approval_mail6"].ToString();
                            ca200Dto.lbAprvMail7 = reader["approval_mail7"].ToString();
                            ca200Dto.lbAprvMail8 = reader["approval_mail8"].ToString();
                            ca200Dto.lbAprvMail9 = reader["approval_mail9"].ToString();
                            ca200Dto.lbAprvMail10 = reader["approval_mail10"].ToString();
                            ca200Dto.fnAprvMail = reader["approval_mail_last"].ToString();
                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            string SQL2 = "Select templete_name, approval_place_start, approval_place1, approval_place2, approval_place3," +
                     "approval_place4, approval_place5, approval_place6, approval_place7, approval_place8," +
                     "approval_place9, approval_place10, approval_place_last, approval_status1," +
                     "approval_status2, approval_status3, approval_status4, approval_status5, approval_status6," +
                     "approval_status7, approval_status8, approval_status9, approval_status10, " +
                     "approval_status_last from t_templete where  templete_id= '" + templeteID +
                     "' AND del_cd = '0'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL2, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            ca200Dto.templeteName = reader["templete_name"].ToString();
                            ca200Dto.lbAprvNum1 = "決裁1";
                            ca200Dto.lbApprover1 = reader["approval_status1"].ToString();
                            ca200Dto.lbAprvPlc1 = reader["approval_place1"].ToString();
                            ca200Dto.lbAprvNum2 = "決裁2";
                            ca200Dto.lbApprover2 = reader["approval_status2"].ToString();
                            ca200Dto.lbAprvPlc2 = reader["approval_place2"].ToString();
                            ca200Dto.lbAprvNum3 = "決裁3";
                            ca200Dto.lbApprover3 = reader["approval_status3"].ToString();
                            ca200Dto.lbAprvPlc3 = reader["approval_place3"].ToString();
                            ca200Dto.lbAprvNum4 = "決裁4";
                            ca200Dto.lbApprover4 = reader["approval_status4"].ToString();
                            ca200Dto.lbAprvPlc4 = reader["approval_place4"].ToString();
                            ca200Dto.lbAprvNum5 = "決裁5";
                            ca200Dto.lbApprover5 = reader["approval_status5"].ToString();
                            ca200Dto.lbAprvPlc5 = reader["approval_place5"].ToString();
                            ca200Dto.lbAprvNum6 = "決裁6";
                            ca200Dto.lbApprover6 = reader["approval_status6"].ToString();
                            ca200Dto.lbAprvPlc6 = reader["approval_place6"].ToString();
                            ca200Dto.lbAprvNum7 = "決裁7";
                            ca200Dto.lbApprover7 = reader["approval_status7"].ToString();
                            ca200Dto.lbAprvPlc7 = reader["approval_place7"].ToString();
                            ca200Dto.lbAprvNum8 = "決裁8";
                            ca200Dto.lbApprover8 = reader["approval_status8"].ToString();
                            ca200Dto.lbAprvPlc8 = reader["approval_place8"].ToString();
                            ca200Dto.lbAprvNum9 = "決裁9";
                            ca200Dto.lbApprover9 = reader["approval_status9"].ToString();
                            ca200Dto.lbAprvPlc9 = reader["approval_place9"].ToString();
                            ca200Dto.lbAprvNum10 = "決裁10";
                            ca200Dto.lbApprover10 = reader["approval_status10"].ToString();
                            ca200Dto.lbAprvPlc10 = reader["approval_place10"].ToString();
                            ca200Dto.fnAprvNum = "最終決裁者";
                            ca200Dto.fnApprover = reader["approval_status_last"].ToString();
                            ca200Dto.fnAprvPlc = reader["approval_place_last"].ToString();

                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            string SQL3 = "Select approval_date_plan from t_approval where report_id= '" + reportID +
                          "' AND templete_id='" + templeteID + "' AND del_cd = '0'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL3, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");
                            ca200Dto.lbPlanYmd = reader["approval_date_plan"].ToString();
                        }
                        else
                        {
                            Console.WriteLine("Select fail");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }
            return ca200Dto;
        }

        /// <summary>
        /// Ca200_帳票修正：アップデートSQL
        /// </summary>
        public void UpdateCa200(Ca200Dto ca200, string fileReport)
        {
            string SQL = "UPDATE t_report SET report_name = '" + ca200.txtReportName
                + "', report_detail = '" + ca200.txtReportDetail
                + "', file_name = '" + ca200.reportFile
                + "', file_binary = '" + fileReport
                + "', report_file_size = '" + ca200.fileSize
                + "', approval_mail1 = '" + ca200.txtAprvMail1
                + "', approval_mail2 = '" + ca200.txtAprvMail2
                + "', approval_mail3 = '" + ca200.txtAprvMail3
                + "', approval_mail4 = '" + ca200.txtAprvMail4
                + "', approval_mail5 = '" + ca200.txtAprvMail5
                + "', approval_mail6 = '" + ca200.txtAprvMail6
                + "', approval_mail7 = '" + ca200.txtAprvMail7
                + "', approval_mail8 = '" + ca200.txtAprvMail8
                + "', approval_mail9 = '" + ca200.txtAprvMail9
                + "', approval_mail10 = '" + ca200.txtAprvMail10
                + "', approval_mail_last = '" + ca200.txtFnAprvMail
                + "', upd_id = '" + ca200.updID
                + "', upd_date = '" + ca200.updDate
                + "' WHERE report_id = '" + ca200.reportID + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Update success");
                    }
                    else
                    {
                        Console.WriteLine("Update fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

            string SQL1 = "UPDATE t_approval SET upd_id = '" + ca200.updID
                + "', upd_date = '" + ca200.updDate
                + "' WHERE report_id = '" + ca200.reportID + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL1, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Update success");
                    }
                    else
                    {
                        Console.WriteLine("Update fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        /// <summary>
        /// CA100_帳票作成INSERT
        /// </summary>
        public void InsertCa200Reregister(Ca200Dto ca200Dto, string reportFile)
        {
            string SQL = "INSERT INTO t_report (report_id, templete_id, report_name, report_detail, " +
                         "file_name, file_binary,report_file_size, approval_mail1, " +
                         "approval_mail2, approval_mail3, approval_mail4, approval_mail5, " +
                         "approval_mail6, approval_mail7, approval_mail8, approval_mail9, approval_mail10," +
                         "approval_mail_last, reg_ym, reg_id, reg_date, del_cd)" +
                         "VALUES('"
                         + ca200Dto.reportID + "','"
                         + ca200Dto.templeteID + "','"
                         + ca200Dto.txtReportName + "','"
                         + ca200Dto.txtReportDetail + "','"
                         + ca200Dto.reportFile + "','"
                         + reportFile + "','"
                         + ca200Dto.fileSize + "','"
                         + ca200Dto.txtAprvMail1 + "','"
                         + ca200Dto.txtAprvMail2 + "','"
                         + ca200Dto.txtAprvMail3 + "','"
                         + ca200Dto.txtAprvMail4 + "','"
                         + ca200Dto.txtAprvMail5 + "','"
                         + ca200Dto.txtAprvMail6 + "','"
                         + ca200Dto.txtAprvMail7 + "','"
                         + ca200Dto.txtAprvMail8 + "','"
                         + ca200Dto.txtAprvMail9 + "','"
                         + ca200Dto.txtAprvMail10 + "','"
                         + ca200Dto.txtFnAprvMail + "','"
                         + ca200Dto.regYm + "','"
                         + ca200Dto.regID + "','"
                         + ca200Dto.regDate + "','"
                         + ca200Dto.delCd + "')";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }
                conn.Close();
            }

            string SQL1 = "INSERT INTO  t_approval (report_id, templete_id, status, maker_mail, " +
                         "maker_stamp, maker_stamp_size," +
                         "approval_date_plan, reg_id, reg_date, del_cd) VALUES('"
                         + ca200Dto.reportID + "','"
                         + ca200Dto.templeteID + "','"
                         + ca200Dto.status + "','"
                         + ca200Dto.startAprvMail + "','"
                         + ca200Dto.startStampImg + "','"
                         + ca200Dto.stampSize + "','"
                         + ca200Dto.lbPlanYmd + "','"
                         + ca200Dto.regID + "','"
                         + ca200Dto.regDate + "','"
                         + ca200Dto.delCd + "')";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL1, conn);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");

                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }
                conn.Close();
            }

        }

        public DownloadDto SelectReportInfoForDownlord(string reportid)
        {
            string SQL = "SELECT a.status, a.maker_stamp AS maker_stamp, a.maker_stamp_size AS maker_stamp_size, t.approval_place_start AS approval_place_start, a.approval_stamp1, "
                + "a.approval_stamp_size1, t.approval_place1, a.approval_stamp2, a.approval_stamp_size2, t.approval_place2, "
                + "a.approval_stamp3, a.approval_stamp_size3, t.approval_place3, a.approval_stamp4, a.approval_stamp_size4, "
                + "t.approval_place4, a.approval_stamp5, a.approval_stamp_size5, t.approval_place5, a.approval_stamp6, a.approval_stamp_size6, "
                + "t.approval_place6, a.approval_stamp7, a.approval_stamp_size7, t.approval_place7, a.approval_stamp8, a.approval_stamp_size8, "
                + "t.approval_place8, a.approval_stamp9, a.approval_stamp_size9, t.approval_place9, a.approval_stamp10, a.approval_stamp_size10, "
                + "t.approval_place10, a.approval_stamp_last, a.approval_stamp_last_size, t.approval_place_last, r.file_name, r.file_binary, "
                + "r.report_file_size FROM t_approval a INNER JOIN t_report r ON r.report_id = a.report_id "
                + "INNER JOIN t_templete t ON t.templete_id = r.templete_id WHERE a.report_id = '" + reportid + "'";

            DownloadDto downloadDto = new DownloadDto();
            downloadDto.stampInfo = new string[12];
            downloadDto.stampInfoPlace = new string[12];
            downloadDto.stampInfoSize = new string[12];

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            downloadDto.status = reader["status"].ToString();
                            downloadDto.stampInfo[0] = System.Text.Encoding.Default.GetString((byte[])reader["maker_stamp"]).ToString();
                            downloadDto.stampInfoSize[0] = reader["maker_stamp_size"].ToString();
                            if (!reader["approval_stamp1"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[1] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp1"]).ToString();
                            }
                            downloadDto.stampInfoPlace[0] = reader["approval_place_start"].ToString();
                            downloadDto.stampInfoSize[1] = reader["approval_stamp_size1"].ToString();
                            downloadDto.stampInfoPlace[1] = reader["approval_place1"].ToString();
                            if (!reader["approval_stamp2"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[2] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp2"]).ToString();
                            }
                            downloadDto.stampInfoSize[2] = reader["approval_stamp_size2"].ToString();
                            downloadDto.stampInfoPlace[2] = reader["approval_place2"].ToString();
                            if (!reader["approval_stamp3"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[3] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp3"]).ToString();
                            }
                            downloadDto.stampInfoSize[3] = reader["approval_stamp_size3"].ToString();
                            downloadDto.stampInfoPlace[3] = reader["approval_place3"].ToString();
                            if (!reader["approval_stamp4"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[4] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp4"]).ToString();
                            }
                            downloadDto.stampInfoSize[4] = reader["approval_stamp_size4"].ToString();
                            downloadDto.stampInfoPlace[4] = reader["approval_place4"].ToString();
                            if (!reader["approval_stamp5"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[5] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp5"]).ToString();
                            }
                            downloadDto.stampInfoSize[5] = reader["approval_stamp_size5"].ToString();
                            downloadDto.stampInfoPlace[5] = reader["approval_place5"].ToString();
                            if (!reader["approval_stamp6"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[6] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp6"]).ToString();
                            }
                            downloadDto.stampInfoSize[6] = reader["approval_stamp_size6"].ToString();
                            downloadDto.stampInfoPlace[6] = reader["approval_place6"].ToString();
                            if (!reader["approval_stamp7"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[7] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp7"]).ToString();
                            }
                            downloadDto.stampInfoSize[7] = reader["approval_stamp_size7"].ToString();
                            downloadDto.stampInfoPlace[7] = reader["approval_place7"].ToString();
                            if (!reader["approval_stamp8"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[8] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp8"]).ToString();
                            }
                            downloadDto.stampInfoSize[8] = reader["approval_stamp_size8"].ToString();
                            downloadDto.stampInfoPlace[8] = reader["approval_place8"].ToString();
                            if (!reader["approval_stamp9"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[9] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp9"]).ToString();
                            }
                            downloadDto.stampInfoSize[9] = reader["approval_stamp_size9"].ToString();
                            downloadDto.stampInfoPlace[9] = reader["approval_place9"].ToString();
                            if (!reader["approval_stamp10"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[10] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp10"]).ToString();
                            }
                            downloadDto.stampInfoSize[10] = reader["approval_stamp_size10"].ToString();
                            downloadDto.stampInfoPlace[10] = reader["approval_place10"].ToString();
                            if (!reader["approval_stamp_last"].ToString().Equals(""))
                            {
                                downloadDto.stampInfo[11] = System.Text.Encoding.Default.GetString((byte[])reader["approval_stamp_last"]).ToString();
                            }
                            downloadDto.stampInfoSize[11] = reader["approval_stamp_last_size"].ToString();
                            downloadDto.stampInfoPlace[11] = reader["approval_place_last"].ToString();
                            downloadDto.file_name = reader["file_name"].ToString();
                            downloadDto.file_binary = reader["file_binary"].ToString();
                            downloadDto.report_file_size = reader["report_file_size"].ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

            return downloadDto;

        }

        public List<Zb400Dto.Zb400listDto> GetInfoBydataZb400(string comCd)
        {
            int i = 0;
            List<Zb400Dto.Zb400listDto> zb400SelectLists = new List<Zb400Dto.Zb400listDto>();
            string SQL = "select ml.mailList_mail, ml.mailList_detail, mb.member_fistname, mb.member_lastname"
                       + " from t_maillist AS ml"
                       + " JOIN"
                       + " t_member AS mb"
                       + " ON ml.reg_id = mb.member_id"
                       + " where mb.com_cd = '" + comCd + "' AND ml.del_cd = '0'"
                       + " order by ml.upd_date desc";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        zb400SelectLists.Add(new Zb400Dto.Zb400listDto()
                        {
                            mail = reader["mailList_mail"].ToString(),
                            detail = reader["mailList_detail"].ToString(),
                            reg = reader["member_fistname"].ToString() + "　" + reader["member_lastname"].ToString(),
                            upd = getUpdidZb400(i, comCd)
                        }); ;
                        i = i + 1;
                    }
                }
                conn.Close();
                return zb400SelectLists;
            }

        }

        public string getUpdidZb400(int i, string comCd)
        {
            string updid = "";
            string SQL = "select mb.member_fistname, mb.member_lastname"
                       + " from t_maillist AS ml"
                       + " JOIN"
                       + " t_member AS mb"
                       + " ON ml.upd_id = mb.member_id"
                       + " where mb.com_cd = '" + comCd + "' AND ml.del_cd = '0'"
                       + " order by ml.upd_date desc"
                       + " LIMIT " + i + ", " + (i + 1) + "";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        updid = reader["member_fistname"].ToString() + "　" + reader["member_lastname"].ToString();
                    }
                }
                conn.Close();
                return updid;
            }
        }

        public void InsertMaillistZb400(Zb400Dto zb400, string date, string id)
        {
            string SQL = "INSERT INTO Hanyokessai.t_mailList (`mailList_mail`, `mailList_detail`, `mail1`, `mail2`, `mail3`, `mail4`, `mail5`, `mail6`, `mail7`, `mail8`, `mail9`, `mail10`, `reg_id`, `reg_date`, `upd_id`, `upd_date`, `del_cd`)"
                       + " VALUES('" + zb400.mailList_mail + "', '" + zb400.mailList_detail + "', '" + zb400.mail1 + "', '" + zb400.mail2 + "', '" + zb400.mail3 + "', '" + zb400.mail4 + "', '" + zb400.mail5 + "', '" + zb400.mail6 + "', '" + zb400.mail7 + "', '" + zb400.mail8 + "', '" + zb400.mail9 + "', '" + zb400.mail10 + "', '" + id + "', '" + date + "', '" + id + "', '" + date + "', '0')";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        public void UpdateMaillistZb400(Zb400Dto zb400, string date, string id)
        {
            string SQL = "update t_maillist set mailList_mail = '" + zb400.mailList_mail + "', mailList_detail = '" + zb400.mailList_detail + "', mail1 = '" + zb400.mail1 + "', mail2 = '" + zb400.mail2 + "', mail3 = '" + zb400.mail3 + "', mail4 = '" + zb400.mail4 + "', mail5 = '" + zb400.mail5 + "', mail6 = '" + zb400.mail6 + "', mail7 = '" + zb400.mail7 + "', mail8 = '" + zb400.mail8 + "', mail9 = '" + zb400.mail9 + "', mail10 = '" + zb400.mail10 + "', upd_id = '" + id + "', upd_date = '" + date + "'"
                       + " where mailList_mail = '" + zb400.mailList_mail +"'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        public void DeleteMaillistZb400(string mail)
        {
            string SQL = "update t_maillist set del_cd = '1' where mailList_mail = '" + mail + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }

        }

        public Zb400Dto SelectMaillistzb400(string chkmail)
        {
            Zb400Dto zb400 = null;
            string SQL = "SELECT mailList_mail, mailList_detail, mail1, mail2, mail3, mail4, mail5, mail6, mail7, mail8, mail9, mail10 FROM t_maillist WHERE mailList_mail = '" + chkmail + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        zb400 = new Zb400Dto();
                        zb400.mailList_mail = reader["mailList_mail"].ToString();
                        zb400.mailList_detail = reader["mailList_detail"].ToString();
                        zb400.mail1 = reader["mail1"].ToString();
                        zb400.mail2 = reader["mail2"].ToString();
                        zb400.mail3 = reader["mail3"].ToString();
                        zb400.mail4 = reader["mail4"].ToString();
                        zb400.mail5 = reader["mail5"].ToString();
                        zb400.mail6 = reader["mail6"].ToString();
                        zb400.mail7 = reader["mail7"].ToString();
                        zb400.mail8 = reader["mail8"].ToString();
                        zb400.mail9 = reader["mail9"].ToString();
                        zb400.mail10 = reader["mail10"].ToString();
                        zb400.Updatebtn = "";
                    }
                }
                conn.Close();
                return zb400;
            }
        }

        public List<Ta200Dto> SelectTempletTa200(string companyCd)
        {
            List<Ta200Dto> ta200List = new List<Ta200Dto>();
            int i = 1;
            string SQL = "SELECT tt.templete_id, tt.templete_name, tt.file_name, tt.file_binary, tt.templete_file_size, tt.com_cd, tm.member_fistname AS member_fistname1, tm.member_lastname AS member_lastname1, tm.pos_cd AS pos_cd1, tt.reg_date, tm2.member_fistname AS member_fistname2, tm2.member_lastname AS member_lastname2, tm.pos_cd AS pos_cd2, tt.upd_date FROM t_templete AS tt Join t_member AS tm ON tt.reg_id = tm.member_id Join t_member AS tm2 ON tt.upd_id = tm2.member_id WHERE tt.del_cd = '0' AND tt.com_cd = '" + companyCd + "'";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ta200Dto ta200Dto = new Ta200Dto();
                        ta200Dto.txtNo = i.ToString();
                        ta200Dto.txtTempleteId = reader["templete_id"].ToString();
                        ta200Dto.txtTempleteName = reader["templete_name"].ToString();
                        ta200Dto.txtFileName = reader["file_name"].ToString();
                        ta200Dto.txtFileBinary = System.Text.Encoding.Default.GetString((byte[])reader["file_binary"]).ToString();
                        ta200Dto.txtTempleteFileSize = reader["templete_file_size"].ToString();
                        ta200Dto.txtCom_cd = reader["com_cd"].ToString();
                        ta200Dto.txtRegName = reader["member_fistname1"].ToString() + reader["member_lastname1"].ToString() + reader["pos_cd1"].ToString();
                        ta200Dto.txtRegDate = reader["reg_date"].ToString();
                        ta200Dto.txtUpdName = reader["member_fistname2"].ToString() + reader["member_lastname2"].ToString() + reader["pos_cd2"].ToString();
                        ta200Dto.txtUpdDate = reader["upd_date"].ToString();
                        ta200List.Add(ta200Dto);
                        i++;
                    }
                }
                conn.Close();
                return ta200List;
            }
        }

        public Ta300Dto SelectTempletTa300(string templeteId)
        {
            Ta300Dto ta300 = null;
            string SQL = "SELECT tm.member_fistname AS member_fistname1, tm.member_lastname AS member_lastname1, tm.pos_cd AS pos_cd1, tm.com_cd AS tmCom_cd, " +
                "md.dep1_name AS mdDep1_name, md2.dep2_name AS md2Dep2_name," +
                "tm2.member_fistname AS member_fistname2, tm2.member_lastname AS member_lastname2, tm2.pos_cd AS pos_cd2, tm2.com_cd AS tm2Com_cd, " +
                "md3.dep1_name AS md3Dep1_name, md4.dep2_name AS md4Dep2_name, " +
                "tt.templete_id, tt.templete_name, tt.templete_detail, tt.file_name, tt.file_binary, tt.templete_file_size, " +
                "tt.approval_place_start, tt.approval_place1, tt.approval_place2, tt.approval_place3, tt.approval_place4, tt.approval_place5, tt.approval_place6, " +
                "tt.approval_place7, tt.approval_place8, tt.approval_place9, tt.approval_place10, tt.approval_place_last, " +
                "tt.approval_mail1, tt.approval_mail2, tt.approval_mail3, tt.approval_mail4, tt.approval_mail5, tt.approval_mail6, tt.approval_mail7, " +
                "tt.approval_mail8, tt.approval_mail9, tt.approval_mail10, tt.approval_mail_last, " +
                "tt.approval_status1, tt.approval_status2, tt.approval_status3, tt.approval_status4, tt.approval_status5, tt.approval_status6," +
                "tt.approval_status7, tt.approval_status8, tt.approval_status9, tt.approval_status10, tt.approval_status_last, " +
                "tt.reg_date, tt.upd_date " +
                "FROM t_templete AS tt Join t_member AS tm ON tt.reg_id = tm.member_id " +
                "Join t_member AS tm2 ON tt.upd_id = tm2.member_id " +
                "Join m_department1 AS md ON tm.dep1_cd = md.dep1_cd " +
                "Join m_department2 AS md2 ON tm.dep2_cd = md2.dep2_cd " +
                "Join m_department1 AS md3 ON tm.dep1_cd = md3.dep1_cd " +
                "Join m_department2 AS md4 ON tm.dep2_cd = md4.dep2_cd " +
                "WHERE tt.del_cd = '0' AND templete_id = '" + templeteId + "'";


            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ta300 = new Ta300Dto();
                        ta300.reg_com_cd = reader["tmCom_cd"].ToString();
                        ta300.reg_dep1_cd = reader["mdDep1_name"].ToString();
                        ta300.reg_dep2_cd = reader["md2Dep2_name"].ToString();
                        ta300.reg_pos_cd = reader["pos_cd1"].ToString();
                        ta300.reg_member_fistname = reader["member_fistname1"].ToString();
                        ta300.reg_member_lastname = reader["member_lastname1"].ToString();
                        ta300.upd_com_cd = reader["tm2Com_cd"].ToString();
                        ta300.upd_dep1_cd = reader["md3Dep1_name"].ToString();
                        ta300.upd_dep2_cd = reader["md4Dep2_name"].ToString();
                        ta300.upd_pos_cd = reader["pos_cd2"].ToString();
                        ta300.upd_member_fistname = reader["member_fistname2"].ToString();
                        ta300.upd_member_lastname = reader["member_lastname2"].ToString();
                        ta300.templete_id = reader["templete_id"].ToString();
                        ta300.templete_name = reader["templete_name"].ToString();
                        ta300.templete_detail = reader["templete_detail"].ToString();
                        ta300.file_name = reader["file_name"].ToString();
                        ta300.file_binary = System.Text.Encoding.Default.GetString((byte[])reader["file_binary"]).ToString();
                        ta300.templete_file_size = reader["templete_file_size"].ToString();
                        ta300.approval_place_start = reader["approval_place_start"].ToString();
                        ta300.approval_place1 = reader["approval_place1"].ToString();
                        ta300.approval_place2 = reader["approval_place2"].ToString();
                        ta300.approval_place3 = reader["approval_place3"].ToString();
                        ta300.approval_place4 = reader["approval_place4"].ToString();
                        ta300.approval_place5 = reader["approval_place5"].ToString();
                        ta300.approval_place6 = reader["approval_place6"].ToString();
                        ta300.approval_place7 = reader["approval_place7"].ToString();
                        ta300.approval_place8 = reader["approval_place8"].ToString();
                        ta300.approval_place9 = reader["approval_place9"].ToString();
                        ta300.approval_place10 = reader["approval_place10"].ToString();
                        ta300.approval_place_last = reader["approval_place_last"].ToString();
                        ta300.approval_mail1 = reader["approval_mail1"].ToString();
                        ta300.approval_mail2 = reader["approval_mail2"].ToString();
                        ta300.approval_mail3 = reader["approval_mail3"].ToString();
                        ta300.approval_mail4 = reader["approval_mail4"].ToString();
                        ta300.approval_mail5 = reader["approval_mail5"].ToString();
                        ta300.approval_mail6 = reader["approval_mail6"].ToString();
                        ta300.approval_mail7 = reader["approval_mail7"].ToString();
                        ta300.approval_mail8 = reader["approval_mail8"].ToString();
                        ta300.approval_mail9 = reader["approval_mail9"].ToString();
                        ta300.approval_mail10 = reader["approval_mail10"].ToString();
                        ta300.approval_mail_last = reader["approval_mail_last"].ToString();
                        ta300.approval_status1 = reader["approval_status1"].ToString();
                        ta300.approval_status2 = reader["approval_status2"].ToString();
                        ta300.approval_status3 = reader["approval_status3"].ToString();
                        ta300.approval_status4 = reader["approval_status4"].ToString();
                        ta300.approval_status5 = reader["approval_status5"].ToString();
                        ta300.approval_status6 = reader["approval_status6"].ToString();
                        ta300.approval_status7 = reader["approval_status7"].ToString();
                        ta300.approval_status8 = reader["approval_status8"].ToString();
                        ta300.approval_status9 = reader["approval_status9"].ToString();
                        ta300.approval_status10 = reader["approval_status10"].ToString();
                        ta300.approval_status_last = reader["approval_status_last"].ToString();
                        ta300.reg_date = reader["reg_date"].ToString();
                        ta300.upd_date = reader["upd_date"].ToString();
                    }
                }
                conn.Close();
                return ta300;
            }
        }

        public void UpdateTempletTa300(String memberId, string templeteId)
        {
            string SQL = "UPDATE t_templete SET upd_id = '" + memberId + "'" +
                ", upd_date = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "'" +
                ", del_cd = '1'" + 
                "WHERE templete_id = '" + templeteId + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Update success");
                    }
                    else
                    {
                        Console.WriteLine("Update fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }
        }

        /// <summary>
        /// テンプレート登録INSERT
        /// </summary>
        public void InsertTemplateRegister(Ta100Dto ta100Dto, string tmpfileText)
        {
            string SQL = "INSERT INTO t_templete (templete_id, templete_name, templete_detail, file_name, file_binary,"
                         + "templete_file_size, com_cd, approval_place_start, approval_place1,"
                         + " approval_place2, approval_place3, approval_place4, approval_place5,"
                         + " approval_place6, approval_place7, approval_place8, approval_place9,"
                         + " approval_place10, approval_place_last, approval_mail1, approval_mail2,"
                         + " approval_mail3, approval_mail4, approval_mail5, approval_mail6,"
                         + " approval_mail7, approval_mail8, approval_mail9, approval_mail10,"
                         + " approval_mail_last, reg_id, reg_date, del_cd)"
                         + "VALUES('"
                         + ta100Dto.selTemType + "','"
                         + ta100Dto.txtTemName + "','"
                         + ta100Dto.txtRepDetail + "','"
                         + ta100Dto.txtFileName + "','"
                         + tmpfileText + "','"
                         + ta100Dto.tmpFileSize + "','"
                         + ta100Dto.selComName + "','"
                         + ta100Dto.selApproverStart + "','"
                         + ta100Dto.txtApproPlace1 + "','"
                         + ta100Dto.txtApproPlace2 + "','"
                         + ta100Dto.txtApproPlace3 + "','"
                         + ta100Dto.txtApproPlace4 + "','"
                         + ta100Dto.txtApproPlace5 + "','"
                         + ta100Dto.txtApproPlace6 + "','"
                         + ta100Dto.txtApproPlace7 + "','"
                         + ta100Dto.txtApproPlace8 + "','"
                         + ta100Dto.txtApproPlace9 + "','"
                         + ta100Dto.txtApproPlace10 + "','"
                         + ta100Dto.txtLastApproPlace + "','"
                         + ta100Dto.txtApprovalMail1 + "','"
                         + ta100Dto.txtApprovalMail2 + "','"
                         + ta100Dto.txtApprovalMail3 + "','"
                         + ta100Dto.txtApprovalMail4 + "','"
                         + ta100Dto.txtApprovalMail5 + "','"
                         + ta100Dto.txtApprovalMail6 + "','"
                         + ta100Dto.txtApprovalMail7 + "','"
                         + ta100Dto.txtApprovalMail8 + "','"
                         + ta100Dto.txtApprovalMail9 + "','"
                         + ta100Dto.txtApprovalMail10 + "','"
                         + ta100Dto.txtLastApproMail + "','"
                         + ta100Dto.txtRegId + "','"
                         + ta100Dto.txtRegDate + "','"
                         + ta100Dto.txtDelCd + "' )";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Insert success");
                    }
                    else
                    {
                        Console.WriteLine("Insert fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }
        }

        /// <summary>
        /// 職位をSELECT
        /// </summary>
        public Ta100Dto SelectPositionName()
        {
            Ta100Dto ta100Dto = new Ta100Dto();
            ta100Dto.selApproverPosList = new List<string>();
            ta100Dto.selApproverNameList = new List<string>();

            string SQL = "SELECT pos_cd, pos_name FROM m_position WHERE pos_cd NOT IN('LD','MB','MG','SLD')";


            using (MySqlConnection conn = GetConnection())
            {

                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                ta100Dto.selApproverPosList.Add("");
                ta100Dto.selApproverNameList.Add("-");
                using (var reader = cmd.ExecuteReader())

                    while (reader.Read())
                    {
                        ta100Dto.selApproverPosList.Add(reader["pos_cd"].ToString());
                        ta100Dto.selApproverNameList.Add(reader["pos_name"].ToString());
                    }
                conn.Close();
                return ta100Dto;
            }
        }

        /// <summary>
        /// シリアル・ナンバーをSELECT！
        /// </summary>
        public String SelectTmpSerialNumber()
        {
            string SQL = "SELECT LPAD((SELECT COUNT(templete_id) + 1 as templete_id_Cnt FROM t_templete),3,0) as templete_id_Cnt";

            String serialNumber = null;

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);
                    using (var reader = cmd.ExecuteReader())

                        if (reader.Read())
                        {
                            Console.WriteLine("Select success");

                            serialNumber = reader["templete_id_Cnt"].ToString();

                        }
                        else
                        {
                            Console.WriteLine("Select fail");

                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());

                }
                conn.Close();
            }

            return serialNumber;

        }
        /// <summary>
        /// テンプレート修正Select
        /// </summary>
        public Ta400Dto Selecttempleteta400(string template_id)
        {
            Ta400Dto ta400 = null;
            string SQL = "SELECT templete_id, templete_name, templete_detail, file_name, file_binary, templete_file_size, approval_place_start, approval_place1, approval_place2, approval_place3, approval_place4, approval_place5, approval_place6, approval_place7, approval_place8, approval_place9, approval_place10, approval_place_last, " +
                "approval_mail1, approval_mail2, approval_mail3, approval_mail4, approval_mail5, approval_mail6, approval_mail7, approval_mail8, approval_mail9, approval_mail10, approval_mail_last, " +
                "approval_status1, approval_status2, approval_status3, approval_status4, approval_status5, approval_status6, approval_status7, approval_status8, approval_status9, approval_status10, approval_status_last " +
                "FROM t_templete WHERE templete_id = '" + template_id + "'";

            string SQL1 = "SELECT pos_cd, pos_name FROM m_position WHERE pos_cd NOT IN('LD','MB','MG','SLD')";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ta400 = new Ta400Dto();
                        ta400.txtTemId = reader["templete_id"].ToString();
                        ta400.txtTemName = reader["templete_name"].ToString();
                        ta400.txtRepDetail = reader["templete_detail"].ToString();
                        ta400.txtTemFileName = reader["file_name"].ToString();
                        ta400.file_binary = System.Text.Encoding.Default.GetString((byte[])reader["file_binary"]).ToString();
                        ta400.temp_filesize = reader["templete_file_size"].ToString();
                        ta400.txtStartApproPlace = reader["approval_place_start"].ToString();
                        ta400.txtApproPlace1 = reader["approval_place1"].ToString();
                        ta400.txtApproPlace2 = reader["approval_place2"].ToString();
                        ta400.txtApproPlace3 = reader["approval_place3"].ToString();
                        ta400.txtApproPlace4 = reader["approval_place4"].ToString();
                        ta400.txtApproPlace5 = reader["approval_place5"].ToString();
                        ta400.txtApproPlace6 = reader["approval_place6"].ToString();
                        ta400.txtApproPlace7 = reader["approval_place7"].ToString();
                        ta400.txtApproPlace8 = reader["approval_place8"].ToString();
                        ta400.txtApproPlace9 = reader["approval_place9"].ToString();
                        ta400.txtApproPlace10 = reader["approval_place10"].ToString();
                        ta400.txtLastApproPlace= reader["approval_place_last"].ToString();
                        ta400.txtApproMail1 = reader["approval_mail1"].ToString();
                        ta400.txtApproMail2 = reader["approval_mail2"].ToString();
                        ta400.txtApproMail3= reader["approval_mail3"].ToString();
                        ta400.txtApproMail4= reader["approval_mail4"].ToString();
                        ta400.txtApproMail5= reader["approval_mail5"].ToString();
                        ta400.txtApproMail6= reader["approval_mail6"].ToString();
                        ta400.txtApproMail7= reader["approval_mail7"].ToString();
                        ta400.txtApproMail8= reader["approval_mail8"].ToString();
                        ta400.txtApproMail9= reader["approval_mail9"].ToString();
                        ta400.txtApproMail10= reader["approval_mail10"].ToString();
                        ta400.txtLastApproMail= reader["approval_mail_last"].ToString();
                        ta400.selApprover1= reader["approval_status1"].ToString();
                        ta400.selApprover2= reader["approval_status2"].ToString();
                        ta400.selApprover3= reader["approval_status3"].ToString();
                        ta400.selApprover4= reader["approval_status4"].ToString();
                        ta400.selApprover5= reader["approval_status5"].ToString();
                        ta400.selApprover6= reader["approval_status6"].ToString();
                        ta400.selApprover7= reader["approval_status7"].ToString();
                        ta400.selApprover8= reader["approval_status8"].ToString();
                        ta400.selApprover9= reader["approval_status9"].ToString();
                        ta400.selApprover10= reader["approval_status10"].ToString();
                        ta400.selLastApprover= reader["approval_status_last"].ToString();
                    }
                }
                ta400.selApproverPosList = new List<string>();
                ta400.selApproverNameList = new List<string>();

                cmd = new MySqlCommand(SQL1, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ta400.selApproverPosList.Add(reader["pos_cd"].ToString());
                        ta400.selApproverNameList.Add(reader["pos_name"].ToString());
                    }
                }
                conn.Close();
                return ta400;
            }
        }

        /// <summary>
        /// テンプレート修正Update
        /// </summary>
        public void UpdateTempleteta400(Ta400Dto ta400, String templete_id)
        {
            string SQL = "UPDATE t_templete SET templete_id = '" + ta400.txtTemId
                + "', templete_name = '" + ta400.txtTemName
                + "', templete_detail = '" + ta400.txtRepDetail
                + "', file_name = '" + ta400.reportFile
                + "', file_binary = '" + ta400.file_binary
                + "', templete_file_size = '" + ta400.fileSize
                + "', approval_place_start = '" + ta400.txtStartApproPlace
                + "', approval_place1 = '" + ta400.txtApproPlace1
                + "', approval_place2 = '" + ta400.txtApproPlace2
                + "', approval_place3 = '" + ta400.txtApproPlace3
                + "', approval_place4 = '" + ta400.txtApproPlace4
                + "', approval_place5 = '" + ta400.txtApproPlace5
                + "', approval_place6 = '" + ta400.txtApproPlace6
                + "', approval_place7 = '" + ta400.txtApproPlace7
                + "', approval_place8 = '" + ta400.txtApproPlace8
                + "', approval_place9 = '" + ta400.txtApproPlace9
                + "', approval_place10 = '" + ta400.txtApproPlace10
                + "', approval_place_last = '" + ta400.txtLastApproPlace
                + "', approval_mail1 = '" + ta400.txtApproMail1
                + "', approval_mail2 = '" + ta400.txtApproMail2
                + "', approval_mail3 = '" + ta400.txtApproMail3
                + "', approval_mail4 = '" + ta400.txtApproMail4
                + "', approval_mail5 = '" + ta400.txtApproMail5
                + "', approval_mail6 = '" + ta400.txtApproMail6
                + "', approval_mail7 = '" + ta400.txtApproMail7
                + "', approval_mail8 = '" + ta400.txtApproMail8
                + "', approval_mail9 = '" + ta400.txtApproMail9
                + "', approval_mail10 = '" + ta400.txtApproMail10
                + "', approval_mail_last = '" + ta400.txtLastApproMail
                + "', approval_status1 = '" + ta400.selApprover1
                + "', approval_status2 = '" + ta400.selApprover2
                + "', approval_status3 = '" + ta400.selApprover3
                + "', approval_status4 = '" + ta400.selApprover4
                + "', approval_status5 = '" + ta400.selApprover5
                + "', approval_status6 = '" + ta400.selApprover6
                + "', approval_status7 = '" + ta400.selApprover7
                + "', approval_status8 = '" + ta400.selApprover8
                + "', approval_status9 = '" + ta400.selApprover9
                + "', approval_status10 = '" + ta400.selApprover10
                + "', approval_status_last = '" + ta400.selLastApprover
                + "', upd_id = '" + ta400.txtRenewalrName
                + "', upd_date = '" + ta400.txtRenewalrDate
                + "' WHERE templete_id = '" + templete_id + "'";

            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, conn);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("Update success");
                    }
                    else
                    {
                        Console.WriteLine("Update fail");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("DB connection fail");
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }
        }

    }
}


