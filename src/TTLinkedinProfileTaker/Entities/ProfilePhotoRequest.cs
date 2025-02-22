namespace TTLinkedinProfileTaker.Entities;

public sealed class ProfilePhotoRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}