using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using Firebase.Database.Query;

namespace Infrastructure
{
    public class FirebaseWrapper<TKey, TStoredObject> : IDatabaseWrapper<TKey, TStoredObject>
    {
        private readonly FirebaseClient firebaseClient;
        private readonly string category;

        public FirebaseWrapper(FirebaseClient firebaseClient)
        {
            this.firebaseClient = firebaseClient;
            category = typeof(TStoredObject).ToString().Split('.').Last();
        }
        
        public void AddRange(TKey key, IEnumerable<TStoredObject> content)
        {
            foreach (var item in content)
                firebaseClient.Child($"{key}/{category}").PostAsync(item);
        }
        
        public bool ContainsKey(TKey key)
        {
            return firebaseClient
                .Child(key.ToString())
                .Child(category)
                .OnceAsync<TStoredObject>()
                .Result
                .Any();
        }

        public IEnumerable<TStoredObject> ReadBy(TKey key)
        {
            return firebaseClient
                .Child(key.ToString())
                .Child(category)
                .OnceAsync<TStoredObject>()
                .Result
                .Select(x => x.Object);
        }
    }
}