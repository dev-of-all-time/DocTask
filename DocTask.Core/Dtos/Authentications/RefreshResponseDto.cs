namespace DocTask.Core.Dtos.Authentications;

public class RefreshResponseDto
{
    public RefreshResponseDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}