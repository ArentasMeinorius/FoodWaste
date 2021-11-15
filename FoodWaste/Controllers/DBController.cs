using FoodWaste.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWaste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DBController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("product")]
        [HttpGet]
        public JsonResult Get() //wntity framework identity db 
        {
            string query = @"select 
                                id as id, 
                                name as name, 
                                expirydate as expiryDate, 
                                state as state, 
                                restaurant_id as restaurant_id, 
                                users_id as user_id 
                            from product
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource)) 
            {
                myCon.Open();
                using(NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [Route("product")]
        [HttpPost]
        public JsonResult Post(Product product)
        {
            string query = @"insert into product (id, name,expirydate, state, restaurant_id, users_id)
                             values (nextval('id_seq'), @name, @expiryDate, @state, @restaurant_id, @user_id)
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@name", product.Name);
                    myCommand.Parameters.AddWithValue("@expiryDate", product.ExpiryDate);
                    myCommand.Parameters.AddWithValue("@state", product.State.ToString());
                    myCommand.Parameters.AddWithValue("@restaurant_id", product.Restaurant_id);
                    myCommand.Parameters.AddWithValue("@user_id", (object)product.User_id ?? DBNull.Value);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added successfully");
        }
        [Route("product")]
        [HttpPut]
        public JsonResult Put(Product product)
        {
            string query = @"update product set
                                name = @name, 
                                expirydate = @expiryDate, 
                                state = @state, 
                                restaurant_id = @restaurant_id, 
                                users_id = @user_id
                             where id = @id
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", product.Id);
                    myCommand.Parameters.AddWithValue("@name", product.Name);
                    myCommand.Parameters.AddWithValue("@expiryDate", product.ExpiryDate);
                    myCommand.Parameters.AddWithValue("@state", product.State.ToString());
                    myCommand.Parameters.AddWithValue("@restaurant_id", product.Restaurant_id);
                    myCommand.Parameters.AddWithValue("@user_id", (object)product.User_id??DBNull.Value);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated successfully");
        }
        [Route("product")]
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from product 
                             where id = @id
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated successfully");
        }
        [Route("restaurant")]
        [HttpGet]
        public JsonResult GetRestaurant()
        {
            string query = @"select 
                                id as id, 
                                name as name, 
                                phonenumber as phoneNumber,  
                                users_id as user_id,
                                address as address
                            from restaurant
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [Route("restaurant")]
        [HttpPost]
        public JsonResult PostRestaurant(Restaurant restaurant)
        {
            string query = @"insert into restaurant (id, name, phonenumber, users_id, address)
                             values (nextval('id_seq'), @name, @phonenumber, @user_id, @address)
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@name", restaurant.Name);
                    myCommand.Parameters.AddWithValue("@phonenumber", restaurant.PhoneNumber);
                    myCommand.Parameters.AddWithValue("@user_id", restaurant.User_Id);
                    myCommand.Parameters.AddWithValue("@address", restaurant.Address);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added successfully");
        }
        [Route("restaurant")]
        [HttpPut]
        public JsonResult PutRestaurant(Restaurant restaurant)
        {
            string query = @"update restaurant set
                                name = @name, 
                                phonenumber = @phonenumber, 
                                users_id = @user_id,
                                address = @address
                             where id = @id
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", restaurant.Id);
                    myCommand.Parameters.AddWithValue("@name", restaurant.Name);
                    myCommand.Parameters.AddWithValue("@phonenumber", restaurant.PhoneNumber);
                    myCommand.Parameters.AddWithValue("@user_id", restaurant.User_Id);
                    myCommand.Parameters.AddWithValue("@address", restaurant.Address);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated successfully");
        }
        [Route("restaurant")]
        [HttpDelete("{id}")]
        public JsonResult DeleteRestaurant(int id)
        {
            string query = @"delete from restaurant 
                             where id = @id
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated successfully");
        }
    }
}
