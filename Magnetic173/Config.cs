using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace Magnetic173
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug Mode")]
        public bool Debug { get; set; } = false;

        [Description("Roles that are allowed to use the .cage173 command")]
        public List<RoleTypeId> AllowedRoles { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.NtfCaptain,
            RoleTypeId.NtfSpecialist,
            RoleTypeId.NtfSergeant
        };

        [Description("Name of the command to activate the cage.")]
        public string CageCommand { get; set; } = "cage173";

        [Description("Countdown time (in seconds) before the frame is activated.")]
        public float CountdownDuration { get; set; } = 5f;

        [Description("Name of the cage schema from Map Editor Reborn (MER). Must be exact!")]
        public string SchematicName { get; set; } = "173Cage";
        
    }
}