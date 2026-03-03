using Terraria;
using Terraria.ModLoader;
using AlphaBestiary.Common.Players;

namespace AlphaBestiary.Common.Global
{
    public class AlphaBestiaryGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int lastHitItemType = 0;
        private int lastHitPlayerWhoAmI = -1;

        // 1) Hit direto com item (espada, etc.)
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByItem(npc, player, item, hit, damageDone);

            if (item.ModItem != null &&
                item.ModItem.Mod == ModContent.GetInstance<AlphaBestiary>())
            {
                lastHitItemType = item.type;
                lastHitPlayerWhoAmI = player.whoAmI;
            }
        }

        // 2) Hit por projétil (cajado, arco, armas de tiro)
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByProjectile(npc, projectile, hit, damageDone);

            // Só projéteis que têm dono válido
            if (projectile.owner < 0 || projectile.owner >= 255)
                return;

            Player player = Main.player[projectile.owner];

            // Item que o player está segurando quando atirou (aproximação boa pra arma mágica / ranged)
            Item item = player.HeldItem;

            if (item != null && !item.IsAir &&
                item.ModItem != null &&
                item.ModItem.Mod == ModContent.GetInstance<AlphaBestiary>())
            {
                lastHitItemType = item.type;
                lastHitPlayerWhoAmI = player.whoAmI;
            }
        }

        // 3) Quando o NPC morre, registrar a kill para a arma certa
        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);

            if (lastHitPlayerWhoAmI < 0 || lastHitItemType == 0)
                return;

            Player player = Main.player[lastHitPlayerWhoAmI];
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            modPlayer.RegisterKill(lastHitItemType, npc.type);

            lastHitItemType = 0;
            lastHitPlayerWhoAmI = -1;
        }
    }
}
