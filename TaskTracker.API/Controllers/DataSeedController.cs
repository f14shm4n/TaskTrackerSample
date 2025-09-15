using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskTracker.API.Application;
using TaskTracker.API.Application.Services;

namespace TaskTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route(RootRoute)]
    [Produces("application/json")]
    public class DataSeedController : ControllerBase
    {
        public const string RootRoute = "seeder";

        private readonly IDataSeeder _dataSeeder;

        public DataSeedController(IDataSeeder dataSeeder)
        {
            _dataSeeder = dataSeeder;
        }

        /// <summary>
        /// Заполняет базу данных тестовым набором данных.
        /// </summary>
        /// <param name="isConfirmed">Подтверждение действия.</param>
        /// <returns></returns>
        [HttpPost("fill")]
        public async Task<ActionResult<ApiResponseValue>> FillDatabase([FromQuery] bool isConfirmed)
        {
            if (isConfirmed)
            {
                return this.ToActionResultResult(await _dataSeeder.FillAsync());
            }
            HttpContext.Features.Set(new ApiRequestErrorFeature($"Вы должны подтвердить действие. Параметр: [{nameof(isConfirmed)}] должен быть задан как 'true'"));
            return BadRequest();
        }

        /// <summary>
        /// Очищает базу данных от всех данных.
        /// </summary>
        /// <param name="isConfirmed">Подтверждение действия.</param>
        /// <returns></returns>
        [HttpPost("clear")]
        public async Task<ActionResult<ApiResponseValue>> ClearDatabase([FromQuery] bool isConfirmed)
        {
            if (isConfirmed)
            {
                return this.ToActionResultResult(await _dataSeeder.ClearAsync());
            }
            HttpContext.Features.Set(new ApiRequestErrorFeature($"Вы должны подтвердить действие. Параметр: [{nameof(isConfirmed)}] должен быть задан как 'true'"));
            return BadRequest();
        }
    }
}
