using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.IO;

namespace PersonalFinance
{
    class Program
    {
        static void Main(string[] argg)
        {
            // добавить ежемесячные расходы, которые будут отниматься от баланса
            // добавить категории покупок
            // добавить просмотр всех операций
            // добавить - пользователь ввел имя если имя найденно в файле то загружаешь его данные - если нет создай нового
            // добавить ? чтобы пользователь мог получить только свои данные


            List<ModelGood> goods = new List<ModelGood>();
            List<DataUser> dataUsers = new List<DataUser>();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "dataUser.txt");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                dataUsers = JsonSerializer.Deserialize<List<DataUser>>(json) ?? new List<DataUser>();
            }


            bool isRunning = true;
            decimal balance = 0;

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            do
            {
                Console.WriteLine("1 - Добавить баланс\n2 - Добавить покупку\n3 - Просмотреть историю покупок\n4 - Выход");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        balance += BalanceUser();
                        Console.WriteLine($"\n{balance} руб");
                        break;

                    case "2":
                        if (balance == 0)
                        {
                            Console.WriteLine("Добавьте баланс.");
                            continue;
                        }
                        balance = Purchases(goods, balance);
                        Console.WriteLine($"\n{balance} руб");
                        break;

                    case "3":
                        if (goods.Count == 0)
                        {
                            Console.WriteLine("Покупок еще нет.\n");
                        }
                        goods = HistoryPurchases(goods);
                        break;

                    case "4":
                        int nextId = dataUsers.Count == 0 ? 1 : dataUsers.Max(u => u.Id) + 1;
                        dataUsers.Add(new DataUser
                        {
                            Id = nextId,
                            Name = name,
                            Balance = balance,
                            HistoryGoods = goods
                        });
                        SaveToDataUser(dataUsers, filePath);
                        isRunning = false;
                        break;
                }
            }
            while (isRunning);
        }


        static decimal BalanceUser() // Инициализация баланса пользователя
        {
            Console.Write("Введите сумму: ");
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
                return 0; // ---------------------- ! изменить
            }

            goods.Add(new ModelGood
            {
                TovarName = tovar,
                Price = price,
            });

            return balance - price;
        }

        static List<ModelGood> HistoryPurchases(List<ModelGood> goods) // Просмотр истории покупок
        {
            foreach (ModelGood good in goods)
            {
                Console.WriteLine($"{good.TovarName} - {good.Price} руб.");
            }
            return goods;
        }

        static void SaveToDataUser(List<DataUser> dataUsers, string filePath) // Сохрагнгие в файл
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string json = JsonSerializer.Serialize(dataUsers, options);
            File.WriteAllText(filePath, json );
        }
    }
}