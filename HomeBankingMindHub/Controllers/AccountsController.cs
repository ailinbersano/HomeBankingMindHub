using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }
        [HttpGet("accounts")]
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();
                var accountsDTO = new List<AccountDTO>();
                foreach (Account account in accounts)
                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO
                        {
                            Id = tr.Id,
                            Type = tr.Type,
                            Amount = tr.Amount,
                            Description = tr.Description,
                            Date = tr.Date,
                        }).ToList()
                    };
                    accountsDTO.Add(newAccountDTO);
                }
                return Ok(accountsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("accounts/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);
                if (account == null)
                {
                    return NotFound();
                }
                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(tr => new TransactionDTO
                    {
                        Id = tr.Id,
                        Type = tr.Type,
                        Amount = tr.Amount,
                        Description = tr.Description,
                        Date = tr.Date,
                    }).ToList()
                };
                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //tarea7

        [HttpGet("clients/current/accounts")]
        public IActionResult GetAccounts()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "cliente no autorizado");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "cliente no encontrado");
                }
                var accountsDTO = new List<AccountDTO>();
                foreach (Account account in client.Accounts)
                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO
                        {
                            Id = tr.Id,
                            Type = tr.Type,
                            Amount = tr.Amount,
                            Description = tr.Description,
                            Date = tr.Date,
                        }).ToList()
                    };
                    accountsDTO.Add(newAccountDTO);
                }
                return Ok(accountsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(403, ex.Message);
            }
        }

        [HttpPost("clients/current/accounts")]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "cliente no autorizado");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "cliente no encontrado");
                }

                if (client.Accounts.Count() > 2)
                {
                    return StatusCode(403, "el cliente alcanzo el limite de cuentas");
                }
                Random random = new Random();
                string numeroAleatorio = random.Next(100000, 999999).ToString("D8");
                Account newAccount = new Account
                {
                    Number = "VIN-" + numeroAleatorio,
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = client.Id
                };
                _accountRepository.Save(newAccount);

                AccountDTO newAccountDTO = new AccountDTO
                {
                    Number = newAccount.Number,
                    CreationDate = newAccount.CreationDate,
                    Balance = newAccount.Balance,
                };

                return Created("", newAccountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       
    }
}
