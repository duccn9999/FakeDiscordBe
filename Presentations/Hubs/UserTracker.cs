using BusinessLogics.Repositories;
using DataAccesses.DTOs.LastSeenMessages;
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
        public async Task<GetLastSeenMessageDTO> TrackLastMessage(string username, IUnitOfWork unitOfWork)
        {
            var lastChannel = usersByChannel
                .Where(x => x.Value.Contains(username))
                .Select(x => x.Key)
                .FirstOrDefault();
            var currentUserId = unitOfWork.Users.GetAll()
            .Where(x => x.UserName == username)
            .Select(x => x.UserId)
            .FirstOrDefault();
            // get the newest message
            var newestMessageId = unitOfWork.Messages.GetAll()
            .Where(x => x.ChannelId == lastChannel)
            .OrderByDescending(x => x.DateCreated)
            .Select(x => x.MessageId)
            .FirstOrDefault();

            var lastSeenMessage = unitOfWork.LastSeenMessages.GetAll()
            .Where(m => m.UserId == currentUserId && m.ChannelId == lastChannel)
            .Select(m => new GetLastSeenMessageDTO
            {
                UserId = m.UserId,
                ChannelId = m.ChannelId,
                MessageId = m.MessageId,
                DateSeen = m.DateSeen,
            })
            .FirstOrDefault() ?? new GetLastSeenMessageDTO
            {
                UserId = currentUserId,
                ChannelId = lastChannel,
                MessageId = newestMessageId,
                DateSeen = DateTime.Now
            };
            if (lastSeenMessage.ChannelId == 0 || lastSeenMessage.MessageId == 0 || lastSeenMessage.UserId == 0)
            {
                return null;
            }
            return lastSeenMessage;
        }
        public async Task<GetLastSeenMessageDTO> TrackLastMessage(string username, IUnitOfWork unitOfWork, int channelId)
        {
            var currentUserId = unitOfWork.Users.GetAll()
            .Where(x => x.UserName == username)
            .Select(x => x.UserId)
            .FirstOrDefault();
            // get the newest message
            var newestMessageId = unitOfWork.Messages.GetAll()
            .Where(x => x.ChannelId == channelId)
            .OrderByDescending(x => x.DateCreated)
            .Select(x => x.MessageId)
            .FirstOrDefault();

            var lastSeenMessage = unitOfWork.LastSeenMessages.GetAll()
            .Where(m => m.UserId == currentUserId && m.ChannelId == channelId)
            .Select(m => new GetLastSeenMessageDTO
            {
                UserId = m.UserId,
                ChannelId = m.ChannelId,
                MessageId = m.MessageId,
                DateSeen = m.DateSeen,
            })
            .FirstOrDefault() ?? new GetLastSeenMessageDTO
            {
                UserId = currentUserId,
                ChannelId = channelId,
                MessageId = newestMessageId,
                DateSeen = DateTime.Now
            };
            if (lastSeenMessage.ChannelId == 0 || lastSeenMessage.MessageId == 0 || lastSeenMessage.UserId == 0)
            {
                return null;
            }
            return lastSeenMessage;
        }
    }
}
