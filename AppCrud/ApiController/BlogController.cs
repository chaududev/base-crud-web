using AppCrud.Interface;
using AppCrud.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCrud.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _repository;
        public BlogController(IBlogRepository repository)
        {
            this._repository = repository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var results = await _repository.FillAllBlogAsync();

                return Ok(results);
            }catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
           // return Ok();
        }
    }
}
