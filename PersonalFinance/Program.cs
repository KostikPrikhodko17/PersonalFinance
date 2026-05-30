using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance
{
    class Program
    {
        static void Main(string[] argg)
        {
            // добавить ежемесячные расходы, которые будут отниматься от баланса
            // добавить историю операций
            // добавить запоминание пользователей , баланса, покупок и т.д
            // добавить цикл 
            // добавить категории покупок
            // добавить просмотр всех операций


            ModelGood modelGood = new ModelGood();
            List<ModelGood> goods = new List<ModelGood>();

            bool isRunning = true;

            Console.WriteLine("Учет финансов.\nЧтобы выйти введите X");
            decimal balance = BalanceUser();


            Console.WriteLine("1. Добавить покупку\n2. Добавить доход\nПосмотреть историю покупок");
            string userChoice = Console.ReadLine();
            if(userChoice == "1")
            {
                decimal result = Purchases(goods, balance);
                Console.WriteLine(result + " руб.");
            }
            if (userChoice == "2")
            {
                // в разработке
            }
            if (userChoice == "3")
            {
                // в разработке
            }


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
            goods.Add(new ModelGood { Name = Console.ReadLine() });
            Console.Write("Сколько вы потратили: ");
            goods.Add(new ModelGood { Price = Console.ReadLine() });

            if (!decimal.TryParse(goods.Last().Price, out decimal price) || price > balance)
            {
                Console.WriteLine("Некорректный данные.");
                Purchases(goods, balance);

            }
            return balance - price;
        }

    }
}