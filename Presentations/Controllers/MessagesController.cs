﻿using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Messages;
using DataAccesses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<MessagesController>
        [HttpGet("GetMessages")]
        public async Task<IActionResult> Get(int custom, int? page, int items)
        {
            var result = await _unitOfWork.Messages.GetMessagesPaginationByChannelIdAsync(custom, page, items);
            return Ok(result);
        }

        // GET api/<MessagesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MessagesController>
        [HttpPost("CreateMessage")]
        public async Task<IActionResult> Post(CreateMessageDTO model)
        {
            _unitOfWork.BeginTransaction();
            var message = _mapper.Map<Message>(model);
            var insertProcess = _unitOfWork.Messages.InsertAsync(message);
            await insertProcess;
            _unitOfWork.Commit();
            var user = await _unitOfWork.Users.GetByIdAsync(message.UserCreated);
            return Created("CreateMessage", new GetMessageDTO
            {
                MessageId = message.MessageId,
                Username = user.UserName,
                Avatar = user.Avatar,
                ReplyTo = message.ReplyTo,
                Content = message.Content,
                DateCreated = message.DateCreated,
                DateModified = message.DateModified,
                ChannelId = message.ChannelId,
            });
        }

        // PUT api/<MessagesController>/5
        [HttpPut("UpdateMessage")]
        public async Task<IActionResult> Put(UpdateMessageDTO model)
        {
            _unitOfWork.BeginTransaction();
            var message = await _unitOfWork.Messages.GetByIdAsync(model.MessageId);
            _mapper.Map(model, message);
            _unitOfWork.Messages.Update(message);
            _unitOfWork.Commit();
            var user = await _unitOfWork.Users.GetByIdAsync(message.UserCreated);
            return Ok(new GetMessageDTO
            {
                MessageId = message.MessageId,
                Username = user.UserName,
                Avatar = user.Avatar,
                ReplyTo = message.ReplyTo,
                Content = message.Content,
                DateCreated = message.DateCreated,
                DateModified = message.DateModified,
                ChannelId = message.ChannelId,
            });
        }

        // DELETE api/<MessagesController>/5
        [HttpDelete("DeleteMessage/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _unitOfWork.BeginTransaction();
            var message = _unitOfWork.Messages.GetById(id);
            if (message == null)
            {
                return NotFound();
            }
            _unitOfWork.Messages.Delete(id);
            _unitOfWork.Commit();
            var user = await _unitOfWork.Users.GetByIdAsync(message.UserCreated);
            return Ok(new GetMessageDTO
            {
                MessageId = message.MessageId,
                Username = user.UserName,
                Avatar = user.Avatar,
                ReplyTo = message.ReplyTo,
                Content = message.Content,
                DateCreated = message.DateCreated,
                DateModified = message.DateModified,
                ChannelId = message.ChannelId,
            });
        }
    }
}
