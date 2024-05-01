using API.DTOs.Photos;
using API.DTOs.Points;
using API.DTOs.Users;
using API.DTOs.Users.Member;
using API.DTOs.Vouchers;
using API.Entities;
using API.Entities.Users;
using API.Extentions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers.Users
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IPointTransactionRepository _pointTransactionRepository;

        public UserController(IUserRepository userRepository, IMapper mapper,
            IPhotoService photoService, IVoucherRepository voucherRepository,
            IPointTransactionRepository pointTransactionRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
            _voucherRepository = voucherRepository;
            _pointTransactionRepository = pointTransactionRepository;
        }

        #region GetUserByUsername
        [HttpGet("GetUserByUsername/{username}", Name = "GetUserByUsername")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            return await _userRepository.GetMemberByUsername(username);
        }
        #endregion

        #region GetUserById
        [HttpGet("GetUserById/{userId}")]
        public async Task<ActionResult<MemberDto>> GetUserById(int userId)
        {
            return await _userRepository.GetMemberById(userId);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetListUsers")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetListUsers()
        {
            var users = await _userRepository.GetListMembers();
            return Ok(users);
        }
        #endregion

        #region UpdateUser
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsername(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _userRepository.UpdateUser(user);

            if (await _userRepository.Save()) return NoContent();

            return BadRequest("Failed to update user");
        }
        #endregion

        #region SetAvatar
        [HttpPost("SetAvatar")]
        public async Task<ActionResult<AvatarDto>> SetAvatar(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsername(User.GetUsername());

            //if(user.Avatar.PublicId != null || user.Avatar.PublicId != "default_avatar")
            //{
            //    await _photoService.DeletePhoto(user.Avatar.PublicId);
            //}

            var result = await _photoService.AddPhoto(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var avatar = new Avatar
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                AppUserId = user.Id,
            };

            user.Avatar = avatar;

            if (await _userRepository.Save())
            {
                return CreatedAtRoute("GetUserByUsername", new { username = user.UserName }, _mapper.Map<AvatarDto>(avatar));
            }

            return BadRequest("Problem adding photo");
        }
        #endregion

        #region GetListPointTransactions
        [HttpGet("GetListPointTransactions")]
        public async Task<ActionResult<IEnumerable<PointTransactionOutputDto>>> GetListPointTransactions()
        {
            var result = await _userRepository.GetListPointTransactions(User.GetUserId());
            return Ok(result);
        }
        #endregion

        #region ConvertPointsToVoucher
        [HttpPost("ConvertPointToVoucher")]
        public async Task<ActionResult> ConvertPointToVoucher([FromBody] int points)
        {
            var user = await _userRepository.GetMemberById(User.GetUserId());
            if (user == null) return NotFound();
            if (user.ContributionPoints < points) return BadRequest("Your points are not enough");

            var userId = user.Id;
            var voucherValue = VoucherExtentions.CalculateVoucherValue(points);
            var expiryDate = DateTime.Now.AddDays(7);

            string uniqueString = $"{userId}-{DateTime.UtcNow}-{Guid.NewGuid()}";
            string voucherCode = GetUniqueHashCode(uniqueString, 12);

            var voucher = new Voucher
            {
                UserId = userId,
                ExpiryDate = expiryDate,
                Value = voucherValue,
                Code = voucherCode,
            };

            var pointsTran = new PointTransactionInputDto
            {
                UserId = userId,
                PointsChange = -points,
                Description = "Convert points to voucher"
            };
            await _voucherRepository.CreateVoucher(voucher);
            await _userRepository.UpdateContributionPoints(pointsTran);
            return Ok();
        }

        private string GetUniqueHashCode(string input, int length)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                string base64Hash = Convert.ToBase64String(hashBytes);
                return base64Hash.Replace("/", "").Replace("+", "").Substring(0, length);
            }
        }

        #endregion

        [HttpGet("GetListVoucherByUserId")]
        public async Task<ActionResult<IEnumerable<VoucherOutputDto>>> GetListVoucherByUserId()
        {
            var vouchers = await _voucherRepository.GetListVoucersByUserId(User.GetUserId());
            return Ok(vouchers);
        }
    }
}