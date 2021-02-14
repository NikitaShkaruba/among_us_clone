using System;
using System.Collections;
using System.Collections.Generic;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Game.Interactions;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Maps.Surveillance
{
    [RequireComponent(typeof(PlayersLockable))]
    public class SecurityPanel : Interactable
    {
        [Header("Scriptable objects")]
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private PacketsSender packetsSender;

        [Header("Self fields")]
        [SerializeField] private PlayersLockable playersLockable;
        [SerializeField] private GameObject securityPanelUI;
        [SerializeField] private Image blackScreenImage;

        [Range(0.0f, 0.1f)]
        [SerializeField] private float fadeSpeed;
        private bool isTransitionGoing;
        private bool isFirstTransition;

        [SerializeField] private List<SecurityCamera> cameras;

        public bool isControlledPlayerViewing;

        public Action onInterfaceToggle;

        public override void Interact()
        {
            if (playGamePhase.clientSkeld.playGamePhaseUserInterface.minimapButton.IsMinimapShown)
            {
                return;
            }

            if (isTransitionGoing)
            {
                Logger.LogNotice(SharedLoggerSection.Interactions, "Unable to interact with security panel, because fade animation is still going");
                return;
            }

            isControlledPlayerViewing = !isControlledPlayerViewing;
            packetsSender.SendSecurityPanelInteractionPacket();

            if (isControlledPlayerViewing)
            {
                playersLockable.Add(playersManager.controlledClientPlayer.basePlayer.information.id);
                Logger.LogEvent(SharedLoggerSection.Interactions, "Has started viewing security panel");
            }
            else
            {
                playersLockable.Remove(playersManager.controlledClientPlayer.basePlayer.information.id);
                Logger.LogEvent(SharedLoggerSection.Interactions, "Has stopped viewing security panel");
            }

            isTransitionGoing = true;
            isFirstTransition = true;
            StartCoroutine(BlinkFade());
        }

        public void EnableSecurityCameras()
        {
            foreach (SecurityCamera camera in cameras)
            {
                camera.Enable();
            }

            Logger.LogEvent(SharedLoggerSection.Interactions, "Enabled security cameras");
        }

        public void DisableSecurityCameras()
        {
            foreach (SecurityCamera camera in cameras)
            {
                camera.Disable();
            }

            Logger.LogEvent(SharedLoggerSection.Interactions, "Enabled security cameras");
        }

        private IEnumerator BlinkFade()
        {
            yield return new WaitForSeconds(0.01f);

            float opacityDifference = isFirstTransition ? fadeSpeed : -fadeSpeed;
            float newOpacity = Mathf.Clamp(blackScreenImage.color.a + opacityDifference, 0f, 1f);
            blackScreenImage.color = new Color(0f, 0f, 0f, newOpacity);

            // If Transition is finished
            if (blackScreenImage.color.a == 0f)
            {
                isTransitionGoing = false;
                yield break;
            }

            // If we did the half
            if (Mathf.Approximately(blackScreenImage.color.a, 1f))
            {
                isFirstTransition = false;
                securityPanelUI.SetActive(!securityPanelUI.activeSelf);
                onInterfaceToggle?.Invoke();
            }

            StartCoroutine(BlinkFade());
        }
    }
}
