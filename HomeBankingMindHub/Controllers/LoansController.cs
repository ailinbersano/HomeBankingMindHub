using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Enumerates;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ILoanRepository _loanRepository;
        private IClientLoanRepository _clientLoanRepository;
        private ITransactionRepository _transactionRepository;
        public LoansController(IClientRepository clientRepository ,IAccountRepository accountRepository, ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository=accountRepository;
            _loanRepository=loanRepository;
            _clientLoanRepository=clientLoanRepository;
            _transactionRepository=transactionRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var loans = _loanRepository.GetAll();
                List<LoanDTO> loanDTOs = new List<LoanDTO>();
                foreach(var loan in loans)
                {
                    LoanDTO loanDTO = new LoanDTO()
                    {
                        Id = loan.Id,
                        Name = loan.Name,
                        MaxAmount = loan.MaxAmount,
                        Payments = loan.Payments,
                    };
                    loanDTOs.Add(loanDTO);
                }
                return Ok(loanDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            try
            {
                if (loanApplicationDTO.Amount < 1 ||string.IsNullOrEmpty(loanApplicationDTO.ToAccountNumber) || string.IsNullOrEmpty(loanApplicationDTO.Payments))
                {
                    return StatusCode(403, "invalid data");
                }
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "unauthorized client");
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return StatusCode(403, "client not found");
                }
                var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);
                if (loan == null ||
                    loanApplicationDTO.Amount >= loan.MaxAmount ||
                    (loanApplicationDTO.Payments == null ||
                    loanApplicationDTO.Payments == string.Empty) ||
                    !loan.Payments.Split(',').Contains(loanApplicationDTO.Payments))
                    return StatusCode(403, "Invalid Data");
                var account = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);
                if (account == null || account.ClientId != client.Id)
                {
                    return Forbid();
                }
                Account updatedAccount = account;
                updatedAccount.Balance = account.Balance + loanApplicationDTO.Amount;
                ClientLoan clientLoan = new ClientLoan
                {
                    ClientId = client.Id,
                    Amount = loanApplicationDTO.Amount + loanApplicationDTO.Amount * 0.2,
                    Payments = loanApplicationDTO.Payments,
                    LoanId = loanApplicationDTO.LoanId,
                };
                _clientLoanRepository.Save(clientLoan);
                Transaction transaction = new Transaction
                {
                    AccountId = account.Id,
                    Amount = loanApplicationDTO.Amount,
                    Date = DateTime.Now,
                    Description = loan.Name + " loan approved",
                    Type = TransactionType.CREDIT.ToString(),
                };
                _transactionRepository.Save(transaction);
                _accountRepository.Save(account);
                return Ok(clientLoan);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
