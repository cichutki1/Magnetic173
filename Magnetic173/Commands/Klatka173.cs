using CommandSystem;
using Exiled.API.Features;
using System;
using UnityEngine;
using PlayerRoles;
using RemoteAdmin;

namespace MagneticCage173.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Klatka173 : ICommand
    {
        public string Command => MagneticCage173.Instance.Config.CageCommand;
        public string[] Aliases => null;
        public string Description => "Rozpoczyna procedurę tworzenia klatki magnetycznej na SCP-173.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "Ta komenda może być użyta tylko przez gracza.";
                return false;
            }

            Player player = Player.Get(playerSender.ReferenceHub);
            if (player == null) 
            {
                response = "Nie można zidentyfikować gracza.";
                return false;
            }

            if (!MagneticCage173.Instance.Config.IsEnabled)
            {
                response = "Funkcja klatki magnetycznej jest obecnie wyłączona.";
                return false;
            }

            if (!MagneticCage173.Instance.Config.AllowedRoles.Contains(player.Role.Type))
            {
                response = "Twoja rola nie pozwala na użycie tej komendy.";
                return false;
            }

            if (MagneticCage173.Instance.ActiveCountdowns.ContainsKey(player) || MagneticCage173.Instance.IsPlayerCurrentlyCaging(player)) 
            {
                response = "Już jesteś w trakcie tworzenia klatki lub masz aktywną klatkę.";
                return false;
            }

            if (Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, out RaycastHit hit, 2f))
            {
                Player target = Player.Get(hit.collider.GetComponentInParent<ReferenceHub>()); 

                if (target != null && target.Role.Type == RoleTypeId.Scp173)
                {
                    if (MagneticCage173.Instance.ActiveCages.ContainsKey(target))
                    {
                        response = "Ten SCP-173 jest już w klatce.";
                        return false;
                    }

                    MagneticCage173.Instance.InitiateCagingProcess(player, target); 
                    response = "Rozpoczynanie procedury tworzenia klatki magnetycznej...";
                    return true;
                }
                else
                {
                    response = $"Musisz patrzeć bezpośrednio na SCP-173 z bliskiej odległości.";
                    return false;
                }
            }
            else 
            {
                response = $"Musisz patrzeć bezpośrednio na SCP-173 z bliskiej odległości.";
                return false;
            }
        }
    }
}