using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using PlayerRoles;

namespace MagneticCage173
{
    public class Config : IConfig
    {
        [Description("Czy plugin jest włączony?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Czy włączyć logowanie debugowe?")]
        public bool Debug { get; set; } = false;

        [Description("Role, które mogą używać komendy .klatka173")]
        public List<RoleTypeId> AllowedRoles { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.NtfCaptain,
            RoleTypeId.NtfSpecialist,
            RoleTypeId.NtfSergeant
        };

        [Description("Nazwa komendy do aktywacji klatki.")]
        public string CageCommand { get; set; } = "klatka173";

        [Description("Czas odliczania (w sekundach) przed aktywacją klatki.")]
        public float CountdownDuration { get; set; } = 5f;

        [Description("Nazwa schematu klatki z Map Editor Reborn (MER). Musi być dokładna!")]
        public string SchematicName { get; set; } = "173Cage";
        
    }
}