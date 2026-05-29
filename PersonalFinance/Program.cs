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

            decimal income, expenditure;

            ModelGood modelGood = new ModelGood();
            

            decimal balance = BalanceUser();
            Console.WriteLine(balance + " руб");
            decimal sppending = Purchases(modelGood, balance);
            Console.WriteLine(sppending);



        }
        static decimal BalanceUser() // Инициализация баланса пользователя
        {
            Console.Write("Введите ваш баланс: ");
            string input = Console.ReadLine();
            if (!decimal.TryParse(input, out decimal balance))
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");
                return BalanceUser();
            }
            return balance;
        }

        static decimal Purchases(ModelGood modelGood, decimal balance) // ? Не уверен 
        {
            List<ModelGood> purchases = new List<ModelGood>();
            Console.Write("Что вы купили: ");
            modelGood.Name = Console.ReadLine();
            Console.Write("Сколько это стоило: ");
            modelGood.Price = decimal.TryParse(Console.ReadLine(), out decimal price) ? price : 0;
            return balance - modelGood.Price;
        }
    }
}