namespace PACTDotNet
{
    public class Info
    {
        public string MO2_EXE { get; set; } = string.Empty;
        public string MO2_PATH { get; set; } = string.Empty;
        public string XEDIT_EXE { get; set; } = string.Empty;
        public string XEDIT_PATH { get; set; } = string.Empty;
        public string LOAD_ORDER_TXT { get; set; } = string.Empty;
        public string LOAD_ORDER_PATH { get; set; } = string.Empty;
        public int Journal_Expiration { get; set; } = 7;
        public int Cleaning_Timeout { get; set; } = 300;

        public bool MO2Mode { get; set; } = false;
        public List<string> XeditListFallout3 { get; set; }
        public HashSet<string> LowerFo3 { get; set; }
        public List<string> XeditListNewVegas { get; set; }
        public HashSet<string> LowerFnv { get; set; }
        public List<string> XeditListFallout4 { get; set; }
        public HashSet<string> LowerFo4 { get; set; }
        public List<string> XeditListSkyrimSe { get; set; }
        public HashSet<string> LowerSse { get; set; }
        public List<string> XeditListUniversal { get; set; }
        public List<string> XeditListSpecific { get; set; }
        public HashSet<string> LowerSpecific { get; set; }
        public HashSet<string> LowerUniversal { get; set; }

        public HashSet<int> CleanResultsUdr { get; set; } = new HashSet<int>();
        public HashSet<int> CleanResultsItm { get; set; } = new HashSet<int>();
        public HashSet<int> CleanResultsNvm { get; set; } = new HashSet<int>();
        public HashSet<int> CleanResultsPartialForms { get; set; } = new HashSet<int>();
        public HashSet<string> CleanFailedList { get; set; } = new HashSet<string>();
        public int PluginsProcessed { get; set; } = 0;
        public int PluginsCleaned { get; set; } = 0;

        public List<string> LclSkipList { get; set; } = new List<string>();

            // Hard exclude plugins per game here
        public List<string> Fo3SkipList { get; set; }
        public List<string> FnvSkipList { get; set; }
        public List<string> Fo4SkipList { get; set; }
        public List<string> SseSkipList { get; set; }
        public List<string> VipSkipList { get; set; }

        public string XeditLogTxt { get; set; } = string.Empty;
        public string XeditExcLog { get; set; } = string.Empty;

        public Info()
        {
            // Initialize lists from yaml settings
            XeditListFallout3 = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.XEdit_Lists.FO3");
            LowerFo3 = new HashSet<string>(XeditListFallout3.Select(s => s.ToLower()));
            XeditListNewVegas = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.XEdit_Lists.FNV");
            LowerFnv = new HashSet<string>(XeditListNewVegas.Select(s => s.ToLower()));
            XeditListFallout4 = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.XEdit_Lists.FO4");
            XeditListFallout4.AddRange(YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.XEdit_Lists.FO4VR"));
            LowerFo4 = new HashSet<string>(XeditListFallout4.Select(s => s.ToLower()));
            XeditListSkyrimSe = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.XEdit_Lists.SSE");
            XeditListSkyrimSe.AddRange(YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.XEdit_Lists.SkyrimVR"));
            LowerSse = new HashSet<string>(XeditListSkyrimSe.Select(s => s.ToLower()));
            XeditListUniversal = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.XEdit_Lists.Universal");
            XeditListSpecific = XeditListFallout3.Concat(XeditListNewVegas).Concat(XeditListFallout4).Concat(XeditListSkyrimSe).ToList();

            LowerSpecific = new HashSet<string>(XeditListSpecific.Select(s => s.ToLower()));
            LowerUniversal = new HashSet<string>(XeditListUniversal.Select(s => s.ToLower()));

            Fo3SkipList = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.Skip_Lists.FO3");
            FnvSkipList = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.Skip_Lists.FNV");
            Fo4SkipList = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.Skip_Lists.FO4");
            SseSkipList = YamlData.Pact_Main.ReadOrUpdateEntry<List<string>>("PACT_Data.Skip_Lists.SSE");

            VipSkipList =
            [
                .. Fo3SkipList,
                .. FnvSkipList,
                .. Fo4SkipList,
                .. SseSkipList
            ];
        }
    }
}

