using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Department;
using Models.ProductCategory;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Category;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        IMainRepository<CategoryEntity> CatRepo;
        IMainRepository<DepartmentEntity> DeptRepo;
        Result Result;
        public CategoryController(IMainRepository<CategoryEntity> mainRepository,
            IMainRepository<DepartmentEntity> deptRepo)
        {
            CatRepo = mainRepository;
            DeptRepo = deptRepo;
            Result = new Result();
        }
        [HttpGet]
        public async Task<Result> Get()
        {
            var allCats = await CatRepo.Get();
            Result.Data = allCats.Select(i => i.ToViewModel());
            return Result;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var cat = await CatRepo.Get(id);
            if (cat == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There IS No Category Has This ID";
            }
            else
            {
                Result.IsSuccess = true;
                Result.Data = cat.ToViewModel();
                Result.Message = "Success";
            }
            return Ok(Result);
        }
        [HttpGet("Department/{id}")]
        public async Task<IActionResult> GetCatsByDeptID(string id)
        {
            var department = await DeptRepo.Get(id);
            if (department == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There IS No Department Has This ID";
            }
            else
            {
                var allCats = await CatRepo.Get();
                Result.IsSuccess = true;
                Result.Data = allCats.Where(i => i.DepartmentID == id)
                    .Select(i => i.ToViewModel());
                Result.Message = "These All Categories In This Deparmtent";
            }
            return Ok(Result);
        }
        [HttpPost()]
        public async Task<IActionResult> Post(AddCategoryViewModel categoryViewModel)
        {
            // map vm to model
            var cat = categoryViewModel.ToCategoryModel();
            // get department entity by dept id and assign it to cat
            cat.Department = await DeptRepo.Get(categoryViewModel.DepartmentID);
            if (cat.Department == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "Cannot Find Department With This ID";
            }
            else
            {
                // add category into cat table in database
                var catAdd = await CatRepo.Add(cat);

                Result.IsSuccess = true;

                Result.Data = new
                {
                    ID = catAdd.ID,
                    CategoryName = catAdd.CategoryName,
                    ImageURL = catAdd.ImageURL,
                };
                Result.Message = "Category Has Been Added Successfully";
            }
            return Ok(Result);
        }
        [HttpPut()]
        public async Task<IActionResult> Edit(GetEditCategoryViewModel getEditCategoryViewModel)
        {
            // get cat
            var cat = await CatRepo.Get(getEditCategoryViewModel.ID);
            if (cat == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There is No Category Has This ID";
            }
            else
            {
                cat.CategoryName = getEditCategoryViewModel.CategoryName;
                cat.DepartmentID = getEditCategoryViewModel.DepartmentID;
                cat.ImageURL = getEditCategoryViewModel.ImageURL;
                cat = await CatRepo.Update(cat);
                Result.IsSuccess = true;
                Result.Data = cat;
                Result.Message = "Category Data Has Been Updated Successfully";
            }
            return Ok(Result);
        }
    }
}
