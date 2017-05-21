using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using WalletService.Exceptions;
using WalletService.Service;
using System.Net;

namespace WalletService.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("getballance/{accountId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAccountBallance(int accountId)
        {
            try
            {
                var accountInfo = await _accountService.GetAccountInformationAsync(accountId).ConfigureAwait(false);

                return Ok(accountInfo.Balance);
            }
            catch (AccountNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to get balance");
            }
        }

        [Route("depositFunds/{accountId}/{ammount:decimal}")]
        [HttpPost]
        public async Task<IHttpActionResult> DepositFunds(int accountId, decimal ammount)
        {
            try
            {
                var newBalance = await _accountService.DepositAmountAsync(accountId, ammount).ConfigureAwait(false);

                return Ok(newBalance);
            }
            catch (AccountNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to withdraw money");
            }
        }

        [Route("withdrawFunds/{accountId}/{ammount:decimal}")]
        [HttpPost]
        public async Task<IHttpActionResult> WithdrawFunds(int accountId, decimal ammount)
        {
            try
            {
                var newBalance = await _accountService.WithdrawAmountAsync(accountId, ammount).ConfigureAwait(false);

                return Ok(newBalance);
            }
            catch (AccountNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch(NotEnoughFundsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to withdraw money");
            }
        }

        [Route("create/{userId}")]
        [HttpPut]
        public async Task<IHttpActionResult> CreateAccountAsync(string userId)
        {
            try
            {
                var accountId = await _accountService.OpenCustomerAccountAsync(userId).ConfigureAwait(false);

                return Ok(accountId);
            }
            catch (UserIdNotUniqueException e)
            {
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to create account");
            }
        }

        [Route("delete/{accountId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> CloseAccountAsync(int accountId)
        {
            try
            {
                var affectedRows = await _accountService.CloseCustomerAccountAsync(accountId).ConfigureAwait(false);

                if (affectedRows == 0)
                    return BadRequest($"No account found for accountId: {accountId}");

                return Ok();
            }
            catch (AccountNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to close account.");
            }
        }
    }
}