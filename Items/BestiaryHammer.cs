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
    // Martelo melee que carrega o ataque antes de golpear
    internal class BestiaryHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Necessário para pesquisa no modo Journey
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Tamanho do item
            Item.width = 40;
            Item.height = 40;

            // Classe e dano base
            Item.DamageType = DamageClass.Melee;
            Item.damage = 30; // maior que a sword
            Item.knockBack = 7f;
            Item.crit = 4;

            // Usa projétil segurável
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = false;

            // O dano vem do projétil, não do item direto
            Item.noMelee = true;
            Item.noUseGraphic = true;

            // Permite segurar para carregar
            Item.channel = true;

            // Projétil do martelo
            Item.shoot = ModContent.ProjectileType<BestiaryHammerProjectile>();
            Item.shootSpeed = 1f;

            Item.UseSound = SoundID.Item1;

            Item.value = Item.buyPrice(silver: 95);
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Aumenta dano com o nível da arma
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            damage *= 1f + level * 0.03f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Mostra nível atual da arma
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "Level",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "Hint",
                "Segure para carregar o golpe. Evolui ao derrotar diferentes criaturas."));
        }

        public override bool CanUseItem(Player player)
        {
            // Impede mais de um martelo ativo ao mesmo tempo
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
                player.MountedCenter,
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

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 10)
                .AddIngredient(ItemID.IronBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}