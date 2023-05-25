using System.Text;
using Tomlyn;

namespace NureCistBot.BackendServices
{
    public class EnviromentManager
    {
        public static void Setup()
        {
            while (true)
            {
                Console.WriteLine(
                    "A token is necessary for the work of the bot. To get the token, you need to create a bot here: @BotFather\nThe mysql database is also required for work.");
                Console.Write("Please enter the token from the bot: ");
                string? tokenBot = Console.ReadLine();
                Console.Write("\nSpecify the address to the database: ");
                string? addressDatabase = Console.ReadLine();
                Console.Write("Specify the name of the database: ");
                string? nameDatabase = Console.ReadLine();
                Console.Write("Now the username: ");
                string? nameUserDatabase = Console.ReadLine();
                Console.Write("And the password: ");
                string? passwordUserDatabase = Console.ReadLine();
                Console.Write("NURE CIST API key: ");
                string? apiKey = Console.ReadLine();

                if (tokenBot != null && addressDatabase != null && nameDatabase != null &&
                nameUserDatabase != null && passwordUserDatabase != null && apiKey != null)
                {
                    using (FileStream fstream = new FileStream("config-bot.toml", FileMode.OpenOrCreate))
                    {
                        string configText =
                            String.Format(
                                "botToken = '{0}'\naddressDatabase = '{1}'\nnameDatabase = '{2}'\nnameUserDatabase = '{3}'\npasswordUserDatabase = '{4}'\napiKey = '{5}'",
                                tokenBot,
                                addressDatabase,
                                nameDatabase,
                                nameUserDatabase,
                                passwordUserDatabase,
                                apiKey);
                        Console.WriteLine(configText);
                        byte[] buffer = Encoding.Default.GetBytes(configText);
                        fstream.Write(buffer, 0, buffer.Length);
                        break;
                    }
                }
            }
        }
        public static string ReadBotToken()
        {
            string token;
            using (FileStream fstream = File.OpenRead("config-bot.toml"))
            {
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);

                var model = Toml.ToModel(textFromFile);
                token = (string)model["botToken"];
            }
            return token;
        }
    }
}