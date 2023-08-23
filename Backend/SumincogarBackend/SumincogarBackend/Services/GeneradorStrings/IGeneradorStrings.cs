using System.Text;

namespace SumincogarBackend.Services.GeneradorStrings
{
    public interface IGeneradorStrings
    {
        string GenerateRandomString(int length);
    }
}
