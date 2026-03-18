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
    // Lança melee que evolui com kills únicas
    internal class BestiarySpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Necessário para liberar no modo Journey
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Tamanho do item no inventário
            Item.width = 42;
            Item.height = 42;

            // Classe da arma
            Item.DamageType = DamageClass.Melee;
            Item.damage = 21;
            Item.knockBack = 4f;
            Item.crit = 4;

            // Estilo de uso de lança/projétil
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.autoReuse = true;

            // O dano vem do projétil da lança
            Item.noMelee = true;

            // Esconde a sprite do item durante o ataque
            Item.noUseGraphic = true;

            // Não deixa virar no meio do golpe
            Item.useTurn = false;

            // Projétil da lança
            Item.shoot = ModContent.ProjectileType<BestiarySpearProjectile>();
            Item.shootSpeed = 3.2f;

            Item.UseSound = SoundID.Item1;

            Item.value = Item.buyPrice(silver: 85);
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Aumenta o dano com base no nível da arma
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            damage *= 1f + level * 0.03f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Mostra o nível atual da arma
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "Level",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "Hint",
                "Evolui ao derrotar diferentes criaturas."));
        }

        public override bool CanUseItem(Player player)
        {
            // Impede várias lanças ativas ao mesmo tempo
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool Shoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            // Cria o projétil manualmente
            int projIndex = Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            // Liga o projétil ao sistema do mod
            Projectile proj = Main.projectile[projIndex];
            var globalProj = proj.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

            globalProj.sourceItemType = Item.type;
            globalProj.sourcePlayerWhoAmI = player.whoAmI;

            return false; // evita projétil duplicado
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 14)
                .AddIngredient(ItemID.IronBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}