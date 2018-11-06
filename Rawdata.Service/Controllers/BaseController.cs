using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Rawdata.Service.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        public const string REGISTER_USER = "RegisterUser";
        public const string SIGN_IN = "SignIn";

        public const string GET_AUTHOR_BY_ID = "GetAuthorById";

        public const string GET_COMMENT_BY_ID = "GetCommentById";
        public const string TOGGLE_MARKED_COMMENT = "ToggleMarkedComment";

        public const string GET_QUESTION_BY_ID = "GetQuestionById";
        public const string QUERY_QUESTIONS = "QueryQuestions";

        public const string GET_ANSWER_BY_ID = "GetAnswerById";

        public const string GET_USER_BY_ID = "GetUserById";

        protected readonly IMapper DtoMapper;

        public BaseController(IMapper dtoMapper)
        {
            DtoMapper = dtoMapper;
        }

        [NonAction]
        public int? GetUserId()
        {
            var claim = User.FindFirst("id")?.Value;

            if (int.TryParse(claim, out int userId)) {
                return userId;
            }

            return null;
        }
    }
}
