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
    // Yo-yo que evolui com kills únicas
    internal class BestiaryYoyo : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // Marca como yo-yo para o jogo aplicar comportamento correto
            ItemID.Sets.Yoyo[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            // Classe melee sem depender de velocidade de ataque
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 18;
            Item.knockBack = 2.5f;

            Item.noMelee = true;
            Item.noUseGraphic = true;

            // Permite segurar para manter o yo-yo ativo
            Item.channel = true;

            Item.shoot = ModContent.ProjectileType<BestiaryYoyoProjectile>();
            Item.shootSpeed = 16f;

            Item.value = Item.buyPrice(silver: 80);
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
                .AddIngredient(ItemID.Wood, 12)
                .AddIngredient(ItemID.Vine, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}