using HomeBankingMindHub.DTOs;
using HomeBankingMindHub.Enumerates;
using HomeBankingMindHub.Models;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Security.Principal;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;
        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }
        [HttpPost]
        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ?
                    User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid("Empty email;");
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid("Client does not exist");
                }
                if (transferDTO.FromAccountNumber == string.Empty || transferDTO.ToAccountNumber == string.Empty)
                {
                    return Forbid("Source account or destination account not provided");
                }
                if(transferDTO.FromAccountNumber==transferDTO.ToAccountNumber)
                {
                    return Forbid("Transfer to the same account is not allowed");
                }
                if (transferDTO.Amount == 0 || transferDTO.Description == string.Empty)
                {
                    return Forbid("Amount or description not provided");
                }
                //buscamos las cuentas
                Account fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);
                if (fromAccount == null) 
                {
                    return Forbid("Source account does not exist");
                }
                //controlamos el monto
                if (fromAccount.Balance < transferDTO.Amount)
                {
                    return Forbid("insufficient funds");
                }
                //buscamos la cuenta de destino
                Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
                if (toAccount == null)
                {
                    return Forbid("Destination account does not exist");
                }
                //insertamos las dos transacciones realizadas
                //desde toAccount se debe generar un debito por lo que multiplicamos por -1
                _transactionRepository.Save(new Transaction
                { 
                    Type=TransactionType.DEBIT.ToString(),
                    Amount=transferDTO.Amount*-1,
                    Description=transferDTO.Description+" "+ toAccount.Number,
                    AccountId = fromAccount.Id,
                    Date = DateTime.Now,
                });
                //ahora una credito para la cuenta fromAccount
                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = transferDTO.Amount,
                    Description = transferDTO.Description + " " + fromAccount.Number,
                    AccountId = toAccount.Id,
                    Date = DateTime.Now,
                });
                //seteamos los valores de las cuentas, a la cuenta de origen le restamos el monto
                fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;
                //actualizamos la cuenta de origen
                _accountRepository.Save(fromAccount);

                //a la cuenta de destino le sumamos el monto
                toAccount.Balance = toAccount.Balance + transferDTO.Amount;
                //actualizamos la cuenta de destino
                _accountRepository.Save(toAccount);
                return Created("Creted succesfully", fromAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
