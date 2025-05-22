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
        public string Description => "Initiating the magnetic cage creation procedure on SCP-173.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "This command can only be used by a player.";
                return false;
            }

            Player player = Player.Get(playerSender.ReferenceHub);
            if (player == null) 
            {
                response = "Player could not be identified.";
                return false;
            }

            if (!MagneticCage173.Instance.Config.IsEnabled)
            {
                response = "The magnetic cage function is currently disabled.";
                return false;
            }

            if (!MagneticCage173.Instance.Config.AllowedRoles.Contains(player.Role.Type))
            {
                response = "Your role does not permit the use of this command.";
                return false;
            }

            if (MagneticCage173.Instance.ActiveCountdowns.ContainsKey(player) || MagneticCage173.Instance.IsPlayerCurrentlyCaging(player)) 
            {
                response = "You are already in the process of creating a cage or have an active cage.";
                return false;
            }

            if (Physics.Raycast(player.CameraTransform.position, player.CameraTransform.forward, out RaycastHit hit, 2f))
            {
                Player target = Player.Get(hit.collider.GetComponentInParent<ReferenceHub>()); 

                if (target != null && target.Role.Type == RoleTypeId.Scp173)
                {
                    if (MagneticCage173.Instance.ActiveCages.ContainsKey(target))
                    {
                        response = "This SCP-173 is already in a cage.";
                        return false;
                    }

                    MagneticCage173.Instance.InitiateCagingProcess(player, target); 
                    response = "Initiating magnetic cage creation procedure...";
                    return true;
                }
                else
                {
                    response = $"You must look directly at SCP-173 from a close distance.";
                    return false;
                }
            }
            else 
            {
                response = $"You must look directly at SCP-173 from a close distance.";
                return false;
            }
        }
    }
}