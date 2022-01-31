namespace MyCompany.NameProject.Domain.Interfaces
{
    /// <summary>
    /// Обогатитель моделей
    /// </summary>
    /// <typeparam name="TModel">Тип модели</typeparam>
    public interface IEnricher<in TModel>
    {
        /// <summary>
        /// Обогащает модель данными
        /// </summary>
        /// <param name="model">Модель</param>
        /// <param name="data">Данные модели</param>
        void Enrich(TModel model, TModel data);
    }
}
