using System;
using Sitecore.TechnicalChallenge.WebApi.Repository;
using System.Web.Http;

namespace Sitecore.TechnicalChallenge.WebApi.Controllers
{
    public class MemberController : ApiController
    {
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetMember(string username)
        {
            try
            {
                var member = new MemberRepository().GetMemberByUsername(username);
                return Ok(member);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}