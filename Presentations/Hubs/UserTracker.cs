using DataAccesses.Models;

namespace Presentations.Hubs
{
    public class UserTracker
    {
        public Dictionary<string, List<string>> connectedUsers { get; set; } = new();
        public Dictionary<int, List<string>> usersByChannel { get; set; } = new();
        public Dictionary<int, List<int>> onlineFriends { get; set; } = new();
        public async Task GetConnectedUsers(string groupChatId, string username)
        {
            lock (connectedUsers)
            {
                if (!connectedUsers.ContainsKey(groupChatId))
                {
                    connectedUsers[groupChatId] = new List<string>();
                }

                if (!connectedUsers[groupChatId].Contains(username))
                {
                    connectedUsers[groupChatId].Add(username);
                }
            }
            await Task.CompletedTask;
        }

        public async Task GetUsersByChannel(int channelId, string username)
        {
            lock (usersByChannel)
            {
                if (!usersByChannel.ContainsKey(channelId))
                {
                    usersByChannel[channelId] = new List<string>();
                }
                if (!usersByChannel[channelId].Contains(username))
                {
                    usersByChannel[channelId].Add(username);
                }
            }
            await Task.CompletedTask;
        }

        public async Task TrackUsersLeave(int channelId, string username)
        {
            lock (usersByChannel)
            {
                if (!usersByChannel.ContainsKey(channelId))
                {
                    return;
                }
                if (usersByChannel[channelId].Contains(username))
                {
                    usersByChannel[channelId].Remove(username);
                }
            }
            await Task.CompletedTask;
        }

        public async Task TrackOnlineFriend(int userId, int friendId)
        {
            lock (onlineFriends)
            {
                if (!onlineFriends.ContainsKey(userId))
                {
                    onlineFriends[userId] = new List<int>();
                }
                if (!onlineFriends[userId].Contains(friendId))
                {
                    onlineFriends[userId].Add(friendId);
                }
            }
            await Task.CompletedTask;
        }

        public async Task TrackOffLineFriend(int userId, int friendId)
        {
            lock (onlineFriends)
            {
                if (!onlineFriends[userId].Contains(friendId))
                {
                    onlineFriends[userId].Remove(friendId);
                }
            }
            await Task.CompletedTask;
        }
    }
}
