using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Server.Options;
using System.Collections.Generic;
using Entities.ViewModels;
using DataAccessLayer.Repositories;
using Entities.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly AccountRepository _accountRepository;

        public AccountController(IOptionsSnapshot<JwtIssuerOptions> jwtOptions, 
            ILoggerFactory loggerFactory, 
            AccountRepository accountRepository)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

            _logger = loggerFactory.CreateLogger<AccountController>();
            _accountRepository = accountRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserVM userVM)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _accountRepository.VerifyUser(userVM);
                if (user == null)
                {
                    _logger.LogInformation($"Invalid username or password");
                    return BadRequest("Invalid credentials");
                }
                return new ObjectResult(await GetToken(user));
                }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserVM userVM)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _accountRepository.CreateUser(userVM);
                if (user == null)
                {
                    return BadRequest();
                }
                _logger.LogInformation($"User with Id = {user.Id} created account.");

                return new ObjectResult(await GetToken(user));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private async Task<TokenResponseVM> GetToken(ApplicationUser user)
        {
            var claims = await GetClaims(user);

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration(),
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            var tokenResponse = new TokenResponseVM
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            return tokenResponse;
            // return JsonConvert.SerializeObject(response, _serializerSettings);
        }

        private async Task<IEnumerable<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.Id),
              new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
              new Claim(JwtRegisteredClaimNames.Iat,
                        ToUnixEpochDate(_jwtOptions.IssuedAt()).ToString(),
                        ClaimValueTypes.Integer64),
            };

            // if (user.Roles != null)
            // {
            //     foreach (var role in user.Roles)
            //     {
            //         claims.Add(new Claim("role", role));
            //     }
            // }
            return claims;
        }
    }
}