using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Department;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Department;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        IMainRepository<DepartmentEntity> DepartmentRepo;
        Result Result;
        public DepartmentController(IMainRepository<DepartmentEntity> deptRepo)
        {
            DepartmentRepo = deptRepo;
            Result = new Result();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var depts = await DepartmentRepo.Get();
                Result.Data = depts.Select(i => i.ToDeptViewModel());
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            try
            {
                var dept = await DepartmentRepo.Get(id);
                if (dept == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "There is No Department Has This ID";
                }
                else
                {
                    Result.IsSuccess = true;
                    Result.Data = dept;
                    Result.Message = "Sucess";
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddDepartmentViewModel departmentViewModel)
        {
            try
            {
                var dept = await DepartmentRepo.Add(departmentViewModel.ToDeprtmentModel());
                if (dept == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "Department Has Not Been Added";
                }
                else
                {
                    Result.IsSuccess = true;
                    Result.Data = dept;
                    Result.Message = "Department Has Been Added Successfully";
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody]GetEditDepartmentViewModel getEditDepartmentViewModel )
        {
            try
            {
                var dept = await DepartmentRepo.Get(getEditDepartmentViewModel.ID);
                if (dept == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "There is No Department Has This ID";
                }
                else
                {
                    dept.DepartmentName = getEditDepartmentViewModel.DepartmentName;
                    dept = await DepartmentRepo.Update(dept);
                    Result.IsSuccess = true;
                    Result.Data = dept;
                    Result.Message = "Department Data Has Been Updated Successfully";
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }
        }
    }
}
