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
    internal class BestiaryYoyo : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;

            Item.noUseGraphic = true;

            Item.shoot = ModContent.ProjectileType<BestiaryYoyoProjectile>();
        }
    }
}