using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using PermissionsN5.Core.DTO;
using PermissionsN5.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace PermissionsN5.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsService _permissionsService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IPermissionsService permissionService, ILogger<PermissionsController> logger)
        {
            _permissionsService = permissionService;
            _logger = logger;
        }

        /// <summary>
        /// Get all Permissions
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            _logger.LogInformation("[ENDPOINT] api/Permissions => GetAllPermissions");
            List<PermissionsDTO> allPermission = await _permissionsService.GetAll();
            return Ok(new CustomResponse<List<PermissionsDTO>>(allPermission, "List of permissions"));
        }

        /// <summary>
        /// Request Permission by Permissions.Id
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RequestPermission(ModPermissionDTO newPermission)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo enviado no corresponde con el modelo de insercion");

            _logger.LogInformation("[ENDPOINT] api/Permissions => RequestPermission - PermissionsDTO = {0}", JsonSerializer.Serialize(newPermission));

            PermissionsDTO permDto = await _permissionsService.Add(newPermission);

            return Ok(new CustomResponse<bool>(permDto != null, "Status request permission"));
        }

        /// <summary>
        /// Modify Permission by Permissions.Id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyPermissionById(int id ,ModPermissionDTO modPermission)
        {
            if (!ModelState.IsValid)
                return BadRequest("Modelo enviado no corresponde con el modelo de actualizacion");


            _logger.LogInformation(
                "[ENDPOINT] api/Permissions => ModifyPermissionById - Id = {0} - Data = {1}",
                id, JsonSerializer.Serialize(modPermission));

            PermissionsDTO permDto = await _permissionsService.Update(id, modPermission);

            return Ok(new CustomResponse<bool>(permDto != null, "Status modify permission"));
        }

    }
}
