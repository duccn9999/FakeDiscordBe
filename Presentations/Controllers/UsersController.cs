using AutoMapper;
using BusinessLogics.Repositories;
using DataAccesses.DTOs.Emails;
using DataAccesses.DTOs.EmailTokens;
using DataAccesses.DTOs.Users;
using DataAccesses.Models;
using DataAccesses.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "CHECK_ACTIVE")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private RandomStringGenerator _randomStringGenerator;
        private readonly ICloudinaryRepository _cloudinaryService;
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, RandomStringGenerator randomStringGenerator, ICloudinaryRepository cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _randomStringGenerator = randomStringGenerator;
            _cloudinaryService = cloudinaryService;
        }
        // GET: api/<UsersController>
        [HttpGet("GetUsersInGroupChat/{groupChatId}/{caller}")]
        public async Task<IActionResult> GetUsersInGroupChat(int groupChatId, int caller)
        {
            var result = _unitOfWork.Users.GetUsersInGroupChat(groupChatId, caller);
            return Ok(result);
        }
        [HttpGet("GetUsersInGroupChatWithRoles/{groupChatId}")]
        public async Task<IActionResult> GetUsersInGroupChatWithRoles(int groupChatId)
        {
            var result = _unitOfWork.Users.GetUsersInGroupChatWithRoles(groupChatId);
            return Ok(result);
        }

        // PUT api/<UsersController>/5
        [HttpPut("ChangeUsername")]
        public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsernameDTO model)
        {
            var user = _unitOfWork.Users.GetById(model.userId);
            if (user == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            _mapper.Map(model, user);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var newJwtToken = _unitOfWork.Authentication.GenerateJSONWebToken(new LoginUserDTO
            {
                UserName = user.UserName,
                Password = user.Password,
            });
            return Ok(newJwtToken);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var user = _unitOfWork.Users.GetById(model.userId);
            if (user == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            _mapper.Map(model, user);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var newJwtToken = _unitOfWork.Authentication.GenerateJSONWebToken(new LoginUserDTO
            {
                UserName = user.UserName,
                Password = user.Password,
            });
            return Ok(newJwtToken);
        }

        [HttpPut("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDTO model)
        {
            var user = _unitOfWork.Users.GetById(model.userId);
            if (user == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            _mapper.Map(model, user);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            _unitOfWork.Commit();

            // send email confirm
            var sendEmailDto = new SendEmailDTO
            {
                From = _configuration["EmailSettings:From"],
                To = user.Email,
                Subject = "EMAIL CHANGING VERIFICATION",
                Body = $@"<!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Email Verification</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }}
                        .header {{
                            background-color: #4A90E2;
                            padding: 20px;
                            color: white;
                            text-align: center;
                        }}
                        .content {{
                            padding: 20px;
                            background-color: #f9f9f9;
                        }}
                        .button {{
                            display: inline-block;
                            background-color: #4A90E2;
                            color: white;
                            text-decoration: none;
                            padding: 12px 30px;
                            border-radius: 5px;
                            margin: 20px 0;
                            font-weight: bold;
                        }}
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                            font-size: 12px;
                            color: #777777;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Email Verification</h1>
                        </div>
                        <div class='content'>
                            <p>Hello,</p>
                            <p>We received a request to change the email address associated with your account. To complete this process, please click the button below:</p>
            
                            <div style='text-align: center;'>
                                <a href='{_configuration["Client"]}emailConfirm/{user.Email}' class='button'>Verify Email Address</a>
                            </div>
            
                            <p>If you didn't request this change, please ignore this email or contact our support team immediately.</p>
            
                            <p>If the button above doesn't work, you can copy and paste the following URL into your browser:</p>
                            <p style='word-break: break-all;'>{_configuration["Client"]}emailConfirm/{user.Email}</p>
            
                            <p>Thank you,<br>The Support Team</p>
                        </div>
                        <div class='footer'>
                            <p>This is an automated message, please do not reply to this email.</p>
                            <p>&copy; {DateTime.Now.Year} Your Company Name. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>"
            };
            await _unitOfWork.Emails.SendEmail(sendEmailDto);
            var newJwtToken = _unitOfWork.Authentication.GenerateJSONWebToken(new LoginUserDTO
            {
                UserName = user.UserName,
                Password = user.Password,
            });
            return Ok(newJwtToken);
        }

        [HttpPut("ChangeAvatar")]
        public async Task<IActionResult> ChangeAvatar([FromForm] ChangeAvatarRequestDTO model)
        {
            var user = _unitOfWork.Users.GetById(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            _unitOfWork.BeginTransaction();
            var avatar = await _cloudinaryService.UploadAttachment(model.Avatar);
            var userModel = new ChangeAvatarDTO
            {
                UserId = model.UserId,
                Avatar = avatar.Url,
            };
            _mapper.Map(userModel, user);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var newJwtToken = _unitOfWork.Authentication.GenerateJSONWebToken(new LoginUserDTO
            {
                UserName = user.UserName,
                Password = user.Password,
            });
            return Ok(newJwtToken);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }


        [HttpGet("ConfirmNewEmail/{userId}/{token}")]
        public async Task<IActionResult> ConfirmNewEmail(int userId, string token)
        {
            var existToken = _unitOfWork.EmailTokens.GetAll().FirstOrDefault(x => x.Token == token).Token;
            if (token != existToken)
            {
                return NotFound();
            }
            return Ok(new { IsVerified = true });
        }

        [HttpGet("GetToken/{userId}/{email}")]
        public async Task<IActionResult> GetToken(int userId, string email)
        {
            _unitOfWork.BeginTransaction();
            var token = _randomStringGenerator.GenerateUniqueRandomString(6);
            var emailTokenDto = new CreateEmailTokenDTO
            {
                UserId = userId,
                Token = token
            };
            var emailToken = _mapper.Map<EmailToken>(emailTokenDto);
            _unitOfWork.EmailTokens.Insert(emailToken);
            _unitOfWork.Save();
            _unitOfWork.Commit();
            var sendEmailDto = new SendEmailDTO
            {
                From = _configuration["EmailSettings:From"],
                To = email,
                Subject = "Your Email Verification Token",
                Body = $@"<!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Email Verification Token</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }}
                        .header {{
                            background-color: #4A90E2;
                            padding: 20px;
                            color: white;
                            text-align: center;
                        }}
                        .content {{
                            padding: 20px;
                            background-color: #f9f9f9;
                        }}
                        .token {{
                            display: inline-block;
                            background-color: #4A90E2;
                            color: white;
                            font-size: 24px;
                            letter-spacing: 4px;
                            padding: 12px 30px;
                            border-radius: 5px;
                            margin: 20px 0;
                            font-weight: bold;
                        }}
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                            font-size: 12px;
                            color: #777777;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Email Verification</h1>
                        </div>
                        <div class='content'>
                            <p>Hello,</p>
                            <p>We received a request to change the email address associated with your account. To complete this process, please use the verification token below:</p>
                            <div style='text-align: center;'>
                                <span class='token'>{token}</span>
                            </div>
                            <p>If you didn't request this change, please ignore this email or contact our support team immediately.</p>
                            <p>Thank you,<br>The Support Team</p>
                        </div>
                        <div class='footer'>
                            <p>This is an automated message, please do not reply to this email.</p>
                            <p>&copy; {DateTime.Now.Year} Your Company Name. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>"
            };
            await _unitOfWork.Emails.SendEmail(sendEmailDto);
            return Ok("Verification token sent to your email.");
        }

    }
}
