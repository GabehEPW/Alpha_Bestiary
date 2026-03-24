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
    internal class BestiaryHammer : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Melee;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shoot = ModContent.ProjectileType<BestiaryHammerProjectile>();
        }
    }
}