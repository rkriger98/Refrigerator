namespace Refrigerator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;

    public class Refrigerator
    {
        private static int lastID = 0;
        public int RefrigeratorID { get; set; }
        public ModelType Model { get; set; }
        public RefrigeratorColor Color { get; set; }
        public int NumberOfShelves { get; set; }
        public List<Shelf> Shelves { get; set; }
        private static readonly List<Refrigerator> AllRefrigerators = new List<Refrigerator>();

        private Dictionary<ModelType, int> ShelvesByModel = new Dictionary<ModelType, int>
        {
            { ModelType.RF01, 3 },
            { ModelType.RF02, 3 },
            { ModelType.RF03, 3 },
            { ModelType.RF04, 4 },
            { ModelType.RF05, 4 },
            { ModelType.RF06, 4 },
            { ModelType.RF07, 5 },
            { ModelType.RF08, 5 },
            { ModelType.RF09, 5 }
        };

        public Refrigerator(ModelType model, RefrigeratorColor color)
        {
            lastID++;
            _ = ShelvesByModel.TryGetValue(model, out int numberOfShelves);

            RefrigeratorID = lastID;
            Model = model;
            Color = color;
            NumberOfShelves = numberOfShelves;
            Shelves = new List<Shelf>();
            for (int i = 1; i <= numberOfShelves; i++)
                Shelves.Add(new Shelf(i));
            AllRefrigerators.Add(this);
        }

        public static List<Refrigerator> SortRefrigerators()
        {
            return AllRefrigerators.OrderByDescending(refrigerator => refrigerator.HowMuchSpaceIsLeft()).ToList();
        }
        private bool IsSpaceLeft(Item item)
        {
            return item.SpaceTaken <= (double)HowMuchSpaceIsLeft();
        }

        public double HowMuchSpaceIsLeft()
        {
            double spaceAvailable = 0;

            foreach (var shelf in Shelves)
            {
                spaceAvailable += shelf.SpaceAvailable;
            }
            return spaceAvailable;
        }

        public void AddItem(Item item)
        {
            if (!item.IsInRefrigerator)
            {
                var flag = false;
                var shelfNumber = 0;
                if (IsSpaceLeft(item))
                {
                    for (int i = NumberOfShelves - 1; i >= 0; i--)
                    {
                        if (Shelves[i].IsSpaceLeft(item))
                        {
                            flag = true;
                            shelfNumber = i;
                        }
                    }
                }
                if (flag)
                    Shelves[shelfNumber].AddItem(item);
                else
                    throw new Exception("There is no space left in the refrigerator.");
            }
            else
                throw new Exception("The item is already in a refrigerator.");
        }

        public void RemoveItem(int itemID)
        {
            var flag = false;
            var shelfNumber = 0;
            var item = Item.GetItemByID(itemID);
            for (int i = 0; i < NumberOfShelves; i++)
            {
                if (Shelves[i].IsItemInShelf(item))
                {
                    flag = true;
                    shelfNumber = i;
                }
            }
            if (flag)
                Shelves[shelfNumber].RemoveItem(item);
            else
                throw new Exception("The refrigerator does not contain such an item.");
        }

        public List<Item> CleanRefrigerator()
        {
            var itemsToTrowAway = new List<Item>();
            foreach (var shelf in Shelves)
                itemsToTrowAway.AddRange(shelf.CleanShelf());
            return itemsToTrowAway;
        }

        public List<Item> FeelLikeEating(KosherType kosher, ItemType type)
        {
            var items = new List<Item>();
            foreach (var shelf in Shelves)
            {
                var itemsToAdd = shelf.GetItemByKosherAndType(kosher, type);
                if (itemsToAdd != null)
                    items.AddRange(itemsToAdd);
            }
            return items;
        }

        public void GettingReadyForShopping()
        {
            if (HowMuchSpaceIsLeft() < 20)
            {
                CleanRefrigerator();
                if (HowMuchSpaceIsLeft() < 20)
                {
                    double takeSpace = 0;
                    double spaceByKosherAndExpiryDate;
                    List<Item> itemsToThrowAway = new List<Item>();
                    List<Item> itemsByKosherAndExpiryDate = new List<Item>();
                    foreach (var shelf in Shelves)
                    {
                        (itemsByKosherAndExpiryDate, spaceByKosherAndExpiryDate) = shelf.CleanByKosherAndExpiryDate();
                        takeSpace += spaceByKosherAndExpiryDate;
                        itemsToThrowAway.AddRange(itemsByKosherAndExpiryDate);
                    }
                    if (HowMuchSpaceIsLeft() - takeSpace < 20)
                    {
                        foreach (var shelf in Shelves)
                        {
                            (itemsByKosherAndExpiryDate, spaceByKosherAndExpiryDate) = shelf.CleanByKosherAndExpiryDate();
                            shelf.RemoveItems(itemsByKosherAndExpiryDate);
                        }
                        Console.WriteLine("Cleaning has been done for non-expired products, shopping can be done");
                    }
                    else
                        Console.WriteLine("There is not enough space in the refrigerator, this is not the time to go shopping");
                }
                else Console.WriteLine("Cleaning has been done, shopping can be done");
            }
            else
            {
                Console.WriteLine("There is enough space in the fridge even without cleaning it. Shopping can be done.");
            }
        }
        public string PrintRefrigeratorWithContents()
        {
            string str = ToString() + "\n";
            str += "\nShelves:";
            foreach (Shelf shelf in Shelves)
            {
                str += "\n" + shelf.PrintShelfWithContents().ToString() + "\n";
            }
            return str;
        }
        public override string ToString()
        {
            return $"Refrigerator ID: {RefrigeratorID}, Model: {Model}, Color: {Color}," +
                $" Number of Shelves: {NumberOfShelves}";
        }
    }
}
