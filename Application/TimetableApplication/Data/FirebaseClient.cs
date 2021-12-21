using System.Collections.Generic;
using System.Linq;
using TimetableDomain;
using Firebase.Database.Query;

namespace TimetableApplication
{
    public class FirebaseClient : IDatabaseClient
    {
        public DatabaseClient Name => DatabaseClient.Firebase;
        private Firebase.Database.FirebaseClient firebaseClient;
        
        public FirebaseClient(string firebaseUrl)
        {
            firebaseClient = new Firebase.Database.FirebaseClient(firebaseUrl);
        }
        
        public void SetInputInfo(string uid, IEnumerable<SlotInfo> slots)
        {
            foreach (var slotInfo in slots)
                firebaseClient.Child("Users/" + uid + "/slotinfo").PostAsync(slotInfo);
        }

        public IEnumerable<SlotInfo> GetInputInfo(string uid)
        {
            return firebaseClient
                .Child("Users")
                .Child(uid)
                .Child("slotinfo")
                .OnceAsync<SlotInfo>()
                .Result
                .Select(x => x.Object);
        }

        public IEnumerable<string> GetTeacherFilters(string uid)
        {
            return firebaseClient
                .Child("Users")
                .Child(uid)
                .Child("slotinfo")
                .OnceAsync<SlotInfo>()
                .Result
                .Select(x => x.Object.Teacher)
                .Distinct();
        }

        public void SetTimeslots(string uid, IEnumerable<TimeSlot> timeslots)
        {
            foreach (var timeslot in timeslots)
                firebaseClient.Child("Users/" + uid + "/timeslots").PostAsync(timeslot);
        }

        public IEnumerable<TimeSlot> GetTimeslots(string uid)
        {
            return firebaseClient
                .Child("Users")
                .Child(uid)
                .Child("timeslots")
                .OnceAsync<TimeSlot>()
                .Result
                .Select(x => x.Object);
        }

        public void DeleteUserData(string uid)
        {
            firebaseClient.Child("Users").Child(uid).DeleteAsync();
        }
    }
}