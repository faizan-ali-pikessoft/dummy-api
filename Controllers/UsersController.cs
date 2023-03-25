using DataService.EService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IOptions<ConnectionStrings> _config;
        private readonly IEmployeeRepository _employeeRepository;
        public UsersController(IOptions<ConnectionStrings> config, IEmployeeRepository employeeRepository) 
        {
            _config = config;
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        [Route("GetValue")]
        public Object GetValue()
        {
            string name = "Faizan Ali";
            return new { data = name };
        }
        [Authorize]
        [HttpGet]
        [Route("GetUserInfo")]
        public Object GetUserInfo()
        {
            string name = "Faizan Test Authentication";
            return new { data = name };
        }
        [Route("GetUserToken")]
        public Object GetUserToken([FromBody] UserInfo userInfo)
        {
            var user = new UserInfo();
            try
            {
                TokenProvider _tokenProvider = new TokenProvider();
                // Get Token
                var response = _tokenProvider.LoginUser(userInfo);
                if (response != null)
                {
                    _employeeRepository.GetLoginUser(userInfo.UserId, userInfo.Password);
                    user.Token = response.Token;
                    return new { data = user };
                }
                else
                {
                    return new { data = "User is not logged in." };
                }
            }
            catch (Exception ex)
            {
                return new { data = ex.Message.ToString() };
            }
        }
    }
}
