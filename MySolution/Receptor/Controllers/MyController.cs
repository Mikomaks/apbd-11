using Microsoft.AspNetCore.Mvc;
using Receptor.Models.DTOs;
using Receptor.Services;

namespace Receptor.Controllers;


[Route("api/")]
[ApiController]
public class MyController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public MyController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> addReceipt([FromBody] PrescriptionAddDto request)
    {
        try
        {
            await _prescriptionService.AddPrescription(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _prescriptionService.GetPrescriptionsFromID(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
}