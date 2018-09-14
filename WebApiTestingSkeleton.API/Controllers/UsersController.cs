using System;
using Microsoft.AspNetCore.Mvc;
using WebApiTestingSkeleton.API.DTO;
using WebApiTestingSkeleton.Core.Models;
using WebApiTestingSkeleton.Core.Repositories;

namespace WebApiTestingSkeleton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        // GET api/users
        [HttpGet]
        public IActionResult Get()
        {
            var results = _usersRepository.GetAll();
            return Ok(results);
        }

        // GET api/users/5
        [HttpGet, Route("{id:guid}", Name = "user-details")]
        public IActionResult Get(Guid id)
        {
            var user = _usersRepository.GetById(id);
            if (user == Core.Models.User.NullUser)
                return NotFound();
            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody] UpserUserDto dto)
        {
            var newUser = _usersRepository.Upsert(new User(Guid.NewGuid(), dto.Fullname, dto.Email));
            return CreatedAtRoute("user-details", new { id = newUser.Id }, newUser.Id);
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] UpserUserDto dto)
        {
            return Ok(id);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!_usersRepository.Remove(id)) 
                return BadRequest();
            return Ok();
        }
    }
}
