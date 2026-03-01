using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace AlphaBestiary.Items
{
    internal class BestiaryWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.mana = 8;
            Item.damage = 24;
            Item.knockBack = 3.2f;

            Item.useTime = 20;
            Item.useAnimation = 15;

            Item.UseSound = SoundID.Item71;

            Item.shoot = ProjectileID.FallingStar;
            Item.shootSpeed = 1f;

            // Misc comuns
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
