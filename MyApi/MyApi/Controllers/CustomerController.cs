using Microsoft.AspNetCore.Mvc;
using MyApi.Data;
using MyApi.Models;
using System.Data;

namespace MyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationContext context;
        public CustomerController(ApplicationContext context) 
        {
            this.context = context;
        }
        [HttpGet] 
        [Route("GetAllCustomers")]
        public IActionResult GetAllCustomers() 
        {
            var data=context.customers.ToList();
            if(data.Count()==0)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }
        [HttpGet]
        [Route("GetCustomerById/{id}")]
        public IActionResult GetCustomerById(int id)
        {
            if(id==0)
            {
                return NotFound();
            }
            else
            {
                var data = context.customers.Where(e => e.Id == id).SingleOrDefault();
                if(data==null)
                {
                    return BadRequest(data);
                }
                else
                {
                    return Ok(data);
                }
            }
        }
        [HttpPost]
        [Route("AddCustomer")]
        public IActionResult AddCustomer([FromBody] Customer model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
               // context.customers.Add(model);
                var data = new Customer
                {
                    Name = model.Name,
                    Gender = model.Gender,
                    IsActive = model.IsActive
                };
                context.customers.Add(data);
                context.SaveChanges();
                return Ok(data);
            }
        }
        [HttpPut]
        [Route("UpdateCustomer")]

        public IActionResult UpdateCustomer([FromBody] Customer model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var data=context.customers.Where(e => e.Id==model.Id).SingleOrDefault();
                if(data ==null)
                {
                    return BadRequest(data);
                }
                else
                {
                    data.Name = model.Name;
                    data.Gender = model.Gender;
                    data.IsActive = model.IsActive;
                   // var newdata = new Customer()
                   // {
                    //    Id = model.Id,
                    //    Name = model.Name,
                    //    Gender = model.Gender,
                    //    IsActive = model.IsActive
                  //  };
                    context.customers.Update(data);
                    context.SaveChanges();
                    return Ok(data);
                }
            }
        }

        [HttpDelete]
        [Route("DeleteCustomer/{id}")]

        public IActionResult DeleteCustomer(int id)
        {
            if(id !=0)
            {
                var data=context.customers.Where(e =>e.Id==id).SingleOrDefault();
                if(data==null)
                {
                    return BadRequest(data);
                }
                else
                {
                    context.customers.Remove(data);
                    context.SaveChanges();

                }
            }
            else
            {
                return BadRequest(ModelState);
            }
           return Ok("Record has been successfully deleted from database");
        }
    }
}
