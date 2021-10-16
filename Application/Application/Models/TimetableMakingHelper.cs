namespace Application
{
    public class TimetableMakingHelper
    {
        public void InputFile()
        {
            //(?) Принять файл ввода - GetEntitiesInfo в MainPageController, файл
            //загружается в папку FilesStorage, возможно, позже придётся перейти к
            //хранению в базе данных
            //Выбрать парсер по формату ввода
        }

        public void TakeFilters() //фильтры
        {
            //Запустить парсер с фильтрами
        }

        public void StartMakingTimetable()
        {
            //(?)Выбрать алгоритм
            //Запустить алгоритм  Start()
        }

        public void Output() //формат вывода
        {
            //Преобразовать таймслоты в формат вывода (запустить output formatter)
            //Вывод
        }
    }
}