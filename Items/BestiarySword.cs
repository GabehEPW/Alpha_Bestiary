using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BestiaryAlpha.Common.Players;

namespace BestiaryAlpha.Items
{
    internal class BestiarySword : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Melee;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int level = player.GetModPlayer<AlphaBestiaryPlayer>().GetWeaponLevel(Item.type);
            damage *= 1f + level * 0.03f;
        }
    }
}