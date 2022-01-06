using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using TimetableDomain;

namespace TimetableApplication
{
    public class TimetableResultsInterface
    {
        private readonly TimeslotDbConverter converter;
        private readonly IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper;

        public TimetableResultsInterface(TimeslotDbConverter converter, IDatabaseWrapper<string, DatabaseTimeslot> timeslotWrapper)
        {
            this.timeslotWrapper = timeslotWrapper;
            this.converter = converter;
        }

        public bool IsTimetableReadyForUser(User user)
        => timeslotWrapper.ContainsKey(user.Id);

        public IEnumerable<TimeSlot> GetResultForUser(User user)
            => timeslotWrapper.ReadBy(user.Id).Select(ts => converter.DbTimeslotToTimeslot(ts));
    }
}