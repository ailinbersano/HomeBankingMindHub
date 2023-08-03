using System;
using System.Linq;
using HomeBankingMindHub.Enumerates;

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
                    new Client
                    {
                        Email = "vcoronado@gmail.com",
                        FirstName = "Victor",
                        LastName = "Coronado",
                        Password = "123456"
                    },
                    new Client
                    {
                        Email = "taylorswift@gmail.com",
                        FirstName = "Taylor",
                        LastName = "Swift",
                        Password = "24680"
                    },
                    new Client
                    {
                        Email = "bersanoailin@gmail.com",
                        FirstName = "Ailin",
                        LastName = "Bersano",
                        Password = "13579"
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
                var accountVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (accountVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId = accountVictor.Id,
                            CreationDate = DateTime.Now,
                            Number = string.Empty,
                            Balance = 0
                        },
                        new Account
                        {
                            ClientId = accountVictor.Id,
                            CreationDate = DateTime.Now,
                            Number = "VIN001",
                            Balance = 0
                        }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();
                }
                var accountTaylor = context.Clients.FirstOrDefault(c => c.Email == "taylorswift@gmail.com");
                if (accountTaylor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId = accountTaylor.Id,
                            CreationDate = DateTime.Now,
                            Number = "tay1",
                            Balance = 56
                        }

                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();

                }
                var accountAilin = context.Clients.FirstOrDefault(c => c.Email == "bersanoailin@gmail.com");
                if (accountAilin != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId = accountAilin.Id,
                            CreationDate = DateTime.Now,
                            Number = "a1",
                            Balance = 330
                        },
                        new Account
                        {
                            ClientId = accountAilin.Id,
                            CreationDate = DateTime.Now,
                            Number = "a2",
                            Balance = 220
                        }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();
                }
            }
            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");
                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction
                        {
                            AccountId = account1.Id,
                            Amount = 10000,
                            Date = DateTime.Now.AddHours(-5),
                            Description = "Transferencia reccibida",
                            Type = TransactionType.CREDIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId = account1.Id,
                            Amount = -2000,
                            Date = DateTime.Now.AddHours(-6),
                            Description = "Compra en tienda mercado libre",
                            Type = TransactionType.DEBIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId = account1.Id,
                            Amount = -3000,
                            Date = DateTime.Now.AddHours(-7),
                            Description = "Compra en tienda xxxx",
                            Type = TransactionType.DEBIT.ToString()
                        }
                    };
                    var account2 = context.Accounts.FirstOrDefault(c => c.Number == "tay1");
                    if (account2 != null)
                    {
                        var transactions2 = new Transaction[]
                        {
                            new Transaction
                            {
                                AccountId = account2.Id,
                                Amount = 10000,
                                Date = DateTime.Now.AddHours(-5),
                                Description = "Transferencia reccibida",
                                Type = TransactionType.CREDIT.ToString()
                            },
                            new Transaction
                            {
                                AccountId = account2.Id,
                                Amount = -3000,
                                Date = DateTime.Now.AddHours(-7),
                                Description = "Compra en tienda mia",
                                Type = TransactionType.DEBIT.ToString()
                            }
                        };
                        var account3 = context.Accounts.FirstOrDefault(c => c.Number == "a2");
                        if (account3 != null)
                        {
                            var transactions3 = new Transaction[]
                            {
                                new Transaction
                                {
                                    AccountId = account3.Id,
                                    Amount = 70000,
                                    Date = DateTime.Now.AddHours(-5),
                                    Description = "Transferencia recibida",
                                    Type = TransactionType.CREDIT.ToString()
                                },
                                new Transaction
                                {
                                    AccountId = account3.Id,
                                    Amount = -3700,
                                    Date = DateTime.Now.AddHours(-7),
                                    Description = "Compra en tienda",
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
            if (!context.Loans.Any())
            {
                //crearemos 3 prestamos Hipotecario, Personal y Automotriz
                var loans = new Loan[]
                {
                    new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                };
                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }
                context.SaveChanges();
                //ahora agregaremos los clientloan (Prestamos del cliente)
                //usaremos al único cliente que tenemos y le agregaremos un préstamo de cada item
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    //ahora usaremos los 3 tipos de prestamos
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }
                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                    //guardamos todos los prestamos
                    context.SaveChanges();
                }
                var client2 = context.Clients.FirstOrDefault(c => c.Email == "taylorswift@gmail.com");
                if (client2 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 920000,
                            ClientId = client2.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    //guardamos todos los prestamos
                    context.SaveChanges();

                }
                var client3 = context.Clients.FirstOrDefault(c => c.Email == "bersano@gmail.com");
                if (client3 != null)
                {
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 63000,
                            ClientId = client3.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }
                    //guardamos todos los prestamos
                    context.SaveChanges();

                }

            }
            if (!context.Cards.Any())
            {
                //buscamos al cliente victor
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    //le agregamos 2 tarjetas de crédito una GOLD y una TITANIUM, de tipo DEBITO Y CREDITO RESPECTIVAMENTE
                    var cards = new Card[]
                    {
                        new Card {
                            ClientId= client1.Id,
                            CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.GOLD.ToString(),
                            Number = "3325-6745-7876-4445",
                            Cvv = 990,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(4),
                        },
                        new Card {
                            ClientId= client1.Id,
                            CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.CREDIT.ToString(),
                            Color = CardColor.TITANIUM.ToString(),
                            Number = "2234-6745-552-7888",
                            Cvv = 750,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(5),
                        },
                    };

                    foreach (Card card in cards)
                    {
                        context.Cards.Add(card);
                    }
                    context.SaveChanges();
                }
                //buscamos a la taylor
                var client2 = context.Clients.FirstOrDefault(c => c.Email == "taylorswift@gmail.com");
                if (client2 != null)
                {
                    //le agregamos 2 tarjetas una GOLD y una TITANIUM, de tipo DEBITO Y CREDITO RESPECTIVAMENTE
                    var cards = new Card[]
                    {
                        new Card {
                            ClientId= client2.Id,
                            CardHolder = client2.FirstName + " " + client2.LastName,
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.GOLD.ToString(),
                            Number = "3325-6745-7876-1989",
                            Cvv = 989,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(4),
                        },
                        new Card {
                            ClientId= client2.Id,
                            CardHolder = client2.FirstName + " " + client2.LastName,
                            Type = CardType.CREDIT.ToString(),
                            Color = CardColor.TITANIUM.ToString(),
                            Number = "2234-6745-552-1888",
                            Cvv = 888,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(5),
                        },
                    };

                    foreach (Card card in cards)
                    {
                        context.Cards.Add(card);
                    }
                    context.SaveChanges();
                }
                //me busco
                var client3 = context.Clients.FirstOrDefault(c => c.Email == "bersanoailin@gmail.com");
                if (client3 != null)
                {
                    //le agregamos 2 tarjetas una SILVER y una TITANIUM, de tipo DEBITO Y CREDITO RESPECTIVAMENTE
                    var cards = new Card[]
                    {
                        new Card {
                            ClientId= client3.Id,
                            CardHolder = client3.FirstName + " " + client3.LastName,
                            Type = CardType.CREDIT.ToString(),
                            Color = CardColor.SILVER.ToString(),
                            Number = "3325-6745-7876-1997",
                            Cvv = 123,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(4),
                        },
                        new Card {
                            ClientId= client3.Id,
                            CardHolder = client3.FirstName + " " + client3.LastName,
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.TITANIUM.ToString(),
                            Number = "2234-6745-552-1968",
                            Cvv = 369,
                            FromDate= DateTime.Now,
                            ThruDate= DateTime.Now.AddYears(5),
                        },
                    };

                    foreach (Card card in cards)
                    {
                        context.Cards.Add(card);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}