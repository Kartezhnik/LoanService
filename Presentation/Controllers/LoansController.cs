using Application.Interactors.Loans.Commands.CreateLoan;
using Application.Interactors.Loans.Commands.ToggleLoanStatus;
using Application.Interactors.Loans.Queries.GetLoans;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly CreateLoanCommandHandler _createLoanCommandHandler;
        private readonly ToggleLoanCommandHandler _toggleLoanCommandHandler;
        private readonly GetLoansQueryHandler _getLoansQueryHandler;

        public LoansController(
            CreateLoanCommandHandler createLoanCommandHandler,
            ToggleLoanCommandHandler toggleLoanCommandHandler,
            GetLoansQueryHandler getLoansQueryHandler)
        {
            _createLoanCommandHandler = createLoanCommandHandler;
            _toggleLoanCommandHandler = toggleLoanCommandHandler;
            _getLoansQueryHandler = getLoansQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetLoans([FromQuery] GetLoansQuery query)
        {
            var response = await _getLoansQueryHandler.Handle(query, HttpContext.RequestAborted);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanCommand command)
        {
            var response = await _createLoanCommandHandler.Handle(command, HttpContext.RequestAborted);

            return Ok(response);
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleStatus([FromRoute] Guid id, [FromBody] ToggleLoanCommand command)
        {
            // Гарантируем, что ID из URL попадет в команду, даже если фронт прислал пустое тело
            command.Id = id;

            var response = await _toggleLoanCommandHandler.Handle(command, HttpContext.RequestAborted);
            return Ok(response);
        }
    }
}
