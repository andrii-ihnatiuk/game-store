using System.Net.Mime;
using System.Text;
using System.Transactions;
using GameStore.Data.Entities.Identity;
using GameStore.Data.Extensions;
using GameStore.Services.Interfaces.Authentication;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Authentication;
using GameStore.Shared.DTOs.User;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GameStore.Services.Authentication;

public class ExternalLoginService : ILoginService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthApiOptions _apiOptions;
    private readonly IJwtProvider _jwtProvider;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExternalLoginService(
        IHttpClientFactory httpClientFactory,
        IOptions<AuthApiOptions> apiOptions,
        IJwtProvider jwtProvider,
        UserManager<ApplicationUser> userManager)
    {
        _httpClientFactory = httpClientFactory;
        _apiOptions = apiOptions.Value;
        _jwtProvider = jwtProvider;
        _userManager = userManager;
    }

    public LoginMethod LoginMethod => LoginMethod.AuthApi;

    public async Task<string> LoginAsync(UserLoginDto dto)
    {
        using var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_apiOptions.BaseApiUrl);

        var loginData = new ExternalLoginRequestDto(dto.Model.Login, dto.Model.Password);
        var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await client.PostAsync(_apiOptions.LoginApiUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            throw new LoginException();
        }

        var user = await _userManager.FindByEmailAsync(dto.Model.Login);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = dto.Model.Login,
                Email = dto.Model.Login,
            };
            await SaveUserLoginInfoAsync(user);
        }

        return await _jwtProvider.GenerateTokenAsync(user);
    }

    private async Task SaveUserLoginInfoAsync(ApplicationUser user)
    {
        var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        using var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        await _userManager.CreateAsync(user).ThrowIfFailedAsync();
        await _userManager.AddToRoleAsync(user, Roles.User);

        scope.Complete();
    }
}