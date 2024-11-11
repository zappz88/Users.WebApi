using Microsoft.AspNetCore.Mvc;
using Common.Database;
using Common.Model;
using Common.Encryption;
using Newtonsoft.Json;
using Common.Dao;
using System.Net;

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
                User user = UserDao.GetUserByCredentials(credential.Username, credential.Password);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(user)));
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
                User user = UserDao.GetUserByCredentials(credential.Username, credential.Password);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(user)));
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
                User user = UserDao.GetUserByCredentials(userCredential.Username, userCredential.Password);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(user)));
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
                int userId = UserDao.GetUserIDByCredentials(credential.Username, credential.Password);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(userId)));
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
                int userId = UserDao.GetUserIDByCredentials(credential.Username, credential.Password);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(userId)));
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
                int userId = UserDao.GetUserIDByCredentials(userCredential.Username, userCredential.Password);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(userId)));
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
                int rowsAffected = UserDao.InsertUser(user);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(user)));
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
                int rowsAffected = UserDao.UpdateUser(user);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(rowsAffected)));
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
                int rowsAffected = UserDao.DeleteUser(user);
                return Ok(new JsonPayload(SerializeJsonAndEncrypt(rowsAffected)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string SerializeJsonAndEncrypt<T>(T val) {
            string json = JsonConvert.SerializeObject(val);
            string encrypted = IEncryptor.Encrypt(json);
            return encrypted;
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
