namespace VerticalSliceArchitecture.Core.Services
{
    public interface IHasher : IService
    {
        string GetSalt();
        string GetHash(string value, string salt);
        (string hash, string salt) GetHash(string value);
    }
}
