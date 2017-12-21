using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAKProtocolManager
{
    class TestHistoryItem
    {
        public string Id, CableMark;
        static string sectionTemplate = "HistoryItem_{0}";
        public TestHistoryItem(string id, string cable_mark)
        {
            this.Id = id;
            this.CableMark = cable_mark;
        }
        static int MaxItemsInStorage = 10;

        public static TestHistoryItem[] PushToHistory(string test_id, string cable_mark)
        {
            TestHistoryItem[] story = GetFromIniFile();
            IniFile ini = new IniFile(Properties.Settings.Default.IniSettingsFileName);
            List<TestHistoryItem> thi = new List<TestHistoryItem>();
            TestHistoryItem[] newArr = new TestHistoryItem[] { };
            thi.Add(new TestHistoryItem(test_id, cable_mark));
            ini.Write("id", test_id, String.Format(sectionTemplate, 0));
            ini.Write("cable_mark", cable_mark, String.Format(sectionTemplate, 0));
            if (story.Length > 0)
            {
                int idx = 1;
                for(int i=0; i < story.Length; i++)
                {
                    if (story[i].Id != test_id)
                    {
                        thi.Add(story[i]);
                        idx++;
                    }
                    if (idx > MaxItemsInStorage - 1) break;
                }
            }
            newArr = thi.ToArray();
            WriteListToHistory(newArr);
            return newArr;
        }

        public static TestHistoryItem[] RemoveFromHistory(string test_id)
        {
            TestHistoryItem[] story, newArr;
            story = GetFromIniFile();
            List<TestHistoryItem> thi = new List<TestHistoryItem>();
            foreach(TestHistoryItem item in story)
            {
                if (item.Id != test_id) thi.Add(item);
            }
            newArr = thi.ToArray();
            WriteListToHistory(newArr);
            return newArr;
            
        }

        public static TestHistoryItem[] GetFromIniFile()
        {
            List<TestHistoryItem> thi = new List<TestHistoryItem>();
            IniFile ini = new IniFile(Properties.Settings.Default.IniSettingsFileName);
            for (int i=0; i < MaxItemsInStorage; i++)
            {
                string _id = ini.Read("id", String.Format(sectionTemplate, i));
                string _cabMark = ini.Read("cable_mark", String.Format(sectionTemplate, i));
                if (!String.IsNullOrWhiteSpace(_id + _cabMark)) thi.Add(new TestHistoryItem(_id, _cabMark));
            }
            return thi.ToArray();
        }

        private static void WriteListToHistory(TestHistoryItem[] history)
        {
            IniFile ini = new IniFile(Properties.Settings.Default.IniSettingsFileName);
            for(int i = 0; i<MaxItemsInStorage; i++)
            {
                if (i<history.Length)
                {
                    ini.Write("id", history[i].Id, String.Format(sectionTemplate, i));
                    ini.Write("cable_mark", history[i].CableMark, String.Format(sectionTemplate, i));
                }else
                {
                    if (ini.KeyExists("id", String.Format(sectionTemplate, i))) ini.DeleteSection(String.Format(sectionTemplate, i));
                }

            }
        }
    }
}
