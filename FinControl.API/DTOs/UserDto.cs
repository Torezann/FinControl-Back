namespace FinControl.API.DTOs;

public record UserProfileDto(
    string Email
);

public record UpdateEmailDto(
    string Email
);

public record ChangePasswordDto(
    string CurrentPassword,
    string NewPassword
);
