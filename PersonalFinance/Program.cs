using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PersonalFinance
{
    class Program
    {
        static void Main(string[] argg)
        {
            // добавить ежемесячные расходы, которые будут отниматься от баланса
            // добавить запоминание пользователей , баланса, покупок и т.д
            // добавить цикл 
            // добавить категории покупок
            // добавить просмотр всех операций


            List<ModelGood> goods = new List<ModelGood>();

            //Console.Write("Введите имя: ");
            //string name = Console.ReadLine();

            //User user = new User
            //{
            //    Name = name,
            //    Balance = BalanceUser(),
            //    HistoryGoods = new List<ModelGood>()
            //};

            //string json = JsonSerializer.Serialize(user);

            decimal balance = BalanceUser();
            bool isRunning = true;

            do
            {
                Console.WriteLine("Выберите действие.\n1. Добавить покупку\n2. Просмотреть историю покупок");
                string input = Console.ReadLine();
                if (input == "1")
                {
                    balance = Purchases(goods, balance);
                    Console.WriteLine(balance + " руб"); 
                }
                if (input == "2")
                {
                    HistoryPurchases(goods);
                }
                if (input == "x")
                {
                    isRunning = false;
                }
            }
            while (isRunning);


        }


        static decimal BalanceUser() // Инициализация баланса пользователя
        {
            Console.Write("Введите ваш баланс: ");
            string input = Console.ReadLine();
            if (!decimal.TryParse(input, out decimal balance))
            {
                Console.WriteLine("Некорректный данные.");
            }
            return balance;
        }

        static decimal Purchases(List<ModelGood> goods, decimal balance) // Добавление покупок
        {
            Console.Write("Что вы купили: ");
            string tovar = Console.ReadLine();
            Console.Write("Сколько вы потратили: ");
            string priceTovar = Console.ReadLine();
            if (!decimal.TryParse(priceTovar, out decimal price) || price > balance)
            {
                Console.WriteLine("Некоректные данные.");
                return 0;
            }

            goods.Add(new ModelGood
            {
                Name = tovar,
                Price = price,
            });

            return balance - price;
        }

        static void HistoryPurchases(List<ModelGood> goods) // Просмотр истории покупок
        {
            foreach (ModelGood good in goods)
            {
                Console.WriteLine($"{good.Name} - {good.Price} руб.\n");
            }
        }
    }
}