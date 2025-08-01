using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RODXUsers.Data;
using RODXUsers.DTO;

namespace RODXUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext userDbContext;
        
        public UsersController(UserDbContext userDbContext)
        {
            this.userDbContext = userDbContext;
        }

        [HttpGet("getallrols")]
        public async Task<ActionResult<IEnumerable<RolDto>>> GetAllRols()
        {
            var rols = await userDbContext.Roles
                .Select(rol => new RolDto
                {
                    Id = rol.IdRol,
                    Rol = rol.Rol
                })
                .ToListAsync();

            return Ok(rols);
        }


        [HttpGet("getallusers")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            var users = await userDbContext.ViewUserRol
                .Select(viewRow => new UserReadDto
                {
                    Id = viewRow.Id,
                    UserName = viewRow.UserName,
                    Name = viewRow.Name,
                    IdRol = viewRow.IdRol,
                    Rol = viewRow.Rol
                })
                .ToListAsync();

            return Ok(users);
        }


        [HttpPost("newuser")]
        public async Task<ActionResult<Models.Users>> CreateUser(UserCreateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var newUser = new Models.Users
            {
                UserName = userDto.UserName.Trim().ToLower(),
                Name = userDto.Name.Trim().ToUpper(),
                FKIdRol = userDto.FkIdRol,
                Password = userDto.Password
            };

            var alreadyExist = await userDbContext.Users.AnyAsync(user => user.Name == newUser.Name || user.UserName ==newUser.UserName);

            if (alreadyExist)
            {
                return Conflict($"User already exist: \"{newUser.Name} , {newUser.UserName}\".");
            }
            
            userDbContext.Users.Add(newUser);
            await userDbContext.SaveChangesAsync();

            var userReadDto = new UserReadDto
            {
                Id = newUser.Id,
                UserName = newUser.UserName,
                Name = newUser.Name,
                IdRol = newUser.FKIdRol
            };

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, userReadDto);

        }

        [HttpGet("getbyid/{id:int}")]
        public async Task<ActionResult<Models.Users>> GetUserById(int id)
        {
            try
            {
                var user = await userDbContext.ViewUserRol.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound($"No se encontró usuario con Id: {id}.");
                }

                var userDto = new UserReadDto
                {
                    Id = user.Id,
                    UserName = user.UserName.Trim(),
                    Name = user.Name.Trim(),
                    IdRol = user.IdRol,
                    Rol = user.Rol.Trim()
                };

                return Ok(userDto);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(503, new { mensaje = "Error al acceder a la base de datos. Intenta más tarde.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpPut("updateuser/{id:int}")]        
        public async Task<ActionResult<UserReadDto>> UpdateUser(int id, UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await userDbContext.Users.FindAsync(id);

            if (existingUser == null)
            {
                return NotFound($"No se encontró usuario con Id: {id}.");
            }
           
            var duplicate = await userDbContext.Users.AnyAsync(u =>
                (u.UserName == userDto.UserName.Trim().ToLower() || u.Name == userDto.Name.Trim().ToUpper()) &&
                u.Id != id);

            if (duplicate)
            {
                return Conflict("Ya existe otro usuario con el mismo nombre o nombre de usuario.");
            }

            existingUser.UserName = userDto.UserName.Trim().ToLower();
            existingUser.Name = userDto.Name.Trim().ToUpper();
            existingUser.FKIdRol = userDto.FkIdRol;

            await userDbContext.SaveChangesAsync();

            var updatedDto = new UserReadDto
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                Name = existingUser.Name,
                IdRol = existingUser.FKIdRol
            };

            return Ok(updatedDto);
        }

        [HttpDelete("deleteuser/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await userDbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound($"No se encontró usuario con Id: {id}.");
            }

            userDbContext.Users.Remove(user);
            await userDbContext.SaveChangesAsync();

            return Ok($"Usuario con Id {id} eliminado correctamente.");
        }

        [HttpPut("updatepassword/{id:int}")]
        public async Task<IActionResult> UpdatePassword(int id, PasswordUpdateDto dto)
        {
            var user = await userDbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"No se encontró usuario con Id: {id}.");
            }

            user.Password = dto.NewPassword; // Aquí podrías aplicar hashing si lo deseas
            await userDbContext.SaveChangesAsync();

            return Ok("Contraseña actualizada correctamente.");
        }
    }
}
