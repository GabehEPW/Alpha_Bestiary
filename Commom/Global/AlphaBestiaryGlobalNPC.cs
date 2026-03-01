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

        // Assinatura correta na sua versão:
        // OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
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
