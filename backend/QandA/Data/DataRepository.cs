using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QandA.Data;
using QandA.Data.Models;

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
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponse>(DatabaseQuery.QuestionGetManyBySearchQuery,
                    new { Search = search });
            }
        }

        public IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponse>(DatabaseQuery.QuestionGetUnansweredQuery);
            }
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestionsWithAnswers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var questions = connection.Query<QuestionGetManyResponse>(DatabaseQuery.QuestionGetManyQuery);
                foreach (var question in questions)
                {
                    question.Answers = connection.Query<AnswerGetResponse>(DatabaseQuery.AnswerGetByQuestionIdQuery,
                        new { QuestionId = question.QuestionId }).ToList();
                }
                return questions;
            }
        }

        public QuestionGetSingleResponse GetQuestion(int questionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var question = connection.QueryFirstOrDefault<QuestionGetSingleResponse>(
                    DatabaseQuery.QuestionGetSingleQuery, new { QuestionId = questionId });
                if (question != null)
                {
                    question.Answers = connection.Query<AnswerGetResponse>(DatabaseQuery.AnswerGetByQuestionIdQuery,
                        new { QuestionId = questionId });
                }
                return question;
            }
        }

        public bool QuestionExists(int questionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirst<bool>(DatabaseQuery.QuestionExistsQuery,
                    new { QuestionId = questionId });
            }
        }

        public AnswerGetResponse GetAnswer(int answerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<AnswerGetResponse>(DatabaseQuery.AnswerGetByAnswerIdQuery,
                    new { AnswerId = answerId });
            }
        }

        public QuestionGetSingleResponse PostQuestion(QuestionPostFullRequest question)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var questionId = connection.QueryFirst<int>(DatabaseQuery.QuestionPostQuery, question);
                return GetQuestion(questionId);
            }
        }

        public QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(DatabaseQuery.QuestionPutQuery,
                    new { QuestionId = questionId, question.Title, question.Content });
                return GetQuestion(questionId);
            }
        }

        public void DeleteQuestion(int questionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(DatabaseQuery.QuestionDeleteQuery, new { QuestionId = questionId });
            }
        }

        public AnswerGetResponse PostAnswer(AnswerPostFullRequest answer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirst<AnswerGetResponse>(DatabaseQuery.AnswerPostQuery, answer);
            }
        }
    }
}
