using Sitecore.TechnicalChallenge.WebApi.Models;
using Sitecore.TechnicalChallenge.WebApi.Repository;
using System;
using System.Web.Http;

namespace Sitecore.TechnicalChallenge.WebApi.Controllers
{
    public class AccountController : ApiController
    {
        public IHttpActionResult Login(LoginModel data)
        {
            try
            {
                var result = new MemberRepository().ValidateLogin(data.Username, data.Password);
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        public IHttpActionResult Register(RegistrationModel data)
        {
            try
            {
                var result = new MemberRepository().RegisterMember(data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
