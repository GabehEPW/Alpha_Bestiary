using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BestiaryAlpha.Common.Players
{
    public class AlphaBestiaryPlayer : ModPlayer
    {
        public Dictionary<int, HashSet<int>> killsPerWeapon = new();

        public int GetWeaponLevel(int itemType)
        {
            if (killsPerWeapon.TryGetValue(itemType, out var set))
                return set.Count;

            return 0;
        }

        public bool RegisterKill(int itemType, int npcType)
        {
            if (!killsPerWeapon.TryGetValue(itemType, out var set))
            {
                set = new HashSet<int>();
                killsPerWeapon[itemType] = set;
            }

            return set.Add(npcType);
        }

        public override void SaveData(TagCompound tag)
        {
            var list = new List<TagCompound>();

            foreach (var kv in killsPerWeapon)
            {
                list.Add(new TagCompound
                {
                    ["itemType"] = kv.Key,
                    ["npcTypes"] = new List<int>(kv.Value)
                });
            }

            tag["killsPerWeapon"] = list;
        }

        public override void LoadData(TagCompound tag)
        {
            killsPerWeapon.Clear();

            if (!tag.ContainsKey("killsPerWeapon"))
                return;

            foreach (var entry in tag.GetList<TagCompound>("killsPerWeapon"))
            {
                int itemType = entry.GetInt("itemType");
                var npcTypes = entry.Get<List<int>>("npcTypes");

                killsPerWeapon[itemType] = new HashSet<int>(npcTypes);
            }
        }
    }
}