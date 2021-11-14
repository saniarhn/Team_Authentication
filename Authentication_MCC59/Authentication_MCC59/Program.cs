using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Authentication_MCC59
{
    class Program
    {
        static Dictionary<string, UserData> Confidential = new();

        static Random Rnd = new ();

        static void Main(string[] args)
        {
            bool start = true;
            while (start)
            {
                Menu();
                Console.Write("Choose Your Action : ");
                int choose = NumInput();
                switch (choose)
                {
                    case 1:
                        Console.Clear();
                        InputData();
                        break;
                    case 2:
                        Console.Clear();
                        ShowUserData();
                        break;
                    case 3:
                        Console.Clear();
                        Edit();
                        break;
                    case 4:
                        Console.Clear();
                        Search();
                        break;
                    case 5:
                        Console.Clear();
                        Login();
                        break;
                    case 6:
                        Delete();
                        break;
                    case 7:
                        start = false;
                        break;
                    default:
                        break;
                }
            }
        }

        static void InputData()
        {
            Console.WriteLine("++++++ Create User ++++++");
            Console.WriteLine("+++++++++++++++++++++++++");
            Console.Write("Input First Name : ");
            string firstName = Console.ReadLine();
            Console.Write("Input Last Name  : ");
            string lastName = Console.ReadLine();

            Console.Write("Input Password   : ");
            string passwordTemp = Console.ReadLine();
            string password = InputPassword(passwordTemp);
            try
            {
                string tempId = firstName.Substring(0, 2) + lastName.Substring(0, 2);
                string id = Makeid(tempId);
                Confidential.Add(id, new UserData(firstName, lastName, password, id));
                Console.Clear();
                Console.WriteLine("\t++++++ Data User ++++++");
                Console.WriteLine("\t+++++++++++++++++++++++++");
                Console.WriteLine("\tYour Account Have Been Made");
                Console.WriteLine($"\tYour ID : {id}");
                Console.WriteLine($"\tYour Password {passwordTemp}");
                Console.WriteLine("\t+++++++++++++++++++++++++\n");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Clear();
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("Please Input Name With More Than 1 Character");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++");
            }
        }

        static string Makeid(string id)
        {
            string idTemp = id;
            bool start = Confidential.ContainsKey(id);
            while (start)
            {
                Console.WriteLine("Same ID");
                int randomNumber1 = Rnd.Next(11, 99);
                idTemp = id + randomNumber1;
                start = Confidential.ContainsKey(idTemp);
            }
            return idTemp;
        }

        static void ShowUserData()
        {
            foreach (var item in Confidential)
            {
                Console.WriteLine("\t++++++ Show User ++++++");
                Console.WriteLine("\t+++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine($"\tFull Name  : {item.Value.FirstName} {item.Value.LastName}");
                Console.WriteLine($"\tID : {item.Value.Id}");
                Console.WriteLine($"\tPassword   : {item.Value.Password}\n");
                Console.WriteLine("\t+++++++++++++++++++++++++++++++++++++++++++++++++++++");
            }
        }

        static void Search()
        {
            Console.WriteLine("++++++ Search User ++++++");
            Console.WriteLine("+++++++++++++++++++++++++++");
            Console.Write("Input ID : ");
            string name = Console.ReadLine();
            foreach (var item in Confidential)
            {
                if (item.Value.FirstName == name || item.Value.LastName == name || item.Value.Id == name)
                {
                    Console.WriteLine("\t+++++++++++++++++++++++++++");
                    Console.WriteLine($"\tFirst Name : {item.Value.FirstName}");
                    Console.WriteLine($"\tLast Name  : {item.Value.LastName}");
                    Console.WriteLine($"\tID         : {item.Value.Id}");
                    Console.WriteLine($"\tPassword   : {item.Value.Password}\n");
                    Console.WriteLine("\t+++++++++++++++++++++++++++");
                }
                else
                {
                    Console.WriteLine("User Not Found");
                }
            }
        }

        static void Menu()
        {
            Console.WriteLine("\t++++++ Basic Authentication ++++++");
            Console.WriteLine("\t++++++++++++++++++++++++++++++++++");
            Console.WriteLine("\t1. Create User");
            Console.WriteLine("\t2. Show User");
            Console.WriteLine("\t3. Edit User");
            Console.WriteLine("\t4. Search User");
            Console.WriteLine("\t5. Login User");
            Console.WriteLine("\t6. Delete User");
            Console.WriteLine("\t7. Exit App");
            Console.WriteLine("\t++++++++++++++++++++++++++++++++++\n");
        }

        static void MenuEdit()
        {
            Console.WriteLine("\t++++++ Edit User ++++++");
            Console.WriteLine("\t+++++++++++++++++++++++++++");
            Console.WriteLine("\t1. Edit Username");
            Console.WriteLine("\t2. Edit FirstName");
            Console.WriteLine("\t3. Edit Lastname");
            Console.WriteLine("\t4. Edit Password");
            Console.WriteLine("\t+++++++++++++++++++++++++++");
        }

        static int NumInput()
        {
            int a = 0;
            try
            {
                a = int.Parse(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine("");
                Console.WriteLine("+++++++++++++++++++++++++++++++++");
                Console.WriteLine("Please Enter The Correct Number!! " + e);
                Console.WriteLine("+++++++++++++++++++++++++++++++++");
            }
            return a;
        }

        static void Login()
        {
            Console.WriteLine("++++++ Login User ++++++");
            Console.WriteLine("+++++++++++++++++++++++++++");
            Console.Write("Input ID : ");
            string id = Console.ReadLine();
            Console.Write("Input Password : ");
            string pass = ReadPassword();
            Console.WriteLine("+++++++++++++++++++++++++++");
            try
            {
                if (BCrypt.Net.BCrypt.Verify(pass, Confidential[id].Password))
                {
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++++++++++++++++++++++++++");
                    Console.WriteLine("Login Successful!");
                    Console.WriteLine("++++++++++++++++++++++++++++++++++\n");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("++++++++++++++++++++++++++++++++++");
                    Console.WriteLine("Password isn't match, Please Try Again!");
                    Console.WriteLine("++++++++++++++++++++++++++++++++++\n");
                }
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("");
                Console.WriteLine("++++++++++++++++++++++++++++++++++");
                Console.WriteLine("ID Not Found, Please Try Again!");
                Console.WriteLine("++++++++++++++++++++++++++++++++++\n");
            }
        }

        static bool PasswordCheck(string input)
        {
            // Jelasin alasan method ini dibuat
            var hashNumber = new Regex(@"[0-9]");
            var upperChar = new Regex(@"[A-Z]");
            var loweChar = new Regex(@"[a-z]");
            int passwordLength = 8;

            bool start = false;

            if (!hashNumber.IsMatch(input))
            {
                Console.WriteLine("");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("Password Must Have at Least 1 Numeric Value");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++");
            }
            else if (!upperChar.IsMatch(input))
            {
                Console.WriteLine("");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("Password Must Have at Least 1 Upper Case (A-Z)");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++");
            }
            else if (!loweChar.IsMatch(input))
            {
                Console.WriteLine("");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("Password Must Have at Least 1 Lower Case (a-z)");
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++");
            }
            else if (input.Length < passwordLength)
            {
                Console.WriteLine("");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("Password Must Have at Least 8 Character Consist of 1 Upper Case, 1 Lower Case, 1 Numeric Value");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            }
            else
            {
                start = true;
            }
            return start;
        }

        static string InputPassword(string passwordTemp)
        {
            bool start = true;
            bool start2 = true;
            string password = null;
            while (start)
            {
                if (PasswordCheck(passwordTemp))
                {
                    Console.Write("comfrim your password password : ");
                    string passwordTemp2 = Console.ReadLine();
                    while (start2)
                    {
                        if (passwordTemp == passwordTemp2)
                        {
                            password = BCrypt.Net.BCrypt.HashPassword(passwordTemp);
                            start = false;
                            start2 = false;
                        }
                        else
                        {
                            Console.Write("Please enter you comfimation password again : ");
                            passwordTemp2 = Console.ReadLine();
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine("Password Must Have at Least 8 Character Consist of 1 Upper Case, 1 Lower Case, 1 Numeric Value");
                    Console.WriteLine("Please Reinsert The Password :)");
                    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    Console.Write("Input Password   :");
                    passwordTemp = Console.ReadLine();
                }
            }
            return password;
        }

        static void Delete()
        {
            ShowUserData();
            Console.WriteLine(" ");
            Console.WriteLine("++++++ Delete User ++++++");
            Console.WriteLine("+++++++++++++++++++++++++++");
            Console.Write("Delete Data (Input Number) :");
            string Name = Console.ReadLine();
            if (Confidential.ContainsKey(Name))
            {
                Confidential.Remove(Name);
                Console.WriteLine("");
                Console.WriteLine("\t+++++++++++++++++++++++++++");
                Console.WriteLine("\tData Deleted!");
                Console.WriteLine("\t+++++++++++++++++++++++++++");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("");
                Console.WriteLine("\t+++++++++++++++++++++++++++");
                Console.WriteLine("\tID Not Found");
                Console.WriteLine("\t+++++++++++++++++++++++++++");
            }
            ShowUserData();
        }

        static void Edit()
        {
            Console.Write("Input Id : ");
            string id = Console.ReadLine();
            if (Confidential.ContainsKey(id))
            {
                MenuEdit();
                Console.Write("Choose your action : ");
                int choose = NumInput();
                switch (choose)
                {
                    case 1:
                        Console.WriteLine("Insert new id : ");
                        string nameTmp = Console.ReadLine();
                        if (Confidential.ContainsKey(nameTmp))
                        {
                            Console.Write("Id have been taken : ");
                        }
                        else
                        {
                            Confidential[id].Id = nameTmp; 
                        }
                        break;
                    case 2:
                        Console.Write("Insert new Firstname : ");
                        string firstTmp = Console.ReadLine();
                        Confidential[id].FirstName = firstTmp;
                        break;
                    case 3:
                        Console.Write("Insert new Lastname : ");
                        string lastTmp = Console.ReadLine();
                        Confidential[id].LastName = lastTmp;
                        break;
                    case 4:
                        string passwordTemp = Console.ReadLine();
                        string password = InputPassword(passwordTemp);
                        Console.Write("Insert new Password : ");
                        Confidential[id].Password = password;
                        break;
                    default:
                        break;
                }
            }
        }

        static string ReadPassword()
        {
            // sourcode : https://stackoverflow.com/questions/29201697/hide-replace-when-typing-a-password-c/29201791
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }
    }
}
  
