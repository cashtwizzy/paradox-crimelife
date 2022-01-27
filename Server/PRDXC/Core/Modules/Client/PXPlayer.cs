using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using PRDXC.Core.Modules.Creator;
using PRDXC.Core.Modules.Hud;

namespace PRDXC.Core.Modules.Client
{
    public class PXPlayer
    {
        public int Id { get; set; }
        public int Money { get; set; }
        public List<uint> Loadout { get; set; }
        public Inventory.Inventory Inventory { get; set; }
        public Customization Customization { get; set; }
        public AdminRank AdminRank { get; set; }
        public Player MPClient { get; set; }
        public bool Dead { get; set; }
        public DateTime DiedAt { get; set; } = DateTime.Now;
        public bool Aduty { get; set; }
        public Clothes Clothes { get; set; }
        public bool Cuffed { get; set; }

        public PXPlayer(int id, int money, List<uint> loadout, Inventory.Inventory inventory, Customization custom, Player player, AdminRank adminRank, bool dead, Clothes clothes)
        {
            Id = id;
            Money = money;
            Loadout = loadout;
            Inventory = inventory;
            Customization = custom;
            AdminRank = adminRank;
            MPClient = player;
            Dead = dead;
            Aduty = false;
            Clothes = clothes;
            Cuffed = false;
        }

        public void Load()
        {
            ApplyCustomization();
            ApplyClothes();
            GiveWeapons();
        }

        public void Notify(string title, string msg, int duration = 5000)
        {
            MPClient?.TriggerEvent("DisplayNotify", NAPI.Util.ToJson(new NotificationInfo(title, msg, duration)));
        }

        public void Progress(string title, string desc, int duration = 5000, string image = "")
        {
            MPClient?.TriggerEvent("DisplayProgress", NAPI.Util.ToJson(new ProgressInfo(title, desc, duration, image)));
        }

        public void SetMoney(int newMoney)
        {
            Money = newMoney;
            if (MPClient != null && MPClient.Exists)
                MPClient.TriggerEvent("SetMoney", newMoney);
        }

        private void GiveWeapons()
        {
            Loadout.ForEach(x => MPClient.GiveWeapon((WeaponHash)x, 9999));
        }

        public void ApplyClothes()
        {
            Clothes.ApplyClothes(MPClient);
        }

        private void ApplyCustomization()
        {
            Customization.ApplyCustomization(MPClient);
        }
    }
}
