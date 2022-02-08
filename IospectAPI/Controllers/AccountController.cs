using IospectAPI.Services.Abstraction.Contracts;
using IospectAPI.Services.Abstraction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace IospectAPI.Controllers
{

    /// <summary>
    /// AccountController
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        /// <summary>
        /// AccountController
        /// </summary>
        /// <param name="accountService"></param>
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        /// <summary>
        /// GetAccounts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAccounts")]
      
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                
                var accounts = this._accountService.GetAccounts();

                if (accounts.Count > 0)
                {
                    return new OkObjectResult(new ApiResponseModel<List<AccountModel>>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = accounts
                    });
                }
                return new StatusCodeResult((int)HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                throw;
                // return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// GetAccountTransactions
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransactionsByAccountId")]

        public async Task<IActionResult> GetAccountTransactions(int accountId)
        {
            try
            {

                var accountsT = this._accountService.GetAccountTransactions(accountId);

                if (accountsT.Count > 0)
                {
                    return new OkObjectResult(new ApiResponseModel<List<TransactionModel>>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = accountsT
                    });
                }
                return new StatusCodeResult((int)HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                throw;
                // return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }






        /// <summary>
        /// AddAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddAccount")]
        public async Task<IActionResult> AddAccount(AccountModel model)
        {
            try
            {
                var result = this._accountService.AddAccount(model);
                if (result == 2) return new StatusCodeResult((int)HttpStatusCode.Created);
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = result == 1 ? "Account number is already created" : "Something went wrong. Please check the values passed."
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// PutDeposit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PutDeposit")]
        public async Task<IActionResult> PutDeposit(TransactionModel model)
        {
            try
            {
                var result = this._accountService.PutDeposit(model);
                if (result == 2) return new StatusCodeResult((int)HttpStatusCode.Created);

                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = result == 0 ? "No account found" : "Something went wrong. Please check the values passed."
                });
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// PutDeposit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PutWithdraw")]
        public async Task<IActionResult> PutWithdraw(TransactionModel model)
        {
            try
            {
                var result = this._accountService.PutWithdraw(model);
                if (result == 2) return new StatusCodeResult((int)HttpStatusCode.Created);

                var errorMessage = "Something went wrong. Please check the values passed.";

                if (result == 0)
                {
                    errorMessage = "No account found";
                }
                else if (result == 1)
                {
                    errorMessage = "There isn`t enough money!";
                }


                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = errorMessage
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
