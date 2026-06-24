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


            List<DataUser> dataUsers = new List<DataUser>();
            DataUser dataUser = new DataUser();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "dataUser.txt");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                dataUsers = JsonSerializer.Deserialize<List<DataUser>>(json) ?? new List<DataUser>();
            }

            string name = UserName();

            bool isNewUser = false;
            dataUser = dataUsers.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (dataUser != null)
            {
                Console.WriteLine($"С возвращением, {dataUser.Name}\nБалансе: {dataUser.Balance} руб");
                isNewUser = false;
            }
            else
            {
                dataUser = new DataUser() { Name = name, Balance = 0, HistoryGoods = new List<ModelGood>() };
                isNewUser = true;
            }

                bool isRunning = true;

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
                                break;
                            }
                            dataUser.Balance = Purchases(dataUser);
                            Console.WriteLine($"\n{dataUser.Balance} руб");
                            break;

                        case "3":
                            if (dataUser.HistoryGoods.Count == 0)
                            {
                                Console.WriteLine("Покупок еще нет.\n");
                                break;
                            }
                            HistoryPurchases(dataUser);
                            break;


                        case "4":
                        if (isNewUser)
                        {
                            int nextId = dataUsers.Count == 0 ? 1 : dataUsers.Max(u => u.Id) + 1;
                            dataUser.Id = nextId;
                            dataUsers.Add(dataUser);
                           
                            SaveToDataUser(dataUsers, filePath);
                        }
                        else
                        {
                            SaveToDataUser(dataUsers, filePath);
                        }
                            isRunning = false;
                            break;
                    }
                }
                while (isRunning);
        }

        static string UserName()
        {
            while (true)
            {
                Console.Write("Введите имя: ");
                string name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    name = name.Trim();
                    return name;
                }
                else
                {
                    Console.WriteLine("Некоректные данные.");
                }
            }
        }

        static decimal BalanceUser() // Инициализация баланса пользователя и пополнение
        {
            while (true)
            {
                Console.Write("Введите сумму: ");
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out decimal balance))
                {
                    return balance;
                }
                else
                {
                    Console.WriteLine("Некоректные данные");
                }
            }
        }

        static decimal Purchases(DataUser dataUser) // Добавление покупок 
        {
            string tovar = TovarName();
            decimal price = PriceTovar(dataUser.Balance);

            dataUser.HistoryGoods.Add(new ModelGood
            {
                TovarName = tovar,
                Price = price
            });

            return dataUser.Balance - price;
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


        static void HistoryPurchases(DataUser dataUser) // Просмотр истории покупок 
        {
            foreach (var good in dataUser.HistoryGoods)
            {
                Console.WriteLine($"{good.TovarName} - {good.Price} руб");
            }
            Console.WriteLine();
        }

        static void SaveToDataUser(List<DataUser> dataUsers, string filePath) // Сохранение в файл
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