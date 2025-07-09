using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal static class SaveManager // 유틸리티 함수는 굳이 싱글턴 사용할 필요x?
    {
        private static readonly string savePath = "player_save.json";
        public static void Save(Player player)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };
            string json = JsonSerializer.Serialize(player, options);
            File.WriteAllText(savePath, json);
            Console.WriteLine("플레이어 저장 중.");
            Thread.Sleep(500);
            Console.WriteLine("플레이어 저장 완료.");
            Thread.Sleep(800);
        }

        public static Player? Load()
        {
            if (!File.Exists(savePath))
            {
                Console.WriteLine("저장된 파일 없음. 새 플레이어 생성.");
                Thread.Sleep(2000);
                Console.Clear();
                return null;
            }

            string json = File.ReadAllText(savePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };
            Player? player = JsonSerializer.Deserialize<Player>(json, options);
            Console.WriteLine("플레이어 불러오기 완료.");
            Thread.Sleep(2000);
            Console.Clear();
            return player;
        }
    }
}
