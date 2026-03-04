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
                Main.NewText($"[OnHitByItem] {item.Name} acertou {npc.FullName}");
            }
        }

        // 2) Hit por projétil (cajado, arco, armas de tiro)
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByProjectile(npc, projectile, hit, damageDone);

            Main.NewText($"[OnHitByProjectile] Proj {projectile.type} acertou {npc.FullName}");

            // Só projéteis que têm dono válido
            if (projectile.owner < 0 || projectile.owner >= 255)
            {
                Main.NewText($"[OnHitByProjectile] Owner inválido: {projectile.owner}");
                return;
            }

            Player player = Main.player[projectile.owner];
            Item item = player.HeldItem;

            Main.NewText($"[OnHitByProjectile] Player: {player.name}, Item na mão: {item.Name}");

            if (item != null && !item.IsAir &&
                item.ModItem != null &&
                item.ModItem.Mod == ModContent.GetInstance<AlphaBestiary>())
            {
                lastHitItemType = item.type;
                lastHitPlayerWhoAmI = player.whoAmI;
                Main.NewText($"[OnHitByProjectile] REGISTRADO: {item.Name} acertou {npc.FullName}");
            }
            else
            {
                Main.NewText($"[OnHitByProjectile] Item não é do mod ou é nulo");
            }
        }

        // 3) Quando o NPC morre, registrar a kill para a arma certa
        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);

            Main.NewText($"[OnKill] {npc.FullName} morreu | lastHitItemType: {lastHitItemType} | lastHitPlayer: {lastHitPlayerWhoAmI}");

            if (lastHitPlayerWhoAmI < 0 || lastHitPlayerWhoAmI >= 255 || lastHitItemType == 0)
            {
                Main.NewText($"[OnKill] Dados inválidos, abortando");
                return;
            }

            Player player = Main.player[lastHitPlayerWhoAmI];
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            modPlayer.RegisterKill(lastHitItemType, npc.type);
            Main.NewText($"[OnKill] ✓ Kill registrada com item type {lastHitItemType}");

            lastHitItemType = 0;
            lastHitPlayerWhoAmI = -1;
        }
    }
}
