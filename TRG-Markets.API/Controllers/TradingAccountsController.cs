using Microsoft.AspNetCore.Mvc;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;
namespace TRG_Markets.API.Controllers;
[Route("api/tradingaccounts")]
[ApiController]
    public class TradingAccountsController : ControllerBase
    {
       private readonly ITradingAccountService _tradingAccountService;
       public TradingAccountsController(ITradingAccountService tradingAccountService) {
           _tradingAccountService = tradingAccountService;
       }
       [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tradingAccounts = await _tradingAccountService.GetAllTradingAccountsAsync();
            return Ok(tradingAccounts);
        }

       [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tradingAccount = await _tradingAccountService.GetTradingAccountByIdAsync(id);
            if (tradingAccount == null)
            {
                return NotFound();
            }
            return Ok(tradingAccount) ; }

       [HttpPost]
        public async Task<IActionResult> Create(TradingAccount tradingAccount)
        {
            var createdTradingAccount = await _tradingAccountService.CreateTradingAccountAsync(tradingAccount);
            return Ok(createdTradingAccount);
        }
       [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TradingAccount tradingAccount)
        {
            var updatedTradingAccount = await _tradingAccountService.UpdateTradingAccountAsync(id, tradingAccount);
            if (updatedTradingAccount == null)
            {
                return NotFound();
            }
            return Ok(updatedTradingAccount);
        }

       [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _tradingAccountService.DeleteTradingAccountAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
