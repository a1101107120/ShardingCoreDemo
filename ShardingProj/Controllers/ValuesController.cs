using Microsoft.AspNetCore.Mvc;
using ShardingProj.Func;
using ShardingProj.DbContext;
using ShardingProj.DbModel;

namespace ShardingProj.Controllers;

[Route("api/[controller]")]
public class ValuesController : Controller
{
    private readonly MyDbContext _myDbContext;

    public ValuesController(MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
    }

    [HttpGet, Route("GetOrder")]
    public async Task<IActionResult> GetOrder(DateTime dateTime)
    {

        //_myDbContext.Add(order);
        //_myDbContext.Udpate(order);
        //_myDbContext.Remove(order);
        //_myDbContext.SaveChanges();
        var order = _myDbContext.Order.Where(r => r.CreateTime > DateTimeHelper.ConvertDateTimeToLong(dateTime)).ToList();
        return Ok(order);
    }
    [HttpGet, Route("GetProduct")]
    public async Task<IActionResult> GetProduct(string id)
    {


        //_myDbContext.Add(order);
        //_myDbContext.Udpate(order);
        //_myDbContext.Remove(order);
        //_myDbContext.SaveChanges();
        var order = _myDbContext.OrderProduct.Where(r => r.Id == id).ToList();
        return Ok(order);
    }
    [HttpPost, Route("AddOrder")]
    public async Task<IActionResult> Add(Order request)
    {
        var entityEntry = await _myDbContext.Order.AddAsync(request);
        var saveChangesAsync = await _myDbContext.SaveChangesAsync();
        return Ok(saveChangesAsync);
    }
    [HttpPost, Route("AddProduct")]
    public async Task<IActionResult> AddProduct(OrderProduct request)
    {
        var entityEntry = await _myDbContext.OrderProduct.AddAsync(request);
        var saveChangesAsync = await _myDbContext.SaveChangesAsync();
        return Ok(saveChangesAsync);
    }
}