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
            // добавить ? чтобы пользователь мог получить только свои данные
            // добавить возможность удалить пользователя


            List<ModelGood> goods = new List<ModelGood>();
            List<DataUser> dataUsers = new List<DataUser>();
            DataUser dataUser = new DataUser();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "dataUser.txt");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                dataUsers = JsonSerializer.Deserialize<List<DataUser>>(json) ?? new List<DataUser>();
            }


            bool isRunning = true;
            dataUser.Balance = 0;

            Console.Write("Введите имя: ");
            dataUser.Name = Console.ReadLine(); // !1 может быть null or empty

            // ---------------- ! изменить ? т.к это двойная проверка
            DataUser? correctUser = dataUsers.FirstOrDefault(u => u.Name.Equals(dataUser.Name, StringComparison.OrdinalIgnoreCase));
            if (correctUser != null)
            {
                Console.WriteLine($"С возвращением, {correctUser.Name}\nБалансе: {correctUser.Balance} руб");
                dataUser.Balance = correctUser.Balance;
                goods = correctUser.HistoryGoods;
            }

                do
                {
                    Console.WriteLine("1 - Добавить баланс\n2 - Добавить покупку\n3 - Просмотреть историю покупок\n4 - Выход");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            dataUser.Balance += BalanceUser();
                            Console.WriteLine($"\n{dataUser.Balance} руб");
                            break;

                        case "2":
                            if (dataUser.Balance == 0)
                            {
                                Console.WriteLine("Ваш баланс отрицательный. Добавьте баланс.");
                            }
                            dataUser.Balance = Purchases(goods, dataUser.Balance);
                            Console.WriteLine($"\n{dataUser.Balance} руб");
                            break;

                        case "3":
                            if (goods.Count == 0)
                            {
                                Console.WriteLine("Покупок еще нет.\n");
                            }
                            goods = HistoryPurchases(goods);
                            break;

                        case "4":
                        if (correctUser == null) // -------------------- !
                        {
                            int nextId = dataUsers.Count == 0 ? 1 : dataUsers.Max(u => u.Id) + 1;
                            dataUsers.Add(new DataUser
                            {
                                Id = nextId,
                                Name = dataUser.Name,
                                Balance = dataUser.Balance,
                                HistoryGoods = goods
                            });
                            SaveToDataUser(dataUsers, filePath);
                        }
                        else
                        {
                            correctUser.Balance = dataUser.Balance; // - возможно не нужны
                            correctUser.HistoryGoods = goods; // -
                            SaveToDataUser(dataUsers, filePath);
                        }
                            isRunning = false;
                            break;
                    }
                }
                while (isRunning);
        }


        static decimal BalanceUser() // Инициализация баланса пользователя и пополнение
        {
            Console.Write("Введите сумму: ");
            string input = Console.ReadLine();

            if (!decimal.TryParse(input, out decimal balance))
            {
                Console.WriteLine("Некоректные данные");
            }

            return balance;
        }

        static decimal Purchases(List<ModelGood> goods, decimal balance) // Добавление покупок 
        {
            string tovar = TovarName();
            decimal price = PriceTovar(balance);

            goods.Add(new ModelGood
            {
                TovarName = tovar,
                Price = price,
            });

            return balance - price;
        }

        static string TovarName() // Возвращает название покупки
        {
            while (true)
            {
                Console.Write("Введите название покупки: ");
                string tovar = Console.ReadLine();
                
                if (!string.IsNullOrWhiteSpace(tovar))
                {
                    tovar = tovar.Trim();
                    return tovar;
                }
                else
                {
                    Console.WriteLine("Некоректные данные.");
                }
            }
        } 

        static decimal PriceTovar(decimal balance) // Возвращает цену товара
        {
            while (true)
            {
                Console.Write("Введите сумму покупки: ");
                string priceTovar = Console.ReadLine();

                if (decimal.TryParse(priceTovar, out decimal price) && price <= balance)
                {
                    return price;
                }
                else
                {
                    Console.WriteLine("Некоректные данные.");
                }
            }
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