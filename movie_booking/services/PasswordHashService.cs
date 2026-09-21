using System.Security.Cryptography;

namespace movie_booking.services
{
    public class PasswordHashService
    {
        private RandomNumberGenerator _rng;
        private byte[] _bytesArray;
        
        public byte[] PBKDF2(string Password, byte[] salt, int iterations, int outputBytes) {
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(Password, salt);  //again derived key in byte[] format
            rfc2898DeriveBytes.IterationCount = iterations;
            return rfc2898DeriveBytes.GetBytes(outputBytes);
        }

        public bool isPasswordEquals(byte[] a1, byte[] a2) {
            uint num = (uint)(a1.Length ^ a2.Length);  //uint means an unisigned integer and cant be negative

            for (int i = 0; i < a1.Length && i < a2.Length; i++) {
                num |= (uint)(a1[i] ^ a2[i]);
            };
            // we use num |= because and it is same as num = num | (uint)(a1[i] ^ a2[i]); “If any difference is found between a1 and a2, keep a 1 somewhere in num so we remember that a difference happened.”
            //And that’s why, in the end, we check num == 0 to see if the arrays are equal. ❤️
            //The operator ^ is bitwise XOR — it compares bits of both bytes.
            //a1[i] = 10110010
            //a2[i] = 10110111
            //---------------- XOR
            //result = 00000101
            return num == 0;
        }
        public string CreateHashedPassword(string Password)
        {

            this._rng = RandomNumberGenerator.Create();
            this._bytesArray = new byte[64];
            this._rng.GetBytes(this._bytesArray); //we have added os level random bytes into bytes array
            byte[] inArray = PBKDF2(Password, this._bytesArray, 1000, 64);
            return 1000 + ":" + Convert.ToBase64String(this._bytesArray) + ":" + Convert.ToBase64String(inArray);
        }

        public bool ValidatePassword(string Password, string HashedPassword)
        {
            char[] separator = new Char[1] { ':' };
            string[] splittedHash = HashedPassword.Split(separator);
            byte[] salt = Convert.FromBase64String(splittedHash[1]);
            byte[] array2 = Convert.FromBase64String(splittedHash[2]);
            int iterationsCount = int.Parse(splittedHash[0]);
            byte[] inArray = PBKDF2(Password, salt, iterationsCount, array2.Length);
            return isPasswordEquals(array2, inArray);
        }

    }
}
