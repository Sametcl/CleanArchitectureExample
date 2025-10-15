using AutoMapper;
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.CreateNewTokenByRefreshToken;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.Login;
using CleanArchitecture.Application.Features.AuthFeatures.Commands.Register;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistance.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IJwtProvider jwtProvider;
        public AuthService(IMapper mapper, UserManager<User> userManager, IJwtProvider jwtProvider)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.jwtProvider = jwtProvider;
        }

        public async Task<LoginCommandResponse> CreateNewTokenByRefreshTokenAsync(CreateNewTokenByRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            User user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("Kullanici bulunamadi ");
            }
            if (user.RefreshToken != request.RefreshToken)
            {
                throw new Exception("Refresh Token bilgisi gecersizdir.");
            }
            if (user.RefreshTokenExpires < DateTime.UtcNow)
            {
                throw new Exception("Refresh Token suresi dolmustur.");
            }

            LoginCommandResponse response = await jwtProvider.CreateTokenAsync(user);
            return response;
        }

        public async Task<LoginCommandResponse> LoginAsync(LoginCommand request ,CancellationToken cancellationToken)
        {
            User? user = await userManager.Users.Where(p =>
            p.UserName == request.UserNameOrEmail || p.Email == request.UserNameOrEmail)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new Exception("Kullanici bulunamadi ");
            }

            bool result = await userManager.CheckPasswordAsync(user, request.Password);
            if (result)
            {
                LoginCommandResponse response = await jwtProvider.CreateTokenAsync(user);
                return response;
            }
            throw new Exception("Kullanici adi ya da sifre hatali ");
        }

        public async Task RegisterAsync(RegisterCommand request)
        {
            User user = mapper.Map<User>(request);
            IdentityResult result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
    }
}
