using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using BestiaryAlpha.Common.Global;
using BestiaryAlpha.Common.Players;

namespace BestiaryAlpha.Items
{
    internal class BestiaryWand : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileID.FallingStar;
        }
    }
}