using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QandA
{
    public class DataRepository: IDataRepository
    {

        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponse>(DatabaseQuery.QuestionGetManyQuery);
            }
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions()
        {
            throw new NotImplementedException();
        }

        public QuestionGetSingleResponse GetQuestion(int questionId)
        {
            throw new NotImplementedException();
        }

        public bool QuestionExists(int questionId)
        {
            throw new NotImplementedException();
        }

        public AnswerGetResponse GetAnswer(int answerId)
        {
            throw new NotImplementedException();
        }
    }
}
