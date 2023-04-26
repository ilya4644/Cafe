using System;
using System.Data.SQLite;

namespace Cafe
{
    class Program
    {
        static public SQLiteConnection connection;

        static public bool Connect(string fileName)
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + fileName + ";Version=3; FailIfMissing=False");
                connection.Open();
                return true;
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
                return false;
            }
        }
        
        public static void Main(string[] args)
        {
            if (Connect("cafe.db"))
            {
                Database.GetNewDatabase(connection);
            }
            else
            {
                Console.WriteLine("Отключено");
            }
            Console.WriteLine("1) Сортировка по калориям\n2) Сортировка по цене\n3) Добавление блюда\n" +
                              "4) Удаление блюда\n5) Изменение блюда");
            Console.Write("Выберите действие: ");
            var action = Convert.ToInt32(Console.ReadLine());
            switch (action)
            {
                case 1:
                    CaloriesSort();
                    break;
                case 2:
                    CostSort();
                    break;
                case 3:
                    AddName();
                    break;
                case 4:
                    DeleteName();
                    break;
                case 5:
                    EditName();
                    break;
            }
        }
        
        static void CaloriesSort()
        {
            Console.Write("Введите максимальное значение калорий: ");
            var limit = Convert.ToInt32(Console.ReadLine());
            var filteredList = Database.FilteredList(connection, "calories", limit);
            filteredList.DefaultView.Sort = "calories desc";
            filteredList = filteredList.DefaultView.ToTable();
            Console.WriteLine($"Блюда, калорийность которых ниже {limit}:");
            var summaryCost = 0;
            for (var i = 0; i <= filteredList.Rows.Count - 1; i++)
            {
                var item = filteredList.Rows[i];
                var name = item[0];
                var description = item[1];
                var ingredients = item[2];
                var calories = item[3];
                var cost = item[4];
                summaryCost += (int)cost;
                Console.WriteLine($"Название блюда: {name}\nОписание: {description}\nИнгредиенты: {ingredients}\n" +
                                  $"Калории: {calories}\nСтоимость: {cost}\n");
            }
            Console.WriteLine($"Суммарная стоимость: {summaryCost}");
        }

        static void CostSort()
        {
            Console.Write("Введите максимальную цену: ");
            var limit = Convert.ToInt32(Console.ReadLine());
            var filteredList = Database.FilteredList(connection, "cost", limit);
            filteredList.DefaultView.Sort = "cost desc";
            filteredList = filteredList.DefaultView.ToTable();
            Console.WriteLine($"Блюда, калорийность которых ниже {limit}:");
            var summaryCalories = 0;
            for (var i = 0; i <= filteredList.Rows.Count - 1; i++)
            {
                var item = filteredList.Rows[i];
                var name = item[0];
                var description = item[1];
                var ingredients = item[2];
                var calories = item[3];
                var cost = item[4];
                summaryCalories += (int)calories;
                Console.WriteLine($"Название блюда: {name}\nОписание: {description}\nИнгредиенты: {ingredients}\n" +
                                  $"Калории: {calories}\nСтоимость: {cost}\n");
            }
            Console.WriteLine($"Суммарная калорийность: {summaryCalories}");
        }
        static void AddName()
        {
            Console.Write("Введите название блюда: ");
            var name = Console.ReadLine();
            Console.Write("Введите описание блюда: ");
            var description = Console.ReadLine();
            Console.Write("Введите ингридиенты блюда:");
            var ingredients = Console.ReadLine();
            Console.Write("Введите калорийность блюда: ");
            var calories = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите цену блюда: ");
            var cost = Convert.ToInt32(Console.ReadLine());
            Database.AddNewItem(connection, name, description, ingredients, calories, cost);
        }
        static void DeleteName()
        {
            Console.Write("Введите название блюда для удаления: ");
            var name = Console.ReadLine();
            Database.DeleteItem(connection, name);
        }
        static void EditName()
        {
            Console.Write("Введите название блюда для редактирования: ");
            var name = Console.ReadLine();
            string editedDescription = null;
            string editedIngredients = null;
            string editedName = null;
            var editedCalories = 0;
            var editedCost = 0;
            Console.WriteLine("1) Изменить название блюда\n2) Изменить описание блюда\n3) Изменить ингридиенты блюда\n" +
                              "4) Изменить количество калорий блюда\n5) Изменить стоимость блюда\n6) Выход");
            var action = 0;
            while (action != 6)
            {
                Console.Write("Выберите действие: ");
                action = Convert.ToInt32(Console.ReadLine());
                switch (action)
                {
                    case 1:
                        Console.Write("Введите новое название блюда: ");
                        editedName = Console.ReadLine();
                        break;
                    case 2:
                        Console.Write("Введите новое описание блюда: ");
                        editedDescription = Console.ReadLine();
                        break;
                    case 3:
                        Console.Write("Введите новые ингридиенты: ");
                        editedIngredients = Console.ReadLine();
                        break;
                    case 4:
                        Console.Write("Введите новую калорийность блюда: ");
                        editedCalories = Convert.ToInt32(Console.ReadLine());
                        break;
                    case 5:
                        Console.Write("Введите новую стоимость блюда: ");
                        editedCost = Convert.ToInt32(Console.ReadLine());
                        break;
                    case 6:
                        Console.WriteLine("Выход...");
                        break;
                    default:
                        Console.WriteLine("Такого действия не существует.");
                        break;
                }
            }
            Database.EditItem(connection, name, editedName, editedDescription, 
                editedIngredients, editedCalories, editedCost);
        }
    }
}