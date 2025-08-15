using System;
using System.Collections.Generic;

namespace FinanceSys
{
    // a. Record type for transactions
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // b. Interface for processing transactions
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // c. Different processors
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Bank Transfer] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Mobile Money] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Crypto Wallet] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    // d. Base account class
    public class Account
    {
        public string AccountNumber { get; private set; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New Balance: {Balance:C}");
        }
    }

    // e. Sealed class for specialized account
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
            }
            else
            {
                Balance -= transaction.Amount;
                Console.WriteLine($"Transaction successful. New Balance: {Balance:C}");
            }
        }
    }

    // f. Finance application
    public class FinanceApp
    {
        private List<Transaction> _transactions = new List<Transaction>();

        public void Run()
        {
            // Create account with balance 1000
            SavingsAccount account = new SavingsAccount("ACC123", 1000m);

            // Create sample transactions
            Transaction t1 = new Transaction(1, DateTime.Now, 200m, "Groceries");
            Transaction t2 = new Transaction(2, DateTime.Now, 300m, "Utilities");
            Transaction t3 = new Transaction(3, DateTime.Now, 150m, "Entertainment");

            // Process transactions
            ITransactionProcessor mobile = new MobileMoneyProcessor();
            ITransactionProcessor bank = new BankTransferProcessor();
            ITransactionProcessor crypto = new CryptoWalletProcessor();

            mobile.Process(t1);
            bank.Process(t2);
            crypto.Process(t3);

            // Apply transactions to account
            account.ApplyTransaction(t1);
            account.ApplyTransaction(t2);
            account.ApplyTransaction(t3);

            // Save all transactions
            _transactions.Add(t1);
            _transactions.Add(t2);
            _transactions.Add(t3);

            Console.WriteLine("\nAll Transactions Recorded:");
            foreach (var t in _transactions)
            {
                Console.WriteLine($"{t.Id}: {t.Category} - {t.Amount:C} on {t.Date}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FinanceApp app = new FinanceApp();
            app.Run();
        }
    }
}
