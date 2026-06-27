using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace PersonalFinance
{
    class Program
    {
        static void Main(string[] argg)
        {
            // добавить ? чтобы пользователь мог получить только свои данные
            // Сделать чтобы баланс расчитывался из пополнений и покупок
            // сделать один класс Operations и в нем регулировать доходы и расходы.


            List<DataUser> dataUsers = new List<DataUser>();
            DataUser dataUser = new DataUser();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "dataUser.txt");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                dataUsers = JsonSerializer.Deserialize<List<DataUser>>(json) ?? new List<DataUser>();
            }

            string name = UserName();
            string settingsUser = "";

            bool isNewUser = false;
            dataUser = dataUsers.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (dataUser != null)
            {
                Console.WriteLine($"\nС возвращением, {dataUser.Name}\nБаланс: {dataUser.Balance} руб");
                settingsUser = "5 - Удалить аккаунт";
                isNewUser = false;
            }
            else
            {
                dataUser = new DataUser() { Name = name, Balance = 0, HistoryTovars = new List<ModelTovar>(), Incomes = new List<Income>() };
                isNewUser = true;
            }

                bool isRunning = true;
            
            do
            {
                Console.WriteLine("\n1 - Добавить баланс\n2 - Добавить покупку\n3 - Просмотреть историю операций\n4 - Выход\n" + settingsUser);
                string input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        dataUser.Balance += BalanceUser(dataUser);
                        Console.WriteLine($"\nБаланс: {dataUser.Balance} руб");
                        break;
                    
                    case "2":
                        if (dataUser.Balance == 0)
                        {
                            Console.WriteLine("Ваш баланс отрицательный. Добавьте баланс.");
                            break;
                        }
                        dataUser.Balance = Purchases(dataUser);
                        Console.WriteLine($"\nБаланс: {dataUser.Balance} руб");
                        break;
                    
                    case "3":
                        if (dataUser.HistoryTovars.Count == 0 && dataUser.Incomes.Count == 0)
                        {
                            Console.WriteLine("Операций еще нет.\n");
                            break;
                        }
                        HistoryOperations(dataUser);
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
                    
                    case "5":
                        bool isDelete = DeleteAcount();
                        if (isDelete)
                        {
                            Console.WriteLine("До новых встреч !");
                            dataUsers.Remove(dataUser);

                            SaveToDataUser(dataUsers, filePath);
                        }
                        else
                        {
                            Console.WriteLine("Спасибо что остаетесь с нами !");
                        }
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Некоректные данные.");
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

        static bool DeleteAcount()
        {
           while (true) 
           {
                Console.WriteLine("\nДействие не обратимо. Вы уверены?\n1 - Да\n2 - Отменить выбор");
                string deleteAcount = Console.ReadLine();

                if (deleteAcount == "1")
                    return true;
                else if (deleteAcount == "2")
                    return false;
                else
                    Console.WriteLine("Некоректные данные");
            }
        }

        static decimal BalanceUser(DataUser dataUser) // Инициализация баланса пользователя и пополнение
        {
            while (true)
            {
                Console.Write("\nx - Вернуться в меню\nВведите сумму: ");
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out decimal balance))
                {
                    dataUser.Incomes.Add(new Income
                    {
                        Amount = balance,
                        IncomeDate = DateTime.Now
                    });
                    return balance;
                }
                else if (input.ToLower() == "x")
                {
                    return 0;
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
            if (tovar == "")
                return dataUser.Balance;

            decimal price = PriceTovar(dataUser.Balance);
            if (price == 0)
                return dataUser.Balance;

            dataUser.HistoryTovars.Add(new ModelTovar
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
                Console.Write("\nx - Вернуться в меню\nВведите название покупки: ");
                string tovar = Console.ReadLine();
                
                if (tovar.ToLower() != "x" && !string.IsNullOrWhiteSpace(tovar))
                {
                    tovar = tovar.Trim();
                    return tovar;
                }
                else if (tovar.ToLower() == "x")
                {
                    return "";
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
                Console.Write("\nx - Вернуться в меню\nВведите сумму покупки: ");
                string priceTovar = Console.ReadLine();

                if (decimal.TryParse(priceTovar, out decimal price) && price <= balance && price != 0)
                {
                    return price;
                }
                else if (priceTovar.ToLower() == "x")
                {
                    return 0;
                }
                else
                {
                    Console.WriteLine("Некоректные данные.");
                }
            }
        }

        static void HistoryOperations(DataUser dataUser) // Просмотр истории покупок 
        {
            Console.WriteLine("\nПокупки: ");
            foreach (var good in dataUser.HistoryTovars)
            {
                Console.WriteLine($"{good.TovarName} - {good.Price} руб");
            }
            Console.WriteLine("\nДоход: ");

            foreach (var income in dataUser.Incomes)
            {
                Console.WriteLine($"+{income.Amount} - {income.IncomeDate.ToString("dd.MM.yyyy")}");
            }
        }

        static void SaveToDataUser(List<DataUser> dataUsers, string filePath) // Сохранение в файл
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string json = JsonSerializer.Serialize(dataUsers, options);
            try
            {
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}