using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AlphaBestiary.Common.Players
{
    public class AlphaBestiaryPlayer : ModPlayer
    {
        // Dicionário que guarda o PROGRESSO por arma:
        // chave: item.type da arma
        // valor: conjunto (HashSet) de npc.type que essa arma já matou
        public Dictionary<int, HashSet<int>> killsPerWeapon = new();

        // Retorna o "nível" da arma = quantos tipos diferentes de NPC
        // essa arma já matou com este jogador
        public int GetWeaponLevel(int itemType)
        {
            if (killsPerWeapon.TryGetValue(itemType, out var set))
                return set.Count;

            return 0; // nunca matou nada com essa arma ainda
        }

        // Registra que uma arma (itemType) matou um NPC (npcType)
        public void RegisterKill(int itemType, int npcType)
        {
            // Se ainda não existe registro para essa arma, cria
            if (!killsPerWeapon.TryGetValue(itemType, out var set))
            {
                set = new HashSet<int>();
                killsPerWeapon[itemType] = set;
            }

            // HashSet garante que cada npcType só entra uma vez
            // (ou seja, só conta +1 nível na primeira kill daquele tipo)
            set.Add(npcType);
        }

        // Salvar os dados do jogador (quando salva mundo/personagem)
        public override void SaveData(TagCompound tag)
        {
            // Vamos transformar o dicionário em uma lista de TagCompound
            var list = new List<TagCompound>();

            foreach (var kv in killsPerWeapon)
            {
                list.Add(new TagCompound {
                    ["itemType"] = kv.Key,                // arma
                    ["npcTypes"] = new List<int>(kv.Value) // lista dos tipos de NPC
                });
            }

            tag["killsPerWeapon"] = list;
        }

        // Carregar os dados do jogador (quando entra no mundo)
        public override void LoadData(TagCompound tag)
        {
            killsPerWeapon.Clear();

            if (!tag.ContainsKey("killsPerWeapon"))
                return;

            foreach (var entry in tag.GetList<TagCompound>("killsPerWeapon"))
            {
                int itemType = entry.GetInt("itemType");
                var npcTypes = entry.Get<List<int>>("npcTypes");

                // Reconstrói o HashSet a partir da lista salva
                killsPerWeapon[itemType] = new HashSet<int>(npcTypes);
            }
        }
    }
}
