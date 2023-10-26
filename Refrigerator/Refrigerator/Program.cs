using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refrigerator
{
    internal class Program
    {
        static void PrintMenu()
        {
            Console.WriteLine("Enter 1 to print all the items in the refrigerator and all its contents.\r\n" +
                "Enter 2 to print how many space is left in the refrigerator.\r\n" +
                "Enter 3 to put an item in the refrigerator.\r\n" +
                "Enter 4 to remove an item from the refrigerator.\r\n" +
                "Enter 5 to clean the refrigerator and print all the checked items.\r\n" +
                "Enter 6 if you what to eat somthing.\r\n" +
                "Enter 7 to print all the items sorted by their expiration date.\r\n" +
                "Enter 8 to print all the shelves sorted according the free space left on them.\r\n" +
                "Enter 9 to print all the refrigerators sorted according the free space left on them.\r\n" +
                "Enter 10: to prepare the refrigerator for shopping.\r\n" +
                "Enter 100 to exit.");
        }

        static void Main(string[] args)
        {
            List<Item> items = Item.InitializeItems();
            Refrigerator refrigerator = new Refrigerator(ModelType.RF01, RefrigeratorColor.Gray);
            PrintMenu();
            var input = Console.ReadLine();
            while (input != null)
            {
                try
                {
                    if (int.TryParse(input, out int inputValue))
                    {
                        switch (inputValue)
                        {
                            case 1:
                                Console.WriteLine(refrigerator.PrintRefrigeratorWithContents().ToString()); break;
                            case 2:
                                Console.WriteLine(refrigerator.HowMuchSpaceIsLeft()); break;
                            case 3:
                                var item = GetUserInput("Please enter an item that you want to add to the refrigerator");
                                _ = int.TryParse(item, out int itemValue);
                                var itemToAdd = Item.GetItemByID(itemValue);
                                refrigerator.AddItem(itemToAdd);
                                break;
                            case 4:
                                item = GetUserInput("Please enter the number of the item you want to take out of the refrigerator");
                                _ = int.TryParse(item, out int itemIDValue);
                                refrigerator.RemoveItem(itemIDValue);
                                break;
                            case 5:
                                var itemsToTrowAway = refrigerator.CleanRefrigerator();
                                if (itemsToTrowAway.Count != 0)
                                    Console.WriteLine("Items thrown: \n" + string.Join("\n", itemsToTrowAway));
                                else
                                    Console.WriteLine("The refrigerator is already clean, " +
                                        "there are no items that need to be thrown away");
                                break;
                            case 6:
                                (var KosherValue, var typeValue) = AskUserKosherAndType();
                                var itemsToEat = refrigerator.FeelLikeEating(KosherValue, typeValue);
                                if (itemsToEat.Count != 0)
                                    Console.WriteLine("items to eat: " + string.Join(", ", itemsToEat));
                                else
                                    Console.WriteLine("Unfortunately, there is no relevant food of the kosher and type " +
                                        "you mentioned in the refrigerator.");
                                break;
                            case 7:
                                Console.WriteLine(string.Join("\n", Item.SortItems()));
                                break;
                            case 8:
                                Console.WriteLine(string.Join("\n", Shelf.SortShelves()));
                                break;
                            case 9:
                                Console.WriteLine(string.Join("\n", Refrigerator.SortRefrigerators()));
                                break;
                            case 10:
                                refrigerator.GettingReadyForShopping();
                                break;
                            case 100:
                                Console.WriteLine("Exit, bye bye");
                                return;
                            default:
                                throw new Exception("Please enter number between 1-10 or 100 to exit");
                        }
                    }
                    else
                        throw new Exception("Please enter number between 1-10 or 100 to exit");
                    input = Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    input = null;
                }
            }
        }
        static string GetUserInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
        static (KosherType, ItemType) AskUserKosherAndType()
        {
            var kosher = GetUserInput("Please enter kosher of food");
            kosher = UpperFirst(kosher);
            var ans = Enum.TryParse(kosher, out KosherType kosherValue);
            if (!ans)
                throw new Exception("Please enter Meat, Dairy or Parve");

            var type = GetUserInput("Please enter type of food");
            type = UpperFirst(type);
            ans = Enum.TryParse(type, out ItemType typeValue);
            if (!ans)
                throw new Exception("Please enter Food or Drink");

            return (kosherValue, typeValue);
        }
        public static string UpperFirst(string s)
        {
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
    }
}
