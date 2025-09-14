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
        public async Task<ActionResult<ApiResponseBase>> FillDatabase([FromQuery] bool isConfirmed)
        {
            if (isConfirmed)
            {
                return this.ToActionResultResult(await _dataSeeder.FillAsync());
            }
            return StatusCode((int)HttpStatusCode.BadRequest, new ApiResponseBase($"Вы должны подтвердить действие. Параметр: [{nameof(isConfirmed)}] должен быть задан как 'true'", HttpStatusCode.BadRequest));
        }

        /// <summary>
        /// Очищает базу данных от всех данных.
        /// </summary>
        /// <param name="isConfirmed">Подтверждение действия.</param>
        /// <returns></returns>
        [HttpPost("clear")]
        public async Task<ActionResult<ApiResponseBase>> ClearDatabase([FromQuery] bool isConfirmed)
        {
            if (isConfirmed)
            {
                return this.ToActionResultResult(await _dataSeeder.ClearAsync());
            }
            return StatusCode((int)HttpStatusCode.BadRequest, new ApiResponseBase($"Вы должны подтвердить действие. Параметр: [{nameof(isConfirmed)}] должен быть задан как 'true'", HttpStatusCode.BadRequest));
        }
    }
}
