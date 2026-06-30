using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace PersonalFinance
{
    class Program
    {
        static void Main(string[] argg)
        {
            // добавить ? чтобы пользователь мог получить только свои данные


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
                dataUser = new DataUser { Name = name, Balance = 0, Operations = new List<Operation>() };
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
                        if (dataUser.Operations.Count == 0)
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
            string nameOfIncome = NameIncomeOrExpence();
            if (nameOfIncome == "")
                return dataUser.Balance;

            decimal income = TheSumOfIncome();
            if (income == 0)
                return dataUser.Balance;

            dataUser.Operations.Add(new Operation
            {
                Name = nameOfIncome,
                Amount = income,
                DateTime = DateTime.Now,
                OperationType = OperationType.Доход
            });

            return income;
        }

        static decimal TheSumOfIncome()
        {
            while(true)
            {
                Console.Write("\nx - Вернуться в меню\nВведите сумму пополнения: ");
                string input = Console.ReadLine();
                
                if (decimal.TryParse(input, out decimal sumIncome))
                {
                    return sumIncome;
                }
                else if (input.ToLower() == "x")
                {
                    return 0;
                }
                else
                {
                    Console.WriteLine("Некоректные данные.");
                }
            }
        }

        static decimal Purchases(DataUser dataUser) // Добавление покупок 
        {
            string tovar = NameIncomeOrExpence();
            if (tovar == "")
                return dataUser.Balance;

            decimal price = PriceTovar(dataUser.Balance);
            if (price == 0)
                return dataUser.Balance;

            dataUser.Operations.Add(new Operation
            {
                Name = tovar,
                Amount = price,
                DateTime = DateTime.Now,
                OperationType = OperationType.Расход
            });

            return dataUser.Balance - price;
        }

        static string NameIncomeOrExpence() // Возвращает название операции (источник дохода-навание товара)
        {
            while (true)
            {
                Console.Write("\nx - Вернуться в меню\nВведите название операции: ");
                string name = Console.ReadLine();

                if (name.ToLower() != "x" && !string.IsNullOrWhiteSpace(name))
                {
                    name = name.Trim();
                    return name;
                }
                else if (name.ToLower() == "x")
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
            Console.WriteLine();
            foreach (var operation in dataUser.Operations)
            {
                string sign = operation.OperationType == OperationType.Доход ? "+" : "-";
                Console.WriteLine($"{operation.DateTime.ToString("dd.MM.yyyy")} {operation.OperationType} {sign}{operation.Amount} {operation.Name}");
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