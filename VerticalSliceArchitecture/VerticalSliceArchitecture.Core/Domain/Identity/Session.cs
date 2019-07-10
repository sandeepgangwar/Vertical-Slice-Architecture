using System;
using VerticalSliceArchitecture.Core.Domain.Shared;
using VerticalSliceArchitecture.Core.Exceptions;
using VerticalSliceArchitecture.Core.Helpers;

namespace VerticalSliceArchitecture.Core.Domain.Identity
{
    public class Session :BaseEntity<Guid>
    {
       
        public  User User { get; private set; }

        public Guid UserId { get; private set; }
        public string DeviceId { get; private set; }
        public string DeviceName { get; private set; }
        public Platform Platform { get; private set; }
        public string RefreshToken { get; private set; }
        public int NumberOfRefreshes { get; private set; }
        public DateTime? RefreshedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        private Session()
        {
        }

        public Session(
            Guid userId,
            string deviceId,
            string deviceName,
            Platform platform,
            string refreshToken,
            DateTime refreshTokenExpiresDate) : this()
        {
            UserId = userId;
            SetDeviceId(deviceId);
            SetDeviceName(deviceName);
            Platform = platform;
            SetRefreshToken(refreshToken, refreshTokenExpiresDate);
        }

        public bool IsExpired()
        {
            return ExpiresAt <= DateTimeHelper.Now;
        }

        public void LogAndSetRefreshToken(string refreshToken, DateTime expiresAt)
        {
            RefreshedAt = DateTimeHelper.Now;
            NumberOfRefreshes++;
            SetRefreshToken(refreshToken, expiresAt);
        }

        public void SetDeviceName(string deviceName)
        {
            if (string.IsNullOrWhiteSpace(deviceName))
            {
                throw new DomainException(DomainExceptionCodes.InvalidDeviceName,
                    ExceptionMessageHelpers.NotEmpty(nameof(DeviceName)));
            }

            if (DeviceName == deviceName)
            {
                return;
            }

            if (deviceName.Length > 255)
            {
                throw new DomainException(DomainExceptionCodes.InvalidDeviceName,
                    ExceptionMessageHelpers.NoLongerThen(nameof(deviceName), 255));
            }

            DeviceName = deviceName;
        }

        private void SetDeviceId(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                throw new DomainException(DomainExceptionCodes.InvalidDeviceId,
                   ExceptionMessageHelpers.NotEmpty(nameof(DeviceId)));
            }

            if (DeviceId == deviceId)
            {
                return;
            }

            if (deviceId.Length > 64)
            {
                throw new DomainException(DomainExceptionCodes.InvalidDeviceId,
                    ExceptionMessageHelpers.NoLongerThen(nameof(DeviceId), 64));
            }

            DeviceId = deviceId;
        }

        private void SetRefreshToken(string refreshToken, DateTime expiresAt)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new DomainException(DomainExceptionCodes.InvalidRefreshToken,
                    ExceptionMessageHelpers.NotEmpty(nameof(RefreshToken)));
            }

            if (RefreshToken == refreshToken && ExpiresAt == expiresAt)
            {
                return;
            }

            if (refreshToken.Length > 64)
            {
                throw new DomainException(DomainExceptionCodes.InvalidRefreshToken,
                    ExceptionMessageHelpers.NoLongerThen(nameof(RefreshToken), 64));
            }

            if (expiresAt <= DateTimeHelper.Now)
            {
                throw new DomainException(DomainExceptionCodes.InvalidRefreshTokenExpiresDate,
                    "Refresh token expires date must be in the future.");
            }

            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
        }
    }
}