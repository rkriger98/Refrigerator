namespace Refrigerator
{
    using System.Collections.Generic;
    using static global::Refrigerator.Refrigerator;
    using System.Reflection;
    using System.Text;
    using System;
    using System.Linq;
    using System.Security.Policy;
    using System.Collections;

    public class Shelf
    {
        private static int lastID = 0;
        public int ShelfID { get; set; }
        public int ShelfPositionNumber { get; set; }
        public double SpaceAvailable { get; set; }
        public List<Item> Items { get; set; }
        private static readonly List<Shelf> AllShelves = new List<Shelf>();

        public Shelf(int shelfPositionNumber)
        {
            lastID++;
            ShelfID = lastID;
            ShelfPositionNumber = shelfPositionNumber;
            SpaceAvailable = 50;
            Items = new List<Item>();
            AllShelves.Add(this);
        }
        public static List<Shelf> SortShelves()
        {
            return AllShelves.OrderByDescending(shelf => shelf.SpaceAvailable).ToList();
        }

        public bool IsSpaceLeft(Item item)
        {
            return SpaceAvailable >= item.SpaceTaken;
        }

        public bool IsItemInShelf(Item item)
        {
            return ShelfID == item.ShelfID;
        }

        public void AddItem(Item item)
        {
            item.ShelfID = ShelfID;
            Items.Add(item);
            item.IsInRefrigerator = true;
            SpaceAvailable -= item.SpaceTaken;
            Console.WriteLine("The item was successfully added");
        }

        public void RemoveItem(Item item)
        {
            item.ShelfID = 0;
            Items.Remove(item);
            item.IsInRefrigerator = false;
            SpaceAvailable += item.SpaceTaken;
            Console.WriteLine("item {0} was successfully removed", item.ItemName);
        }

        public void RemoveItems(List<Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].ShelfID = 0;
                Items.Remove(items[i]);
                items[i].IsInRefrigerator = false;
                SpaceAvailable += items[i].SpaceTaken;
                Console.WriteLine("item {0} was successfully removed", items[i].ItemName);
            }
        }

        public List<Item> CleanShelf()
        {
            var itemsToTrowAway = new List<Item>();
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].IsExpired())
                {
                    itemsToTrowAway.Add(Items[i]);
                    RemoveItem(Items[i]);
                }
            }
            return itemsToTrowAway;
        }

        public List<Item> GetItemByKosherAndType(KosherType kosher, ItemType type)
        {
            var items = new List<Item>();
            foreach (Item item in Items)
            {
                if (!item.IsExpired() && item.Kosher == kosher && item.Type == type)
                    items.Add(item);
            }
            return items;
        }

        public (List<Item>, double) CleanByKosherAndExpiryDate()
        {
            double spaceByKosherAndExpiryDate = 0;
            List<Item> itemsByKosherAndExpiryDate = new List<Item>();
            foreach (Item item in Items)
                if (item.CheckSpaceByKosherAndExpiryDate())
                {
                    spaceByKosherAndExpiryDate += item.SpaceTaken;
                    itemsByKosherAndExpiryDate.Add(item);
                }
            return (itemsByKosherAndExpiryDate, spaceByKosherAndExpiryDate);
        }

        public string PrintShelfWithContents()
        {
            string str = ToString() + "\n";
            if (Items.Count != 0)
            {
                str += "Items on the Shelf:";
                foreach (Item item in Items)
                    str += "\n" + item.ToString();
            }
            else
                str += "No items on this shelf.";
            return str;
        }
        public override string ToString()
        {
            return $"Shelf ID: {ShelfID}, Position Number: {ShelfPositionNumber}, Space Available: {SpaceAvailable} sq.m.";
        }
    }
}
