using PRDXC.Core.Modules.Client;
using PRDXC.Core.Modules.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRDXC.Core.Modules.Inventory
{
    public class Inventory
    {
        public static ItemModel GetModelFromName(string name)
        {
            switch (name.ToLower())
            {
                case "schutzweste":
                    return new Schutzweste();
                case "medikit":
                    return new Medikit();
                case "assaultrifle":
                    return new Assaultrifle();
                case "carbinerifle_mk2":
                    return new Carbinerifle_MK2();
                case "gusenberg":
                    return new Gusenberg();
                case "heavyrifle":
                    return new Heavyrifle();
                case "reperaturkasten":
                    return new Reperaturkasten();
                case "seile":
                    return new Seile();
                case "specialcarbine":
                    return new Specialcarbine();
            }
            return null;
        }

        public List<Item> Items { get; set; }
        public int MaxKG { get; set; }

        public Inventory()
        {
            Items = new List<Item>();
            MaxKG = 40;
        }

        public Inventory(List<Item> items, int maxkg)
        {
            Items = items;
            MaxKG = maxkg;
        }

        public void UseItem(PXPlayer pxPlayer, int slot)
        {
            var item = Items.Find(e => e.Slot == slot);
            if (item == null || item.Amount < 1) return;
            item.Use(pxPlayer);
            RemoveItem(slot, 1);
        }

        public bool AddItem(ItemModel itemModel, int amount)
        {
            var Items = new List<Item>(this.Items);
            var amount2 = amount;
            if (itemModel.MaxAmount > 1)
            {
                for (var i = 0; i < Items.Count; i++)
                {
                    if (GetInventoryWeight() >= MaxKG) return false;
                    if (Items[i].Model.Name == itemModel.Name)
                    {
                        if ((Items[i].Amount + amount2) > Items[i].Model.MaxAmount)
                        {
                            var diff = Items[i].Model.MaxAmount - Items[i].Amount;
                            Items[i].Amount += diff;
                            amount2 -= diff;
                            continue;
                        }
                        else
                        {
                            Items[i].Amount += amount2;
                            amount2 = 0;
                            break;
                        }
                    }
                }
            }

            while (amount2 > 0)
            {
                var freeslot = GetFirstFreeSlot(Items);
                if (freeslot == -1 || GetInventoryWeight(Items) >= MaxKG) return false;
                if (amount2 > itemModel.MaxAmount)
                {
                    Items.Add(new Item(itemModel, itemModel.MaxAmount, freeslot));
                    amount2 -= itemModel.MaxAmount;
                    continue;
                }
                else
                {
                    Items.Add(new Item(itemModel, amount2, freeslot));
                    break;
                }
            }

            this.Items = Items;
            return true;
        }

        public bool RemoveItem(int slot, int amount)
        {
            var item = Items.Find(e => e.Slot == slot);
            if (item == null || item.Amount < amount) return false;
            item.Amount -= amount;
            if(item.Amount < 1) Items.Remove(item);
            return true;
        }

        public bool RemoveItem(ItemModel model, int amount)
        {
            var item = Items.Find(e => e.Model.Name == model.Name);
            if (item == null || item.Amount < amount) return false;
            item.Amount -= amount;
            if (item.Amount < 1) Items.Remove(item);
            return true;
        }

        public bool MoveAllToSlot(int oldSlot, int newSlot)
        {
            var movedItem = Items.Find(e => e.Slot == oldSlot);
            var secondItem = Items.Find(e => e.Slot == newSlot);
            if(movedItem != null)
            {
                if(secondItem == null)
                {
                    movedItem.Slot = newSlot;
                    return true;
                }

                if (secondItem.Model.Name != movedItem.Model.Name)
                {
                    movedItem.Slot = newSlot;
                    secondItem.Slot = oldSlot;
                }
                else
                {
                    var freeSpace = secondItem.Model.MaxAmount - secondItem.Amount;
                    if(freeSpace < 1)
                    {
                        return true;
                    }
                    else
                    {
                        if(freeSpace <= movedItem.Amount)
                        {
                            secondItem.Amount += freeSpace;
                            movedItem.Amount -= freeSpace;
                            if(movedItem.Amount == 0) 
                                Items.Remove(movedItem);
                        }
                        else
                        {
                            secondItem.Amount += movedItem.Amount;
                            Items.Remove(movedItem);
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public bool MoveAmountToSlot(int oldSlot, int newSlot, int amount)
        {
            var item = Items.Find(e => e.Slot == oldSlot);
            if (item == null || item.Amount <= amount || Items.Find(e => e.Slot == newSlot) != null) return false;
            item.Amount -= amount;
            Items.Add(new Item(item.Model, amount, newSlot));
            return true;
        }

        private int GetFirstFreeSlot(List<Item> items)
        {
            for (var i = 1; i <= 20; i++)
            {
                var r = items.FirstOrDefault(e => e.Slot == i);
                if (r == null || r.Amount < 1)
                {
                    return i;
                }
            }
            return -1;
        }

        public float GetInventoryWeight(List<Item> items)
        {
            float result = 0;
            for (var i = 0; i < items.Count; i++)
                result += (items[i].Model.Weight * items[i].Amount);
            return result;
        }

        public float GetInventoryWeight()
        {
            float result = 0;
            for (var i = 0; i < Items.Count; i++)
                result += (Items[i].Model.Weight * Items[i].Amount);
            return result;
        }

        public bool HasItem(ItemModel model)
        {
            return Items.Find(x => x.Model.Name == model.Name) != null;
        }

        public DBInventory ToDBInventory()
        {
            return new DBInventory(Items, MaxKG);
        }
    }
}
