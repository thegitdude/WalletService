using System;
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

        [HttpGet]
        [Route("getballance/{accountId}")]
        public async Task<IHttpActionResult> GetAccountBallance(int accountId)
        {
            try
            {
                var accountInfo = await _accountService.GetAccountInformationAsync(accountId).ConfigureAwait(false);

                return Ok(accountInfo.Balance);
            }
            catch (AccountNotFoundException e)
            {
                return Content(HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to get balance");
            }
        }

        [HttpPost]
        [Route("depositFunds/{accountId}/{ammount:decimal}")]
        public async Task<IHttpActionResult> DepositFunds(int accountId, decimal ammount)
        {
            try
            {
                var newBalance = await _accountService.DepositAmountAsync(accountId, ammount).ConfigureAwait(false);

                return Ok(newBalance);
            }
            catch (AccountNotFoundException e)
            {
                return Content(HttpStatusCode.NotFound, e.Message);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to withdraw money");
            }
        }

        [HttpPost]
        [Route("withdrawFunds/{accountId}/{ammount:decimal}")]
        public async Task<IHttpActionResult> WithdrawFunds(int accountId, decimal ammount)
        {
            try
            {
                var newBalance = await _accountService.WithdrawAmountAsync(accountId, ammount).ConfigureAwait(false);

                return Ok(newBalance);
            }
            catch (AccountNotFoundException e)
            {
                return Content(HttpStatusCode.NotFound, e.Message);
            }
            catch (NotEnoughFundsException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to withdraw money");
            }
        }

        [HttpPut]
        [Route("create/{userId}")]
        public async Task<IHttpActionResult> CreateAccountAsync(string userId)
        {
            try
            {
                var accountId = await _accountService.OpenCustomerAccountAsync(userId).ConfigureAwait(false);

                return Ok(accountId);
            }
            catch (UserIdNotUniqueException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, "Failed to create account");
            }
        }

        [HttpDelete]
        [Route("delete/{accountId}")]
        public async Task<IHttpActionResult> CloseAccountAsync(int accountId)
        {
            try
            {
                await _accountService.CloseCustomerAccountAsync(accountId).ConfigureAwait(false);

                return Ok();
            }
            catch (AccountNotFoundException e)
            {
                return Content(HttpStatusCode.NotFound, e.Message);
            }
            catch(AccountCloseException e)
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