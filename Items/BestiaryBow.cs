using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using AlphaBestiary.Common.Global;
using AlphaBestiary.Common.Players;

namespace AlphaBestiary.Items
{
    // Classe do item arco
    internal class BestiaryBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Quantidade necessária para pesquisa no modo Journey
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Tamanho do item
            Item.width = 20;
            Item.height = 38;

            // Dano base e classe da arma
            Item.damage = 17;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2.5f;
            Item.crit = 4;

            // Estilo de uso de arma de disparo
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.autoReuse = true;

            // Impede virar junto durante o uso
            Item.useTurn = false;

            // O arco não dá dano corpo a corpo diretamente
            Item.noMelee = true;

            // A sprite do item continua aparecendo na mão
            Item.noUseGraphic = false;

            // Define que o arco usa flechas como munição
            Item.useAmmo = AmmoID.Arrow;

            // Projétil padrão caso nenhuma flecha especial substitua
            Item.shoot = ProjectileID.WoodenArrowFriendly;

            // Velocidade de disparo
            Item.shootSpeed = 9f;

            // Som do arco
            Item.UseSound = SoundID.Item5;

            // Valor e raridade
            Item.value = Item.buyPrice(silver: 90);
            Item.rare = ItemRarityID.Blue;
        }

        public override Vector2? HoldoutOffset()
        {
            // Ajusta a posição visual do arco na mão do player
            return new Vector2(-2f, 0f);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Acessa os dados salvos do jogador para o sistema Alpha Bestiary
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            // Lê o nível atual dessa arma
            int level = modPlayer.GetWeaponLevel(Item.type);

            // Aplica bônus de dano por nível
            float bonusPerLevel = 0.03f;
            damage *= 1f + level * bonusPerLevel;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Usa o jogador local para exibir o nível da arma no tooltip
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryLevel",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryHint",
                "Aumenta de nível ao matar tipos diferentes de monstros usando este arco."));
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
            // Cria manualmente a flecha/projétil disparado
            int projIndex = Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            // Recupera o projétil criado
            Projectile proj = Main.projectile[projIndex];

            // Recupera o GlobalProjectile do mod para salvar informações extras
            var globalProj = proj.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

            // Guarda qual item gerou o projétil
            globalProj.sourceItemType = Item.type;

            // Guarda qual jogador lançou
            globalProj.sourcePlayerWhoAmI = player.whoAmI;

            // Retorna false para evitar que o jogo crie outro projétil automaticamente
            return false;
        }

        public override void AddRecipes()
        {
            // Receita básica do arco
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 12)
                .AddIngredient(ItemID.Vine, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}