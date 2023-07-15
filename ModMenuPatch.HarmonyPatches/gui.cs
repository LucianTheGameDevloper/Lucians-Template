using System;
using BepInEx;
using Photon.Pun;
using UnityEngine;

namespace MSMSTEMP.ModMenuPatch.HarmonyPatches
{
    // Token: 0x02000002 RID: 2
    [BepInPlugin("kyslemming", "kyslemming", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public void OnGUI()
        {
            GUI.backgroundColor = Color.blue;
            GUI.contentColor = Color.blue;
            GUI.color = Color.blue;
            GUI.color = Color.blue;
            this.windowRect = GUI.Window(9999, this.windowRect, new GUI.WindowFunction(this.MainWindowFunction), "<color=red>KYS LEMMING V1</color>");
        }

        public static void keymovement()
        {
            bool key = UnityInput.Current.GetKey(KeyCode.W);
            if (key)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 5f;
            }
            bool key2 = UnityInput.Current.GetKey(KeyCode.S);
            if (key2)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * -5f;
            }
            bool key3 = UnityInput.Current.GetKey(KeyCode.D);
            if (key3)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.right * Time.deltaTime * 5f;
            }
            bool key4 = UnityInput.Current.GetKey(KeyCode.A);
            if (key4)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.right * Time.deltaTime * -5f;
            }
            bool key5 = UnityInput.Current.GetKey(KeyCode.LeftArrow);
            if (key5)
            {
                GorillaLocomotion.Player.Instance.transform.Rotate(0f, -1f, 0f);
            }
            bool key6 = UnityInput.Current.GetKey(KeyCode.RightArrow);
            if (key6)
            {
                GorillaLocomotion.Player.Instance.transform.Rotate(0f, 1f, 0f);
            }
            bool key7 = UnityInput.Current.GetKey(KeyCode.LeftControl);
            if (key7)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.up * Time.deltaTime * -5f;
            }
            bool key8 = UnityInput.Current.GetKey(KeyCode.Space);
            if (key8)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.up * Time.deltaTime * 5f;
            }
            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }


        // Token: 0x06000003 RID: 3 RVA: 0x00002144 File Offset: 0x00000344
        public void MainWindowFunction(int WindowId)
        {
            GUI.color = Color.blue;
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
            bool flag = GUI.Button(new Rect(8f, 20f, 100f, 30f), "<color=white>Keyboard Movement</color>");
            bool flag2 = flag;
            if (flag2)
            {
                this.Player1 = !this.Player1;
            }
            bool player = this.Player1;
            bool flag3 = player;
            if (flag3)
            {
                Main.keymovement();
            }
        }

        // Token: 0x04000001 RID: 1
        private Rect windowRect = new Rect(60f, 20f, 400f, 370f);

        // Token: 0x04000002 RID: 2
        private bool Player1;
    }
}
