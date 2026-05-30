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
            // добавить функцию: пользователь указал что у него зп N числа и N сумммы. Каждый месяц программа сама добавляет эту сумму к балансу
            // добавить ежемесячные расходы, которые будут отниматься от баланса
            // добавить историю покупок
            // добавить запоминание пользователей , баланса, покупок и т.д


            ModelGood modelGood = new ModelGood();
            List<ModelGood> goods = new List<ModelGood>();

            Console.WriteLine("Учет финансов.");
            decimal balance = BalanceUser();
            Console.WriteLine("1. Добавить покупку\tчто то еще");
            string input = Console.ReadLine();
            if (input == "1")
            {
                decimal currentBalance = Purchases(goods, balance);
                Console.WriteLine($"Ваш баланс: {currentBalance}");
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

        static decimal Purchases(List<ModelGood> goods, decimal balance) // Функция для совершения покупок и добавления их в список покупок
        {
            Console.WriteLine("Что вы купили?");
            string name = Console.ReadLine();
            Console.WriteLine("Сколько это стоило?");
            string input = Console.ReadLine();
            if (!decimal.TryParse(input, out decimal price))
            {
                Console.WriteLine("Некорректный данные.");
            }
            goods.Add(new ModelGood { Name = name, Price = price });
            return balance - price;
        }
    }
}