using System;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
namespace Bring
{
    public static class Bring
    {
        public static readonly string[] Items = {
            "牛乳",
            "ジュース",
            "お米",
            "牛肉",
            "豚肉",
            "鶏肉",
            "ミンチ",
            "ハム",
            "ウィンナー",
            "シャケ",
            "秋刀魚",
            "サバ",
            "鯛",
            "たまご",
            "じゃがいも",
            "人参",
            "トマト",
            "キャベツ",
            "きゅうり",
            "レタス",
            "玉ねぎ",
            "ネギ",
            "豆腐",
            "コンニャク",
            "バナナ",
            "みかん",
            "りんご",
            "油",
            "焼肉のタレ",
            "食パン",
            "バター",
            "ヨーグルト",
        };

        public static readonly IList<bool> ParentStatus;
        public static readonly IList<bool> MainStatus;

        static Bring()
        {
            MainStatus = new bool[Items.Length];
            ParentStatus = new bool[Items.Length];
        }

        public static IList<bool> Reset(this IList<bool> items)
        {
            for (var i = 0; i < items.Count(); i++) items[i] = false;

            return items;
        }

        public static IEnumerable<string> GetItems(uint code)
        {
            for (var i = 0; code > 0; code >>= 1, i++)
            {
                if ((code & 1) > 0) yield return Items[i];
            }
        }
    }
}
