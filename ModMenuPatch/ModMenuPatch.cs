using System.ComponentModel;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using ModMenuPatch.HarmonyPatches;
using Utilla;

namespace ModMenuPatch
{
	[Description("HauntedModMenu")]
	[BepInPlugin("org.Mangos.gorillatag.modmenupatch", "Mod Menu Patch", "1.0.2")]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[ModdedGamemode]
	public class ModMenuPatch : BaseUnityPlugin
	{
		public static bool modmenupatch = true;

		public static ConfigEntry<float> multiplier;

		public static ConfigEntry<float> speedMultiplier;

		public static ConfigEntry<float> jumpMultiplier;

		public static ConfigEntry<bool> randomColor;

		public static ConfigEntry<float> cycleSpeed;

		public static ConfigEntry<float> glowAmount;

		private void OnEnable()
		{
			ModMenuPatches.ApplyHarmonyPatches();
			ConfigFile configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "ModMonkeyPatch.cfg"), saveOnInit: true);
			speedMultiplier = configFile.Bind("Configuration", "SpeedMultiplier", 100f, "How much to multiply the speed. 10 = 10x higher jumps");
			jumpMultiplier = configFile.Bind("Configuration", "JumpMultiplier", 1.5f, "How much to multiply the jump height/distance by. 10 = 10x higher jumps");
			randomColor = configFile.Bind("rgb_monke", "RandomColor", defaultValue: false, "Whether to cycle through colours of rainbow or choose random colors");
			cycleSpeed = configFile.Bind("rgb_monke", "CycleSpeed", 0.004f, "The speed the color cycles at each frame (1=Full colour cycle). If random colour is enabled, this is the time in seconds before switching color");
			glowAmount = configFile.Bind("rgb_monke", "GlowAmount", 1f, "The brightness of your monkey. The higher the value, the more emissive your monkey is");
		}

		private void OnDisable()
		{
			ModMenuPatches.RemoveHarmonyPatches();
		}

		[ModdedGamemodeJoin]
		private void RoomJoined()
		{
			modmenupatch = true;
		}

		[ModdedGamemodeLeave]
		private void RoomLeft()
		{
			modmenupatch = true;
		}
	}
}
