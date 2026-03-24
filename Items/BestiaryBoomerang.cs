using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using BestiaryAlpha.Common.Global;
using BestiaryAlpha.Common.Players;
using BestiaryAlpha.Common.Global.Projectiles;

namespace BestiaryAlpha.Items
{
    internal class BestiaryBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 20;
            Item.knockBack = 4f;

            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<BestiaryBoomerangProjectile>();
            Item.shootSpeed = 10f;

            Item.UseSound = SoundID.Item1;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int level = player.GetModPlayer<AlphaBestiaryPlayer>().GetWeaponLevel(Item.type);
            damage *= 1f + level * 0.03f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projIndex = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            var proj = Main.projectile[projIndex];
            var globalProj = proj.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

            globalProj.sourceItemType = Item.type;
            globalProj.sourcePlayerWhoAmI = player.whoAmI;

            return false;
        }
    }
}