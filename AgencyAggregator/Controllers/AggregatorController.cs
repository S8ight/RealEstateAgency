using AgencyAggregator.Interfaces;
using AgencyAggregator.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgencyAggregator.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AggregatorController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAdvertService _advertService;
    private readonly IDiscountService _discountService;

    public AggregatorController(IUserService userService, IAdvertService advertService, IDiscountService discountService)
    {
        _userService = userService;
        _advertService = advertService;
        _discountService = discountService;
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserInformationModel>> GetInformation(string userId)
    {
        try
        {
            var adverts = await _advertService.GetAdverts(userId);
            var user = await _userService.GetUser(userId);
            var discountList = new List<DiscountModel>();

            foreach (var advert in adverts)
            {
                var result = await _discountService.CheckForDiscount(advert.AdvertID);
                discountList.Add(result);
            }

            var resposne = new UserInformationModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                Email = user.Email,
                DateOfBirthd = user.DateOfBirthd,
                Created = user.Created,
                Photo = user.Photo,
                userAdverts = adverts.ToList(),
                userDiscounts = discountList
            };

            return Ok(resposne);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}