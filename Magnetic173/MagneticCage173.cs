using System;
using Exiled.API.Features;
using Exiled.API.Enums;
using MEC;
using System.Collections.Generic;
using ProjectMER.Features.Objects; 
namespace MagneticCage173
{
    public class MagneticCage173 : Plugin<Config>
    {
        public override string Name => "MagneticCage173";
        public override string Author => "Feniks Studio - Tymek";
        public override Version Version => new Version(2, 2, 1); 
        public override Version RequiredExiledVersion => new Version(9, 6, 0);

        public static MagneticCage173 Instance { get; private set; }

        internal EventHandlers handlers;

        public Dictionary<Player, CageInfo> ActiveCages { get; } = new Dictionary<Player, CageInfo>();
        public Dictionary<Player, CoroutineHandle> ActiveCountdowns { get; } = new Dictionary<Player, CoroutineHandle>();

        public class CageInfo
        {
            public Player CagingPlayer { get; }
            public Player CagedScp173 { get; }
            public SchematicObject CageSchematic { get; set; }
            public CoroutineHandle UpdateCoroutine { get; }
            public float CurrentHealth { get; set; } 

            public CageInfo(Player cagingPlayer, Player cagedScp173, CoroutineHandle updateCoroutine)
            {
                CagingPlayer = cagingPlayer;
                CagedScp173 = cagedScp173;
                CageSchematic = null; 
                UpdateCoroutine = updateCoroutine;
                CurrentHealth = 0; 
            }
        }

        public override void OnEnabled()
        {
            Instance = this;
            handlers = new EventHandlers();

            Exiled.Events.Handlers.Server.RoundStarted += handlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += handlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Verified += handlers.OnPlayerVerified;
            Exiled.Events.Handlers.Player.Destroying += handlers.OnPlayerDestroying;
            Exiled.Events.Handlers.Player.Died += handlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.Hurting += handlers.OnPlayerHurting;
            Exiled.Events.Handlers.Player.ChangingRole += handlers.OnChangingRole;
            Exiled.Events.Handlers.Player.Shooting += handlers.OnPlayerShooting;
            Exiled.Events.Handlers.Map.ExplodingGrenade += handlers.OnExplodingGrenade;

            if (!IsMERLoaded())
            {
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= handlers.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= handlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Verified -= handlers.OnPlayerVerified;
            Exiled.Events.Handlers.Player.Destroying -= handlers.OnPlayerDestroying;
            Exiled.Events.Handlers.Player.Died -= handlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.Hurting -= handlers.OnPlayerHurting;
            Exiled.Events.Handlers.Player.ChangingRole -= handlers.OnChangingRole;
            Exiled.Events.Handlers.Player.Shooting -= handlers.OnPlayerShooting;
            Exiled.Events.Handlers.Map.ExplodingGrenade -= handlers.OnExplodingGrenade;

            handlers.CleanupAllCages();

            handlers = null;
            Instance = null;
            base.OnDisabled();
        }

        public void InitiateCagingProcess(Player cagingPlayer, Player targetScp173)
        {
            handlers?.StartCagingProcess(cagingPlayer, targetScp173);
        }

        public bool IsPlayerCurrentlyCaging(Player player)
        {
            return handlers?.IsAnyPlayerCaging(player) ?? false;
        }

        private bool IsMERLoaded()
        {
            try
            {
                var typeCheck = typeof(ProjectMER.Features.MapUtils);
                return true;
            }
            catch (TypeLoadException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}