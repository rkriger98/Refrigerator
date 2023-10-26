namespace Refrigerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Item
    {
        private static int lastID = 0;
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int ShelfID { get; set; }
        public ItemType Type { get; set; }
        public KosherType Kosher { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double SpaceTaken { get; set; }
        public bool IsInRefrigerator { get; set; }
        private static readonly List<Item> AllItems = new List<Item>();
        public Item(string productName, ItemType type, KosherType kosher, DateTime expiryDate, double spaceTaken)
        {
            lastID++;
            ItemID = lastID;
            ItemName = productName;
            Type = type;
            Kosher = kosher;
            ExpiryDate = expiryDate;
            SpaceTaken = spaceTaken;
            AllItems.Add(this);
        }

        public static List<Item> InitializeItems()
        {
            return new List<Item>
            {
                new Item("Milk", ItemType.Drink, KosherType.Dairy, DateTime.Now.AddDays(-5), 18),
                new Item("Cheese", ItemType.Food, KosherType.Dairy, DateTime.Now.AddDays(15), 16),
                new Item("Chicken", ItemType.Food, KosherType.Meat, DateTime.Now.AddMonths(1), 25),
                new Item("Lettuce", ItemType.Food, KosherType.Parve, DateTime.Now.AddDays(10), 23),
                new Item("Orange Juice", ItemType.Drink, KosherType.Parve, DateTime.Now.AddDays(20), 19),
                new Item("Ketchup", ItemType.Food, KosherType.Parve, DateTime.Now.AddMonths(3), 17),
                new Item("Apples", ItemType.Food, KosherType.Parve, DateTime.Now.AddDays(15), 20),
                new Item("Yogurt", ItemType.Food, KosherType.Dairy, DateTime.Now.AddDays(15), 16),
                new Item("Eggs", ItemType.Food, KosherType.Parve, DateTime.Now.AddDays(15), 25),
                new Item("Butter", ItemType.Food, KosherType.Dairy, DateTime.Now.AddMonths(3), 14)
            };
        }

        public static Item GetItemByID(int itemID)
        {
            for (int i = 0; i < AllItems.Count; i++)
            {
                if (AllItems[i].ItemID == itemID)
                    return AllItems[i];
            }
            throw new Exception("No item with this ID exists");
        }

        public static List<Item> SortItems()
        {
            return AllItems.OrderBy(item => item.ExpiryDate).ToList();
        }

        public bool IsExpired()
        {
            return ExpiryDate < DateTime.Now;
        }
        public bool CheckSpaceByKosherAndExpiryDate()
        {
            if (KosherType.Dairy == Kosher && ExpiryDate.AddDays(3) > DateTime.Now ||
                KosherType.Meat == Kosher && ExpiryDate.AddDays(7) > DateTime.Now ||
                KosherType.Parve == Kosher && ExpiryDate.AddDays(1) > DateTime.Now)
                return true;
            return false;
        }

        public override string ToString()
        {
            return $"Item ID: {ItemID}, Item Name: {ItemName}, Shelf ID: {ShelfID}, Type: {Type}," +
                $" Kosher: {Kosher}, Expiry Date: {ExpiryDate.ToShortDateString()}, Space Taken: {SpaceTaken} sq.m.";
        }
    }

}
