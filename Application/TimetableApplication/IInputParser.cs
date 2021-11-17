using System.Collections;

namespace TimetableApplication
{
    public interface IInputParser
    {
        void ParseFile(); //IEnumerable<> сущностей для заполнения базы, принимаемый тип: выяснить, что возможно вернуть, не сохраняя файл
    }
}