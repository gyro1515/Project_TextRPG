using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TextRPG
{
    internal class Dungeon
    {
        public Dungeon()
        {
            rand = new Random();
            Name = "";
            Def = 0;
            Gold = 0;
        }
        /*public enum DungeonType
        {
            Easy, Normal, Hard
        }*/
        //DungeonType dgtype = DungeonType.Easy;
        // 랜덤 생성용
        Random rand;
        
        // 던전 이름 = 난이도
        public string Name {  get; set; }
        // 권장 방어력
        public int Def { get; set; }
        // 클리어 골드
        public int Gold { get; set; }
        // dgtype 세터
        //public DungeonType DgType { set { dgtype = value; } }

        public void Clear() // 던전 클리어 시
        {
            // 기본 체력 감소량 20 ~ 35 랜덤, 플레이어와 던전 방어력에 따라 추가 감소
            int hpLoss = rand.Next(20, 36) - (Player.Instance.TotalDef - Def);
            // hpLoss가 0보다 작거나 같다면 체력 감소x
            Player.Instance.CurHP -= hpLoss <= 0 ? 0 : hpLoss;
            if (Player.Instance.CurHP < 0) Player.Instance.CurHP = 0;

            // 골드 증가, (공격력 ~ 공격력 * 2)% 만큼 추가 골드
            float bonusGold = rand.Next(Player.Instance.TotalAtk, Player.Instance.TotalAtk * 2 + 1) / 100.0f;
            Player.Instance.Gold += Gold + (int)(Gold * bonusGold);
        }
    }
}
