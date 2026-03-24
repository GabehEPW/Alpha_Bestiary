using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using BestiaryAlpha.Common.Global;
using BestiaryAlpha.Common.Players;

namespace BestiaryAlpha.Items
{
    internal class BestiaryBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Ranged;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.autoReuse = true;

            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 9f;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int level = player.GetModPlayer<AlphaBestiaryPlayer>().GetWeaponLevel(Item.type);
            damage *= 1f + level * 0.03f;
        }
    }
}