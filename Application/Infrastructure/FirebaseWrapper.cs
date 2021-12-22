using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using Firebase.Database.Query;

namespace Infrastructure
{
    public class FirebaseWrapper<TKey, TStoredObject> : IDatabaseWrapper<TKey, TStoredObject>
    {
        public Database BaseName => Database.Firebase;
        private FirebaseClient firebaseClient;
        private string keyName;
        private string category;

        public FirebaseWrapper(string firebaseUrl, string keyName)
        {
            firebaseClient = new FirebaseClient(firebaseUrl);
            this.keyName = keyName;
            category = typeof(TStoredObject).ToString().Split('.').Last();
        }
        
        public void AddRange(TKey key, IEnumerable<TStoredObject> content)
        {
            foreach (var item in content)
                firebaseClient.Child($"{keyName}/{key}/{category}").PostAsync(item);
        }

        public IEnumerable<TStoredObject> ReadBy(TKey key)
        {
            return firebaseClient
                .Child(keyName)
                .Child(key.ToString)
                .Child(category)
                .OnceAsync<TStoredObject>()
                .Result
                .Select(x => x.Object);
        }

        public void DeleteKey(TKey key)
        {
            firebaseClient
                .Child(keyName)
                .Child(key.ToString)
                .DeleteAsync();
        }
    }
}