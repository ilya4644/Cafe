using System.Data;
using System.Data.SQLite;

namespace Cafe
{
    internal static class Database
    {
        public static void GetNewDatabase(SQLiteConnection connection)
        {
            var command = new SQLiteCommand(connection)
            {
                CommandText = "CREATE TABLE IF NOT EXISTS [menu] " +
                              "([name] TEXT," +
                              "[description] TEXT," +
                              "[ingredients] TEXT," +
                              "[calories] INT," +
                              "[cost] INT);"
            };
            command.ExecuteNonQuery();
        }

        public static void AddNewItem(SQLiteConnection connection, string name, string description, string ingredients, int calories, int cost)
        {
            var command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO menu (name, description, ingredients, calories, cost) " +
                              "VALUES (:name, :description, :ingredients, :calories, :cost)";
            command.Parameters.AddWithValue("name", name);
            command.Parameters.AddWithValue("description", description);
            command.Parameters.AddWithValue("ingredients", ingredients);
            command.Parameters.AddWithValue("calories", calories);
            command.Parameters.AddWithValue("cost", cost);
            command.ExecuteNonQuery();
        }

        public static void DeleteItem(SQLiteConnection connection, string name)
        {
            var command = new SQLiteCommand(connection);
            command.CommandText = "DELETE FROM menu WHERE name=:name";
            command.Parameters.AddWithValue("name", name);
            command.ExecuteNonQuery();
        }

        public static DataTable FilteredList(SQLiteConnection connection, string filter, int limit)
        {
            var command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM menu WHERE {filter} <= {limit}";
            DataTable filteredList = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(filteredList);
            return filteredList;
        }

        public static void EditItem(SQLiteConnection connection, string name, string editedName, string editedDescription, string editedIngredients, int editedCalories, int editedCost)
        {
            var element = GetElement(new SQLiteConnection("Data Source=menu.db"), name);
            var editName = editedName ?? name;
            var editDescription = editedDescription ?? element.Rows[0][1];
            var editIngredients = editedIngredients ?? element.Rows[0][2];
            var editCalories = editedCalories != 0 ? editedCalories : element.Rows[0][3];
            var editCost = editedCost != 0 ? editedCost : element.Rows[0][4];
            var command = new SQLiteCommand(connection);
            command.CommandText = $"UPDATE menu" +
                                  $"SET name={editName}, description={editDescription}," +
                                  $"ingredients={editIngredients}, calories={editCalories}," +
                                  $"cost={editCost}," +
                                  $"WHERE name={name}";
            command.ExecuteNonQuery();
        }

        public static DataTable GetElement(SQLiteConnection connection, string name)
        {
            var command = new SQLiteCommand(connection);
            command.CommandText = $"SELECT * FROM menu WHERE name = {name}";
            DataTable element = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(element);
            return element;
        }
    }
}