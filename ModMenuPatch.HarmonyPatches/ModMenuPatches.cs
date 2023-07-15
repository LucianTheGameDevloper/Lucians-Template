using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;


namespace ModMenuPatch.HarmonyPatches
{
    // Token: 0x02000010 RID: 16
    public class ModMenuPatches : MonoBehaviour
    {
        // Token: 0x17000005 RID: 5
        // (get) Token: 0x060000DD RID: 221 RVA: 0x00002385 File Offset: 0x00000585
        // (set) Token: 0x060000DE RID: 222 RVA: 0x0000238C File Offset: 0x0000058C
        public static bool IsPatched { get; private set; }

        // Token: 0x060000DF RID: 223 RVA: 0x00002394 File Offset: 0x00000594
        internal static void ApplyHarmonyPatches()
        {
            if (!ModMenuPatches.IsPatched)
            {
                if (ModMenuPatches.instance == null)
                {
                    ModMenuPatches.instance = new Harmony("com.legoandmars.gorillatag.modmenupatch");
                }
                ModMenuPatches.instance.PatchAll(Assembly.GetExecutingAssembly());
                ModMenuPatches.IsPatched = true;
            }
        }

        // Token: 0x060000E0 RID: 224 RVA: 0x000023C8 File Offset: 0x000005C8
        internal static void RemoveHarmonyPatches()
        {
            if (ModMenuPatches.instance != null && ModMenuPatches.IsPatched)
            {
                ModMenuPatches.IsPatched = false;
            }
        }


        // Token: 0x040000AB RID: 171
        private static Harmony instance;

        // Token: 0x040000AC RID: 172
        public const string InstanceId = "com.legoandmars.gorillatag.modmenupatch";

        // Token: 0x040000AE RID: 174
        public static string SetTaggedTime = "SetTaggedTime";
    }
}
