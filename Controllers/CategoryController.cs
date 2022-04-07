using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEnterprise_mssql.Api.Dtos;
using WebEnterprise_mssql.Api.Models;
using WebEnterprise_mssql.Api.Repository;

namespace WebEnterprise_mssql.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/Category
    public class CategoryController : ControllerBase 
    {
        private readonly IRepositoryWrapper repo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public CategoryController(
            IRepositoryWrapper repo, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        //Post Create Cate Tag
        [HttpPost]
        [Route("CreateTag")]
        public IActionResult CreateTask(string CategoryName) {
            var newCate = new Categories();
            newCate.CategoryName = CategoryName;
            repo.Categories.Create(newCate);
            repo.Save();
            return Ok();
        }

        //GET get all cate tag
        [HttpGet]
        [Route("AllTag")]
        public async Task<IActionResult> GetAllCateTagAsync() {
            var listCate = await repo.Categories.FindAll().ToListAsync();
            return Ok(listCate);
        }

        //Post Add Cate Tag to Post
        [HttpPost]
        [Route("AddTagToPost")]
        public async Task<IActionResult> AddCateTagToPostAsync(PostCateGoryDto postCategoryDto) {
            var cate = await repo.Categories.FindByCondition(x => x.CategoryId.Equals(postCategoryDto.CategoryId)).FirstOrDefaultAsync();
            cate.PostId = Guid.Parse(postCategoryDto.PostId);
            repo.Categories.Update(cate);
            repo.Save();

            return Ok($"Category Tag {cate.CategoryName} added to post successfully!");

        }
    }
}