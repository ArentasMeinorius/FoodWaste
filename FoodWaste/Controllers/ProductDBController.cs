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
    public class ProductDBController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProductDBController(IConfiguration configuration)
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
       // [Route("post/{value}")]
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
                    myCommand.Parameters.AddWithValue("@user_id", product.User_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added successfully");
        }
        //[Route("put/{value}")]
        [HttpPut]
        public JsonResult Put(Product product)
        {
            string query = @"update product 
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
                    myCommand.Parameters.AddWithValue("@state", product.State);
                    myCommand.Parameters.AddWithValue("@restaurant_id", product.Restaurant_id);
                    myCommand.Parameters.AddWithValue("@user_id", product.User_id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated successfully");
        }
        //[Route("delete/{value}")]
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
    }
}
