using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using AlphaBestiary.Common.Players;

namespace AlphaBestiary.Items
{
    internal class BestiaryGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Use valores compatíveis com o tamanho REAL da sprite no jogo.
            // Para gun estilo Terraria, o ideal é a textura final ser ~30x15.
            Item.width = 30;
            Item.height = 15;

            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1.5f;
            Item.crit = 4;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.reuseDelay = 0;
            Item.autoReuse = true;
            Item.useTurn = false;

            Item.noMelee = true;
            Item.noUseGraphic = false;

            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10f;

            Item.UseSound = SoundID.Item11;

            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;

            // Para guns, não conte com isso para “encolher” visualmente a sprite.
            Item.scale = 1f;
        }

        // Ajusta a posição da arma na mão do player.
        // Isso é o jeito correto para itens com useStyle = Shoot.
        public override Vector2? HoldoutOffset()
        {
            // Ajuste fino:
            // X negativo puxa a arma mais para trás
            // Y negativo sobe a arma, Y positivo desce
            return new Vector2(-6f, 0f);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            float bonusPerLevel = 0.03f;
            damage *= 1f + level * bonusPerLevel;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryLevel",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryHint",
                "Aumenta de nível ao matar tipos diferentes de monstros usando esta metralhadora."));
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IllegalGunParts, 1)
                .AddIngredient(ItemID.IronBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}