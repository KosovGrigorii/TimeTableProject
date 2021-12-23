using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace TimetableApplication
{
    public class DatabasesChooser
    {
        private IReadOnlyDictionary<Database, IDatabaseWrapper<string, DatabaseSlot>> slotWrappers;
        private IReadOnlyDictionary<Database, IDatabaseWrapper<string, DatabaseTimeslot>> timeslotWrappers;

        public DatabasesChooser(
            IEnumerable<IDatabaseWrapper<string, DatabaseSlot>> slotWrappers,
            IEnumerable<IDatabaseWrapper<string, DatabaseTimeslot>> timeslotWrappers)
        {
            this.slotWrappers = slotWrappers.ToDictionary(x => x.BaseName);
            this.timeslotWrappers = timeslotWrappers.ToDictionary(x => x.BaseName);
        }

        public TimetableDatabases GetDatabaseWrappers()
            => new (){SlotWrapper = GetWrapper(slotWrappers), TimeslotWrapper = GetWrapper(timeslotWrappers)};

        private IDatabaseWrapper<string, TItem> GetWrapper<TItem>(IReadOnlyDictionary<Database, IDatabaseWrapper<string, TItem>> wrappersDict)
        {
            foreach (var db in Enum.GetValues(typeof(Database)).Cast<Database>())
                if (wrappersDict.TryGetValue(db, out var slotWrapper))
                    return slotWrapper;
            throw new Exception("Suitable database was not found");
        }
    }
}