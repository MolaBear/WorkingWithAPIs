using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using WorkingWithAPIs.Data;
using WorkingWithAPIs.Models;
using WorkingWithAPIs.Models.ViewModel;

namespace WorkingWithAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly WorkingWithAPIsContext _userContext;

        public UsersController(WorkingWithAPIsContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet("[Action]/")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers()
        {
            if(_userContext.Users == null)
            {
                return NotFound();
            }
            var usersList = _userContext.Users.ToList();
            var allUsersList  = _userContext.MapUsersToView(usersList);
            return allUsersList;
        }


        [HttpPost("[Action]/")]
        public async Task<ActionResult<UsersModel>> SaveUser(UsersModel users)
        {
            
            if (_userContext.Users == null)
            {
                return Problem("Entity set 'UserContext.UserModel' is null.");
            }
            _userContext.Users.Add(users);
            await _userContext.SaveChangesAsync();


            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }


        [HttpPost("[Action]/")]

        public async Task<ActionResult<IEnumerable<UserViewModel>>> SaveMultUsers(IEnumerable<UsersModel> usersList)
        {
            if (usersList == null || !usersList.Any())
            {
                return Problem("No user data provided.");
            }

            if (_userContext.Users == null)
            {
                return Problem("Entity set 'UserContext.UserModel' is null.");
            }

            _userContext.Users.AddRange(usersList);
            await _userContext.SaveChangesAsync();

            var listOfUsersView = usersList.Select(usersList => new UserViewModel()).ToList();

            return CreatedAtAction("GetUsers", usersList);
        }


        [HttpPut("[Action]/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UsersModel user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _userContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _userContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckIfUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("[Action]/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_userContext.Users == null)
            {
                return NotFound();
            }
            var pako = await _userContext.Users.FindAsync(id);
            if (pako == null)
            {
                return NotFound();
            }

            _userContext.Users.Remove(pako);
            await _userContext.SaveChangesAsync();

            return NoContent();
        }


        private bool CheckIfUserExists(int id)
        {
            return _userContext.Users?.Any(e => e.Id == id) ?? false;
        }
    }
}
