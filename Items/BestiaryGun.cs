using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using BestiaryAlpha.Common.Global;
using BestiaryAlpha.Common.Players;

namespace BestiaryAlpha.Items
{
    internal class BestiaryGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.autoReuse = true;

            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int level = player.GetModPlayer<AlphaBestiaryPlayer>().GetWeaponLevel(Item.type);
            damage *= 1f + level * 0.03f;
        }
    }
}