using FinanceControl.Application.UseCases.Accounts.CreateAccount;
using FinanceControl.Application.UseCases.Accounts.Deposit;
using FinanceControl.Application.UseCases.Accounts.GetAccountById;
using FinanceControl.Application.UseCases.Accounts.GetAccounts;
using FinanceControl.Application.UseCases.Accounts.Transfer;
using FinanceControl.Application.UseCases.Accounts.UpdateAccount;
using FinanceControl.Application.UseCases.Accounts.Withdraw;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Get all accounts
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllAccountsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    /// Create a new account
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result);
    }
    
    /// <summary>
    /// Create a new deposit
    /// </summary>
    [HttpPut("/deposit")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deposit([FromBody] DepositCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result);
    }
    
    /// <summary>
    /// Create a new transfer
    /// </summary>
    [HttpPut("/transfer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Transfer([FromBody] TransferCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result);
    }
    
    /// <summary>
    /// Create a new withdraw
    /// </summary>
    [HttpPut("/withdraw")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = result.Data?.Id }, result);
    }
    
    /// <summary>
    /// Get account by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id));
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
    
    /// <summary>
    /// Update account
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");
    
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}