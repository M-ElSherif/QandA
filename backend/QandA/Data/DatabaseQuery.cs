using Microsoft.AspNetCore.Authentication;

namespace QandA.Data
{
    public struct DatabaseQuery
    {
        public static readonly string AnswerDeleteQuery = 
            "DELETE FROM Answer " +
            "WHERE AnswerID = @AnswerId;";

        public static readonly string AnswerGetByQuestionIdQuery =
            "SELECT AnswerId, QuestionId, Content, Username, Created " +
            "FROM Answer " +
            "WHERE QuestionId = @QuestionId;";

        public static readonly string AnswerPostQuery =
            "INSERT INTO Answer (QuestionId, Content, UserId, UserName, Created) " +
            "SELECT @QuestionId, @Content, @UserId, @UserName, @Created " +
            "SELECT AnswerId, Content, UserName, UserId, Created " +
            "FROM dbo.Answer " +
            "WHERE AnswerId = SCOPE_IDENTITY();";

        public static readonly string AnswerPutQuery =
            "UPDATE Answer " +
            "SET Content = @Content " +
            "WHERE AnswerId = @AnswerId " +
            "SELECT a.AnswerId, a.QuestionId, a.Content, u.UserName, a.Created " +
            "FROM dbo.Answer a " +
            "LEFT JOIN AspNetUsers u ON a.UserId = u.Id " +
            "WHERE AnswerId = @AnswerId;";

        //private static readonly string QuestionAddForLoadTestQuery = 
        //    ""

        public static readonly string QuestionGetManyQuery =
            "SELECT QuestionId, Title, Content, UserId, UserName, Created " +
            "FROM dbo.Question;";

        public static readonly string QuestionDeleteQuery =
            "DELETE " +
            "FROM Question " +
            "WHERE QuestionID = @QuestionId;";

        public static readonly string QuestionExistsQuery =
            "SELECT CASE WHEN EXISTS (SELECT QuestionId " +
            "FROM dbo.Question " +
            "WHERE QuestionId = @QuestionId) " +
            "THEN CAST(1 AS BIT) " +
            "ELSE CAST(0 AS BIT) END AS Result;";

        public static readonly string QuestionGetManyBySearchQuery =
            "SELECT QuestionId, Title, Content, UserId, UserName, Created " +
            "FROM dbo.Question " +
            "WHERE Title LIKE '%' + @Search + '%' " +
            "UNION " +
            "SELECT QuestionId, Title, Content, UserId, UserName, Created " +
            "FROM dbo.Question " +
            "WHERE Content LIKE '%' + @Search + '%';";

        public static readonly string QuestionGetManyBySearchWithPagingQuery =
            "SELECT QuestionId, Title, Content, UserId, UserName, Created " +
            "FROM " +
            "(SELECT QuestionId, Title, Content, UserId, UserName, Created " +
            "FROM dbo.Question " +
            "WHERE Title LIKE '%' + @Search + '%' " +
            "UNION " +
            "SELECT QuestionId, Title, Content, UserId, UserName, Created " +
            "FROM dbo.Question " +
            "WHERE Content LIKE '%' + @Search + '%') Sub " +
            "ORDER BY QuestionId " +
            "OFFSET @PageSize * (@PageNumber - 1) ROW " +
            "FETCH NEXT @PageSize ROWS ONLY;";

        public static readonly string QuestionGetManyWithAnswersQuery =
            "SELECT q.QuestionId, q.Title, q.Content, q.UserName, q.Created, " +
            "a.QuestionId, a.AnswerId, a.Content, a.Username, a.Created " +
            "FROM dbo.Question q " +
            "LEFT JOIN dbo.Answer a ON q.QuestionId = a.QuestionId;";

        public static readonly string QuestionGetSingleQuery =
            "SELECT QuestionId, Title, Content, UserId, Username, Created " +
            "FROM dbo.Question " +
            "WHERE QuestionId = @QuestionId;";

        public static readonly string QuestionGetUnansweredQuery =
            "SELECT QuestionId, Title, Content, UserId, UserName, Created " +
            "FROM dbo.Question q " +
            "WHERE NOT EXISTS (SELECT* " +
            "FROM dbo.Answer a " +
            "WHERE a.QuestionId = q.QuestionId);";

        public static readonly string QuestionPostQuery = 
            "INSERT INTO dbo.Question " +
            "(Title, Content, UserId, UserName, Created) " +
            "VALUES(@Title, @Content, @UserId, @UserName, @Created) " +
            "SELECT SCOPE_IDENTITY() AS QuestionId;";

        public static readonly string QuestionPutQuery =
            "UPDATE dbo.Question " +
            "SET Title = @Title, Content = @Content " +
            "WHERE QuestionID = @QuestionId;";

        public static readonly string AnswerGetByAnswerIdQuery =
            "SELECT AnswerId, Content, Username, Created " +
            "FROM dbo.Answer " +
            "WHERE AnswerId = @AnswerId;";

    }
}