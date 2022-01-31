namespace MyCompany.NameProject.Domain.Interfaces
{
    /// <summary>
    /// Преобразователь моделей друг в друга
    /// </summary>
    /// <typeparam name="TSource">Исходный тип модели</typeparam>
    /// <typeparam name="TTarget">Целевой тип модели</typeparam>
    public interface IMapper<in TSource, out TTarget>
    {
        /// <summary>
        /// Преобразовать модель
        /// </summary>
        /// <param name="source">Исходная модель</param>
        /// <returns>Целевая модель</returns>
        TTarget Map(TSource source);
    }
}
