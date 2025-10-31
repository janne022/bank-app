using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using bank_app.Models.Users;
using bank_app.Utility;

namespace bank_app.Utility.UI.Components
{
    internal class Feed<T> : UIComponent
    {
        public List<T> FeedContents { get; private set; }
        private int FeedLines;
        private int FeedColumns;
        private int _pastEntries;
        private int _currentIndex;

        public Feed(List<T> listOfObjects, int lines, int columns)
        {
            FeedContents = listOfObjects;
            FeedLines = lines;
            FeedColumns = columns;
            _pastEntries = 0;
            Width = 50;
            Height = 3;
            X = 0;
            Y = 0;
        }


        public override (int, int) Pressed()
        {
            while (true)
            {
                Console.CursorVisible = false;
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                switch (pressed.Key)
                {
                    case ConsoleKey.Enter:
                        break;

                    case ConsoleKey.Escape:
                        return (0, 0);

                    case ConsoleKey.PageUp:
                        Scroll(UpOrDown.Up);
                        break;

                    case ConsoleKey.PageDown:
                        Scroll(UpOrDown.Down);
                        break;
                }
                Render();
            }
        }

        public override void Measure()
        {
            
        }

        public void Scroll(UpOrDown direction)
        {
            //Console.Clear(); // TAKE THIS AWAY AFTER TESTING OMG
            if (direction == UpOrDown.Up && _pastEntries > 0)
            {
                _pastEntries--;
            }

            if (direction == UpOrDown.Down && _pastEntries < FeedContents.Count - FeedLines)
            {
                _pastEntries++;
            }

        }

        public override void Render()
        {
            Render(0);
        }

        public void Render(int startingNumber)
        {
            if (startingNumber > 0)
            {
                _pastEntries = startingNumber;
            }

            string objectType = string.Empty;

            if (FeedContents is List<Client> clients)
            {
                objectType = "client";

                Console.SetCursorPosition(0, 0);
                var relevantClients = clients
                    .Skip(_pastEntries)
                    .Take(FeedLines);
                Console.WriteLine($"{"Name",-15}{"Phone Number",-15}{"Email",-25}");
                for (int i = 0; i < 50; i++)
                {
                    ColourManager.Write("─", ColourFG.None, ColourBG.None);
                }
                Console.WriteLine(new string('─', 50));
                foreach (var client in relevantClients)
                {
                    ColourManager.Write($"{client.UserName,-15}{client.PhoneNumber,-15}{client.Email,-25}\n", ColourFG.None, ColourBG.None);
                }


                ColourManager.Write($"\n{_pastEntries + 1}-{Math.Min(_pastEntries + FeedLines, clients.Count)} of {clients.Count} {objectType}s.", ColourFG.None, ColourBG.None);


            }

            //if (FeedContents is List<Transaction> transactions)
            //{
            //    objectType = "transaction";
            //    Console.SetCursorPosition(0, 0);
            //    var relevantTransactions = transactions
            //        .Skip(_pastEntries)
            //        .Take(FeedLines);
            //    Console.Write($"{"Name",-15}{"Phone Number",-15}{"Email",-25}");
                
            //    Console.Write(new string('─', 50));
            //    foreach (var transaction in relevantTransactions)
            //    {
            //        //ColourManager.Write($"{transaction.SenderId.ToString(),-15}{transaction.PhoneNumber,-15}{transaction.Email,-25}\n", ColourFG.None, ColourBG.None);
            //    }


            //    ColourManager.Write($"\n{_pastEntries + 1}-{Math.Min(_pastEntries + FeedLines, transactions.Count)} of {transactions.Count} {objectType}s.", ColourFG.None, ColourBG.None);

            //}
        }
    }
}
