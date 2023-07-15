using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using GorillaNetworking;
using GTAG_NotificationLib;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static MB3_MeshBakerRoot.ZSortObjects;

namespace ModMenuPatch.HarmonyPatches
{


	[HarmonyPatch(typeof(GorillaLocomotion.Player))]
	[HarmonyPatch("LateUpdate", MethodType.Normal)]
	internal class MenuPatch
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000021D8 File Offset: 0x000003D8
		private static void Prefix()
		{
			try
			{
				bool flag = MenuPatch.maxJumpSpeed == null;
				bool flag2 = flag;
				if (flag2)
				{
					MenuPatch.maxJumpSpeed = new float?(GorillaLocomotion.Player.Instance.maxJumpSpeed);
				}
				bool flag3 = MenuPatch.jumpMultiplier == null;
				bool flag4 = flag3;
				if (flag4)
				{
					MenuPatch.jumpMultiplier = new float?(GorillaLocomotion.Player.Instance.jumpMultiplier);
				}
				bool flag5 = MenuPatch.maxArmLengthInitial == null;
				bool flag6 = flag5;
				if (flag6)
				{
					MenuPatch.maxArmLengthInitial = new float?(GorillaLocomotion.Player.Instance.maxArmLength);
					MenuPatch.leftHandOffsetInitial = new Vector3?(GorillaLocomotion.Player.Instance.leftHandOffset);
					MenuPatch.rightHandOffsetInitial = new Vector3?(GorillaLocomotion.Player.Instance.rightHandOffset);
				}

				GameObject gameObject = GameObject.Find("Shoulder Camera");
				Camera camera = (gameObject != null) ? gameObject.GetComponent<Camera>() : null;
				List<InputDevice> list = new List<InputDevice>();
				List<InputDevice> list2 = new List<InputDevice>();
				InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
				InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list2);
				list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out MenuPatch.gripDown);
				list2[0].TryGetFeatureValue(CommonUsages.primaryButton, out MenuPatch.primaryRightDown);
				list[0].TryGetFeatureValue(CommonUsages.grip, out MenuPatch.BoomGrip);
				list[0].TryGetFeatureValue(CommonUsages.trigger, out MenuPatch.SpawnGrip);
				bool flag7 = MenuPatch.gripDown && MenuPatch.menu == null;
				bool flag8 = flag7;
				if (flag8)
				{
					MenuPatch.Draw();
					bool flag9 = MenuPatch.reference == null;
					bool flag10 = flag9;
					if (flag10)
					{
						MenuPatch.reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
						UnityEngine.Object.Destroy(MenuPatch.reference.GetComponent<MeshRenderer>());
						MenuPatch.reference.transform.parent = GorillaLocomotion.Player.Instance.rightControllerTransform;
						MenuPatch.reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
						MenuPatch.reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
					}
				}
				else
				{
					bool flag11 = !MenuPatch.gripDown && MenuPatch.menu != null;
					bool flag12 = flag11;
					if (flag12)
					{
						UnityEngine.Object.Destroy(MenuPatch.menu);
						MenuPatch.menu = null;
						UnityEngine.Object.Destroy(MenuPatch.reference);
						MenuPatch.reference = null;
					}
				}
				bool flag13 = MenuPatch.gripDown && MenuPatch.menu != null;
				bool flag14 = flag13;
				if (flag14)
				{
					MenuPatch.menu.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
					MenuPatch.menu.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
				}
				bool? flag15 = MenuPatch.buttonsActive[0];
				bool flag16 = true;
				bool flag17 = flag15.GetValueOrDefault() == flag16 & flag15 != null;
				bool flag18 = flag17;
				if (flag18)
				{
					PhotonNetwork.Disconnect();
				}
				if (buttonsActive[1] == true)
				{
					PhotonNetwork.JoinRandomRoom();
				}
				if (buttonsActive[2] == true)
				{
					Application.Quit();
				}
            }
			catch (Exception ex)
			{
				File.WriteAllText("urmodmenu.log", ex.ToString());
			}
			{
				GameObject.Find("motdtext").GetComponent<Text>().text = "<color=white> Full Menu By Lucian Check Code Of Conduct For Credits </color>";
				GameObject.Find("COC Text").GetComponent<Text>().text = "<color=white> Lucian: Menu, Motd \n Klone: Some Code For The Menu And More!\n Sirskelo: Made Idea For This Menu And Made Comp \n Sir: Ideas And Is Cool  </color>";
				GameObject.Find("CodeOfConduct").GetComponent<Text>().text = "<color=white> KYS LEMMING MOFO </color>";
				GameObject.Find("motd").GetComponent<Text>().text = "<color=white> KYS LEMMING MOFO </color>";
			}
		}

		private static float Teletime;

		// Token: 0x04000092 RID: 146
		public static float minlag;

		// Token: 0x04000093 RID: 147
		public static float maxlag;


        // Token: 0x06000014 RID: 20 RVA: 0x0000387C File Offset: 0x00001A7C

        private static GradientColorKey[] colorKeys;

      
        private static void ProcessTriggerPlatformMonke()
        {
            MenuPatch.colorKeys[0].color = Color.red;
            MenuPatch.colorKeys[0].time = 0f;
            MenuPatch.colorKeys[1].color = Color.green;
            MenuPatch.colorKeys[1].time = 0.3f;
            MenuPatch.colorKeys[2].color = Color.blue;
            MenuPatch.colorKeys[2].time = 0.6f;
            MenuPatch.colorKeys[3].color = Color.red;
            MenuPatch.colorKeys[3].time = 1f;
            if (!MenuPatch.once_networking)
            {
                PhotonNetwork.NetworkingClient.EventReceived += MenuPatch.PlatformNetwork;
                MenuPatch.once_networking = true;
            }
            List<InputDevice> list = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
            list[0].TryGetFeatureValue(CommonUsages.triggerButton, out MenuPatch.gripDown_left);
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
            list[0].TryGetFeatureValue(CommonUsages.triggerButton, out MenuPatch.gripDown_right);
            if (MenuPatch.gripDown_right)
            {
                if (!MenuPatch.once_right && MenuPatch.jump_right_local == null)
                {
                    MenuPatch.jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    MenuPatch.jump_right_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                    MenuPatch.jump_right_local.transform.localScale = MenuPatch.scale;
                    MenuPatch.jump_right_local.transform.position = new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                    MenuPatch.jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
                    object[] eventContent = new object[]
                    {
                new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
                GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
                    };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others
                    };
                    PhotonNetwork.RaiseEvent(70, eventContent, raiseEventOptions, SendOptions.SendReliable);
                    MenuPatch.once_right = true;
                    MenuPatch.once_right_false = false;
                    MenuPatch.ColorChanger colorChanger = MenuPatch.jump_right_local.AddComponent<MenuPatch.ColorChanger>();
                    colorChanger.colors = new Gradient
                    {
                        colorKeys = MenuPatch.colorKeys
                    };
                    colorChanger.Start();
                }
            }
            else if (!MenuPatch.once_right_false && MenuPatch.jump_right_local != null)
            {
                UnityEngine.Object.Destroy(MenuPatch.jump_right_local);
                MenuPatch.jump_right_local = null;
                MenuPatch.once_right = false;
                MenuPatch.once_right_false = true;
                RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(72, null, raiseEventOptions2, SendOptions.SendReliable);
            }
            if (MenuPatch.gripDown_left)
            {
                if (!MenuPatch.once_left && MenuPatch.jump_left_local == null)
                {
                    MenuPatch.jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    MenuPatch.jump_left_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                    MenuPatch.jump_left_local.transform.localScale = MenuPatch.scale;
                    MenuPatch.jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                    MenuPatch.jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
                    object[] eventContent2 = new object[]
                    {
                GorillaLocomotion.Player.Instance.leftControllerTransform.position,
                GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
                    };
                    RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others
                    };
                    PhotonNetwork.RaiseEvent(69, eventContent2, raiseEventOptions3, SendOptions.SendReliable);
                    MenuPatch.once_left = true;
                    MenuPatch.once_left_false = false;
                    MenuPatch.ColorChanger colorChanger2 = MenuPatch.jump_left_local.AddComponent<MenuPatch.ColorChanger>();
                    colorChanger2.colors = new Gradient
                    {
                        colorKeys = MenuPatch.colorKeys
                    };
                    colorChanger2.Start();
                }
            }
            else if (!MenuPatch.once_left_false && MenuPatch.jump_left_local != null)
            {
                UnityEngine.Object.Destroy(MenuPatch.jump_left_local);
                MenuPatch.jump_left_local = null;
                MenuPatch.once_left = false;
                MenuPatch.once_left_false = true;
                RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent(71, null, raiseEventOptions4, SendOptions.SendReliable);
            }
            if (!PhotonNetwork.InRoom)
            {
                for (int i = 0; i < MenuPatch.jump_right_network.Length; i++)
                {
                    UnityEngine.Object.Destroy(MenuPatch.jump_right_network[i]);
                }
                for (int j = 0; j < MenuPatch.jump_left_network.Length; j++)
                {
                    UnityEngine.Object.Destroy(MenuPatch.jump_left_network[j]);
                }
            }
        }

        private static void ProcessTeleportGun()
		{
			bool flag = false;
			bool flag2 = false;
			List<InputDevice> list = new List<InputDevice>();
			InputDevices.GetDevices(list);
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
			list[0].TryGetFeatureValue(CommonUsages.triggerButton, out flag);
			list[0].TryGetFeatureValue(CommonUsages.gripButton, out flag2);
			bool flag3 = flag2;
			bool flag4 = flag3;
			if (flag4)
			{
				RaycastHit raycastHit;
				Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit);
				bool flag5 = MenuPatch.pointer == null;
				bool flag6 = flag5;
				if (flag6)
				{
					MenuPatch.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					UnityEngine.Object.Destroy(MenuPatch.pointer.GetComponent<Rigidbody>());
					UnityEngine.Object.Destroy(MenuPatch.pointer.GetComponent<SphereCollider>());
					MenuPatch.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
				}
				MenuPatch.pointer.transform.position = raycastHit.point;
				bool flag7 = flag;
				bool flag8 = flag7;
				if (flag8)
				{
					bool flag9 = !MenuPatch.teleportGunAntiRepeat;
					bool flag10 = flag9;
					if (flag10)
					{
						GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().isKinematic = true;
						GorillaLocomotion.Player.Instance.transform.position = raycastHit.point;
						GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
						GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().isKinematic = false;
						MenuPatch.teleportGunAntiRepeat = true;
					}
				}
				else
				{
					MenuPatch.teleportGunAntiRepeat = false;
				}
			}
			else
			{
				UnityEngine.Object.Destroy(MenuPatch.pointer);
				MenuPatch.pointer = null;
				MenuPatch.teleportGunAntiRepeat = false;
			}
		}



		// Token: 0x06000016 RID: 22 RVA: 0x00003BEC File Offset: 0x00001DEC
		private static void ProcessNoClip()
		{
			bool flag = false;
			List<InputDevice> list = new List<InputDevice>();
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
			list[0].TryGetFeatureValue(CommonUsages.triggerButton, out flag);
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				bool flag4 = !MenuPatch.flag2;
				bool flag5 = flag4;
				if (flag5)
				{
					MeshCollider[] array = Resources.FindObjectsOfTypeAll<MeshCollider>();
					foreach (MeshCollider meshCollider in array)
					{
						meshCollider.transform.localScale = meshCollider.transform.localScale / 10000f;
					}
					MenuPatch.flag2 = true;
					MenuPatch.flag1 = false;
				}
			}
			else
			{
				bool flag6 = !MenuPatch.flag1;
				bool flag7 = flag6;
				if (flag7)
				{
					MeshCollider[] array3 = Resources.FindObjectsOfTypeAll<MeshCollider>();
					foreach (MeshCollider meshCollider2 in array3)
					{
						meshCollider2.transform.localScale = meshCollider2.transform.localScale * 10000f;
					}
					MenuPatch.flag1 = true;
					MenuPatch.flag2 = false;
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003D0C File Offset: 0x00001F0C
		private static void ProcessInvisPlatformMonke()
		{
			List<InputDevice> list = new List<InputDevice>();
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
			list[0].TryGetFeatureValue(CommonUsages.gripButton, out MenuPatch.gripDown_left);
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
			list[0].TryGetFeatureValue(CommonUsages.gripButton, out MenuPatch.gripDown_right);
			bool flag = MenuPatch.gripDown_right;
			if (flag)
			{
				bool flag2 = !MenuPatch.once_right && MenuPatch.jump_right_local == null;
				if (flag2)
				{
					MenuPatch.jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
					MenuPatch.jump_right_local.GetComponent<Renderer>().enabled = false;
					MenuPatch.jump_right_local.transform.localScale = MenuPatch.scale;
					MenuPatch.jump_right_local.transform.position = new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
					MenuPatch.jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
					object[] array = new object[]
					{
						new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
						GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
					};
					MenuPatch.once_right = true;
					MenuPatch.once_right_false = false;
				}
			}
			else
			{
				bool flag3 = !MenuPatch.once_right_false && MenuPatch.jump_right_local != null;
				if (flag3)
				{
					UnityEngine.Object.Destroy(MenuPatch.jump_right_local);
					MenuPatch.jump_right_local = null;
					MenuPatch.once_right = false;
					MenuPatch.once_right_false = true;
				}
			}
			bool flag4 = MenuPatch.gripDown_left;
			if (flag4)
			{
				bool flag5 = !MenuPatch.once_left && MenuPatch.jump_left_local == null;
				if (flag5)
				{
					MenuPatch.jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
					MenuPatch.jump_left_local.GetComponent<Renderer>().enabled = false;
					MenuPatch.jump_left_local.transform.localScale = MenuPatch.scale;
					MenuPatch.jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
					MenuPatch.jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
					object[] array2 = new object[]
					{
						GorillaLocomotion.Player.Instance.leftControllerTransform.position,
						GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
					};
					MenuPatch.once_left = true;
					MenuPatch.once_left_false = false;
				}
			}
			else
			{
				bool flag6 = !MenuPatch.once_left_false && MenuPatch.jump_left_local != null;
				if (flag6)
				{
					UnityEngine.Object.Destroy(MenuPatch.jump_left_local);
					MenuPatch.jump_left_local = null;
					MenuPatch.once_left = false;
					MenuPatch.once_left_false = true;
				}
			}
			bool flag7 = !PhotonNetwork.InRoom;
			if (flag7)
			{
				for (int i = 0; i < MenuPatch.jump_right_network.Length; i++)
				{
					UnityEngine.Object.Destroy(MenuPatch.jump_right_network[i]);
				}
				for (int j = 0; j < MenuPatch.jump_left_network.Length; j++)
				{
					UnityEngine.Object.Destroy(MenuPatch.jump_left_network[j]);
				}
			}
		}

        // Token: 0x06000018 RID: 24 RVA: 0x0000403C File Offset: 0x0000223C

        public static int bigmonkecooldown;
        private static void AntiBan2()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.RemovedFromList = true;
                if (MenuPatch.bigmonkecooldown == 0)
                {
                    MenuPatch.bigmonkecooldown = Time.frameCount + 100;
                    foreach (GorillaNot gorillaNot in UnityEngine.Object.FindObjectsOfType<GorillaNot>())
                    {
                        gorillaNot.photonView.RequestOwnership();
                        if (gorillaNot.photonView.IsMine)
                        {
                            PhotonNetwork.Destroy(gorillaNot.photonView);
                            UnityEngine.Object.Destroy(UnityEngine.Object.FindObjectOfType<GorillaNot>().gameObject);
                        }
                    }
                    PhotonNetwork.CurrentRoom.RemovedFromList = true;
                    PhotonNetwork.OfflineMode = true;
                    PhotonNetwork.OpRemoveCompleteCache();
                    PhotonNetwork.QuickResends = 9999;
                    PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
                    PhotonNetwork.SendRate = 99999;
                    PhotonNetwork.SerializationRate = 8888;
                    PhotonNetwork.CurrentRoom.ClearExpectedUsers();
                    PhotonNetwork.CurrentRoom.EmptyRoomTtl = 8888;
                    PhotonNetwork.CurrentRoom.PlayerTtl = 9999;
                    PhotonNetwork.SendAllOutgoingCommands();
                    PhotonNetwork.OpRemoveCompleteCache();
                }
                GorillaNot.instance.enabled = false;
                GorillaNot.instance.rpcCallLimit = 99999;
                GorillaGameManager.instance.enabled = false;
                PhotonNetwork.OpRemoveCompleteCache();
                PhotonNetwork.SendAllOutgoingCommands();
            }
            GorillaGameManager.instance.tempString = "";
            GorillaNot.instance.testAssault = false;
            foreach (GorillaNot gorillaNot2 in UnityEngine.Object.FindObjectsOfType<GorillaNot>())
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                }
                if (!gorillaNot2.photonView.IsMine)
                {
                    gorillaNot2.photonView.RequestOwnership();
                }
                PhotonNetwork.Destroy(gorillaNot2.photonView);
                UnityEngine.Object.Destroy(gorillaNot2.gameObject);
                PhotonNetwork.SendAllOutgoingCommands();
            }
        }

        private static void undmaster()
        {
            new GorillaNot();
            GorillaNot.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            GorillaGameManager.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);
            GorillaGameManager.instance.photonView.RequestOwnership();
            GorillaGameManager.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            GorillaGameManager.instance.photonView.RequestOwnership();
            GorillaGameManager.instance.currentRoom.masterClientId = PhotonNetwork.LocalPlayer.ActorNumber;
            GorillaGameManager.instance.photonView.RequestOwnership();
            GorillaGameManager.instance.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            GorillaGameManager.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            GorillaNot.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            GorillaGameManager.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);
            GorillaNot.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            GorillaGameManager.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);
            GorillaGameManager.instance.photonView.RequestOwnership();
            GorillaGameManager.instance.currentMasterClient = PhotonNetwork.LocalPlayer;
            GorillaGameManager.instance.photonView.RequestOwnership();
            GorillaGameManager.instance.currentRoom.masterClientId = PhotonNetwork.LocalPlayer.ActorNumber;
            GorillaComputer.instance.OnMasterClientSwitched(PhotonNetwork.LocalPlayer);
            GorillaComputer.instance.OnConnectedToMaster();
            GorillaGameManager.instance.tempString = "nobanplspls";
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            GorillaGameManager.instance.photonView.RequestOwnership();
            GorillaGameManager.instance.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            MenuPatch.AntiBan2();
            MenuPatch.AntiBan2();
            MenuPatch.AntiBan2();
            MenuPatch.AntiBan2();
            MenuPatch.AntiBan2();
            MenuPatch.AntiBan2();
        }

        private static void ProcessPlatformMonke()
		{
			MenuPatch.colorKeysPlatformMonke[0].color = Color.red;
			MenuPatch.colorKeysPlatformMonke[0].time = 0f;
			MenuPatch.colorKeysPlatformMonke[1].color = Color.green;
			MenuPatch.colorKeysPlatformMonke[1].time = 0.3f;
			MenuPatch.colorKeysPlatformMonke[2].color = Color.blue;
			MenuPatch.colorKeysPlatformMonke[2].time = 0.6f;
			MenuPatch.colorKeysPlatformMonke[3].color = Color.red;
			MenuPatch.colorKeysPlatformMonke[3].time = 1f;
			bool flag = !MenuPatch.once_networking;
			bool flag2 = flag;
			if (flag2)
			{
				PhotonNetwork.NetworkingClient.EventReceived += MenuPatch.PlatformNetwork;
				MenuPatch.once_networking = true;
			}
			List<InputDevice> list = new List<InputDevice>();
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
			list[0].TryGetFeatureValue(CommonUsages.gripButton, out MenuPatch.gripDown_left);
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
			list[0].TryGetFeatureValue(CommonUsages.gripButton, out MenuPatch.gripDown_right);
			bool flag3 = MenuPatch.gripDown_right;
			bool flag4 = flag3;
			if (flag4)
			{
				bool flag5 = !MenuPatch.once_right;
				bool flag6 = flag5;
				if (flag6)
				{
					bool flag7 = MenuPatch.jump_right_local == null;
					bool flag8 = flag7;
					if (flag8)
					{
						MenuPatch.jump_right_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
						MenuPatch.jump_right_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
						MenuPatch.jump_right_local.transform.localScale = MenuPatch.scale;
						MenuPatch.jump_right_local.transform.position = new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position;
						MenuPatch.jump_right_local.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation;
						object[] eventContent = new object[]
						{
							new Vector3(0f, -0.0075f, 0f) + GorillaLocomotion.Player.Instance.rightControllerTransform.position,
							GorillaLocomotion.Player.Instance.rightControllerTransform.rotation
						};
						RaiseEventOptions raiseEventOptions = new RaiseEventOptions
						{
							Receivers = ReceiverGroup.Others
						};
						PhotonNetwork.RaiseEvent(70, eventContent, raiseEventOptions, SendOptions.SendReliable);
						MenuPatch.once_right = true;
						MenuPatch.once_right_false = false;
						MenuPatch.ColorChanger colorChanger = MenuPatch.jump_right_local.AddComponent<MenuPatch.ColorChanger>();
						colorChanger.colors = new Gradient
						{
							colorKeys = MenuPatch.colorKeysPlatformMonke
						};
						colorChanger.Start();
					}
				}
			}
			else
			{
				bool flag9 = !MenuPatch.once_right_false;
				bool flag10 = flag9;
				if (flag10)
				{
					bool flag11 = MenuPatch.jump_right_local != null;
					bool flag12 = flag11;
					if (flag12)
					{
						UnityEngine.Object.Destroy(MenuPatch.jump_right_local);
						MenuPatch.jump_right_local = null;
						MenuPatch.once_right = false;
						MenuPatch.once_right_false = true;
						RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
						{
							Receivers = ReceiverGroup.Others
						};
						PhotonNetwork.RaiseEvent(72, null, raiseEventOptions2, SendOptions.SendReliable);
					}
				}
			}
			bool flag13 = MenuPatch.gripDown_left;
			bool flag14 = flag13;
			if (flag14)
			{
				bool flag15 = !MenuPatch.once_left;
				bool flag16 = flag15;
				if (flag16)
				{
					bool flag17 = MenuPatch.jump_left_local == null;
					bool flag18 = flag17;
					if (flag18)
					{
						MenuPatch.jump_left_local = GameObject.CreatePrimitive(PrimitiveType.Cube);
						MenuPatch.jump_left_local.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
						MenuPatch.jump_left_local.transform.localScale = MenuPatch.scale;
						MenuPatch.jump_left_local.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
						MenuPatch.jump_left_local.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation;
						object[] eventContent2 = new object[]
						{
							GorillaLocomotion.Player.Instance.leftControllerTransform.position,
							GorillaLocomotion.Player.Instance.leftControllerTransform.rotation
						};
						RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
						{
							Receivers = ReceiverGroup.Others
						};
						PhotonNetwork.RaiseEvent(69, eventContent2, raiseEventOptions3, SendOptions.SendReliable);
						MenuPatch.once_left = true;
						MenuPatch.once_left_false = false;
						MenuPatch.ColorChanger colorChanger2 = MenuPatch.jump_left_local.AddComponent<MenuPatch.ColorChanger>();
						colorChanger2.colors = new Gradient
						{
							colorKeys = MenuPatch.colorKeysPlatformMonke
						};
						colorChanger2.Start();
					}
				}
			}
			else
			{
				bool flag19 = !MenuPatch.once_left_false;
				bool flag20 = flag19;
				if (flag20)
				{
					bool flag21 = MenuPatch.jump_left_local != null;
					bool flag22 = flag21;
					if (flag22)
					{
						UnityEngine.Object.Destroy(MenuPatch.jump_left_local);
						MenuPatch.jump_left_local = null;
						MenuPatch.once_left = false;
						MenuPatch.once_left_false = true;
						RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
						{
							Receivers = ReceiverGroup.Others
						};
						PhotonNetwork.RaiseEvent(71, null, raiseEventOptions4, SendOptions.SendReliable);
					}
				}
			}
			bool flag23 = !PhotonNetwork.InRoom;
			bool flag24 = flag23;
			if (flag24)
			{
				for (int i = 0; i < MenuPatch.jump_right_network.Length; i++)
				{
					UnityEngine.Object.Destroy(MenuPatch.jump_right_network[i]);
				}
				for (int j = 0; j < MenuPatch.jump_left_network.Length; j++)
				{
					UnityEngine.Object.Destroy(MenuPatch.jump_left_network[j]);
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00004584 File Offset: 0x00002784
		private static void PlatformNetwork(EventData eventData)
		{
			byte code = eventData.Code;
			bool flag = code == 69;
			bool flag2 = flag;
			if (flag2)
			{
				object[] array = (object[])eventData.CustomData;
				MenuPatch.jump_left_network[eventData.Sender] = GameObject.CreatePrimitive(PrimitiveType.Cube);
				MenuPatch.jump_left_network[eventData.Sender].GetComponent<Renderer>().material.SetColor("_Color", Color.black);
				MenuPatch.jump_left_network[eventData.Sender].transform.localScale = MenuPatch.scale;
				MenuPatch.jump_left_network[eventData.Sender].transform.position = (Vector3)array[0];
				MenuPatch.jump_left_network[eventData.Sender].transform.rotation = (Quaternion)array[1];
				MenuPatch.ColorChanger colorChanger = MenuPatch.jump_left_network[eventData.Sender].AddComponent<MenuPatch.ColorChanger>();
				colorChanger.colors = new Gradient
				{
					colorKeys = MenuPatch.colorKeysPlatformMonke
				};
				colorChanger.Start();
			}
			else
			{
				bool flag3 = code == 70;
				bool flag4 = flag3;
				if (flag4)
				{
					object[] array2 = (object[])eventData.CustomData;
					MenuPatch.jump_right_network[eventData.Sender] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					MenuPatch.jump_right_network[eventData.Sender].GetComponent<Renderer>().material.SetColor("_Color", Color.black);
					MenuPatch.jump_right_network[eventData.Sender].transform.localScale = MenuPatch.scale;
					MenuPatch.jump_right_network[eventData.Sender].transform.position = (Vector3)array2[0];
					MenuPatch.jump_right_network[eventData.Sender].transform.rotation = (Quaternion)array2[1];
					MenuPatch.ColorChanger colorChanger2 = MenuPatch.jump_right_network[eventData.Sender].AddComponent<MenuPatch.ColorChanger>();
					colorChanger2.colors = new Gradient
					{
						colorKeys = MenuPatch.colorKeysPlatformMonke
					};
					colorChanger2.Start();
				}
				else
				{
					bool flag5 = code == 71;
					bool flag6 = flag5;
					if (flag6)
					{
						UnityEngine.Object.Destroy(MenuPatch.jump_left_network[eventData.Sender]);
						MenuPatch.jump_left_network[eventData.Sender] = null;
					}
					else
					{
						bool flag7 = code == 72;
						bool flag8 = flag7;
						if (flag8)
						{
							UnityEngine.Object.Destroy(MenuPatch.jump_right_network[eventData.Sender]);
							MenuPatch.jump_right_network[eventData.Sender] = null;
						}
					}
				}
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000047D4 File Offset: 0x000029D4
		private static void ProcessCheckpointTeleport()
		{
			bool flag = false;
			bool flag2 = false;
			List<InputDevice> list = new List<InputDevice>();
			InputDevices.GetDevices(list);
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, list);
			list[0].TryGetFeatureValue(CommonUsages.triggerButton, out flag);
			list[0].TryGetFeatureValue(CommonUsages.gripButton, out flag2);
			bool flag3 = flag;
			bool flag4 = flag3;
			if (flag4)
			{
				MenuPatch.checkpointPos = new Vector3?(GorillaLocomotion.Player.Instance.transform.position);
			}
			bool flag5 = flag2 && MenuPatch.checkpointPos != null;
			bool flag6 = flag5;
			if (flag6)
			{
				bool flag7 = !MenuPatch.checkpointTeleportAntiRepeat;
				bool flag8 = flag7;
				if (flag8)
				{
					GorillaLocomotion.Player.Instance.transform.position = MenuPatch.checkpointPos.Value;
					GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
					MenuPatch.checkpointTeleportAntiRepeat = true;
				}
			}
			else
			{
				MenuPatch.checkpointTeleportAntiRepeat = false;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000048D4 File Offset: 0x00002AD4
		private static void TeleportToRandomPlayer()
		{
			bool flag = !MenuPatch.foundPlayer;
			bool flag2 = flag;
			if (flag2)
			{
				Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
				System.Random random = new System.Random();
				int num = random.Next(playerList.Length);
				PhotonView photonView = GorillaGameManager.instance.FindVRRigForPlayer(playerList[num]);
				bool flag3 = photonView != null;
				bool flag4 = flag3;
				if (flag4)
				{
					GorillaLocomotion.Player.Instance.transform.position = photonView.transform.position;
					GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
					MenuPatch.foundPlayer = true;
				}
			}
		}




		// Token: 0x0600001E RID: 30 RVA: 0x00004D1C File Offset: 0x00002F1C
		private static void ProcessTagGun()
		{
			bool flag = false;
			bool flag2 = false;
			List<InputDevice> list = new List<InputDevice>();
			InputDevices.GetDevices(list);
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
			list[0].TryGetFeatureValue(CommonUsages.triggerButton, out flag);
			list[0].TryGetFeatureValue(CommonUsages.gripButton, out flag2);
			bool flag3 = !flag2;
			if (flag3)
			{
				UnityEngine.Object.Destroy(MenuPatch.pointer);
				MenuPatch.pointer = null;
				MenuPatch.checkpointTeleportAntiRepeat = false;
			}
			else
			{
				RaycastHit raycastHit;
				Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position - GorillaLocomotion.Player.Instance.rightControllerTransform.up, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out raycastHit);
				bool flag4 = MenuPatch.pointer == null;
				if (flag4)
				{
					MenuPatch.pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					UnityEngine.Object.Destroy(MenuPatch.pointer.GetComponent<Rigidbody>());
					UnityEngine.Object.Destroy(MenuPatch.pointer.GetComponent<SphereCollider>());
					MenuPatch.pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
				}
				MenuPatch.pointer.transform.position = raycastHit.point;
				bool flag5 = !flag;
				if (flag5)
				{
					MenuPatch.checkpointTeleportAntiRepeat = false;
				}
				else
				{
					bool flag6 = !MenuPatch.checkpointTeleportAntiRepeat;
					if (flag6)
					{
						foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
						{
							PhotonView.Get(GorillaGameManager.instance.GetComponent<GorillaGameManager>()).RPC("ReportTagRPC", RpcTarget.MasterClient, new object[]
							{
								player
							});
						}
						MenuPatch.checkpointTeleportAntiRepeat = true;
					}
				}
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00004ECC File Offset: 0x000030CC
		private static void ProcessLongArms()
		{
			List<InputDevice> list = new List<InputDevice>();
			InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, list);
			list[0].TryGetFeatureValue(CommonUsages.gripButton, out MenuPatch.gain);
			list[0].TryGetFeatureValue(CommonUsages.triggerButton, out MenuPatch.less);
			list[0].TryGetFeatureValue(CommonUsages.primaryButton, out MenuPatch.reset);
			list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out MenuPatch.fastr);
			MenuPatch.timeSinceLastChange += Time.deltaTime;
			bool flag = MenuPatch.timeSinceLastChange > 0.2f;
			if (flag)
			{
				GorillaLocomotion.Player.Instance.leftHandOffset = new Vector3(0f, MenuPatch.myVarY1, 0f);
				GorillaLocomotion.Player.Instance.rightHandOffset = new Vector3(0f, MenuPatch.myVarY2, 0f);
				GorillaLocomotion.Player.Instance.maxArmLength = 200f;
				bool flag2 = MenuPatch.gain;
				if (flag2)
				{
					MenuPatch.timeSinceLastChange = 0f;
					MenuPatch.myVarY1 += MenuPatch.gainSpeed;
					MenuPatch.myVarY2 += MenuPatch.gainSpeed;
					bool flag3 = MenuPatch.myVarY1 >= 201f;
					if (flag3)
					{
						MenuPatch.myVarY1 = 200f;
						MenuPatch.myVarY2 = 200f;
					}
				}
				bool flag4 = MenuPatch.less;
				if (flag4)
				{
					MenuPatch.timeSinceLastChange = 0f;
					MenuPatch.myVarY1 -= MenuPatch.gainSpeed;
					MenuPatch.myVarY2 -= MenuPatch.gainSpeed;
					bool flag5 = MenuPatch.myVarY2 <= -6f;
					if (flag5)
					{
						MenuPatch.myVarY1 = -5f;
						MenuPatch.myVarY2 = -5f;
					}
				}
				bool flag6 = MenuPatch.reset;
				if (flag6)
				{
					MenuPatch.timeSinceLastChange = 0f;
					MenuPatch.myVarY1 = 0f;
					MenuPatch.myVarY2 = 0f;
				}
				bool flag7 = MenuPatch.fastr && MenuPatch.myVarY1 == 5f;
				if (flag7)
				{
					MenuPatch.myVarY1 = 10f;
					MenuPatch.myVarY2 = 10f;
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000050E4 File Offset: 0x000032E4
		private static void AddButton(float offset, string text)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
			gameObject.GetComponent<BoxCollider>().isTrigger = true;
			gameObject.transform.parent = MenuPatch.menu.transform;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f);
			gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.4f - offset);
			gameObject.AddComponent<BtnCollider>().relatedText = text;
			int num = -1;
			for (int i = 0; i < MenuPatch.buttons.Length; i++)
			{
				bool flag = text == MenuPatch.buttons[i];
				bool flag2 = flag;
				if (flag2)
				{
					num = i;
					break;
				}
			}
			bool? flag3 = MenuPatch.buttonsActive[num];
			bool flag4 = false;
			bool flag5 = flag3.GetValueOrDefault() == flag4 & flag3 != null;
			bool flag6 = flag5;
			if (flag6)
			{
				gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
			}
			else
			{
				flag3 = MenuPatch.buttonsActive[num];
				flag4 = true;
				bool flag7 = flag3.GetValueOrDefault() == flag4 & flag3 != null;
				bool flag8 = flag7;
				if (flag8)
				{
					gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 25, 106, 1));
				}
				else
				{
					gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 25, 106, 1));
                }
			}
			Text text2 = new GameObject
			{
				transform =
				{
					parent = MenuPatch.canvasObj.transform
				}
			}.AddComponent<Text>();
			text2.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
			text2.text = text;
			text2.fontSize = 1;
            text2.fontStyle = FontStyle.Bold;
            text2.alignment = TextAnchor.MiddleCenter;
			text2.resizeTextForBestFit = true;
			text2.resizeTextMinSize = 0;
			RectTransform component = text2.GetComponent<RectTransform>();
			component.localPosition = Vector3.zero;
			component.sizeDelta = new Vector2(0.2f, 0.03f);
			component.localPosition = new Vector3(0.064f, 0f, 0.16f - offset / 2.55f);
			component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
		}

        // Token: 0x06000021 RID: 33 RVA: 0x00005358 File Offset: 0x00003558
        public static void Draw()
        {
            MenuPatch.menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(MenuPatch.menu.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(MenuPatch.menu.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(MenuPatch.menu.GetComponent<Renderer>());
            MenuPatch.menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f);
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
            gameObject.transform.parent = MenuPatch.menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.1f, 1f, 0.7f);
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 25, 106, 1));
            gameObject.transform.position = new Vector3(0.05f, 0f, 0f);
            GradientColorKey[] array = new GradientColorKey[3];
			array[0].color = new Color32(0, 25, 106, 1);
            array[0].time = 0f;
			array[1].color = new Color32(0, 25, 106, 1);
            array[1].time = 0.5f;
			array[2].color = new Color32(0, 25, 106, 1);
            array[2].time = 1f;
            MenuPatch.ColorChanger colorChanger = gameObject.AddComponent<MenuPatch.ColorChanger>();
            colorChanger.colors = new Gradient
            {
                colorKeys = array
            };
            colorChanger.Start();
            MenuPatch.canvasObj = new GameObject();
            MenuPatch.canvasObj.transform.parent = MenuPatch.menu.transform;
            Canvas canvas = MenuPatch.canvasObj.AddComponent<Canvas>();
            CanvasScaler canvasScaler = MenuPatch.canvasObj.AddComponent<CanvasScaler>();
            MenuPatch.canvasObj.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 1000f;
            Text text = new GameObject
            {
                transform =
        {
            parent = MenuPatch.canvasObj.transform
        }
            }.AddComponent<Text>();
            text.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
            text.text = "Kys Lemming V1";
            text.color = Color.white;
            text.fontStyle = FontStyle.Bold;
            text.fontSize = 1;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.28f, 0.05f);
            component.position = new Vector3(0.06f, 0f, 0.11f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            MenuPatch.AddPageButtons();
            string[] array2 = MenuPatch.buttons.Skip(MenuPatch.pageNumber * MenuPatch.pageSize).Take(MenuPatch.pageSize).ToArray<string>();
            for (int i = 0; i < array2.Length; i++)
            {
                MenuPatch.AddButton((float)i * 0.13f + 0.26f, array2[i]);
                MenuPatch.PanicButton();
				MenuPatch.Disconnect();
            }
        }



        public static void PanicButton()
        {
            float num = 0.05f;
            int num2 = (MenuPatch.buttons.Length + MenuPatch.pageSize + 1) / MenuPatch.pageSize;
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = MenuPatch.menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.09f, 0.8f, 0.13f);
            gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.45f);
            gameObject.AddComponent<BtnCollider>().relatedText = "panic";
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 25, 106, 1));
            Text text = new GameObject
            {
                transform =
        {
            parent = MenuPatch.canvasObj.transform
        }
            }.AddComponent<Text>();
            text.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
            text.fontStyle = FontStyle.BoldAndItalic;
            text.text = "Panic";
            text.color = Color.white;
            text.fontSize = 1;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.2f, 0.03f);
            component.localPosition = new Vector3(0.064f, 0f, 0.2f - num / 2.55f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        private static void NOSNITCHUTILLA()
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("mods", null);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable, null, null);
        }


        public static void activatelucy()
		{
			GameObject.Find("Halloween Ghost").SetActive(true);
		}
		
		public static void gotocaves()
		{
            GameObject.Find("Level/cave/CaveBlockerMeshBaker").SetActive(false);  //CaveBlockerMeshBaker-mesh-mesh
        }

        public static void Disconnect()
        {
            float num = 0.05f;
            int num2 = (MenuPatch.buttons.Length + MenuPatch.pageSize + 1) / MenuPatch.pageSize;
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = MenuPatch.menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.09f, 0.8f, 0.13f);
            gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.65f);
            gameObject.AddComponent<BtnCollider>().relatedText = "disconnect";
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 25, 106, 1));
            Text text = new GameObject
            {
                transform =
        {
            parent = MenuPatch.canvasObj.transform
        }
            }.AddComponent<Text>();
            text.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
            text.fontStyle = FontStyle.BoldAndItalic;
            text.text = "Disconnect";
            text.color = Color.white;
            text.fontSize = 1;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.2f, 0.03f);
            component.localPosition = new Vector3(0.064f, 0f, 0.25f - num / 2.55f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        // Token: 0x06000022 RID: 34 RVA: 0x00005690 File Offset: 0x00003890
        private static void AddPageButtons()
        {
            int num = (MenuPatch.buttons.Length + MenuPatch.pageSize - 1) / MenuPatch.pageSize;
            int num2 = MenuPatch.pageNumber + 1;
            int num3 = MenuPatch.pageNumber - 1;
            bool flag = num2 > num - 1;
            bool flag2 = flag;
            if (flag2)
            {
            }
            bool flag3 = num3 < 0;
            bool flag4 = flag3;
            if (flag4)
            {
                num3 = num - 1;
            }
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = MenuPatch.menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.03f, 0.2f, 0.5f);
            gameObject.transform.localPosition = new Vector3(0.6f, 0.65f, 0f);
            gameObject.AddComponent<BtnCollider>().relatedText = "PreviousPage";
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 25, 106, 1));
            Text text = new GameObject
            {
                transform =
        {
            parent = MenuPatch.canvasObj.transform
        }
            }.AddComponent<Text>();
            text.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
            text.fontStyle = FontStyle.BoldAndItalic;
            text.text = "<";
            text.color = Color.white;
            text.fontSize = 2;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.2f, 0.03f);
            component.localPosition = new Vector3(0.064f, 0.2f, 0f);
            component.localScale = new Vector3(1f, 1f, 1f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
            gameObject2.GetComponent<BoxCollider>().isTrigger = true;
            gameObject2.transform.parent = MenuPatch.menu.transform;
            gameObject2.transform.rotation = Quaternion.identity;
            gameObject2.transform.localScale = new Vector3(0.03f, 0.2f, 0.5f);
            gameObject2.transform.localPosition = new Vector3(0.6f, -0.65f, 0f);
            gameObject2.AddComponent<BtnCollider>().relatedText = "NextPage";
            gameObject2.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 25, 106, 1));
            Text text2 = new GameObject
            {
                transform =
        {
            parent = MenuPatch.canvasObj.transform
        }
            }.AddComponent<Text>();
            text2.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
            text2.fontStyle = FontStyle.BoldAndItalic;
            text2.text = ">";
            text2.color = Color.white;
            text2.fontSize = 2;
            text2.alignment = TextAnchor.MiddleCenter;
            text2.resizeTextForBestFit = true;
            text2.resizeTextMinSize = 0;
            RectTransform component2 = text2.GetComponent<RectTransform>();
            component2.localPosition = Vector3.zero;
            component2.localScale = new Vector3(1f, 1f, 1f);
            component2.sizeDelta = new Vector2(0.2f, 0.03f);
            component2.localPosition = new Vector3(0.064f, -0.2f, 0f);
            component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }


        // Token: 0x06000023 RID: 35 RVA: 0x00005A64 File Offset: 0x00003C64
        public static void Toggle(string relatedText)
        {
            int num = (MenuPatch.buttons.Length + MenuPatch.pageSize - 1) / MenuPatch.pageSize;
            bool flag = relatedText == "NextPage";
            if (flag)
            {
                bool flag2 = MenuPatch.pageNumber < num - 1;
                if (flag2)
                {
                    MenuPatch.pageNumber++;
                }
                else
                {
                    MenuPatch.pageNumber = 0;
                }
                UnityEngine.Object.Destroy(MenuPatch.menu);
                MenuPatch.menu = null;
                MenuPatch.Draw();
            }
            else
            {
                bool flag3 = relatedText == "panic";
                if (flag3)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        
                    }
                }
                bool flag4 = relatedText == "PreviousPage";
                if (flag4)
                {
                    bool flag5 = MenuPatch.pageNumber > 0;
                    if (flag5)
                    {
                        MenuPatch.pageNumber--;
                    }
                    else
                    {
                        MenuPatch.pageNumber = num - 1;
                    }
                    UnityEngine.Object.Destroy(MenuPatch.menu);
                    MenuPatch.menu = null;
                    MenuPatch.Draw();
                }
                bool flag8 = relatedText == "disconnect";
                if (flag8)
				{
					PhotonNetwork.Disconnect();
				}
                else
                {
                    int num2 = -1;
                    for (int i = 0; i < MenuPatch.buttons.Length; i++)
                    {
                        bool flag6 = relatedText == MenuPatch.buttons[i];
                        if (flag6)
                        {
                            num2 = i;
                            break;
                        }
                    }
                    bool flag7 = MenuPatch.buttonsActive[num2] != null;
                    if (flag7)
                    {
                        MenuPatch.buttonsActive[num2] = !MenuPatch.buttonsActive[num2];
                        UnityEngine.Object.Destroy(MenuPatch.menu);
                        MenuPatch.menu = null;
                        MenuPatch.Draw();
                    }
                }
            }
        }


        // Token: 0x06000024 RID: 36 RVA: 0x00005BF8 File Offset: 0x00003DF8
        private void Init()
		{
			this.MainCamera = GameObject.Find("Main Camera");
			this.HUDObj = new GameObject();
			this.HUDObj2 = new GameObject();
			this.HUDObj2.name = "NOTIFICATIONLIB_HUD_OBJ";
			this.HUDObj.name = "NOTIFICATIONLIB_HUD_OBJ";
			this.HUDObj.AddComponent<Canvas>();
			this.HUDObj.AddComponent<CanvasScaler>();
			this.HUDObj.AddComponent<GraphicRaycaster>();
			this.HUDObj.GetComponent<Canvas>().enabled = true;
			this.HUDObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
			this.HUDObj.GetComponent<Canvas>().worldCamera = this.MainCamera.GetComponent<Camera>();
			this.HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
			this.HUDObj.GetComponent<RectTransform>().position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z);
			this.HUDObj2.transform.position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z - 4.6f);
			this.HUDObj.transform.parent = this.HUDObj2.transform;
			this.HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 1.6f);
			Vector3 eulerAngles = this.HUDObj.GetComponent<RectTransform>().rotation.eulerAngles;
			eulerAngles.y = -270f;
			this.HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
			this.HUDObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(eulerAngles);
			this.Testtext = new GameObject
			{
				transform =
				{
					parent = this.HUDObj.transform
				}
			}.AddComponent<Text>();
			this.Testtext.text = "";
			this.Testtext.fontSize = 10;
			this.Testtext.font = GameObject.Find("COC Text").GetComponent<Text>().font;
			this.Testtext.rectTransform.sizeDelta = new Vector2(260f, 70f);
			this.Testtext.alignment = TextAnchor.LowerLeft;
			this.Testtext.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
			this.Testtext.rectTransform.localPosition = new Vector3(-1.5f, -0.9f, -0.6f);
			this.Testtext.material = this.AlertText;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00005F14 File Offset: 0x00004114
		private void FixedUpdate()
		{
			bool flag = !this.HasInit && GameObject.Find("Main Camera") != null;
			if (flag)
			{
				this.Init();
				this.HasInit = true;
			}
			this.HUDObj2.transform.position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z);
			this.HUDObj2.transform.rotation = this.MainCamera.transform.rotation;
			bool flag2 = this.Testtext.text != "";
			if (flag2)
			{
				this.NotificationDecayTimeCounter++;
				bool flag3 = this.NotificationDecayTimeCounter > this.NotificationDecayTime;
				if (flag3)
				{
					this.Notifilines = null;
					this.newtext = "";
					this.NotificationDecayTimeCounter = 0;
					this.Notifilines = this.Testtext.text.Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray<string>();
					foreach (string text in this.Notifilines)
					{
						bool flag4 = text != "";
						if (flag4)
						{
							this.newtext = this.newtext + text + "\n";
						}
					}
					this.Testtext.text = this.newtext;
				}
			}
			else
			{
				this.NotificationDecayTimeCounter = 0;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000060BC File Offset: 0x000042BC

		// Token: 0x0400000C RID: 12
		public static bool ResetSpeed = false;

		// Token: 0x0400000D RID: 13
		private static string[] buttons = new string[]
		{
			"Disconnect",
			"Join Random",
			"Exit",
        };

        // Token: 0x0400000E RID: 14
        private static bool[] buttonsActive = new bool[buttons.Length];
		

		// Token: 0x0400000F RID: 15
		private static bool gripDown;

		// Token: 0x04000010 RID: 16
		private static bool primaryRightDown;

		// Token: 0x04000011 RID: 17
		private static GameObject menu = null;

		// Token: 0x04000012 RID: 18
		private static GameObject canvasObj = null;

		// Token: 0x04000013 RID: 19
		private static GameObject reference = null;

		// Token: 0x04000014 RID: 20
		public static int framePressCooldown = 0;

		// Token: 0x04000015 RID: 21
		private static GameObject pointer = null;

		// Token: 0x04000016 RID: 22
		private static bool gravityToggled = false;

		// Token: 0x04000017 RID: 23
		private static bool flying = false;

		// Token: 0x04000018 RID: 24
		private static int btnCooldown = 0;

		// Token: 0x04000019 RID: 25
		private static int soundCooldown = 0;

		// Token: 0x0400001A RID: 26
		private static float? maxJumpSpeed = null;

		// Token: 0x0400001B RID: 27
		private static float? jumpMultiplier = null;

		// Token: 0x0400001C RID: 28
		private static object index;

		// Token: 0x0400001D RID: 29
		public static int BlueMaterial = 5;

		// Token: 0x0400001E RID: 30
		private GameObject HUDObj;

		// Token: 0x0400001F RID: 31
		private GameObject HUDObj2;

		// Token: 0x04000020 RID: 32
		private GameObject MainCamera;

		// Token: 0x04000021 RID: 33
		private Text Testtext;

		// Token: 0x04000022 RID: 34
		private Material AlertText = new Material(Shader.Find("GUI/Text Shader"));

		// Token: 0x04000023 RID: 35
		private int NotificationDecayTime = 150;

		// Token: 0x04000024 RID: 36
		private int NotificationDecayTimeCounter;

		// Token: 0x04000025 RID: 37
		private string[] Notifilines;

		// Token: 0x04000026 RID: 38
		private string newtext;

		// Token: 0x04000027 RID: 39
		public static string PreviousNotifi;

		// Token: 0x04000028 RID: 40
		private bool HasInit;

		// Token: 0x04000029 RID: 41
		public static int TransparentMaterial = 6;

		// Token: 0x0400002A RID: 42
		public static int LavaMaterial = 2;

		// Token: 0x0400002B RID: 43
		public static int RockMaterial = 1;

		// Token: 0x0400002C RID: 44
		public static int DefaultMaterial = 5;

		// Token: 0x0400002D RID: 45
		public static int NeonRed = 3;

		// Token: 0x0400002E RID: 46
		public static int RedTransparent = 4;

		// Token: 0x0400002F RID: 47
		public static int self = 0;

		// Token: 0x04000030 RID: 48
		private static Vector3? leftHandOffsetInitial = null;

		// Token: 0x04000031 RID: 49
		private static Vector3? rightHandOffsetInitial = null;

		// Token: 0x04000032 RID: 50
		private static float? maxArmLengthInitial = null;

		// Token: 0x04000033 RID: 51
		private static bool noClipDisabledOneshot = false;

		// Token: 0x04000034 RID: 52
		private static bool noClipEnabledAtLeastOnce = false;

		// Token: 0x04000035 RID: 53
		private static Vector3 head_direction;

		// Token: 0x04000036 RID: 54
		private static Vector3 roll_direction;

		// Token: 0x04000037 RID: 55
		private static Vector2 left_joystick;

		// Token: 0x04000038 RID: 56
		private static float acceleration;

		// Token: 0x04000039 RID: 57
		private static float maxs;

		// Token: 0x0400003A RID: 58
		private static float distance;

		// Token: 0x0400003B RID: 59
		private static float multiplier;

		// Token: 0x0400003C RID: 60
		private static float speed;

		// Token: 0x0400003D RID: 61
		private static bool Start;

		// Token: 0x0400003E RID: 62
		private static bool ghostToggle = false;

		// Token: 0x0400003F RID: 63
		private static bool bigMonkeyEnabled = false;

		// Token: 0x04000040 RID: 64
		private static bool bigMonkeAntiRepeat = false;

		// Token: 0x04000041 RID: 65
		private static int bigMonkeCooldown = 0;

		// Token: 0x04000042 RID: 66
		private static bool ghostMonkeEnabled = false;

		// Token: 0x04000043 RID: 67
		private static bool ghostMonkeAntiRepeat = false;

		// Token: 0x04000044 RID: 68
		private static int ghostMonkeCooldown = 0;

		// Token: 0x04000045 RID: 69
		private static bool checkedProps = false;

		// Token: 0x04000046 RID: 70
		private static bool teleportGunAntiRepeat = false;

        private static bool createdroom;

        public static string roomCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYX123456789";

        // Token: 0x04000047 RID: 71
        private static Color colorRgbMonke = new Color(0f, 0f, 0f);

		// Token: 0x04000048 RID: 72
		private static float hueRgbMonke = 0f;

		// Token: 0x04000049 RID: 73
		private static float timerRgbMonke = 0f;

		// Token: 0x0400004A RID: 74
		private static float updateRateRgbMonke = 0f;

		// Token: 0x0400004B RID: 75
		private static float updateTimerRgbMonke = 0f;

		// Token: 0x0400004C RID: 76
		private static bool flag2 = false;

		// Token: 0x0400004D RID: 77
		private static bool flag1 = true;

		// Token: 0x0400004E RID: 78
		private static Vector3 scale = new Vector3(0.0125f, 0.28f, 0.3825f);

		// Token: 0x0400004F RID: 79
		private static bool gripDown_left;

		// Token: 0x04000050 RID: 80
		private static bool gripDown_right;

		// Token: 0x04000051 RID: 81
		private static bool once_left;

		// Token: 0x04000052 RID: 82
		private static bool once_right;

		// Token: 0x04000053 RID: 83
		private static bool once_left_false;

		// Token: 0x04000054 RID: 84
		private static bool once_right_false;

		// Token: 0x04000055 RID: 85
		private static bool once_networking;


		// Token: 0x0400005B RID: 91
		private static GameObject[] jump_left_network = new GameObject[9999];

		// Token: 0x0400005C RID: 92
		private static GameObject[] jump_right_network = new GameObject[9999];

		// Token: 0x0400005D RID: 93
		private static GameObject jump_left_local = null;

		// Token: 0x0400005E RID: 94
		private static GameObject jump_right_local = null;

		// Token: 0x0400005F RID: 95
		private static GradientColorKey[] colorKeysPlatformMonke = new GradientColorKey[4];

		// Token: 0x04000060 RID: 96
		private static Vector3? checkpointPos;

		// Token: 0x04000061 RID: 97
		private static bool checkpointTeleportAntiRepeat = false;

		// Token: 0x04000062 RID: 98
		private static bool foundPlayer = false;

		// Token: 0x04000063 RID: 99
		private static int btnTagSoundCooldown = 0;

		// Token: 0x04000064 RID: 100
		private static float timeSinceLastChange = 0f;

		// Token: 0x04000065 RID: 101
		private static float myVarY1 = 0f;

		// Token: 0x04000066 RID: 102
		private static float myVarY2 = 0f;

		// Token: 0x04000067 RID: 103
		private static bool gain = false;

		// Token: 0x04000068 RID: 104
		private static bool less = false;

		// Token: 0x04000069 RID: 105
		private static GameObject C4;

		// Token: 0x0400006A RID: 106
		private static bool spawned;

		// Token: 0x0400006B RID: 107
		private static float SpawnGrip;

		// Token: 0x0400006C RID: 108
		private static float BoomGrip;

		// Token: 0x0400006D RID: 109
		private static float rSpawnGrip;

		// Token: 0x0400006E RID: 110
		private static float rBoomGrip;

		// Token: 0x0400006F RID: 111
		private static bool reset = false;

		// Token: 0x04000070 RID: 112
		private static bool fastr = false;

		// Token: 0x04000071 RID: 113
		private static Color color;

		// Token: 0x04000072 RID: 114
		private static bool speed1 = true;

		// Token: 0x04000073 RID: 115
		private static float gainSpeed = 1f;

		// Token: 0x04000074 RID: 116
		private static float bigScale = 2f;

		// Token: 0x04000075 RID: 117
		private static int pageSize = 4;

		// Token: 0x04000076 RID: 118
		private static int pageNumber = 0;

		// Token: 0x04000077 RID: 119
		private static float updateRate;

		// Token: 0x04000078 RID: 120
		private static float updateTimer;

		// Token: 0x04000079 RID: 121
		private static float timer;

		// Token: 0x0400007A RID: 122
		private static float hue;

		// Token: 0x0400007B RID: 123
		private static int layers;

		// Token: 0x0400007C RID: 124
		private static bool up;

		// Token: 0x0400007D RID: 125
		private static bool down;

		// Token: 0x0200000A RID: 10
		
		public enum PhotonEventCodes
		{
			// Token: 0x04000082 RID: 130
			left_jump_photoncode = 69,
			// Token: 0x04000083 RID: 131
			right_jump_photoncode,
			// Token: 0x04000084 RID: 132
			left_jump_deletion,
			// Token: 0x04000085 RID: 133
			right_jump_deletion
		}

		// Token: 0x0200000B RID: 11
		
		public class TimedBehaviour : MonoBehaviour
		{
			// Token: 0x0600002E RID: 46 RVA: 0x000066F4 File Offset: 0x000048F4
			public virtual void Start()
			{
				this.startTime = Time.time;
			}

			// Token: 0x0600002F RID: 47 RVA: 0x00006704 File Offset: 0x00004904
			public virtual void Update()
			{
				bool flag = !this.complete;
				bool flag2 = flag;
				if (flag2)
				{
					this.progress = Mathf.Clamp((Time.time - this.startTime) / this.duration, 0f, 1f);
					bool flag3 = Time.time - this.startTime > this.duration;
					bool flag4 = flag3;
					if (flag4)
					{
						bool flag5 = this.loop;
						bool flag6 = flag5;
						if (flag6)
						{
							this.OnLoop();
						}
						else
						{
							this.complete = true;
						}
					}
				}
			}

			// Token: 0x06000030 RID: 48 RVA: 0x0000678B File Offset: 0x0000498B
			public virtual void OnLoop()
			{
				this.startTime = Time.time;
			}

			// Token: 0x04000086 RID: 134
			public bool complete = false;

			// Token: 0x04000087 RID: 135
			public bool loop = true;

			// Token: 0x04000088 RID: 136
			public float progress = 0f;

			// Token: 0x04000089 RID: 137
			protected bool paused = false;

			// Token: 0x0400008A RID: 138
			protected float startTime;

			// Token: 0x0400008B RID: 139
			protected float duration = 2f;
		}

		// Token: 0x0200000C RID: 12
		public class ColorChanger : MenuPatch.TimedBehaviour
		{
			// Token: 0x06000032 RID: 50 RVA: 0x000067CD File Offset: 0x000049CD
			public override void Start()
			{
				base.Start();
				this.gameObjectRenderer = base.GetComponent<Renderer>();
			}

			// Token: 0x06000033 RID: 51 RVA: 0x000067E4 File Offset: 0x000049E4
			public override void Update()
			{
				base.Update();
				bool flag = this.colors != null;
				bool flag2 = flag;
				if (flag2)
				{
					bool flag3 = this.timeBased;
					bool flag4 = flag3;
					if (flag4)
					{
						this.color = this.colors.Evaluate(this.progress);
					}
					this.gameObjectRenderer.material.SetColor("_Color", this.color);
					this.gameObjectRenderer.material.SetColor("_EmissionColor", this.color);
				}
			}

			// Token: 0x0400008C RID: 140
			public Renderer gameObjectRenderer;

			// Token: 0x0400008D RID: 141
			public Gradient colors = null;

			// Token: 0x0400008E RID: 142
			public Color color;

			// Token: 0x0400008F RID: 143
			public bool timeBased = true;
		}
	}
}
