using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            InstanceContext context = new InstanceContext(new MyCallback());
            Proxy.ChatServiceClient server = new Proxy.ChatServiceClient(context);
            var x = 0;


            MySqlConnection connection = new MySqlConnection("datasource=localhost;Initial Catalog=clienti;port=3306;username=root;password=pleomax1");
            connection.Open();
            if( connection.State == ConnectionState.Open)
            {
                Console.WriteLine("Connection to Database is open");
                Console.WriteLine("Please insert your username and password:");
                var username = Console.ReadLine();
                var password = Console.ReadLine();

                string selectQuery= $"Select * FROM datelogare Where username = '{username}'";
                using (var cmd = new MySqlCommand(selectQuery, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            x++;
                            var parola = reader.GetString(2);
                            if (password == parola)
                            {
                                MessageBox.Show("Valid credentials. Welcome to chat:");
                            }
                            else if (password != parola)
                            {
                                MessageBox.Show("Incorect password");
                                return ;
                            }
                        }
                        if (x == 0)
                        {
                            reader.Close();
                            string insertQuery = $"Insert into datelogare (username, password) Values ( '{username}', '{password}' )";
                            using (var command = new MySqlCommand(insertQuery, connection))
                            {
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    MessageBox.Show("Your Account does not exist. I will register you to the database now.");
                                }
                                else
                                {
                                    MessageBox.Show("Registration failed");
                                }
                            }
                        }
                    }
                }


                server.Join(username);

            }
            else
            {
                Console.WriteLine("Invalid Connection");
            }

            
            Console.WriteLine();
            Console.WriteLine("Enter Message");
            Console.WriteLine("Press Q to Exit");

            var message = Console.ReadLine();

            while (message != "Q")
            {
                if (!string.IsNullOrEmpty(message))
                    server.SendMessage(message);
                message = Console.ReadLine();
            }

        }
    }
}
