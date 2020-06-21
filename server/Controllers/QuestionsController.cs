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
    public class QuestionsController : ApiController
    {
        string connectionString = @"Data Source=|DataDirectory|\BaseData.db";

        // GET api/categories
        public IEnumerable<Category> Get()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var Categories = connection.Query<Category>("SELECT * FROM Categories");
            return Categories;
        }

        // GET api/categories/5
        [Route("api/questions/{NickName}/{CategoryId}")]
        public QuestionModel Get(string NickName, int CategoryId)
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var Questions = connection.Query<QuestionModel>("SELECT * FROM Questions WHERE Category = @curCat", new { curCat = CategoryId });
            var History = connection.Query<int>("SELECT QuestionID FROM History INNER JOIN Users ON Users.ID = History.UserID WHERE History.CategoryID = @curCat AND Users.Nickname = @curNickName"
                , new { curCat = CategoryId, curNickName = NickName });

            QuestionModel Result = new QuestionModel();
            if (History.Count() == 0)
            {
                Result = Questions.First();
                Result.Answer = "\0";
                return Result;
            }
            foreach (QuestionModel temp in Questions)
            {
                if (History.Contains(temp.Id)) continue;
                else
                {
                    Result = temp;
                    Result.Answer = "\0";
                    return Result;
                }
            }

            Result.Question = "Вы ответили на все вопросы данной категории";
            Result.Category = -1;
            return Result;
        }

        [Route("api/questions/ans/{QuestionId}")]
        public IEnumerable<Answer> Get(int QuestionId)
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var Ans = connection.Query<Answer>("SELECT * FROM Answers WHERE QuestionID = @qId", new { qId = QuestionId });
            return Ans;
        }

        // POST api/values
        [Route("api/questions/{NickName}/{QuestionId}/{CategoryId}/{Answer}")]
        public bool Get(string NickName, int QuestionId, int CategoryId, string Answer)
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();

            bool Result = false;
            var tempAns = connection.Query<string>("SELECT Answer FROM Questions WHERE Id = @qId", new { qId = QuestionId });
            string RAns = "\0";
            foreach (string grb in tempAns) RAns = grb;
            if (Answer == RAns) Result = true;

            var temp = connection.Query<int>("SELECT ID FROM Users WHERE NickName = @curNickName", new { curNickName = NickName });
            int UserId = 0;
            if (temp.Count() == 0)
            {
                connection.Query("INSERT INTO Users (NickName, GoodAns) VALUES( @curNickName, 0)", new { curNickName = NickName });
                var id = connection.Query<long>("SELECT last_insert_rowid();");

                foreach (int grb in id) UserId = grb;
            }
            foreach (int grb in temp) UserId = grb;

            if (Result)
            {
                connection.Query("INSERT INTO History (QuestionId, UserId, CategoryId) VALUES (@qId, @uId, @cId)", new { qId = QuestionId, uId = UserId, cId = CategoryId });
                connection.Query("UPDATE Users SET GoodAns = GoodAns + 1 WHERE ID = @curId", new { curId = UserId });
            }
            else
            {
                connection.Query("UPDATE Users SET GoodAns = GoodAns - 1 WHERE ID = @curId", new { curId = UserId });
            }

            return Result;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
