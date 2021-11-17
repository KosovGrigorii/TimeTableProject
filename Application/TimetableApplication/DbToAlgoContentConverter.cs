using System;
using System.Collections.Generic;
using TimetableDomain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TimetableApplication
{
    public class DbToAlgoContentConverter
    {
        public IEnumerable<Entity> Convert(DbContext dbData)
        {
            throw new NotImplementedException();
        }
    }
}