using Dapper;
using server.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace server.Controllers
{
        public class UsersController : ApiController
        {
            string connectionString = @"Data Source=|DataDirectory|\BaseData.db";

            // GET api/users
            public IEnumerable<User> Get()
            {
                var connection = new SQLiteConnection(connectionString);
                connection.Open();

                var UsersList = connection.Query<User>("SELECT * FROM Users");
                var Result = UsersList.OrderByDescending(cUser => cUser.GoodAns);
                return Result;
            }
        }
    }
