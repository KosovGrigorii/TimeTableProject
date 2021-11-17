

using System.Collections.Generic;

namespace TimetableApplication
{
    public class FilterInputHandler
    {
        public static IEnumerable<string> GetFilterTypes()
        {
            //Запрос к базе на типы фильтров
            var filterTypes = new List<string>
            {
                "Teacher", 
                "Group"
            };
            return filterTypes;
        }

        public static IEnumerable<string> GetFiltersOfType(string filterType)
        {
            //Запросить у базы и кэшировать список всех имён по ключу
            var typeToFilter = new Dictionary<string, List<string>>
            {
                {"Teacher", new List<string>{"T1", "T2", "T3"}},
                {"Group", new List<string>{"First", "Second", "Third"}}
            };
            return typeToFilter[filterType];
        }
    }
}