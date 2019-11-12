using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordGateway.Discord.Constants
{
    public enum EventNames
    {
        Hello,
        Ready,
        Resumed,
        InvalidSession,
        ChannelCreate,
        ChannelUpdate,
        ChannelDelete,
        ChannelPinsUpdate,
        GuildCreate,
        GuildUpdate,
        GuildDelete,
        GuildBanAdd,
        GuildBanRemove,
        GuildEmojisUpdate,
        GuildIntegrationsUpdate,
        GuildMemberAdd,
        GuildMemberRemove,
        GuildMemberUpdate,
        GuildMembersChunk,
        GuildRoleCreate,
        GuildRoleUpdate,
        GuildRoleDelete,
        MessageCreate,
        MessageUpdate,
        MessageDelete,
        MessageDeleteBulk,
        MessageReactionAdd,
        MessageReactionRemove,
        MessageReactionRemoveAll,
        PresenceUpdate,
        TypingStart,
        UserUpdate,
        VoiceStateUpdate,
        VoiceServerUpdate,
        WebhooksUpdate
    }
}
