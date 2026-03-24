using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using BestiaryAlpha.Common.Global;
using BestiaryAlpha.Common.Players;
using BestiaryAlpha.Common.Global.Projectiles;

namespace BestiaryAlpha.Items
{
    internal class BestiaryWhip : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.SummonMeleeSpeed;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;

            Item.shoot = ModContent.ProjectileType<BestiaryWhipProjectile>();
        }
    }
}