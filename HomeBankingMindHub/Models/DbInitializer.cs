using System;
using System.Linq;
namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com",
                        FirstName="Victor",
                        LastName="Coronado",
                        Password="123456"
                    },
                    new Client { Email = "estebanrey@gmail.com",
                        FirstName="Esteban",
                        LastName="Rey",
                        Password="357384"
                    },
                    new Client { Email = "juliarobert@gmail.com",
                        FirstName="Julia",
                        LastName="Robert",
                        Password="10101010"
                    }
                };
                
                foreach (Client client in clients)
                {
                    context.Clients.Add(client);
                }
                //guardamos
                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                var accountVictor = context.Clients.FirstOrDefault(
                    c => c.Email == "vcoronado@gmail.com");
                if (accountVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId=accountVictor.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 0",
                            Balance=0
                        },
                        new Account
                        {
                            ClientId=accountVictor.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 1",
                            Balance=11111
                        },
                        new Account
                        {
                            ClientId=accountVictor.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 2",
                            Balance=22222
                        },
                        new Account
                        {
                            ClientId=accountVictor.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 3",
                            Balance=33333
                        }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }

                    //guardamos
                    context.SaveChanges();
                }
                var account2 = context.Clients.FirstOrDefault(
                    c => c.Email == "estebanrey@gmail.com");
                if (account2 != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId=account2.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 1",
                            Balance=99999
                        },
                        new Account
                        {
                            ClientId=account2.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 2",
                            Balance=888888
                        }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }

                    //guardamos
                    context.SaveChanges();
                }
                var account3 = context.Clients.FirstOrDefault(
                    c => c.Email == "juliarobert@gmail.com");
                if (account3 != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId=account3.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 1",
                            Balance=8745623
                        },
                        new Account
                        {
                            ClientId=account3.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 2",
                            Balance=7894652
                        },
                        new Account
                        {
                            ClientId=account3.Id,
                            CreationDate=DateTime.Now,
                            Number="cuenta 3",
                            Balance=33333
                        }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }

                    //guardamos
                    context.SaveChanges();
                }
            }
            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "cuenta 1");

                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction
                        {
                            AccountId= account1.Id,
                            Amount = 10000,
                            Date= DateTime.Now.AddHours(-5),
                            Description = "Transferencia recibida",
                            Type = TransactionType.CREDIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId= account1.Id,
                            Amount = -2000,
                            Date= DateTime.Now.AddHours(-6),
                            Description = "Compra en tienda mercado libre",
                            Type = TransactionType.DEBIT.ToString() },

                        new Transaction {
                            AccountId= account1.Id,
                            Amount = -3000,
                            Date= DateTime.Now.AddHours(-7),
                            Description = "Compra en tienda xxxx",
                            Type = TransactionType.DEBIT.ToString() },
                    };

                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
                var account2 = context.Accounts.FirstOrDefault(c => c.Number == "cuenta 1");

                if (account2 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction
                        {
                            AccountId= account2.Id,
                            Amount = 500,
                            Date= DateTime.Now.AddHours(-5),
                            Description = "Transferencia recibida",
                            Type = TransactionType.CREDIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId= account2.Id,
                            Amount = -700,
                            Date= DateTime.Now.AddHours(-6),
                            Description = "Pago servicios publicos",
                            Type = TransactionType.DEBIT.ToString()
                        }
                    };

                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}