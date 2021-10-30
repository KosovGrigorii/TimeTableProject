using System.Collections;

namespace TimetableApplication
{
    public interface IInputParser
    {
        void ParseFile(byte[] input); //IEnumerable<> сущностей для заполнения базы
    }
}