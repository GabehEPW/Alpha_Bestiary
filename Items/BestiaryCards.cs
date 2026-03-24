using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using System.Collections.Generic;
using BestiaryAlpha.Common.Global;
using BestiaryAlpha.Common.Players;
using BestiaryAlpha.Common.Global.Projectiles;

namespace BestiaryAlpha.Items
{
    internal class BestiaryCards : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;

            Item.shoot = ModContent.ProjectileType<BestiaryCardProjectile>();
            Item.shootSpeed = 10f;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int level = player.GetModPlayer<AlphaBestiaryPlayer>().GetWeaponLevel(Item.type);
            damage *= 1f + level * 0.03f;
        }
    }
}