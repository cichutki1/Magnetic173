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
        public string Command => Magnetic173.MagneticCage173.Instance.Config.CageCommand;
        public string[] Aliases => null;
        public string Description => "Inicjalizuje zakładanie klatki dla SCP-173.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "Ta komenda może zostać użyta tylko przez gracza.";
                return false;
            }

            Player player = Player.Get(playerSender.ReferenceHub);
            if (player == null) 
            {
                response = "Nie można zidentyfikować gracza.";
                return false;
            }

            if (!Magnetic173.MagneticCage173.Instance.Config.IsEnabled)
            {
                response = "Funkcjonalność klatki magnetycznej jest obecnie wyłączona.";
                return false;
            }

            if (!Magnetic173.MagneticCage173.Instance.Config.AllowedRoles.Contains(player.Role.Type))
            {
                response = "Nie możesz założyć klatki magnetycznej jako obecna klasa!";
                return false;
            }

            if (Magnetic173.MagneticCage173.Instance.ActiveCountdowns.ContainsKey(player) || Magnetic173.MagneticCage173.Instance.IsPlayerCurrentlyCaging(player)) 
            {
                response = "Zakładasz klatkę magnetyczną lub już ją założyłeś.";
                return false;
            }

            if (Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, out RaycastHit hit, 2f))
            {
                Player target = Player.Get(hit.collider.GetComponentInParent<ReferenceHub>()); 

                if (target != null && target.Role.Type == RoleTypeId.Scp173)
                {
                    if (Magnetic173.MagneticCage173.Instance.ActiveCages.ContainsKey(target))
                    {
                        response = "SCP-173 jest już w klatce.";
                        return false;
                    }

                    Magnetic173.MagneticCage173.Instance.InitiateCagingProcess(player, target); 
                    response = "Inicjalizacja procedury klatki magnetycznej...";
                    return true;
                }
                else
                {
                    response = $"Musisz patrzeć się na górną część SCP-173 z bardzo bliskiego dystansu.";
                    return false;
                }
            }
            else 
            {
                response = $"Musisz patrzeć się na górną część SCP-173 z bardzo bliskiego dystansu.";
                return false;
            }
        }
    }
}