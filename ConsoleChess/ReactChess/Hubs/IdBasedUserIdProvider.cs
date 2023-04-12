﻿using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ReactChess.Hubs
{
    public class IdBasedUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {

            return connection.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
