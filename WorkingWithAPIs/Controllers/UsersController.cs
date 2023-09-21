using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkingWithAPIs.Data;
using WorkingWithAPIs.Models;
using WorkingWithAPIs.Models.ViewModel;

namespace WorkingWithAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly WorkingWithAPIsContext _userContext;

        public UsersController(WorkingWithAPIsContext userContext)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAll()
        {
            var usersList = await _userContext.Users.ToListAsync();
            var allUsersList = _userContext.MapUsersToView(usersList);
            return allUsersList;
        }

        /// <summary>
        /// Gets a single user by ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsersModel>> Get(int id)
        {
            var user = await _userContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UsersModel>> Create([FromBody] UsersModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _userContext.Users.Add(user);
            await _userContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsersModel user)
        {
            if (id != user.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

        /// <summary>
        /// Deletes a user.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userToDelete = await _userContext.Users.FindAsync(id);
            if (userToDelete == null)
            {
                return NotFound();
            }

            _userContext.Users.Remove(userToDelete);
            await _userContext.SaveChangesAsync();

            return NoContent();
        }

        private bool CheckIfUserExists(int id)
        {
            return _userContext.Users.Any(e => e.Id == id);
        }
    }
}
