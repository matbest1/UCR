using System.Collections.Generic;

namespace UCP
{
    internal class PacketInfos
    {
        private static readonly Dictionary<int, string> knownPackets = new Dictionary<int, string>
        {
            {10101, "Login"},
            {10108, "KeepAlive"},
            {14715, "SendGlobalChatLine"},
            {24715, "GlobalChatLine"},
            {24101, "OwnHomeData"},
            {14102, "EndClientTurn"},
            {20104, "LoginOK"},
            {20103, "LoginFailed"},
            {20100, "AntiModding"},
            {10100, "FirstAuthentication"},
            {10103, "CreateAccount"},
            {10116, "ResetAccount"},
            {10118, "AccountSwitched"},
            {10117, "ReportUser"},
            {10200, "CreateAvatar"},
            {10201, "SelectAvatar"},
            {10502, "AddFriend"},
            {14101, "AttackResult"},
            {14113, "VisitHome"},
            {14114, "HomeBattleReplay"},
            {14134, "AttackNpc"},
            {14201, "BindFacebookAccount"},
            {14211, "UnbindFacebookAccount"},
            {14123, "AttackMatchedHome"},
            {24103, "AllianceData"},
            {24133, "NpcData"},
            {24115, "ServerError"},
            {24113, "VisitedHomeData"},
            {24104, "OutOfSync"},
            {24310, "AllianceList"},
            {24311, "AllianceStream"},
            {20108, "ServerKeepAlive"},
            {24411, "AvatarStream"},
            {24340, "BookmarksList"}
        };

        public static string GetPacketName(int packetid)
        {
            var packetname = "";
            if (knownPackets.ContainsKey(packetid))
            {
                packetname = knownPackets[packetid] + "(" + packetid + ")";
            }
            else
            {
                packetname = "Unknown Packet (" + packetid + ")";
            }
            return packetname;
        }
    }
}