using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;


namespace WebAPI.Models
{
    public class DbContext
    {
        public string ConnectionString { get; set; }

        public DbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<UserModels.UserFromDbModel> GetAllUsers()
        {
            List<UserModels.UserFromDbModel> list = new List<UserModels.UserFromDbModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE uactive = TRUE", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new UserModels.UserFromDbModel()
                        {
                            uid = reader.GetInt32("uid"),
                            ulogin = reader.GetString("ulogin"),
                            upassword = reader.GetString("upassword"),
                            uactive = reader.GetBoolean("uactive")
                        });
                    }
                }
            }

            return list;
        }

        public UserModels.UserFromDbModel GetCurrentUser(string UserName)
        {
            UserModels.UserFromDbModel _user = null;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE uactive = TRUE AND ulogin = \"" + UserName + "\"", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _user = (new UserModels.UserFromDbModel()
                        {
                            uid = reader.GetInt32("uid"),
                            ulogin = reader.GetString("ulogin"),
                            upassword = reader.GetString("upassword"),
                            uactive = reader.GetBoolean("uactive"),
                            customer = reader.GetBoolean("customer"),
                            accounter = reader.GetBoolean("accounter"),
                            administrator = reader.GetBoolean("administrator")
                        });
                    }
                }
            }

            return _user;
        }

        public List<CabinetProducts> GetAllProductsFromCabinet(int cabinet_number, string Constraint)
        {
            List<CabinetProducts> list = new List<CabinetProducts>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("SELECT * FROM cabinet{0} {1}", cabinet_number.ToString(), Constraint), conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CabinetProducts()
                        {
                            pid = reader.GetInt32("pid"),
                            pname = reader.GetString("pname"),
                            code = reader.GetString("code"),
                            pprice = reader.GetDouble("pprice"),
                            pquantity = reader.GetInt32("pquantity"),
                            active = reader.GetBoolean("active")
                        });
                    }
                }
            }

            return list;
        }

        public bool UpdateCabinet3(UpdateRequestComponent component)
        {
            int Prev_count = 0;
            MySqlCommand cmd;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                cmd = new MySqlCommand(string.Format("SELECT pquantity FROM cabinet3 WHERE code = \"{0}\"", component.code), conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Prev_count = reader.GetInt32("pquantity");
                    }
                }

                Prev_count += component.count;
                cmd.Dispose();
                cmd = new MySqlCommand(string.Format("UPDATE cabinet3 SET pquantity = {0} WHERE code = \"{1}\"", Prev_count, component.code), conn);
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public bool UpdateCabinet4(UpdateRequestComponent component)
        {
            int Prev_count = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("SELECT pquantity FROM cabinet4 WHERE code = \"{0}\"", component.code), conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Prev_count = reader.GetInt32("pquantity");
                    }
                }

                Prev_count += component.count;
                cmd.Dispose();
                cmd = new MySqlCommand(string.Format("UPDATE cabinet4 SET pquantity = {0} WHERE code = \"{1}\"", Prev_count, component.code), conn);
                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public int AddRowToReceivingTable(string uid, int storage)
        {
            int result = 0;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("INSERT INTO receiving VALUE (NULL, DATE(NOW()), {0}, {1})", uid, storage), conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = new MySqlCommand("SELECT MAX(rid) as id FROM receiving", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    result = reader.GetInt32("id");
                }
            }

            return result;
        }

        public bool AddRowToContentReceivingTable(int rid, string code, int count)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("INSERT INTO contentrec VALUE (NULL, {0}, (SELECT pid FROM cabinet3 where code = \"{1}\"), {2}, 3)", rid, code, count), conn);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    cmd.Dispose();
                }
                cmd.Dispose();
            }
            return true;
        }

        public int AddRowToOrdersTable(string uid, string did)
        {
            int result = 0;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("INSERT INTO orders VALUE (NULL, NOW(), {0}, {1})", uid, did), conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = new MySqlCommand("SELECT MAX(oid) as id FROM orders", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    result = reader.GetInt32("id");
                }
            }

            return result;
        }

        public bool AddRowToContentOrderTable(int rid, string code, int count)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("INSERT INTO ordercontent VALUE (NULL, {0}, (SELECT pid FROM cabinet4 where code = \"{1}\"), {2})", rid, code, count), conn);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    cmd.Dispose();
                }
                cmd.Dispose();
            }
            return true;
        }


        public List<Orders.OrderViewModel> GetCurrentOrdersGeneral(string uid)
        {
            List<Orders.OrderViewModel> result = new List<Orders.OrderViewModel>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("SELECT orders.oid as 'id', DATE(orders.dt) as 'dt', CONCAT_WS(', ', departments.city, departments.street, departments.housenumber) as 'department' FROM orders INNER JOIN departments ON departments.did = orders.did WHERE orders.uid = {0}", uid), conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Orders.OrderViewModel()
                        {
                            oid = reader.GetInt32("id"),
                            dt = reader.GetString("dt").Substring(0, 10),
                            departmentName = reader.GetString("department"),
                            components = new List<Orders.OrderComponent>()
                        });
                        GetFullOrders(reader.GetInt32("id"), result);
                    }
                }
                cmd.Dispose();
            }
            return result;
        }

        private void GetFullOrders(int id, List<Orders.OrderViewModel> obj)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(string.Format("SELECT cabinet4.pname as 'name', ordercontent.pquantity as 'quantity' FROM ordercontent INNER JOIN cabinet4 ON cabinet4.pid = ordercontent.pid WHERE ordercontent.oid = {0}", id), conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        obj[obj.Count - 1].components.Add(new Orders.OrderComponent()
                        {
                            pname = reader.GetString("name"),
                            pquantity = reader.GetInt32("quantity")
                        });

                    }
                }
                cmd.Dispose();
            }
        }

        public List<WebAPI.Models.Departments.DepartmentModel> GetAllDepartments()
        {
            List<WebAPI.Models.Departments.DepartmentModel> result = new List<WebAPI.Models.Departments.DepartmentModel>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT did, dname, city FROM departments WHERE dactive = true", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new WebAPI.Models.Departments.DepartmentModel()
                        {
                            did = reader.GetInt32("did"),
                            dname = reader.GetString("dname"),
                            city = reader.GetString("city")
                        });
                    }
                }
                cmd.Dispose();
            }
            return result;
        }

        public bool HideElement(string table, string pid)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(string.Format("UPDATE TABLE{0} SET active = FALSE WHERE pid = {1}", table, pid), conn);
                    cmd.ExecuteNonQueryAsync();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}