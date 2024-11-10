using Microsoft.AspNetCore.Mvc;
using Common.Database;
using Common.Model;
using Common.Encryption;
using Newtonsoft.Json;
using Common.Dao;

namespace Users.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDao UserDao;
        private readonly IEncryptor IEncryptor;

        public UsersController()
        {
            UserDao = new UserDao(DbContextDaoType.SqlServer, "Server=localhost\\SQLEXPRESS;Database=BetaTest;Trusted_Connection=True;");
            IEncryptor = EncryptorFactory.GetEncryptor(EncryptorType.BASIC);
        }

        [HttpPost(Name = "GetUserByCredentials")]
        public IActionResult GetUserByCredentials([FromBody] JsonPayload credentialJson)
        {
            try
            {
                Credential credential = DecryptAndDeserializeJson<Credential>(credentialJson.Data);
                return Ok(UserDao.GetUserByCredentials(credential.Username, credential.Password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetUserByCredential")]
        public IActionResult GetUserByCredential([FromBody] JsonPayload credentialJson)
        {
            try
            {
                Credential credential = DecryptAndDeserializeJson<Credential>(credentialJson.Data);
                return Ok(UserDao.GetUserByCredentials(credential.Username, credential.Password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetUserByUserCredential")]
        public IActionResult GetUserByUserCredential([FromBody] JsonPayload userCredentialJson)
        {
            try
            {
                UserCredential userCredential = DecryptAndDeserializeJson<UserCredential>(userCredentialJson.Data);
                return Ok(UserDao.GetUserByCredentials(userCredential.Username, userCredential.Password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetUserIDByCredentials")]
        public IActionResult GetUserIDByCredentials([FromBody] JsonPayload credentialJson)
        {
            try
            {
                Credential credential = DecryptAndDeserializeJson<Credential>(credentialJson.Data);
                return Ok(UserDao.GetUserIDByCredentials(credential.Username, credential.Password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetUserIDByCredential")]
        public IActionResult GetUserIDByCredential([FromBody] JsonPayload credentialJson)
        {
            try
            {
                Credential credential = DecryptAndDeserializeJson<Credential>(credentialJson.Data);
                return Ok(UserDao.GetUserIDByCredential(credential));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetUserIDByUserCredential")]
        public IActionResult GetUserIDByUserCredential([FromBody] JsonPayload userCredentialJson)
        {
            try
            {
                UserCredential userCredential = DecryptAndDeserializeJson<UserCredential>(userCredentialJson.Data);
                return Ok(UserDao.GetUserIDByUserCredential(userCredential));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "InsertUser")]
        public IActionResult InsertUser([FromBody] JsonPayload userJson)
        {
            try
            {
                User user = DecryptAndDeserializeJson<User>(userJson.Data);
                return Ok(UserDao.InsertUser(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "UpdateUser")]
        public IActionResult UpdateUser(JsonPayload userJson)
        {
            try
            {
                User user = DecryptAndDeserializeJson<User>(userJson.Data);
                return Ok(UserDao.UpdateUser(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "DeleteUser")]
        public IActionResult DeleteUser([FromBody] JsonPayload userJson)
        {
            try
            {
                User user = DecryptAndDeserializeJson<User>(userJson.Data);
                return Ok(UserDao.DeleteUser(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private T DecryptAndDeserializeJson<T>(string payload) 
        {
            T result = default;
            string decrypted = IEncryptor.Decrypt(payload);
            result = JsonConvert.DeserializeObject<T>(decrypted);
            return result;
        }
    }
}
