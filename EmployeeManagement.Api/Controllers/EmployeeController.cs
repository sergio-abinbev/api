using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Services;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(EmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        /// <param name="createDto">Dados para criação do funcionário.</param>
        /// <returns>O funcionário criado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CreateEmployee: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Attempting to create employee with email: {Email}", createDto.Email);
                var createdEmployee = await _employeeService.CreateEmployeeAsync(createDto);
                _logger.LogInformation("Employee created successfully with ID: {EmployeeId}", createdEmployee.Id);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Business validation error during employee creation: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during employee creation.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        /// <summary>
        /// Obtém um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <returns>O funcionário encontrado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            _logger.LogInformation("Attempting to get employee by ID: {EmployeeId}", id);
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID: {EmployeeId} not found.", id);
                return NotFound($"Employee with ID '{id}' not found.");
            }
            _logger.LogInformation("Employee with ID: {EmployeeId} retrieved successfully.", id);
            return Ok(employee);
        }

        /// <summary>
        /// Obtém todos os funcionários.
        /// </summary>
        /// <returns>Uma lista de funcionários.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeResponseDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEmployees()
        {
            _logger.LogInformation("Attempting to retrieve all employees.");
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                _logger.LogInformation("Retrieved {Count} employees.", ((List<EmployeeResponseDto>)employees).Count);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving all employees.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        /// <summary>
        /// Atualiza um funcionário existente.
        /// </summary>
        /// <param name="id">ID do funcionário a ser atualizado.</param>
        /// <param name="updateDto">Dados para atualização do funcionário.</param>
        /// <returns>No content ou bad request/not found.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for UpdateEmployee for ID: {EmployeeId}", id);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Attempting to update employee with ID: {EmployeeId}", id);
                await _employeeService.UpdateEmployeeAsync(id, updateDto);
                _logger.LogInformation("Employee with ID: {EmployeeId} updated successfully.", id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Business validation or not found error during employee update for ID: {EmployeeId}: {Message}", id, ex.Message);
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during employee update for ID: {EmployeeId}.", id);
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        /// <summary>
        /// Deleta (desativa) um funcionário.
        /// </summary>
        /// <param name="id">ID do funcionário a ser deletado.</param>
        /// <returns>No content ou not found.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete employee with ID: {EmployeeId}", id);
                await _employeeService.DeleteEmployeeAsync(id);
                _logger.LogInformation("Employee with ID: {EmployeeId} deleted (deactivated) successfully.", id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Not found error during employee deletion for ID: {EmployeeId}: {Message}", id, ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during employee deletion for ID: {EmployeeId}.", id);
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}