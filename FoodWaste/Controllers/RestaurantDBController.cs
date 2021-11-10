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
    public class RestaurantDBController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RestaurantDBController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("get")]
        [HttpGet]
        public JsonResult Get() 
        {
            string query = @"select 
                                id as id, 
                                name as name, 
                                phonenumber as phoneNumber,  
                                users_id as user_id 
                            from restaurant
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
        [HttpPost]
        public JsonResult Post(Restaurant restaurant)
        {
            string query = @"insert into restaurant (id, name, phonenumber, users_id)
                             values (nextval('id_seq'), @name, @phonenumber, @user_id)
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
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added successfully");
        }
        [HttpPut]
        public JsonResult Put(Restaurant restaurant)
        {
            string query = @"update restaurant 
                                name = @name, 
                                phonenumber = @phonenumber, 
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
                    myCommand.Parameters.AddWithValue("@id", restaurant.Id);
                    myCommand.Parameters.AddWithValue("@name", restaurant.Name);
                    myCommand.Parameters.AddWithValue("@expiryDate", restaurant.PhoneNumber);
                    myCommand.Parameters.AddWithValue("@user_id", restaurant.User_Id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated successfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
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
