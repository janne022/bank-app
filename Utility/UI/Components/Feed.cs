using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
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
        private string _objectType;
        private int _currentIndex;

        public Feed(List<T> listOfObjects, int lines, int columns)
        {
            FeedContents = listOfObjects;
            FeedLines = lines;
            FeedColumns = columns;
            _pastEntries = 0;

            //if (listOfObjects is List<Client>)
            //{
            //    _objectType = "client";
            //}
            //if (listOfObjects is List<Transaction>)
            //{
            //    _objectType = "transaction";
            //}

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

                    case ConsoleKey.UpArrow:
                        Scroll(UpOrDown.Up);
                        break;

                    case ConsoleKey.DownArrow:
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

            int allEntries = FeedContents.Count;

            if (direction == UpOrDown.Up)
            {
                if (_currentIndex > 0)
                {
                    _currentIndex--;

                    // Scroll if need be
                    if (_currentIndex < _pastEntries)
                    {
                        _pastEntries--;
                    }
                }
            }

            if (direction == UpOrDown.Down)
            {
                if (_currentIndex < allEntries - 1)
                {
                    _currentIndex++;

                    if (_currentIndex >= _pastEntries + FeedLines)
                    {
                        _pastEntries++;
                    }
                }
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

            /*  Thoughts for the future: 
            *
            *   Save object properties into a List for use generically?
            *   Could be an idea to be able to get away from needing to
            *   write so much code over and over.
            * 
            *   For now, testing continues with Clients.
            */


            // displayFeed[object][property]
            //List<string[]> displayFeed = new List<string[]>();

            //if (FeedContents is List<Client> clients)
            //{

                Console.SetCursorPosition(0, 0);
                //var relevantLines = 
                //    .Skip(_pastEntries)
                //    .Take(FeedLines);
                               
                
                Console.WriteLine($"{"  Name",-15}{"Phone Number",-15}{"Email",-25}");
                for (int i = 0; i < 50; i++)
                {
                    ColourManager.Write("─", ColourFG.None, ColourBG.None);
                }
                Console.WriteLine(new string('─', 50));
                //foreach (var client in relevantClients)
                //{
                //    if (!String.IsNullOrEmpty(client.UserName) &&
                //        !String.IsNullOrEmpty(client.PhoneNumber) &&
                //        !String.IsNullOrEmpty(client.Email)
                //    )
                //    {
                //        displayFeed.Add(new string[]
                //            {
                //                client.UserName,
                //                client.PhoneNumber,
                //                client.Email
                //            }
                //        );
                //    }
                //}
            }

            PresentFeed(displayFeed);


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


        private void PresentFeed(List<string[]> displayFeed)
        {
            for (int i = 0; i < displayFeed.Count; i++)
            {
                int allEntries = _pastEntries + i;

                if (allEntries == _currentIndex)
                {
                    ColourManager.Set(ColourBG.White);
                    ColourManager.Set(ColourFG.Black);
                    ColourManager.Write($"-> {displayFeed[i][0],-13}{displayFeed[i][1],-15}{displayFeed[i][2],-25}\n",
                        ColourFG.None, ColourBG.None);
                }
                else
                {
                    ColourManager.Set(ColourBG.Reset);
                    ColourManager.Set(ColourFG.Reset);
                    ColourManager.Write($"   {displayFeed[i][0],-13}{displayFeed[i][1],-15}{displayFeed[i][2],-25}\n", 
                        ColourFG.None, ColourBG.None);
                }
            }
            ColourManager.Set(ColourBG.Reset);
            ColourManager.Set(ColourFG.Reset);
            ColourManager.Write($"\n{_pastEntries + 1}-{Math.Min(_pastEntries + FeedLines, FeedContents.Count)} " +
                $"of {FeedContents.Count} {_objectType}s.", ColourFG.None, ColourBG.None);
        }
    }
}
