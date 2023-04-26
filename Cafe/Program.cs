using System;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;

namespace Cafe
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("1) Сортировка по калориям\n2) Сортировка по цене\n3) Добавление блюда\n4) Удаление блюда\n5) Изменение блюда");
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
            var maximum = Convert.ToInt32(Console.ReadLine());
            using (var connection = new SqliteConnection("Data Source=menu.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM menu WHERE calories <= {maximum}";
                command.ExecuteNonQuery();
            }
        }

        static void CostSort()
        {
            Console.Write("Введите максимальную цену: ");
            var maximum = Convert.ToInt32(Console.ReadLine());
            using (var connection = new SqliteConnection("Data Source=menu.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM menu WHERE cost <= {maximum}";
                command.ExecuteNonQuery();
            }
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
            using (var connection = new SqliteConnection("Data Source=menu.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO menu (name, description, ingredients, calories, cost) " +
                                      $"VALUES ({name}, {description}, {ingredients}, {calories}, {cost})";
                command.ExecuteNonQuery();
            }
        }
        static void DeleteName()
        {
            Console.Write("Введите название блюда для удаления: ");
            var name = Console.ReadLine();
            using (var connection = new SqliteConnection("Data Source=menu.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM menu WHERE name = {name}";
                command.ExecuteNonQuery();
            }
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

            using (var connection = new SqliteConnection("Data Source=menu.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE menu" +
                                      $"SET name={editedName}, description={editedDescription}," +
                                      $"ingredients={editedIngredients}, calories={editedCalories}," +
                                      $"cost={editedCost}," +
                                      $"WHERE name={name}";
                command.ExecuteNonQuery();
            }
        }
    }
}