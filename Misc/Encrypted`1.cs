using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DLeh.Util
{

    /// <summary>
    /// This class is used to help with encrypting values when using on the public site, to help obfuscate IDs like FundID or AcctIDs.
    /// </summary>
    /// <remarks>
    /// There is no constructor for these. You must explictly new one up and set either the Value or the EncryptedValue. 
    /// Otherwise, there would be a constructor that accepts a T value and one that accepts a string to decrypt into a T, but technically you 
    /// could have T be a string, and then the constructors would be ambiguous. I'm not sure if the compiler would handle that or if it'd just confuse
    /// the programmer at runtime.
    /// 
    /// So, in short, use these two ways to encrypt a value
    /// var encryptedInt = new Encrypted<int> { Value = 5 };
    /// Encrypted<int> encryptedInt = 5;
    /// 
    /// And this way to decrypt
    /// var encryptedInt = new Encrypted<int> { EncryptedValue = "asdfasdf" };
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class Encrypted<T>
    {

        // These implicits allow for simple conversion between an encrypted value and it's base type. I believe that this is also necessary
        // for use with the MVC model binder. This way you can provide a string and it will implicitly convert it to an Encrypted<T> when it's expected.
        public static implicit operator T(Encrypted<T> id) => id.Value;
        public static implicit operator Encrypted<T>(T id) => new Encrypted<T> { Value = id };
        public static implicit operator Encrypted<T>(string encryptedValue) => new Encrypted<T> { EncryptedValue = encryptedValue };

        static Encrypted()
        {
            //Password = ConfigurationManager.AppSettings["OldAdminSiteSharedSecret"];
            Password = "Some Secret"; //todo: make an ICryptoSecretProvider?
        }

        static string Password;


        /// <summary>
        /// alias for EncryptedValue for use in urls. urls will show up as "?acctID.ev=asdfasdf" 
        /// instead of "acctID.EncryptedValue" which is a bit more telling and verbose.
        /// </summary>
        public string ev { get { return EncryptedValue; } set { EncryptedValue = value; } }

        private string encryptedValue;
        /// <summary>
        /// The encrypted string value of the provided value.
        /// </summary>
        public string EncryptedValue { get { return encryptedValue; } set { SetValueFromEncrypted(value); } }


        private T _value;
        public T Value { get { return _value; } set { SetValue(value); } }

        private void SetValue(T v)
        {
            _value = v;
            encryptedValue = CryptoHelper.EncryptString(v.ToString(), Password);
        }
        private void SetValueFromEncrypted(string ev)
        {
            encryptedValue = ev;
            _value = (T)Convert.ChangeType(CryptoHelper.DecryptString(encryptedValue, Password), typeof(T));
        }

        /// <summary>
        /// This must return the EncryptedValue in order for binding to work properly.
        /// </summary>
        /// <returns>The Encrypted Value</returns>
        public override string ToString() => EncryptedValue;
    }

}