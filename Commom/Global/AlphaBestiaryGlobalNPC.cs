using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AlphaBestiary.Common.Players;

namespace AlphaBestiary.Common.Global
{
    public class AlphaBestiaryGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int lastHitItemType = 0;
        private int lastHitPlayerWhoAmI = -1;

        private bool IsValidModWeapon(Item item)
        {
            return item != null &&
                   !item.IsAir &&
                   item.ModItem != null &&
                   item.ModItem.Mod == Mod;
        }

        private bool CanCountForBestiaryProgress(NPC npc)
        {
            if (npc == null || !npc.active)
                return false;

            if (npc.lifeMax <= 5)
                return false;

            if (npc.friendly)
                return false;

            if (npc.townNPC)
                return false;

            if (npc.type <= NPCID.None)
                return false;

            return true;
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByItem(npc, player, item, hit, damageDone);

            if (!CanCountForBestiaryProgress(npc))
                return;

            if (IsValidModWeapon(item))
            {
                lastHitItemType = item.type;
                lastHitPlayerWhoAmI = player.whoAmI;
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitByProjectile(npc, projectile, hit, damageDone);

            if (!CanCountForBestiaryProgress(npc))
                return;

            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player player = Main.player[projectile.owner];
            if (player == null || !player.active)
                return;

            Item item = player.HeldItem;

            if (IsValidModWeapon(item))
            {
                lastHitItemType = item.type;
                lastHitPlayerWhoAmI = player.whoAmI;
            }
        }

        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);

            if (!CanCountForBestiaryProgress(npc))
                return;

            if (lastHitPlayerWhoAmI < 0 || lastHitPlayerWhoAmI >= Main.maxPlayers)
                return;

            if (lastHitItemType == 0)
                return;

            Player player = Main.player[lastHitPlayerWhoAmI];
            if (player == null || !player.active)
                return;

            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            bool wasNewKill = modPlayer.RegisterKill(lastHitItemType, npc.type);

            if (wasNewKill && Main.myPlayer == player.whoAmI)
            {
                int newLevel = modPlayer.GetWeaponLevel(lastHitItemType);
                Main.NewText($"Nova criatura registrada! Nível da arma: {newLevel}");
            }

            lastHitItemType = 0;
            lastHitPlayerWhoAmI = -1;
        }
    }
}