using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Notifications;
using DataAccesses.DTOs.UserFriends;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Net.WebSockets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserFriendsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserFriendsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<UserFriendsController>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFriendsByUser(int userId)
        {
            return Ok(_unitOfWork.UserFriends.GetFriendsByUser(userId));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBlockedUser(int userId)
        {
            return Ok(_unitOfWork.UserFriends.GetBlockedUsers(userId));
        }

        // POST api/<UserFriendsController>
        [HttpPost]
        public async Task<IActionResult> SendFriendRequest([FromBody] SendFriendRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return bad request if the model is invalid
            }
            _unitOfWork.BeginTransaction();
            var receiver = _unitOfWork.Users.GetByUsername(model.Receiver);
            var sender = _unitOfWork.Users.GetById(model.UserId1);
            var existUserFriend = _unitOfWork.UserFriends.GetAll().SingleOrDefault(x => x.UserId1 == Math.Min(receiver.UserId, model.UserId1) && x.UserId2 == Math.Max(receiver.UserId, model.UserId1));
            if (existUserFriend != null && existUserFriend.Status == (int)FriendStatus.Accepted)
            {
                return Conflict($"You and {receiver.UserName} has already been friend!");
            }else if (existUserFriend != null && existUserFriend.Status == (int)FriendStatus.Blocked)
            {

            }
                var userFriend = new UserFriend
                {
                    UserId1 = Math.Min(model.UserId1, receiver.UserId),
                    UserId2 = Math.Max(model.UserId1, receiver.UserId),
                    Status = model.Status,
                    RequestDate = DateTime.UtcNow
                };
            _unitOfWork.UserFriends.Insert(userFriend);
            _unitOfWork.Save();
            /* send notification */
            var notificationDto = new CreateNotificationDTO
            {
                UserId1 = model.UserId1,
                UserId2 = receiver.UserId,
                Message = $"{sender.UserName} has sent you a friend request.",
                IsRead = false,
                Type = true,
            };
            var notification = _mapper.Map<Notification>(notificationDto);
            _unitOfWork.Notifications.Insert(notification);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Created("SendFriendRequest", new GetNotificationDTO
            {
                NotificationId = notification.NotificationId,
                UserId1 = notification.UserId1,
                UserId2 = notification.UserId2,
                Message = notification.Message,
                IsRead = notification.IsRead,
                Type = notification.Type,
                DateCreated = notification.DateCreated
            });
        }

        // PUT api/<UserFriendsController>/5
        [HttpPut]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] AcceptUserFriendRequestDTO model)
        {
            var userFriend = _unitOfWork.UserFriends.GetAll().SingleOrDefault(x => x.UserId1 == Math.Min(model.UserId1, model.UserId2) && x.UserId2 == Math.Max(model.UserId1, model.UserId2));
            if (userFriend == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            userFriend.Status = model.Status;
            _unitOfWork.UserFriends.Update(userFriend);
            var sender = _unitOfWork.Users.GetById(model.UserId1);
            var receiver = _unitOfWork.Users.GetById(model.UserId2);
            /* send notification */
            var notificationDto = new CreateNotificationDTO
            {
                UserId1 = model.UserId1,
                UserId2 = model.UserId2,
                Message = $"{sender.UserName} has accepted your friend request.",
                Type = false,
            };
            var notification = _mapper.Map<Notification>(notificationDto);
            _unitOfWork.Notifications.Insert(notification);
            _unitOfWork.Commit();
            return Ok(new GetNotificationDTO
            {
                NotificationId = notification.NotificationId,
                UserId1 = notification.UserId1,
                UserId2 = notification.UserId2,
                Message = notification.Message,
                IsRead = notification.IsRead,
                Type = notification.Type,
                DateCreated = notification.DateCreated
            });
        }

        // DELETE api/<UserFriendsController>/5
        [HttpDelete("{userId1}/{userId2}")]
        public async Task<IActionResult> CancelFriendRequest(int userId1, int userId2)
        {
            var userFriend = _unitOfWork.UserFriends.GetAll().SingleOrDefault(x => x.UserId1 == Math.Min(userId1, userId2) && x.UserId2 == Math.Max(userId1, userId2));
            if (userFriend == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            var sender = _unitOfWork.Users.GetById(userId1);
            _unitOfWork.UserFriends.Delete(userFriend.Id);
            /* send notification */
            var notificationDto = new CreateNotificationDTO
            {
                UserId1 = userId1,
                UserId2 = userId2,
                Message = $"{sender.UserName} has canceled your friend request.",
                Type = false,
            };
            var notification = _mapper.Map<Notification>(notificationDto);
            _unitOfWork.Notifications.Insert(notification);
            _unitOfWork.Commit();
            return Ok(new GetNotificationDTO
            {
                NotificationId = notification.NotificationId,
                UserId1 = notification.UserId1,
                UserId2 = notification.UserId2,
                Message = notification.Message,
                IsRead = notification.IsRead,
                Type = notification.Type,
                DateCreated = notification.DateCreated
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var userFriend = _unitOfWork.UserFriends.GetById(id);
            var user = _unitOfWork.Users.GetById(userFriend.UserId2);
            if (userFriend == null)
            {
                return NotFound();
            }
            var responseModel = new GetUserFriendDTO
            {
                Id = userFriend.Id,
                Avatar = user.Avatar,
                UserName = user.UserName,
                RequestDate = userFriend.RequestDate,
                Status = userFriend.Status,
                UserId1 = userFriend.UserId1,
                UserId2 = userFriend.UserId2,
            };
            _unitOfWork.BeginTransaction();
            userFriend.Status = (int)FriendStatus.Blocked;
            _unitOfWork.UserFriends.Update(userFriend);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(responseModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unfriend(int id)
        {
            var userFriend = _unitOfWork.UserFriends.GetById(id);
            var user = _unitOfWork.Users.GetById(userFriend.UserId2);
            if (userFriend == null)
            {
                return NotFound();
            }
            var responseModel = new GetUserFriendDTO
            {
                Id = userFriend.Id,
                Avatar = user.Avatar,
                UserName = user.UserName,
                RequestDate = userFriend.RequestDate,
                Status = userFriend.Status,
                UserId1 = userFriend.UserId1,
                UserId2 = userFriend.UserId2,
            };
            _unitOfWork.BeginTransaction();
            _unitOfWork.UserFriends.Delete(id);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(responseModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> UnblockUser(int id)
        {
            var userFriend = _unitOfWork.UserFriends.GetById(id);
            var user = _unitOfWork.Users.GetById(userFriend.UserId2);
            if (userFriend == null)
            {
                return NotFound();
            }
            var responseModel = new GetUserFriendDTO
            {
                Id = userFriend.Id,
                Avatar = user.Avatar,
                UserName = user.UserName,
                RequestDate = userFriend.RequestDate,
                Status = userFriend.Status,
                UserId1 = userFriend.UserId1,
                UserId2 = userFriend.UserId2,
            };
            _unitOfWork.BeginTransaction();
            userFriend.Status = (int)FriendStatus.Accepted;
            _unitOfWork.UserFriends.Update(userFriend);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            return Ok(responseModel);
        }
    }
}
