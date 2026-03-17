using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using AlphaBestiary.Common.Global;
using AlphaBestiary.Common.Players;
using AlphaBestiary.Projectiles;

namespace AlphaBestiary.Items
{
    // Chicote summoner que evolui com kills
    internal class BestiaryWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;

            // Classe summoner (padrão para chicotes)
            Item.damage = 22;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.knockBack = 2f;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            // Projétil do chicote
            Item.shoot = ModContent.ProjectileType<BestiaryWhipProjectile>();
            Item.shootSpeed = 4f;

            Item.UseSound = SoundID.Item152;

            Item.value = Item.buyPrice(silver: 90);
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Escala com nível da arma
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            damage *= 1f + level * 0.03f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "Level",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "Hint",
                "Evolui ao derrotar diferentes criaturas."));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projIndex = Projectile.NewProjectile(
                source, position, velocity, type, damage, knockback, player.whoAmI
            );

            var proj = Main.projectile[projIndex];
            var globalProj = proj.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

            globalProj.sourceItemType = Item.type;
            globalProj.sourcePlayerWhoAmI = player.whoAmI;

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 14)
                .AddIngredient(ItemID.Vine, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}